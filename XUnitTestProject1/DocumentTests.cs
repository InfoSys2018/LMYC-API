using LmycWeb.APIControllers;
using LmycWeb.Interfaces;
using LmycWeb.Models;
using LmycWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XUnitTestProject1
{
    public class DocumentTests
    {
        [Fact]
        public void GetAllDocuments()
        {
            var dbContext = new Mock<IDbContext>();
            var ISContext = new Mock<IServiceProvider>();
            var mockList = MockDbSet(testDocuments);
            dbContext.Setup(c => c.Documents).Returns(mockList.Object);

            var controller = new DocumentsAPIController(dbContext.Object, ISContext.Object);
            var resultList = controller.GetDocuments().ToList();

            Assert.Equal(3, resultList.Count);
        }

        [Fact]
        public void GetDocument_WhenModelStateIsInvalid()
        {
            var dbContext = new Mock<IDbContext>();
            var ISContext = new Mock<IServiceProvider>();
            string documentId = "abc123";
            var controller = new DocumentsAPIController(dbContext.Object, ISContext.Object);
            controller.ModelState.AddModelError("key", "message");
            var result = controller.GetDocument(documentId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        ////[Fact(Skip = "Issue with SingleOrDefaultAsync call")]
        //public void GetDocumet_WhenDocumentNotFound()
        //{
        //    string documentId = "x";
        //    var dbContext = new Mock<IDbContext>();
        //    var ISContext = new Mock<IServiceProvider>();
        //    var mockList = MockDbSet(testDocuments);
        //    dbContext.Setup(c => c.Documents).Returns(mockList.Object);

        //    var controller = new DocumentsAPIController(dbContext.Object, ISContext.Object);

        //    var result = controller.GetDocument(documentId);

        //    Assert.IsType<NotFoundResult>(result.Result);
        //}

        [Fact]
        public void PutDocument_WhenModelStateIsInvalid()
        {
            var dbContext = new Mock<IDbContext>();
            var ISContext = new Mock<IServiceProvider>();
            var controller = new DocumentsAPIController(dbContext.Object, ISContext.Object);
            controller.ModelState.AddModelError("key", "message");
            var result = controller.PutDocument(null, null);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void PutDocument_WhenIdDoesNotMatchDocumentId()
        {
            string id = "abc123";
            var document = testDocuments[0];
            var dbContext = new Mock<IDbContext>();
            var ISContext = new Mock<IServiceProvider>();
            var controller = new DocumentsAPIController(dbContext.Object, ISContext.Object);
            int code = 400;

            var result = controller.PutDocument(id, document);

            var badRequestResult = Assert.IsType<BadRequestResult>(result.Result);
            Assert.Equal(code, badRequestResult.StatusCode);
        }

        //[Fact(Skip = "Incomplete: mock dbcontext.Entry()")]
        //[Fact]
        //public async Task PutBoat()
        //{
        //    string id = "B01";
        //    var boat = testBoats[0];

        //    var dbContext = new Mock<IDbContext>();
        //    //var mockList = MockDbSet(testBoats);
        //    var mockList = Helpers.ToAsyncDbSetMock(testBoats);
        //    dbContext.Setup(c => c.Boats).Returns(mockList.Object);
        //    var controller = new BoatsApiController(dbContext.Object);

        //    var result = controller.PutBoat(id, boat);
        //    Assert.IsType<NoContentResult>(result);
        //}

        [Fact]
        public void PostDocument_WhenModelStateIsInvalid()
        {
            DocumentViewModel document = new DocumentViewModel();
            var dbContext = new Mock<IDbContext>();
            var ISContext = new Mock<IServiceProvider>();
            var controller = new DocumentsAPIController(dbContext.Object, ISContext.Object);
            controller.ModelState.AddModelError("key", "message");

            var result = controller.PostDocument(document);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        //[Fact]
        //public void PostDocument_Successful()
        //{
        //    DocumentViewModel document = new DocumentViewModel();
        //    document.Content = null;
        //    var dbContext = new Mock<IDbContext>();
        //    var ISContext = new Mock<IServiceProvider>();
        //    var controller = new DocumentsAPIController(dbContext.Object, ISContext.Object);
        //    var mockList = MockDbSet(testDocuments);
        //    dbContext.Setup(x => x.Documents).Returns(mockList.Object);

        //    var result = controller.PostDocument(document);

        //    Assert.IsType<CreatedAtActionResult>(result.Result);
        //}


        /* Helper methods and sample data */

        List<Document> testDocuments = new List<Document>()
                {
                    new Document
                    {
                        DocumentId = "A11",
                        Content = null,
                        DocumentName = "FirstDoc",
                        ContentType = "type1",
                        Id = "AAA111"
                    },
                    new Document
                    {
                        DocumentId = "B22",
                        Content = null,
                        DocumentName = "SecondDoc",
                        ContentType = "type1",
                        Id = "BBB222"
                    },
                    new Document
                    {
                        DocumentId = "C33",
                        Content = null,
                        DocumentName = "ThirdDoc",
                        ContentType = "type2",
                        Id = "CCC333"
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
