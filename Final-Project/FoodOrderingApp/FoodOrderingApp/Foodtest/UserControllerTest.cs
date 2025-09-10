using FoodOrderingApp.Controllers;
using FoodOrderingApp.Context;
using FoodOrderingApp.Models;
using FoodOrderingApp.Repositories.UserRepositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FoodOrderingApp.Tests
{
    [TestFixture]
    internal class UserControllerTests
    {
        private Mock<IUserRepository> mockUserRepo;
        private UserController controller;

        [SetUp]
        public void Setup()
        {
            mockUserRepo = new Mock<IUserRepository>();
            controller = new UserController(mockUserRepo.Object, null!);
        }

        [Test]
        public async Task GetAllUsers_ReturnsListOfUsers()
        {
         
            var users = new List<User>
            {
                new User { UserId = 1, FullName = "John" },
                new User { UserId = 2, FullName = "Alice" }
            };
            mockUserRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            
            var result = await controller.GetAllUsers();
            var okResult = result.Result as OkObjectResult;

            
            Assert.That(okResult, Is.Not.Null);
            var returnedUsers = okResult.Value as IEnumerable<User>;
            Assert.That(returnedUsers.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetUserById_ValidId_ReturnsUser()
        {
         
            var user = new User { UserId = 1, FullName = "John" };
            mockUserRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

            
            var result = await controller.GetUserById(1);
            var okResult = result.Result as OkObjectResult;

            
            Assert.That(okResult, Is.Not.Null);
            var returnedUser = okResult.Value as User;
            Assert.That(returnedUser.UserId, Is.EqualTo(1));
            Assert.That(returnedUser.FullName, Is.EqualTo("John"));
        }

        [Test]
        public async Task GetUserById_InvalidId_ReturnsNotFound()
        {
         
            mockUserRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

            
            var result = await controller.GetUserById(99);

            
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task AddUser_ReturnsCreatedUser()
        {
         
            var newUser = new User { UserId = 3, FullName = "Bob" };
            mockUserRepo.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(newUser);

            
            var result = await controller.AddUser(newUser);
            var createdResult = result.Result as CreatedAtActionResult;

            
            Assert.That(createdResult, Is.Not.Null);
            var returnedUser = createdResult.Value as User;
            Assert.That(returnedUser.UserId, Is.EqualTo(3));
            Assert.That(returnedUser.FullName, Is.EqualTo("Bob"));
        }

        [Test]
        public async Task GetUserByEmail_ValidEmail_ReturnsUser()
        {
         
            var user = new User { UserId = 5, FullName = "Sam", Email = "sam@test.com" };
            mockUserRepo.Setup(r => r.GetByEmailAsync("sam@test.com")).ReturnsAsync(user);

            
            var result = await controller.GetUserByEmail("sam@test.com");
            var okResult = result.Result as OkObjectResult;

            
            Assert.That(okResult, Is.Not.Null);
            var returnedUser = okResult.Value as User;
            Assert.That(returnedUser.Email, Is.EqualTo("sam@test.com"));
        }

        [Test]
        public async Task GetUserByEmail_InvalidEmail_ReturnsNotFound()
        {
         
            mockUserRepo.Setup(r => r.GetByEmailAsync("invalid@test.com"))
                        .ReturnsAsync((User?)null);

            
            var result = await controller.GetUserByEmail("invalid@test.com");

            
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        }
    }
}
