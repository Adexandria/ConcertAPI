using Concert.Application.DTO;
using Concert.Application.Interface;
using Concert.Domain.Entities;
using Concert.Infrastructure.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcertAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        readonly UserManager<UserModel> userManager;
        readonly IAuth auth;
        public SettingsController(UserManager<UserModel> userManager, IAuth auth)
        {
            this.userManager = userManager;
            this.auth = auth;
            
        }


    

        ///<param name="userName">
        /// user's username
        ///</param>
        ///<param name="newUserName">
        ///an object used to change user's username
        ///</param>
        /// <summary>
        /// change username
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpPost("{userName}/change")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> ChangeUsername(UserNameDTO newUserName, string userName)
        {
            try
            {
                var currentUser = await userManager.FindByNameAsync(userName);
                if (currentUser == null)
                {
                    return NotFound("username doesn't exist");
                }
                
                var result = await userManager.SetUserNameAsync(currentUser, newUserName.UserName);
                if (result.Succeeded)
                {
                    return this.StatusCode(StatusCodes.Status200OK, "Successfully Changed");
                }

                return BadRequest(result.Errors);
                
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

        ///<param name="userName">
        ///user's username
        ///</param>
        ///<param name="userPassword">
        ///an object used to change the authorized user's password
        ///</param>
        /// <summary>
        /// change password
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpPost("{userName}/password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> ChangePassword(string userName,UserPassword userPassword)
        {
            try
            {
                var currentUser = await userManager.FindByNameAsync(userName);
                if (currentUser == null)
                {
                    return NotFound("username doesn't exist");
                }
               
                var result = await userManager.ChangePasswordAsync(currentUser, userPassword.CurrentPassword, userPassword.NewPassword);
                if (result.Succeeded)
                {
                    return this.StatusCode(StatusCodes.Status200OK, "Successfully Changed");
                }

                return BadRequest(result.Errors);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        ///<param name="userName">
        /// user's username
        ///</param>
        /// <summary>
        /// Get Authentication Key
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpGet("{userName}/Authentication")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> TwoFactorAuthentication(string userName)
        {
            try
            {
                var currentUser = await userManager.FindByNameAsync(userName);
                if (currentUser == null)
                {
                    return NotFound("username doesn't exist");
                }
                return Ok(auth.TwoFactorAuthentication(currentUser.Email));
             
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        ///<param name="userName">
        /// user's username
        ///</param>
        ///<param name="key">
        ///Google verification Key
        ///</param>
        /// <summary>
        /// Enabled Two authentication
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpPost("{userName}/Authentication/{key}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> AuthenticationKeyVerification(string userName, string key)
        {
            try
            {
                var currentUser = await userManager.FindByNameAsync(userName);
                if (currentUser == null)
                {
                    return NotFound("username doesn't exist");
                }
                if (auth.KeyVerification(currentUser.Email, key))
                {
                    await userManager.SetTwoFactorEnabledAsync(currentUser,true);
                    return Ok("Successful");
                }
                return BadRequest("Invalid Key, Try Again");

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
    }
}
