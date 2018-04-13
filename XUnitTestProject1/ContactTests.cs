using LmycWeb.APIControllers;
using LmycWeb.Interfaces;
using LmycWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XUnitTestProject1
{
    public class ContactTests
    {
        [Fact]
        public void GetAllContacts()
        {
            var dbContext = new Mock<IDbContext>();
            var mockList = MockDbSet(testContacts);
            dbContext.Setup(c => c.Contacts).Returns(mockList.Object);

            var controller = new ContactsController(dbContext.Object);
            var resultList = controller.GetContacts().ToList();

            Assert.Equal(3, resultList.Count);
        }

        [Fact]
        public void GetContact_WhenModelStateIsInvalid()
        {
            var dbContext = new Mock<IDbContext>();
            string documentId = "abc123";
            var controller = new ContactsController(dbContext.Object);
            controller.ModelState.AddModelError("key", "message");
            var result = controller.GetContact(documentId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void PutContact_WhenModelStateIsInvalid()
        {
            var dbContext = new Mock<IDbContext>();
            var controller = new ContactsController(dbContext.Object);
            controller.ModelState.AddModelError("key", "message");
            var result = controller.PutContact(null, null);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void PutContact_WhenIdDoesNotMatchDocumentId()
        {
            string id = "abc123";
            var contact = testContacts[0];
            var dbContext = new Mock<IDbContext>();
            var controller = new ContactsController(dbContext.Object);
            int code = 400;

            var result = controller.PutContact(id, contact);

            var badRequestResult = Assert.IsType<BadRequestResult>(result.Result);
            Assert.Equal(code, badRequestResult.StatusCode);
        }


        [Fact]
        public void PostContact_WhenModelStateIsInvalid()
        {
            Contact contact = new Contact();
            var dbContext = new Mock<IDbContext>();
            var controller = new ContactsController(dbContext.Object);
            controller.ModelState.AddModelError("key", "message");

            var result = controller.PostContact(contact);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void PostContact_Successful()
        {
            Contact contact = new Contact();
            var dbContext = new Mock<IDbContext>();
            var controller = new ContactsController(dbContext.Object);
            var mockList = MockDbSet(testContacts);
            dbContext.Setup(x => x.Contacts).Returns(mockList.Object);

            var result = controller.PostContact(contact);

            Assert.IsType<CreatedAtActionResult>(result.Result);
        }


        /* Helper methods and sample data */

        List<Contact> testContacts = new List<Contact>()
                {
                    new Contact
                    {
                        ContactId = "A11",
                        FirstName = "Dude",
                        LastName = "Bro",
                        Email = "a@a.com",
                        Subject = "blah",
                        Other = "other"
                    },
                    new Contact
                    {
                        ContactId = "B22",
                        FirstName = "Dudette",
                        LastName = "Gal",
                        Email = "b@b.com",
                        Subject = "blah",
                        Other = "other"
                    },
                    new Contact
                    {
                        ContactId = "C33",
                        FirstName = "bob",
                        LastName = "barker",
                        Email = "a@a.com",
                        Subject = "blah",
                        Other = "other"
                    },
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
