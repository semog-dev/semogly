namespace Semogly.Core.Domain.Shared;

public static class Configuration
{
    public static EnvironmentConfiguration Environment { get; set; } = new();
    public static SecurityConfiguration Security { get; set; } = new();
    public static DatabaseConfiguration Database { get; set; } = new();
    public static JwtConfiguration Jwt { get; set; } = new();
    public static RedisConfiguration Redis { get; set; } = new();
    public static MediatRConfiguration MediatR { get; set; } = new();
    public static MailgunConfiguration Mailgun { get; set; } = new();

    public class EnvironmentConfiguration
    {
        public string FrontBaseUrl { get; set; } = string.Empty;
    }

    public class SecurityConfiguration
    {
        public string PasswordSaltKey { get; set; } = string.Empty;
        public short SaltSize { get; set; }
        public short KeySize { get; set; }
        public int Iterations { get; set; }
        public char SplitChar { get; set; }
        public string RefreshTokenSecret { get; set; } = string.Empty;
    }

    public class DatabaseConfiguration
    {
        public string ConnectionString { get; set; } = string.Empty;
    }

    public class JwtConfiguration
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public int AccessTokenExpirationMinutes { get; set; }
    } 

    public class RedisConfiguration
    {
        public string Connection { get; set; } = string.Empty;
        public string RefreshTokenPrefix { get; set; } = string.Empty;
    }

    public class MediatRConfiguration
    {
        public string LicenseKey { get; set; } = string.Empty;
    }

    public class MailgunConfiguration
    {
        public string ApiKey { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
    }
}