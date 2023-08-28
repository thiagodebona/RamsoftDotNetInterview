namespace Dotnet.MiniJira.Tests;
using NUnit.Framework;

[TestFixture]
public class UsersTests : MockedBaseTest
{
    [Test]
    public void Create_New_Admin_User()
    {
        var userToCreate = new Domain.Models.Users.CreateUserRequest
        {
            Email = $"TestAdminTest",
            Username = "TestAdminTest",
            Name = $"TestAdminTest",
            Password = $"TestAdminTest",
            Profile = Domain.Enums.User.UserProfile.Administrator
        };

        var result = _userService.CreateUser(userToCreate, "").Result;

        Assert.IsNotNull(result, "Result should not be null");
        Assert.AreEqual(userToCreate.Name, result.Name, "Name should not be null");
        Assert.AreEqual(userToCreate.Email, result.Email, "Email should not be null");
        Assert.IsNotNull(userToCreate.Password, "Password should not be null");
        Assert.AreEqual(userToCreate.Profile, Domain.Enums.User.UserProfile.Administrator, "Name should not be null");
    }

    [Test]
    public void Create_New_Developer_User()
    {
        var userToCreate = new Domain.Models.Users.CreateUserRequest
        {
            Email = $"Developer_",
            Name = $"Developer_",
            Username = "Developer_",
            Password = $"Developer_",
            Profile = Domain.Enums.User.UserProfile.Developer
        };

        var result = _userService.CreateUser(userToCreate, "").Result;

        Assert.IsNotNull(result, "Result should not be null");
        Assert.AreEqual(userToCreate.Name, result.Name, "Name should not be null");
        Assert.AreEqual(userToCreate.Email, result.Email, "Email should not be null");
        Assert.IsNotNull(userToCreate.Password, "Password should not be null");
        Assert.AreEqual(userToCreate.Profile, Domain.Enums.User.UserProfile.Developer, "Name should not be null");
    }

    [Test]
    public void Create_New_Tester_User()
    {
        var userToCreate = new Domain.Models.Users.CreateUserRequest
        {
            Email = $"Tester",
            Name = $"Tester",
            Username = "Tester",
            Password = $"Tester",
            Profile = Domain.Enums.User.UserProfile.Tester
        };

        var result = _userService.CreateUser(userToCreate, "").Result;

        Assert.IsNotNull(result, "Result should not be null");
        Assert.AreEqual(userToCreate.Name, result.Name, "Name should not be null");
        Assert.AreEqual(userToCreate.Email, result.Email, "Email should not be null");
        Assert.IsNotNull(userToCreate.Password, "Password should not be null");
        Assert.AreEqual(userToCreate.Profile, Domain.Enums.User.UserProfile.Tester, "Name should not be null");
    }

    [Test]
    public void Authenticate_User_Refresh_Token()
    {
        var userToCreate = new Domain.Models.Users.CreateUserRequest
        {
            Email = $"TestToken",
            Name = $"TestToken",
            Username = "TestToken",
            Password = $"TestToken",
            Profile = Domain.Enums.User.UserProfile.Administrator
        };

        var result = _userService.CreateUser(userToCreate, "").Result;

        Assert.IsNotNull(result.RefreshToken, "Result should not be null");
        Assert.AreEqual(userToCreate.Name, result.Name, "Name should not be null");
    }

    [Test]
    public void Get_User_By_Id_Should_Be_Ok()
    {
        var userToCreate = new Domain.Models.Users.CreateUserRequest
        {
            Email = $"TestToken2",
            Name = $"TestToken2",
            Username = "TestToken2",
            Password = $"TestToken2",
            Profile = Domain.Enums.User.UserProfile.Administrator
        };

        var result = _userService.CreateUser(userToCreate, "").Result;

        Assert.IsNotNull(result.RefreshToken, "Result should not be null");
        Assert.AreEqual(userToCreate.Name, result.Name, "Name should not be null");

        var resultGet = _userService.GetById(result.Id).Result;
        Assert.IsNotNull(resultGet?.Id, "Result should not be null");
    }

    [Test]
    public void Create_User_That_Already_Exists_Should_Not_Be_Ok()
    {
        var userToCreate = new Domain.Models.Users.CreateUserRequest
        {
            Email = $"TestToken3",
            Name = $"TestToken3",
            Username = "TestToken3",
            Password = $"TestToken3",
            Profile = Domain.Enums.User.UserProfile.Administrator
        };
        var result = _userService.CreateUser(userToCreate, "").Result;
        Assert.IsNotNull(result?.Id);
        Assert.Throws<AggregateException>(() => _userService.CreateUser(userToCreate, "").Wait());
    }
}