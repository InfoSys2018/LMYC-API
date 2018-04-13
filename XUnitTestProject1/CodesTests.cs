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
    public class CodesTests
    {
        [Fact]
        public void GetAllCodes()
        {
            var dbContext = new Mock<IDbContext>();
            var mockList = MockDbSet(testCodes);
            dbContext.Setup(c => c.ClassificationCodes).Returns(mockList.Object);

            var controller = new ClassificationCodesApiController(dbContext.Object);
            var resultList = controller.GetClassificationCodes().ToList();

            Assert.Equal(3, resultList.Count);
        }

        [Fact]
        public void GetCode_WhenModelStateIsInvalid()
        {
            var dbContext = new Mock<IDbContext>();
            string codeId = "abc123";
            var controller = new ClassificationCodesApiController(dbContext.Object);
            controller.ModelState.AddModelError("key", "message");
            var result = controller.GetClassificationCode(codeId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void PutCode_WhenModelStateIsInvalid()
        {
            var dbContext = new Mock<IDbContext>();
            var controller = new ClassificationCodesApiController(dbContext.Object);
            controller.ModelState.AddModelError("key", "message");
            var result = controller.PutClassificationCode(null, null);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void PutCode_WhenIdDoesNotMatchDocumentId()
        {
            string id = "abc123";
            var classCode = testCodes[0];
            var dbContext = new Mock<IDbContext>();
            var controller = new ClassificationCodesApiController(dbContext.Object);
            int code = 400;

            var result = controller.PutClassificationCode(id, classCode);

            var badRequestResult = Assert.IsType<BadRequestResult>(result.Result);
            Assert.Equal(code, badRequestResult.StatusCode);
        }


        [Fact]
        public void PostDocument_WhenModelStateIsInvalid()
        {
            ClassificationCode code = testCodes[0];
            code.CodeId = "D1";
            var dbContext = new Mock<IDbContext>();
            var controller = new ClassificationCodesApiController(dbContext.Object);
            controller.ModelState.AddModelError("key", "message");

            var result = controller.PutClassificationCode(code.CodeId, code);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void PostCode_Successful()
        {
            ClassificationCode code = testCodes[0];
            var dbContext = new Mock<IDbContext>();
            var controller = new ClassificationCodesApiController(dbContext.Object);
            var mockList = MockDbSet(testCodes);
            dbContext.Setup(x => x.ClassificationCodes).Returns(mockList.Object);

            var result = controller.PostClassificationCode(code);

            Assert.IsType<CreatedAtActionResult>(result.Result);
        }
        /* Helper methods and sample data */

        List<ClassificationCode> testCodes = new List<ClassificationCode>()
                {
                    new ClassificationCode
                    {
                        CodeId = "A11",
                        Classification = "AAA111"
                    },
                    new ClassificationCode
                    {
                        CodeId = "B22",
                        Classification = "BBB222"
                    },
                    new ClassificationCode
                    {
                        CodeId = "C33",
                        Classification = "CCC333"
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
