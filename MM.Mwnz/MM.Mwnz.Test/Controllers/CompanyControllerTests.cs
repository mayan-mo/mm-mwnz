using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MM.Mwnz.Controllers;
using MM.Mwnz.Services;
using Moq;
using Mwnz.Client.Services;

namespace MM.Mwnz.Test.Controllers
{
    [TestClass]
    public class CompanyControllerTests
    {
        private CompanyController _companyController;
        private Mock<ILogger<CompanyController>> _mockLogger;
        private Mock<IMwnzClientService> _mockMwnzClientService;

        [TestInitialize]
        public void Init()
        {
            _mockLogger = new Mock<ILogger<CompanyController>>();
            _mockMwnzClientService = new Mock<IMwnzClientService>();

            _companyController = new CompanyController(_mockLogger.Object, _mockMwnzClientService.Object);
        }

        [TestMethod]
        public async Task GetCompany_ShouldCall_MwnzClientGetCompany()
        {
            await _companyController.GetCompany(1);

            _mockMwnzClientService.Verify(x => x.GetCompany(1), Times.Once);
        }

        [TestMethod]
        public async Task GetCompany_ShouldReturnCompany_ReturnedFromMwnzClient()
        {
            _mockMwnzClientService.Setup(x => x.GetCompany(It.IsAny<double>()))
                .ReturnsAsync(new Company
                {
                    Id = 1,
                    Name = "test name",
                    Description = "test description"
                });

            var result = await _companyController.GetCompany(1);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var company = okObjectResult.Value as Company;
            Assert.AreEqual(1, company.Id);
            Assert.AreEqual("test name", company.Name);
            Assert.AreEqual("test description", company.Description);
        }

        [TestMethod]
        public async Task GetCompany_ShouldReturnBadRequest_WhenMwnzClientThrowsError()
        {
            _mockMwnzClientService.Setup(x => x.GetCompany(It.IsAny<double>()))
                .ThrowsAsync(new ApiException("test exception", StatusCodes.Status500InternalServerError, "test exception", null, null));

            var result = await _companyController.GetCompany(1);

            var badRequest = result as BadRequestResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }
    }
}
