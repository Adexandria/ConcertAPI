using Concert.Application.DTO;
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
        readonly IPasswordHasher<UserModel> passwordHasher;
        readonly UserManager<UserModel> userManager;
        readonly EmailService emailService;
        public SettingsController(UserManager<UserModel> userManager, EmailService emailService, IPasswordHasher<UserModel> passwordHasher)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.passwordHasher = passwordHasher;
        }


    

        ///<param name="userName">
        ///\a user's username
        ///</param>
        ///<param name="newUserName">
        ///an object used to change user's username
        ///</param>
        /// <summary>
        /// change username
        /// </summary>
        /// 
        /// <returns>A string status</returns>
        [HttpPost("{username}/username/change")]
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
    }
}
