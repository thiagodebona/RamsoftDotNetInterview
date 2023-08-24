using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Models.Users;

namespace Dotnet.MiniJira.Application.Interface
{
    public interface IUserService
    {
        public Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress);
        public Task<AuthenticateResponse> CreateUser(CreateUserRequest model, string ipAddress);
        public Task<User> GetById(string id);
    }
}
