namespace Dotnet.MiniJira.Infrastructure;

using BCrypt.Net;
using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Helpers;
using Dotnet.MiniJira.Domain.Models.Users;
using MongoDB.Entities;


public class UserRepository<T> : IUserRepository<T> where T : Entity
{
    private readonly IJwtRepository _jwtRepository;
    public UserRepository(IJwtRepository jwtRepository)
    {
        _jwtRepository = jwtRepository;
    }

    public async Task<AuthenticateResponse> CreateUser(CreateUserRequest model, string ipAddress)
    {
        var user = (await DB.Find<User>().ManyAsync(x => x.Username == model.Username || x.Email == model.Email, new CancellationToken())).FirstOrDefault();

        if (user == null)
        {
            await DB.InsertAsync(new User
            {
                Email = model.Email,
                Username = model.Username,
                Name = model.Name,
                PasswordHash = BCrypt.HashPassword(model.Password),
                Profile = model.Profile ?? Domain.Enums.User.UserProfile.Developer
            });

            return await Authenticate(new AuthenticateRequest
            {
                Username = model.Username,
                Password = model.Password
            }, ipAddress);
        }

        throw new AppException("Username or email already registered");
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
    {
        var user = (await DB.Find<User>().ManyAsync(x => x.Username == model.Username, new CancellationToken())).FirstOrDefault();

        // validate
        if (user == null)
            throw new UnauthorizedException("Invalid user");
        else if(!BCrypt.Verify(model.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid credentials, please try again");

        // authentication successful so generate jwt and refresh tokens
        var jwtToken = _jwtRepository.GenerateJwtToken(user);
        var refreshToken = _jwtRepository.GenerateRefreshToken(ipAddress);

        return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
    }

    public async Task<User?> OneAsync(string userId)
    {
        return await DB.Find<User>().OneAsync(userId);
    }
}