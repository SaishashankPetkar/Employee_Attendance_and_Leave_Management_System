using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using ALMSystem2.Controllers;
using ALMSystem2.Models;
using System.Linq;
using System.Web.Mvc;
using System.Security.Policy;

namespace ALMSystem2.Tests
{
    [TestClass]
    public class AdminControllerTests
    {
        private AdminController _controller;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _mockHttpClient;

        [TestInitialize]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _mockHttpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new System.Uri("https://localhost:44323/api/")
            };

            _controller = new AdminController
            {
                HttpClient = _mockHttpClient // Assuming HttpClient is a property in your controller
            };

            // Mock the Session if your controller accesses it
            var mockControllerContext = new Mock<ControllerContext>();
            mockControllerContext.Setup(c => c.HttpContext.Session["User"]).Returns("admin");
            _controller.ControllerContext = mockControllerContext.Object;
        }

        // Helper method to mock HttpClient responses
        private void MockHttpClientResponse<T>(T responseObject, HttpStatusCode statusCode)
        {
            var responseMessage = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(JsonConvert.SerializeObject(responseObject))
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(responseMessage);
        }

        [TestMethod]
        public async Task AdminLogin_ValidCredentials_RedirectsToDashboard()
        {
            // Arrange
            var username = "admin";
            var password = "password";

            // Act
            var result = _controller.AdminLogin(username, password) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("AdminDashboard", result.RouteValues["action"]);
        }

        [TestMethod]
        public async Task AdminLogin_InvalidCredentials_ReturnsView()
        {
            // Arrange
            var username = "wronguser";
            var password = "wrongpassword";

            // Act
            var result = _controller.AdminLogin(username, password) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ModelState.IsValid);
        }

        [TestMethod]
        public async Task AdminLogin_ValidCredentials_SetsSession()
        {
            // Arrange
            var username = "admin";
            var password = "password";

            // Act
            _controller.AdminLogin(username, password);
            var user = _controller.Session["User"];

            // Assert
            Assert.AreEqual("admin", user);
        }

        [TestMethod]
        public async Task AdminLogin_WithNullUsername_ReturnsView()
        {
            // Arrange
            var username = (string)null; // Null username
            var password = "password";

            // Act
            var result = _controller.AdminLogin(username, password) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ModelState.IsValid);
        }

        [TestMethod]
        public void AdminLogin_NoCredentials_ReturnsView()
        {
            // Arrange
            var username = string.Empty; // No username
            var password = string.Empty; // No password

            // Act
            var result = _controller.AdminLogin(username, password) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ModelState.IsValid);
        }
    }
}
