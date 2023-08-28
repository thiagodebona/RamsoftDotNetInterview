using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Enums.Board;
using Dotnet.MiniJira.Domain.Models.Board;
using Dotnet.MiniJira.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Dotnet.MiniJira.Application.Seeder
{
    public class InitialDataSeeder : IInitialDataSeederService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IMongoBaseRepository<User> _userRepository;
        private readonly IMongoBaseRepository<Board> _boardRepository;
        public InitialDataSeeder(
            ILogger<UserService> logger,
            IMongoBaseRepository<User> userRepository,
            IMongoBaseRepository<Board> boardRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _boardRepository = boardRepository;

        }

        public async System.Threading.Tasks.Task SeedDatabase()
        {
            User? user = (await _userRepository.FindBy(x => x.Username == "admin", new CancellationToken())).FirstOrDefault();
            if (user == null)
            {
                /// Create the admin/developer and test user profiles and a board
                User adminToAdd = new()
                {
                    Email = "admin@admin.com",
                    Username = "admin",
                    Name = "Administrator",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                    Profile = Domain.Enums.User.UserProfile.Administrator
                };

                // Create a test user
                User testToAdd = new()
                {
                    Email = "tester@tester.com",
                    Username = "tester",
                    Name = "QA Tester",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("tester"),
                    Profile = Domain.Enums.User.UserProfile.Tester
                };

                // Create a developer profile
                User devToAdd = new()
                {
                    Email = "dev@dev.com",
                    Username = "developer",
                    Name = "Dotnet Developer",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("developer"),
                    Profile = Domain.Enums.User.UserProfile.Developer
                };

                await _userRepository.AddAsync(adminToAdd, new CancellationToken());
                await _userRepository.AddAsync(devToAdd, new CancellationToken());
                await _userRepository.AddAsync(testToAdd, new CancellationToken());

                List<string> attachItemToAdd = new() { "http://link_to_my_image/photos/cat.jpg" };
                // Board creation
                Board boardToAdd = new()
                {
                    Name = "Ramsoft - Issues board",
                    Description = "All the issues goes to this board",
                    UserCreated = adminToAdd,
                    Columns = new List<BoardColumns> {
                       new BoardColumns {
                           Name = "Todo",
                           Description = "All the todo tasks",
                           Type = ColumnType.Todo,
                           Tasks = new List<Domain.Entities.Task>() {
                                new Domain.Entities.Task {
                                    Assignee = devToAdd,
                                    DeadLine = DateTime.Now.AddDays(7),
                                    Name = "Issue on billing module",
                                    UserCreated = adminToAdd,
                                    Attachments = attachItemToAdd,
                                    Description = "The last few days I've notice that in the period of afternoon the service sometimes responds with 500 interval server error"
                                },
                                new Domain.Entities.Task {
                                    Assignee = devToAdd,
                                    DeadLine = DateTime.Now.AddDays(7),
                                    Name = "Issue on login module Error 403",
                                    UserCreated = adminToAdd,
                                    Attachments = attachItemToAdd,
                                    Description = "The last few days I've notice that in the period of morning the service sometimes responds with 403 unauthorized error"
                                }
                           }
                       },
                       new BoardColumns {
                           Name = "In progress",
                           Description = "In progress tasks",
                           Type = ColumnType.InProgress,
                           Tasks = new List<Domain.Entities.Task>() {
                                new Domain.Entities.Task {
                                    Assignee = devToAdd,
                                    DeadLine = DateTime.Now.AddDays(7),
                                    Name = "Problem when converting EURO to DOLAR",
                                    UserCreated = adminToAdd,
                                    Attachments = attachItemToAdd,
                                    Description = "The billing scheen is responding wrong currency conversions"
                                }
                           }
                       },
                       new BoardColumns {
                           Name = "Testing",
                           Description = "Testing tasks",
                           Type = ColumnType.InTest,

                           Tasks = new List<Domain.Entities.Task>() {
                                new Domain.Entities.Task {
                                    Assignee = testToAdd,
                                    DeadLine = DateTime.Now.AddDays(7),
                                    Name = "Problem when converting EURO to DOLAR",
                                    UserCreated = adminToAdd,
                                    Attachments = attachItemToAdd,
                                    Description = "The billing scheen is responding wrong currency conversions"
                                },
                                new Domain.Entities.Task {
                                    Assignee = testToAdd,
                                    DeadLine = DateTime.Now.AddDays(7),
                                    Name = "Problem when converting BRL to canadian dolar",
                                    UserCreated = testToAdd,
                                    Attachments = attachItemToAdd,
                                    Description = "Currency conversions not working as expected"
                                }
                           }
                       },
                       new BoardColumns {
                           Name = "Done",
                           Description = "Done tasks",
                           Type = ColumnType.Done,
                           Tasks = new List<Domain.Entities.Task>() {
                                new Domain.Entities.Task {
                                    Assignee = testToAdd,
                                    DeadLine = DateTime.Now.AddDays(7),
                                    Name = "Docker image not building",
                                    UserCreated = adminToAdd,
                                    Attachments = attachItemToAdd,
                                    Description = "Something is blocking the CD/CI pipelines to go ahead, need further check"
                                },
                           }
                       }
                    }
                };

                _logger.LogInformation("A new board has just been created");

                await _boardRepository.AddAsync(boardToAdd, new CancellationToken());

                _logger.LogInformation("Db successfully seeded");
            }

        }
    }
}
