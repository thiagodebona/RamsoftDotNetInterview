namespace Dotnet.MiniJira.Application;

using BCrypt.Net;
using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Helpers;
using Dotnet.MiniJira.Domain.Models.Broadcaster;
using Dotnet.MiniJira.Domain.Models.Users;
using Dotnet.MiniJira.Infrastructure;
using System.Collections.Generic;
using System.Linq;

public class UserService : IUserService
{
    private readonly IMongoBaseRepository<User> _userRepository;
    private readonly IBroadcasterService _broadcasterService;
    private readonly IJwtService _jwtUtils;

    public UserService(
        IMongoBaseRepository<User> userRepository,
        IBroadcasterService broadcasterService,
        IJwtService jwtUtils)
    {
        _userRepository = userRepository;
        _broadcasterService = broadcasterService;
        _jwtUtils = jwtUtils;
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
    {
        var user = (await _userRepository.FindBy(x => x.Username == model.Username, new CancellationToken())).FirstOrDefault();

        // validate
        if (user == null || !BCrypt.Verify(model.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid credentials, please try again");

        // authentication successful so generate jwt and refresh tokens
        var jwtToken = _jwtUtils.GenerateJwtToken(user);
        var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);

        await _broadcasterService.BroadcastEvent(new BroadCasterMessageModel
        {
            Message = $"{DateTime.Now.ToUniversalTime()} -> {user.Name}' has just connected",
        });

        return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
    }


    public async Task<AuthenticateResponse> CreateUser(CreateUserRequest model, string ipAddress)
    {
        var user = (await _userRepository.FindBy(x => (x.Username == model.Username || x.Email == model.Email), new CancellationToken())).FirstOrDefault();

        if (user == null)
        {
            await _userRepository.AddAsync(new User
            {
                Email = model.Email,
                Username = model.Username,
                Name = model.Name,
                PasswordHash = BCrypt.HashPassword(model.Password),
                Profile = model.Profile ?? Domain.Enums.User.UserProfile.Developer
            }, new CancellationToken());

            return await Authenticate(new AuthenticateRequest
            {
                Username = model.Username,
                Password = model.Password
            }, ipAddress);
        }

        throw new AppException("Username or email already registered");
    }

    public async Task<User> GetById(string id)
    {
        var user = (await _userRepository.FindBy(x => x.Id == id, new CancellationToken())).FirstOrDefault();
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }

    public async Task<List<User>> GetAll()
    {
        return await _userRepository.FindAsync(new CancellationToken());
    }
}
