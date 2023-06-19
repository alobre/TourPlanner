namespace nUnitTest
{
    public class Tests
    {
        Tour tour = new Tour();
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AddToursCheckIfContent()
        {
            Tour tour = new Tour(name: "Test1", tour_desc: "description", from:"testFrom", to: "testTo", distance: 2, image_link: "www.link.net", transport_type: "testo", time:10, route_information: new BitmapImage());
            Assert.IsTrue(tour.Name == "Test1");
        }
    }
}