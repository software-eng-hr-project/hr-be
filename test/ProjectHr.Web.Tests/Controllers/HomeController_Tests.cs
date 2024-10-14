using System.Threading.Tasks;
using ProjectHr.Models.TokenAuth;
using ProjectHr.Web.Controllers;
using Shouldly;
using Xunit;

namespace ProjectHr.Web.Tests.Controllers
{
    public class HomeController_Tests: ProjectHrWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}