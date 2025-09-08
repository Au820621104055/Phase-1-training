using FoodOrderingApp.Controllers;
using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.RestaurantRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TestAPI
{
    [TestFixture]
    public class RestaurantControllerTest
    {
        private Mock<IRestaurantRepository> _mockRepo;
        private RestaurantController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRestaurantRepository>();
            _controller = new RestaurantController(_mockRepo.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1") 
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        public async Task GetProfile_ReturnsOk_WhenProfileExists()
        {
            
            var restaurant = new Restaurant
            {
                RestaurantId = 1,
                Name = "Pizza Hut",
                CuisineType = "Italian",
                Address = "Main Street",
                PhoneNumber = "1234567890"
            };

            _mockRepo.Setup(r => r.GetProfile(1)).ReturnsAsync(restaurant);

            
            var result = await _controller.GetProfile();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var json = System.Text.Json.JsonSerializer.Serialize(okResult.Value);

            StringAssert.Contains("Pizza Hut", json);
            StringAssert.Contains("Italian", json);
        }




        [Test]
        public async Task GetMenu_ReturnsOk_WithMenuItems()
        {
            
            var restaurant = new Restaurant { RestaurantId = 1, Name = "Pizza Hut" };
            var menuItems = new List<MenuItem>
            {
                new MenuItem { MenuItemId = 1, Name = "Burger", Price = 100, IsAvailable = true }
            };

            _mockRepo.Setup(r => r.GetProfile(1)).ReturnsAsync(restaurant);
            _mockRepo.Setup(r => r.GetMenu(1)).ReturnsAsync(menuItems);

            
            var result = await _controller.GetMenu();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var items = okResult.Value as IEnumerable<MenuItem>;
            Assert.That(items, Is.Not.Null);
            Assert.That(items, Has.Exactly(1).Items);
        }
    }
}
