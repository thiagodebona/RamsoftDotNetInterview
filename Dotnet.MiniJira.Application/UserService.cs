namespace Dotnet.MiniJira.Application;

using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Models.Users;
using Dotnet.MiniJira.Infrastructure;
using MongoDB.Entities;

public class UserService : IUserService
{
    private readonly IUserRepository<User> _userRepository;

    public UserService(IUserRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
    {
        return await _userRepository.Authenticate(model, ipAddress);
    }

    public async Task<AuthenticateResponse> CreateUser(CreateUserRequest model, string ipAddress)
    {
        using (var TN = DB.Transaction())
        {
            var createdUser = _userRepository.CreateUser(model, ipAddress);

            await TN.CommitAsync();

            return createdUser.Result;
        }
    }

    public async Task<User?> OneAsync(string userId)
    {
        return await _userRepository.OneAsync(userId);
    }
}
