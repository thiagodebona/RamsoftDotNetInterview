namespace Dotnet.MiniJira.Domain.Helpers;

public sealed class AppSettings
{
    public string Secret { get; set; }

    // refresh token time to live (in days), inactive tokens are
    // automatically deleted from the database after this time
    public int RefreshTokenTTL { get; set; }

    public string ServerUrl { get; set; }

    public DatabaseSettings Database { get; set; }

    public class DatabaseSettings
    {
        public string Host { get; set; }
        public int? Port { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}