using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using LmycWeb.Data;
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
    [Route("api/accounts")]
    //[Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    //[EnableCors("CorsPolicy")]
    public class AccountAPIController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountAPIController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        // POST api/AccountAPI/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EmergencyContact emergencyContact = new EmergencyContact
            {
                Name1 = model.EmergencyContactName1,
                Phone1 = model.EmergencyContactPhone1,
                Name2 = model.EmergencyContactName2,
                Phone2 = model.EmergencyContactPhone2
            };

            await _context.EmergencyContacts.AddAsync(emergencyContact);

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
                EmergencyContacts = emergencyContact
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
