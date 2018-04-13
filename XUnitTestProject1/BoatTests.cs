using LmycWeb.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using LmycWeb.APIControllers;
using LmycWeb.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XUnitTestProject1
{
    public class BoatTests
    {
        [Fact]
        public void GetAllBoats()
        {
            var dbContext = new Mock<IDbContext>();
            var mockList = MockDbSet(testBoats);
            dbContext.Setup(c => c.Boats).Returns(mockList.Object);

            var controller = new BoatsApiController(dbContext.Object);
            var resultList = controller.GetBoats().ToList();

            Assert.Equal(7, resultList.Count);
        }

        [Fact]
        public void GetProject_WhenModelStateIsInvalid()
        {
            string boatId = "abc123";
            var controller = new BoatsApiController(null);
            controller.ModelState.AddModelError("key", "message");
            var result = controller.GetBoat(boatId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        //[Fact]
        //public void PutProject_WhenModelStateIsInvalid()
        //{
        //    var controller = new BoatsApiController(null);
        //    controller.ModelState.AddModelError("key", "message");
        //    var result = controller.PutBoat(null, null);
        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        //    Assert.IsType<SerializableError>(badRequestResult.Value);
        //}

        //[Fact]
        //public void PutBoat_WhenIdDoesNotMatchProjectNumber()
        //{
        //    string id = "abc123";
        //    var Boat = testBoats[0];
        //    var controller = new BoatsApiController(null);
        //    int code = 400;

        //    var result = controller.PutBoat(id, Boat);

        //    var badRequestResult = Assert.IsType<BadRequestResult>(result.Result);
        //    Assert.Equal(code, badRequestResult.StatusCode);
        //}

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

        //[Fact]
        //public void PostBoat_WhenModelStateIsInvalid()
        //{
        //    Boat project = testBoats[0];
        //    var controller = new BoatsApiController(null);
        //    controller.ModelState.AddModelError("key", "message");

        //    var result = controller.PostBoat(project);

        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        //    Assert.IsType<SerializableError>(badRequestResult.Value);
        //}

        //[Fact]
        //public void PostBoat_Successful()
        //{
        //    Boat project = testBoats[0];
        //    var dbContext = new Mock<IDbContext>();
        //    var mockList = MockDbSet(testBoats);
        //    dbContext.Setup(x => x.Boats).Returns(mockList.Object);
        //    var controller = new BoatsApiController(dbContext.Object);

        //    var result = controller.PostBoat(project);

        //    Assert.IsType<CreatedAtActionResult>(result.Result);
        //}


        /* Helper methods and sample data */

        List<Boat> testBoats = new List<Boat>()
                {
                    new Boat
                    {
                        BoatId = "B01",
                        Name = "Sharquis",
                        CreditsPerHour = 6,
                        Status = "Out-of Service",
                        Photo = null,
                        Description = "Sharqui was added to the fleet in 2016.  Another of the very popular C&C designs for style, comfort, and speed. Sharqui sleeps five comfortably, has an aftermarket outboard motor, and sports a very generous dodger for protection on heavy weather days.",
                        Length = 27,
                        Make = "C&C",
                        Year = 1981
                    },
                    new Boat
                    {
                        BoatId = "B02",
                        Name = "Pegasus",
                        CreditsPerHour = 6,
                        Status = "Out-of Service",
                        Photo = null,
                        Description = "Pegasus will be oufitted for travelling to Desolation Sound for the first time this summer. Members are looking forward to a roomier more comfortable boat with generous side decks.",
                        Length = 27,
                        Make = "C&C",
                        Year = 1979
                    },
                    new Boat
                    {
                        BoatId = "B03",
                        Name = "Lightcure",
                        CreditsPerHour = 6,
                        Status = "Out-of Service",
                        Photo = null,
                        Description = "She is one of our most popular boats, being a good sailor and comfortable while cruising.\nShe sleeps 5 adults comfortably. She was refitted in 2005 and is powered by a remote controlled Yamaha outboard.\nLightcure has a BBQ, cockpit table, asymmetrical spinnaker and all the extras to be comfortable for cruising.She is also rigged for use in local sailboat races.",
                        Length = 27,
                        Make = "C&C Mark 3",
                        Year = 1979
                    },
                    new Boat
                    {
                        BoatId = "B04",
                        Name = "Frankie",
                        CreditsPerHour = 6,
                        Status = "Out-of Service",
                        Photo = null,
                        Description = "She is designated as a “day sailor”, and is available for use in Semiahmoo Bay.\nShe is outfitted with some of the amenities for cruising and may be used occasionally for overnight trips.\nShe might sleep 4 adults comfortably.Frankie has a spray dodger and is powered by a Yamaha outboard.",
                        Length = 25,
                        Make = "Cal Mark 2",
                        Year = 1983
                    },
                    new Boat
                    {
                        BoatId = "B05",
                        Name = "White Swan",
                        CreditsPerHour = 6,
                        Status = "Out-of Service",
                        Photo = null,
                        Description = "She is a cruising boat, with a spray dodger, inboard diesel engine and enclosed head.\nWhite Swan is popular for longer trips to the local islands.She sleeps 4 adults very comfortably with a private aft cabin and V-berth.",
                        Length = 28,
                        Make = "C&C Mark 2",
                        Year = 1983
                    },
                    new Boat
                    {
                        BoatId = "B06",
                        Name = "Peak Time",
                        CreditsPerHour = 6,
                        Status = "Out-of Service",
                        Photo = null,
                        Description = "She has a spray dodger, BBQ and a comfortable cockpit.\nShe has all the amenities and can be used as a cruiser or day sailing boat.\nShe can sleep 4 adults. Peak Time is powered by a Yamaha outboard engine.\nShe is also rigged for use in local sailboat races.",
                        Length = 27,
                        Make = "C&C Mark 5",
                        Year = 1985
                    },
                    new Boat
                    {
                        BoatId = "B07",
                        Name = "Y-Knot",
                        CreditsPerHour = 6,
                        Status = "Out-of Service",
                        Photo = null,
                        Description = "A spacious fast cruiser.\nShe has a comfortable cockpit, spray dodger.\nShe has all the amenities of a cruiser.\nLarge aft head/shower.\nShe can sleep up to 6 adults in comfort.\nPowered by Yanmar diesel.\nStable wing keel design.\nOpen transom with swim grid,BBQ for sailing adventures.",
                        Length = 30,
                        Make = "Cruiser",
                        Year = 1985
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
