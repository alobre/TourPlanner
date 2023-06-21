using TourPlanner.DL.DB;
using NUnit.Framework;
using TourPlanner.BL.Models;
using System.Windows.Media.Imaging;

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

        [Test]
        public void AddToursCheckIfContent()
        {
            Tour tour = new Tour(name: "Test1", tour_desc: "description", from:"testFrom", to: "testTo", distance: 2, image_link: "www.link.net", transport_type: "testo", time:10);
            Assert.IsTrue(tour.Name == "Test1");
        }
        [Test]
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
            distance: 123,
            image_link: "https://www.odtap.com/wp-content/uploads/2019/04/Route-optimization-software-odtap.jpg");

            // Act
            _context.Tours.Add(tour);
            _context.SaveChanges();

            // Assert
            var addedTour = _context.Tours.FirstOrDefault(t => t.Name == "Test Tour");
            Assert.NotNull(addedTour);
        }
        [Test]
        public void DeleteTour_WhenCalled_DeletesTourFromDatabase()
        {
            // Arrange
            var tour = new Tour(
             name: "testTour",
             time: 1,
             tour_desc: "testTour",
             from: "From 1",
             to: "To 1",
             transport_type: "Bus",
             distance: 123,
             image_link: "https://www.odtap.com/wp-content/uploads/2019/04/Route-optimization-software-odtap.jpg");
            
            // Act
            _context.Tours.Remove(tour);
            _context.SaveChanges();

            // Assert
            var deletedTour = _context.Tours.FirstOrDefault(t => t.Name == "Test Tour");
            Assert.Null(deletedTour);
        }
    }
}