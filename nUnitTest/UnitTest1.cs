using TourPlanner.DL.DB;
using NUnit.Framework;
using TourPlanner.BL.Models;
using System.Windows.Media.Imaging;
using log4net;
using TourPlanner.BL.Services.MapQuest;
using Haley.Utils;
using System.Windows.Input;

namespace nUnitTest
{
    public class Tests 
    {
        Tour tour = new Tour();
        private TourPlannerDbContext _context = new TourPlannerDbContext();

        [SetUp]
        public void Setup()
        {
            _context = new TourPlannerDbContext();
        }
        [TearDown]
        public void Cleanup()
        {
            _context.Dispose();
        }
        

        [Test, Order(1)]
        public void AddTourIfContent()
        {
            Tour tour = new Tour(name: "Test1", tour_desc: "description", from:"testFrom", to: "testTo", distance: 9999, image_link: "www.link.net", transport_type: "testo", time:10);
            Assert.IsTrue(tour.Name == "Test1");
        }
        [Test, Order(2)]
        public void AddTour_WhenCalled_AddsTourToDatabase()
        {
            // Arrange
            var tour = new Tour(
            name: "testTour",
            time: 1,
            tour_desc: "testTour",
            from: "From 1",
            to: "To 1",
            transport_type: "Bus",
            distance: 9999,
            image_link: "https://www.odtap.com/wp-content/uploads/2019/04/Route-optimization-software-odtap.jpg");

            // Act
            _context.Tours.Add(tour);
            _context.SaveChanges();

            // Assert
            var addedTour = _context.Tours.FirstOrDefault(t => t.Name == "testTour");
            Assert.NotNull(addedTour);
        }
        [Test, Order(3)]
        public void DeleteTour_WhenCalled_DeletesTourFromDatabase()
        {
            // Arrange
            var tour = new Tour(
             name: "testDeleteTour",
             time: 2,
             tour_desc: "testTour",
             from: "From 2",
             to: "To 2",
             transport_type: "Bus",
             distance: 9999,
             image_link: "https://www.odtap.com/wp-content/uploads/2019/04/Route-optimization-software-odtap.jpg");

            // Act
            _context.Tours.Add(tour);
            _context.Tours.Remove(tour);
            _context.SaveChanges();

            // Assert
            var deletedTour = _context.Tours.FirstOrDefault(t => t.Name == "testDeleteTour");
            Assert.Null(deletedTour);
        }
        [Test, Order(4)]
        public void CreateTour_WhenCalled_CheckIfTour_IdIsSet()
        {
            Tour tour = new Tour(name: "Test1", tour_desc: "description", from: "testFrom", to: "testTo", distance: 9999, image_link: "www.link.net", transport_type: "testo", time: 10);
            Assert.IsTrue(tour.Tour_id >= 0);
        }
        [Test, Order(5)]
        public async Task RequestRoute_WhenCalled_CheckResponseIsGiven()
        {
            var startAdress = "Schumanngasse";
            var startAreacode = "1170";
            var startCity = "Vienna";
            var startCountry = "Austria";
            var destAdress = "Schottentor";
            var destAreacode = "1010";
            var destCity = "Vienna";
            var destCountry = "Austria";
            MapQuestRequestData start = new MapQuestRequestData(startAreacode, startAdress, startCity, startCountry);
            MapQuestRequestData dest = new MapQuestRequestData(destAreacode, destAdress, destCity, destCountry);

            var route = await MapQuestRequestHandler.GetRouteAsync(start, dest);

            Assert.IsNotNull(route);
        }
        [Test, Order(6)]
        public async Task RequestRoute_WhenCalled_CheckIfRouteDistanceIsGiven()
        {
            var startAdress = "Schumanngasse";
            var startAreacode = "1170";
            var startCity = "Vienna";
            var startCountry = "Austria";
            var destAdress = "Schottentor";
            var destAreacode = "1010";
            var destCity = "Vienna";
            var destCountry = "Austria";
            MapQuestRequestData start = new MapQuestRequestData(startAreacode, startAdress, startCity, startCountry);
            MapQuestRequestData dest = new MapQuestRequestData(destAreacode, destAdress, destCity, destCountry);

            var route = await MapQuestRequestHandler.GetRouteAsync(start, dest);

            Assert.IsTrue(route.route.route.distance > 0);
        }
        [Test, Order(7)]
        public async Task RequestRoute_WhenCalled_CheckIfRouteStartLocationIsGiven()
        {
            var startAddress = "Schumanngasse";
            var startAreacode = "1170";
            var startCity = "Vienna";
            var startCountry = "Austria";
            var destAddress = "Schottentor";
            var destAreacode = "1010";
            var destCity = "Vienna";
            var destCountry = "Austria";

            var start = new MapQuestRequestData(startAreacode, startAddress, startCity, startCountry);
            var dest = new MapQuestRequestData(destAreacode, destAddress, destCity, destCountry);

            var route = await MapQuestRequestHandler.GetRouteAsync(start, dest);

            Assert.IsTrue(route.route.route.locations[0].street == startAddress);
        }

        [Test, Order(8)]
        public async Task RequestRoute_WhenCalled_CheckIfRouteEndLocationIsGiven()
        {
            var startAddress = "Schumanngasse";
            var startAreacode = "1170";
            var startCity = "Vienna";
            var startCountry = "Austria";
            var destAddress = "Schottentor";
            var destAreacode = "1010";
            var destCity = "Vienna";
            var destCountry = "Austria";

            var start = new MapQuestRequestData(startAreacode, startAddress, startCity, startCountry);
            var dest = new MapQuestRequestData(destAreacode, destAddress, destCity, destCountry);

            var route = await MapQuestRequestHandler.GetRouteAsync(start, dest);

            Assert.IsNotNull(route.route.route.locations[1].street == destAddress);
        }
        [Test, Order(9)]
        public async Task UpdateTour_WhenCalled_UpdatesTourInDatabase()
        {
            // Arrange
            var tour = new Tour(
                name: "oldName",
                time: 1,
                tour_desc: "testTour",
                from: "From 1",
                to: "To 1",
                transport_type: "Bus",
                distance: 9999,
                image_link: "https://www.odtap.com/wp-content/uploads/2019/04/Route-optimization-software-odtap.jpg");

            _context.Tours.Add(tour);
            _context.SaveChanges();

            // Act
            var updatedTour = _context.Tours.FirstOrDefault(t => t.Name == "oldName");
            updatedTour.Name = "UpdatedTour";
            _context.SaveChanges();

            // Assert
            var modifiedTour = _context.Tours.FirstOrDefault(t => t.Name == "UpdatedTour");
            Assert.NotNull(modifiedTour);
        }
        [Test, Order(10)]
        public void GetTourById_WhenCalled_ReturnsTourWithMatchingId()
        {
            // Arrange
            var tour = new Tour(
                name: "testTour",
                time: 1,
                tour_desc: "testTour",
                from: "From 1",
                to: "To 1",
                transport_type: "Bus",
                distance: 9999,
                image_link: "https://www.odtap.com/wp-content/uploads/2019/04/Route-optimization-software-odtap.jpg");

            _context.Tours.Add(tour);
            _context.SaveChanges();

            // Act
            var tourId = tour.Tour_id;
            var dbTour = _context.Tours.FirstOrDefault(t => t.Tour_id == tourId);

            // Assert
            Assert.NotNull(dbTour);
            Assert.AreEqual(tourId, dbTour.Tour_id);
        }
        [Test, Order(11)]
        public void Add2Tours_WhenCalled_CheckIfGiven()
        {
            // Arrange
            var tour1 = new Tour(
                name: "specialTour1",
                time: 1,
                tour_desc: "description1",
                from: "From 1",
                to: "To 1",
                transport_type: "Bus",
                distance: 9999,
                image_link: "https://www.odtap.com/wp-content/uploads/2019/04/Route-optimization-software-odtap.jpg");

            var tour2 = new Tour(
                name: "specialTour2",
                time: 2,
                tour_desc: "description2",
                from: "From 2",
                to: "To 2",
                transport_type: "Train",
                distance: 9999,
                image_link: "https://www.odtap.com/wp-content/uploads/2019/04/Route-optimization-software-odtap.jpg");

            _context.Tours.AddRange(tour1, tour2);
            _context.SaveChanges();

            // Act
            var allTours = _context.Tours.ToList();

            // Assert
            Assert.IsTrue(allTours.Contains(tour1));
            Assert.IsTrue(allTours.Contains(tour2));
        }
        [Test, Order(12)]
        public async Task SelectToursWhereDistance9999_WhenCalled_ChecksIfNotEmpty()
        {
            var toursToDelete = _context.Tours.Where(t => t.Distance == 9999).ToList();
            Assert.IsNotEmpty(toursToDelete);
        }
        [Test, Order(13)]
        public async Task CreateTourWithDistance10000_WhenCalled_DeleteTourWhere10000()
        {
            var tour1 = new Tour(
                name: "specialTour1",
                time: 1,
                tour_desc: "description1",
                from: "From 1",
                to: "To 1",
                transport_type: "Bus",
                distance: 10000,
                image_link: "https://www.odtap.com/wp-content/uploads/2019/04/Route-optimization-software-odtap.jpg");

            _context.Tours.Add(tour1);

            var dbTour = _context.Tours.Where(t => t.Distance == 10000).ToList();
            Assert.IsNotNull(dbTour);

            _context.Tours.Remove(tour1);
            _context.SaveChanges();

            var deletedTour = _context.Tours.Where(t => t.Distance == 10000).ToList();
            Assert.IsEmpty(deletedTour);

        }
        [Test, Order(20)]
        public Task DeleteAllByTestGeneratedTours_WhenCalled_CheckIfEmpty()
        {
            var toursToDelete = _context.Tours.Where(t => t.Distance == 9999).ToList();

            _context.Tours.RemoveRange(toursToDelete);
            _context.SaveChanges();

            var remainingTours = _context.Tours.Where(t => t.Distance == 9999).ToList();
            Assert.IsEmpty(remainingTours);
        }
    }
}