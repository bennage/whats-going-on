namespace WhatsGoingOn
{
    public interface IConfiguration
    {
        string GitHubUserName { get; }
        string GitHubPassword { get; }
        string GitHubOwner { get; }
    }
}