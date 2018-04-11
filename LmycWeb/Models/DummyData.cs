using LmycWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmycWeb.Models
{
    public class DummyData
    {
        public static List<Boat> GetBoats()
        {
            return new List<Boat>
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
        }

        public static List<Booking> GetBookings(ApplicationDbContext db)
        {
            List<Booking> bookings = new List<Booking>
            {
                new Booking
                {
                    BookingId = "C0001",
                    StartDateTime = new DateTime(2018,6,30,6,0,0),
                    EndDateTime = new DateTime(2018,6,30,12,0,0),
                    CreditsUsed = 36,
                    Boat = db.Boats.FirstOrDefault(b => b.Name == "Y-Knot"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m1@m.m")
                },
                new Booking
                {
                    BookingId = "C0002",
                    StartDateTime = new DateTime(2018,7,1,20,0,0),
                    EndDateTime = new DateTime(2018,7,2,6,0,0),
                    CreditsUsed = 60,
                    Boat = db.Boats.FirstOrDefault(b => b.Name == "Y-Knot"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m1@m.m")
                },
                new Booking
                {
                    BookingId = "C0003",
                    StartDateTime = new DateTime(2018,7,30,1,0,0),
                    EndDateTime = new DateTime(2018,7,30,18,0,0),
                    CreditsUsed = 102,
                    Boat = db.Boats.FirstOrDefault(b => b.Name == "Y-Knot"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m1@m.m")

                },
                new Booking
                {
                    BookingId = "C0004",
                    StartDateTime = new DateTime(2018,7,30,1,0,0),
                    EndDateTime = new DateTime(2018,7,30,18,0,0),
                    CreditsUsed = 102,
                    Boat = db.Boats.FirstOrDefault(b => b.Name == "White Swan"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m1@m.m")

                },
                new Booking
                {
                    BookingId = "C0005",
                    StartDateTime = new DateTime(2018,1,1,10,0,0),
                    EndDateTime = new DateTime(2018,1,1,13,0,0),
                    CreditsUsed = 18,
                    Boat = db.Boats.FirstOrDefault(b => b.Name == "Y-Knot"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m1@m.m")
                },
            };
            return bookings;
        }

        // TO-DO: need to report with real user in ApplicationUser
        public static List<Report> GetReports(ApplicationDbContext db)
        {
            ApplicationUser ReportUser1 = db.Users.FirstOrDefault(u => u.Email == "m1@m.m"); 

            ApplicationUser ReportUser2 = db.Users.FirstOrDefault(u => u.Email == "m2@m.m");

            ApplicationUser ReportUser3 = db.Users.FirstOrDefault(u => u.Email == "a1@a.a");

            ClassificationCode Class1 = new ClassificationCode
            {
                Classification = "Class1",
                CodeId = "1"
            };

            ClassificationCode Class2 = new ClassificationCode
            {
                Classification = "Class2",
                CodeId = "2"
            };


            List<Report> Reports = new List<Report>
            {
                new Report
                {
                    Content = "Test Report 1",
                    Hours = 3,
                    Approved = true,
                    DateCreated = DateTime.Now,
                    User = ReportUser1,
                    UserId = ReportUser1.Id,
                    Code = Class1,
                    CodeId = Class1.CodeId,
                },
                new Report
                {
                    Content = "Test Report 2",
                    Hours = 5,
                    Approved = true,
                    DateCreated = DateTime.Now,
                    User = ReportUser1,
                    UserId = ReportUser1.Id,
                    Code = Class1,
                    CodeId = Class1.CodeId
                },
                new Report
                {
                    Content = "Test Report 3",
                    Hours = 5,
                    Approved = true,
                    DateCreated = DateTime.Now,
                    User = ReportUser2,
                    UserId = ReportUser2.Id,
                    Code = Class2,
                    CodeId = Class2.CodeId,
                }
            };
            return null;
        }
    }
}