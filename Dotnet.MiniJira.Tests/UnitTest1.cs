using Dotnet.MiniJira.Application.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Dotnet.MiniJira.Tests
{
    [TestFixture]
    public class UnitTest1 : BaseMongoDbMocker
    {
        public static IUserService? _userService;
        [SetUp]
        public void SetUp()
        {
            ConfigureServicesAndStuff();
            _userService = ServiceProviderServiceExtensions.GetService<IUserService>(_serviceProvider);
        }

        [Test]
        public void IsPrime_InputIs1_ReturnFalse()
        {
            var resultad = _userService.CreateUser(new Domain.Models.Users.CreateUserRequest
            {
                Email = "Test",
                Name = "Test",
                Password = "Test",
                Profile = Domain.Enums.User.UserProfile.ADMIN
            }, "").Result;

            var haha = true;

            Assert.IsFalse(haha, "1 should not be prime");
        }
    }
}