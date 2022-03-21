using Concert.Application.DTO;
using Concert.Domain.Entities;
using Concert.Infrastructure.Service;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConcertAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly UserManager<UserModel> userManager;
        readonly IPasswordHasher<UserModel> passwordHasher;
        readonly SignInManager<UserModel> signInManager;
        readonly MappingConfig config;
        readonly IdentityServices identityServices;
        readonly EmailService emailService;
        


        public AccountController(SignInManager<UserModel> signInManager, UserManager<UserModel> userManager,
            MappingConfig config, IPasswordHasher<UserModel> passwordHasher,
            IdentityServices identityServices, EmailService emailService)
        {
           this.config = config;
            this.userManager = userManager;
            this.passwordHasher = passwordHasher;
            this.signInManager = signInManager;
            this.identityServices = identityServices;
            this.emailService = emailService;
        }


        ///<param name="user">
        ///an object to sign up a user
        ///</param>
        /// <summary>
        /// Sign up a new user
        /// </summary>
        /// 
        /// <returns>200 response</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<ActionResult> SignUp(SignUp user)
        {
            try
            {           
                UserModel newUser = user.Adapt<UserModel>(config.UserModelConfig());
                if (user.Password.Equals(user.RetypePassword))
                {
                    IdentityResult identity = await userManager.CreateAsync(newUser, newUser.PasswordHash);
                    if (identity.Succeeded)
                    {
                        await identityServices.AddUserClaim(newUser);
                        await identityServices.AddUserRole(newUser, "User");   
                        string token = await identityServices.EmailConfirmationToken(newUser);

                        Mail mail = new Mail 
                        {
                            To = newUser.Email,
                            Text = token
                        };

                        await emailService.SendSimpleMessage(mail);
                        return this.StatusCode(StatusCodes.Status201Created, $"Welcome,{newUser.UserName} Check your mail to verify your account");
                    }
                    else
                    {
                        return BadRequest(identity.Errors);
                    }
                }
                return this.StatusCode(StatusCodes.Status400BadRequest, "Password not equal,retype password");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }


        ///<param name="email">
        ///a user's email
        ///</param>
        ///<param name="token">
        ///a token to confirm user's email
        ///</param>
        /// <summary>
        ///email confrimation
        /// </summary>
        /// 
        /// <returns>200 response</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("emailconfirmation", Name = "EmailConfirmation")]
        public async Task<ActionResult> VerifyEmailToken([FromBody] Token token, string email)
        {
            try
            {
                UserModel currentUser = await userManager.FindByEmailAsync(email);
                if (currentUser == null)
                {
                    return NotFound("email doesn't exist");
                }
                IdentityResult result = await userManager.ConfirmEmailAsync(currentUser, token.GeneratedToken);
                if (result.Succeeded)
                {
                    return this.StatusCode(StatusCodes.Status200OK, $"Welcome,{currentUser.UserName} Email has been verified");
                }
                else
                {
                    return this.StatusCode(StatusCodes.Status400BadRequest, "Invalid Token");
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

        ///<param name="user">
        ///an object to login
        ///</param>
        /// <summary>
        ///Login User
        /// </summary>
        /// 
        /// <returns>200 response</returns>

        //To generate the token to reset password
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("login")]
        public async Task<ActionResult> Login(Login user)
        {
            try
            {
                UserModel currentUser = await userManager.FindByEmailAsync(user.Email);
                if (currentUser == null)
                {
                    return NotFound("username or password is not correct");
                }

                //This verfies the user password by using IPasswordHasher interface
                PasswordVerificationResult passwordVerifyResult = passwordHasher.VerifyHashedPassword(currentUser, currentUser.PasswordHash, user.Password);
                if (passwordVerifyResult.ToString() == "Success")
                {
                    var claims = await userManager.GetClaimsAsync(currentUser);
                    var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                    this.User.AddIdentity(identity);
                    await signInManager.SignInWithClaimsAsync(currentUser, null, claims);

                    return Ok();
                }

                return BadRequest("username or password is not correct");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

        /// <summary>
        ///sign out current user
        /// </summary>
        /// 
        /// <returns>200 response</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [HttpPost("signout")]
        public async Task<ActionResult> SignOut()
        {
            try
            {
                await signInManager.SignOutAsync();
                return Ok("Signed out");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

    }
}
