using LmycWeb.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace LmycWeb.Models
{
    public class DummyData
    {
        public static List<Boat> GetBoats()
        {
            string path = Directory.GetCurrentDirectory();

            return new List<Boat>
                {
                    new Boat
                    {
                        BoatId = "B01",
                        Name = "Sharquis",
                        CreditsPerHour = 6,
                        Status = "Out-of Service",
                        Photo = File.ReadAllBytes(path + "\\images\\sharquis.jpg"),
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
                        Photo = File.ReadAllBytes(path + "\\images\\pegasus.jpg"),
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
                        Photo = File.ReadAllBytes(path + "\\images\\lightcure.jpg"),
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
                        Photo = File.ReadAllBytes(path + "\\images\\frankie.jpg"),
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
                        Photo = File.ReadAllBytes(path + "\\images\\whiteswan.jpg"),
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
                        Photo = File.ReadAllBytes(path + "\\images\\peaktime.jpg"),
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
                        Photo = File.ReadAllBytes(path + "\\images\\yknot.jpg"),
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
                    User = db.Users.FirstOrDefault(u => u.Email == "m1@m.m"),
                    Itinerary = "Day 1: Sail"

                },
                new Booking
                {
                    BookingId = "C0002",
                    StartDateTime = new DateTime(2018,7,1,20,0,0),
                    EndDateTime = new DateTime(2018,7,2,6,0,0),
                    CreditsUsed = 60,
                    Boat = db.Boats.FirstOrDefault(b => b.Name == "Y-Knot"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m1@m.m"),
                    Itinerary = "Day 2: Sail"
                },
                new Booking
                {
                    BookingId = "C0003",
                    StartDateTime = new DateTime(2018,7,30,1,0,0),
                    EndDateTime = new DateTime(2018,7,30,18,0,0),
                    CreditsUsed = 102,
                    Boat = db.Boats.FirstOrDefault(b => b.Name == "Y-Knot"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m1@m.m"),
                    Itinerary = "Day 3: Sail"

                },
                new Booking
                {
                    BookingId = "C0004",
                    StartDateTime = new DateTime(2018,7,30,1,0,0),
                    EndDateTime = new DateTime(2018,7,30,18,0,0),
                    CreditsUsed = 102,
                    Boat = db.Boats.FirstOrDefault(b => b.Name == "White Swan"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m1@m.m"),
                    Itinerary = "Day 4: Sail"

                },
                new Booking
                {
                    BookingId = "C0005",
                    StartDateTime = new DateTime(2018,1,1,10,0,0),
                    EndDateTime = new DateTime(2018,1,1,13,0,0),
                    CreditsUsed = 18,
                    Boat = db.Boats.FirstOrDefault(b => b.Name == "Y-Knot"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m1@m.m"),
                    Itinerary = "Day 5: Sail"
                },
            };
            return bookings;
        }

        public static List<Member> GetMembers(ApplicationDbContext db)
        {
            List<Member> MemberList = new List<Member>
            {
                new Member
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0001"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m1@m.m"),
                    AllocatedCredits = 18,
                },

                new Member
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0001"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m2@m.m"),
                    AllocatedCredits = 18,
                },

                new Member
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0002"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m1@m.m"),
                    AllocatedCredits = 30,
                },

                new Member
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0002"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m2@m.m"),
                    AllocatedCredits = 30,
                },
                new Member
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0003"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m1@m.m"),
                    AllocatedCredits = 51,
                },

                new Member
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0003"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m2@m.m"),
                    AllocatedCredits = 51,
                },
                new Member
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0004"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m1@m.m"),
                    AllocatedCredits = 51,
                },

                new Member
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0004"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m2@m.m"),
                    AllocatedCredits = 51,
                },
                new Member
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0005"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m1@m.m"),
                    AllocatedCredits = 9,
                },

                new Member
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0005"),
                    User = db.Users.FirstOrDefault(u => u.Email == "m2@m.m"),
                    AllocatedCredits = 9,
                }
            };
            return MemberList;
        }

        public static List<NonMember> GetNonMembers(ApplicationDbContext db)
        {
            List<NonMember> NonMemberList = new List<NonMember>
            {
                new NonMember
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0001"),
                    Name = "David Spade"
                },

                new NonMember
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0001"),
                    Name = "Adam Sandler"
                },
                new NonMember
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0002"),
                    Name = "Kevin James"
                },

                new NonMember
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0002"),
                    Name = "Rob Schneider"
                },
                new NonMember
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0003"),
                    Name = "Kevin Hart"
                },

                new NonMember
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0003"),
                    Name = "Marcus Naslund"
                },
                new NonMember
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0004"),
                    Name = "Brendan Morrison"
                },

                new NonMember
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0004"),
                    Name = "Todd Bertuzzi"
                },
                new NonMember
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0005"),
                    Name = "Dan Cloutier"
                },

                new NonMember
                {
                    Booking = db.Bookings.FirstOrDefault(b => b.BookingId == "C0005"),
                    Name = "Jarko Ruutu"
                },
            };
            return NonMemberList;
        }

        public static List<ClassificationCode> GetClassificationCodes(ApplicationDbContext db)
        {
            List<ClassificationCode> ClassificationCodeList = new List<ClassificationCode>
            {
                new ClassificationCode
                {
                    CodeId = "Boat Main - Hard",
                    Classification = "Boat Maint - Hard"
                },

                new ClassificationCode
                {
                    CodeId = "Boat Maint - Monthly",
                    Classification = "Boat Maint - Monthly"
                },

                new ClassificationCode
                {
                    CodeId = "Training - Cruiser Skipper",
                    Classification = "Training - Cruise Skipper"
                },

                new ClassificationCode
                {
                    CodeId = "Training - Day Skipper",
                    Classification = "Training - Day Skipper"
                },

                new ClassificationCode
                {
                    CodeId = "Executive",
                    Classification = "Executive"
                },

                new ClassificationCode
                {
                    CodeId = "Winter Watch",
                    Classification = "Winter Watch"
                }
            };

            return ClassificationCodeList;
        }

        // TO-DO: need to report with real user in ApplicationUser
        public static List<Report> GetReports(ApplicationDbContext db)
        {
            ApplicationUser ReportUser1 = db.Users.FirstOrDefault(u => u.Email == "m1@m.m"); 

            ApplicationUser ReportUser2 = db.Users.FirstOrDefault(u => u.Email == "m2@m.m");

            ApplicationUser ReportUser3 = db.Users.FirstOrDefault(u => u.Email == "a1@a.a");

            ClassificationCode ClassCode1 = db.ClassificationCodes.FirstOrDefault(c => c.Classification == "Winter Watch");

            ClassificationCode ClassCode2 = db.ClassificationCodes.FirstOrDefault(c => c.Classification == "Executive");

            ClassificationCode ClassCode3 = db.ClassificationCodes.FirstOrDefault(c => c.Classification == "Training - Cruise Skipper");

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
                    Code = ClassCode1,
                    CodeId = ClassCode1.CodeId,
                },
                new Report
                {
                    Content = "Test Report 2",
                    Hours = 5,
                    Approved = true,
                    DateCreated = DateTime.Now,
                    User = ReportUser1,
                    UserId = ReportUser1.Id,
                    Code = ClassCode2,
                    CodeId = ClassCode2.CodeId
                },
                new Report
                {
                    Content = "Test Report 3",
                    Hours = 5,
                    Approved = true,
                    DateCreated = DateTime.Now,
                    User = ReportUser2,
                    UserId = ReportUser2.Id,
                    Code = ClassCode3,
                    CodeId = ClassCode3.CodeId,
                },
                new Report
                {
                    Content = "Test Report 4",
                    Hours = 5,
                    Approved = false,
                    DateCreated = DateTime.Now,
                    User = ReportUser1,
                    UserId = ReportUser1.Id,
                    Code = ClassCode1,
                    CodeId = ClassCode1.CodeId,
                },
                new Report
                {
                    Content = "Test Report 5",
                    Hours = 2,
                    Approved = false,
                    DateCreated = DateTime.Now,
                    User = ReportUser1,
                    UserId = ReportUser1.Id,
                    Code = ClassCode1,
                    CodeId = ClassCode1.CodeId,
                }
            };
            return Reports;
        }
    }
}