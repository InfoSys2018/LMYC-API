using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmycWeb.Models;
using LmycWeb.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LmycWeb.APIControllers
{
    [Produces("application/json")]
    [Route("api/AccountAPI")]
    [EnableCors("AllowAllOrigins")]
    public class AccountAPIController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountAPIController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        // POST api/AccountAPI/Register
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Street = model.Street,
                City = model.City,
                Province = model.Province,
                PostalCode = model.PostalCode,
                Country = model.Country,
                MobilePhone = model.MobilePhone,
                HomePhone = model.HomePhone,
                WorkPhone = model.WorkPhone,
                SailingQualifications = model.SailingQualifications,
                Skills = model.Skills,
                SailingExperience = model.SailingExperience,
                Credits = 320,

                EmergencyContacts = new EmergencyContact
                {
                    Name1 = model.EmergencyContactName1,
                    Phone1 = model.EmergencyContactPhone1,
                    Name2 = model.EmergencyContactName2,
                    Phone2 = model.EmergencyContactPhone2
                }
            };

            var result = await _userManager.CreateAsync(user, model.Password);


            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }

    }
}
