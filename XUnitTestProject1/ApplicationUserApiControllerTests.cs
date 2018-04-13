using LmycWeb.APIControllers;
using LmycWeb.Interfaces;
using LmycWeb.Models;
using LmycWeb.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject1
{
    public class ApplicationUserApiControllerTests
    { 
        [Fact]
        public void GetApplicationUserWhenModelStateInvalid()
        {
            var controller = new ApplicationUsersController(null);
            controller.ModelState.AddModelError("key", "message");

            var result = controller.GetApplicationUser(null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        //check negative id's can't be used to get an ApplicationUser
        [Fact]
        public void GetApplicationUserWithNegativeIDReturnsNotFound()
        {
            var controller = new ApplicationUsersController(null);
            var result = controller.GetApplicationUser("-1");
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostapplicationUserSuccess()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "a006",
                FirstName = "Don",
                LastName = "watson",
                MemberStatus = "member"
            };

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            var mockContext = new Mock<IDbContext>();
            var mockList = Helpers.ToAsyncDbSetMock(testMembers);
            mockContext.Setup(c => c.ApplicationUser).Returns(mockList.Object);

            var controller = new ApplicationUsersController(mockContext.Object);
            var result = await controller.PostApplicationUser(user);
                        Assert.IsType<CreatedAtActionResult>(result);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task PostapplicationUserWhenModelStateIsNotValid()
        {
            ApplicationUser user = new ApplicationUser();
            var controller = new ApplicationUsersController(null);
            controller.ModelState.AddModelError("key", "error message");

            var result = await controller.PostApplicationUser(user);
            var requestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        //[Fact(Skip = "need to debug")]
        //public async Task RegisterApplicationUserWhenPasswordsDontMatch()
        //{
        //    string expectedResult = "Passwords don't match";

        //    RegisterViewModel user = new RegisterViewModel { Password = " ", ConfirmPassword = "test" };

        //    var controller = new AccountAPIController(null, null, null, null);
        //    var result = controller.Register(user);

        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
 
        //}

        [Theory]
        [InlineData("a002")]
        public async Task DeleteUserSuccess(string id)
        {
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            var mockContext = new Mock<IDbContext>();
            var mockList = Helpers.ToAsyncDbSetMock(testMembers);
            mockContext.Setup(c => c.ApplicationUser).Returns(mockList.Object);

            var controller = new ApplicationUsersController(mockContext.Object);
            var result = await controller.DeleteApplicationUser(id);
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData("a001")]
        public async Task DeleteUserModeStateNotValid(string id)
        {
            var controller = new ApplicationUsersController(null);
            controller.ModelState.AddModelError("key", "error message");
            var result = await controller.DeleteApplicationUser(id);
            var requestResult = Assert.IsType<BadRequestObjectResult>(result);
        }


        /* Helper methods and sample data */
        List<ApplicationUser> testMembers = new List<ApplicationUser>()
        {
            new ApplicationUser {Id="a001", FirstName="Luli", LastName="Lowenstein", MemberStatus="Member", SkipperStatus="none", Street="somewhere", City="vancouver", Province="BC", PostalCode="vvvvvv", Country="Canada", MobilePhone="5454546678",HomePhone="4532345655"},
            new ApplicationUser {Id="a002", FirstName="Evan", LastName="Blah", MemberStatus="Member", SkipperStatus="none", Street="somewhere", City="vancouver", Province="BC", PostalCode="vvvvvv", Country="Canada", MobilePhone="5454546678",HomePhone="4532345655"},
            new ApplicationUser {UserName="luli" ,Id="a003", FirstName="Sally", LastName="Oyster", MemberStatus="Member", SkipperStatus="none", Street="somewhere", City="vancouver", Province="BC", PostalCode="vvvvvv", Country="Canada", MobilePhone="5454546678",HomePhone="4532345655"},
            new ApplicationUser {Id="a004", FirstName="Carl", LastName="Weathers", MemberStatus="Member", SkipperStatus="none", Street="somewhere", City="vancouver", Province="BC", PostalCode="vvvvvv", Country="Canada", MobilePhone="5454546678",HomePhone="4532345655"},
            new ApplicationUser {Id="a005", FirstName="Michael", LastName="Cera", MemberStatus="Member", SkipperStatus="none", Street="somewhere", City="vancouver", Province="BC", PostalCode="vvvvvv", Country="Canada", MobilePhone="5454546678",HomePhone="4532345655"}
        };

        List<EmergencyContact> testEmergencyContacts = new List<EmergencyContact>()
        {
            new EmergencyContact
            {
                EmergencyContactId = "e01",
                Name1 = "myname",
                Phone1 = "666",
                Name2 = "otherName",
                Phone2 = "666"
            }
        };


        Mock<DbSet<T>> MockDbSet<T>(IEnumerable<T> list) where T : class, new()
        {
            IQueryable<T> queryableList = list.AsQueryable();
            Mock<DbSet<T>> dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(x => x.Provider).Returns(queryableList.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.Expression).Returns(queryableList.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(queryableList.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(() => queryableList.GetEnumerator());

            return dbSetMock;
        }
    }
}
