using LmycWeb.APIControllers;
using LmycWeb.Interfaces;
using LmycWeb.Models;
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
    public class ReportsTests
    {
        [Theory]
        [InlineData("0001")]
        public async Task GetReportsByID(string id)
        {
            var mockContext = new Mock<IDbContext>();
            var mockList = Helpers.ToAsyncDbSetMock(testReports);
            mockContext.Setup(c => c.Reports).Returns(mockList.Object);

            var controller = new ReportsController(mockContext.Object);

            var result = await controller.GetReport(id);
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData("0001")]
        public async Task GetReportsWithInvalidModelState(string id)
        {
            var controller = new ReportsController(null);
            controller.ModelState.AddModelError("key", "error message");
            var result = await controller.GetReport(id);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PostReportSuccess()
        {
            Report report = new Report
            {
                ReportID = "0004",
                Content = "some more content",
                Hours = 5
            };

            var mockContext = new Mock<IDbContext>();
            var mockList = Helpers.ToAsyncDbSetMock(testReports);
            mockContext.Setup(c => c.Reports).Returns(mockList.Object);

            var controller = new ReportsController(mockContext.Object);
            var result = await controller.PostReport(report);
            Assert.IsType<CreatedAtActionResult>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PostReportModelStateNotValid()
        {
            Report report = new Report();
            var controller = new ReportsController(null);
            controller.ModelState.AddModelError("key", "error message");

            var result = await controller.PostReport(report);
            var requestedResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData("0002")]
        public async Task DeleteReportSuccess(string id)
        {
            var mockContext = new Mock<IDbContext>();
            var mockList = Helpers.ToAsyncDbSetMock(testReports);
            mockContext.Setup(c => c.Reports).Returns(mockList.Object);

            var controller = new ReportsController(mockContext.Object);
            var result = await controller.DeleteReport(id);
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData("0001")]
        public async Task DeleteReportModelStateInvalid(string id)
        {
            var controller = new ReportsController(null);
            controller.ModelState.AddModelError("key", "error message");
            var result = await controller.DeleteReport(id);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        /* Helper methods and sample data */
        List<Report> testReports = new List<Report>()
        {
            new Report { ReportID = "0001", Content = "blah blah blah"},
            new Report { ReportID = "0002", Content = "blah blah blah"},
            new Report { ReportID = "0003", Content = "blah blah blah"}
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
