namespace WhatsGoingOn
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Nancy;
    using Octokit;
    using RepoIssues = System.Tuple<Octokit.Repository, System.Collections.Generic.IReadOnlyList<Octokit.Issue>>;

    public class IndexModule : NancyModule
    {
        private readonly GitHubClient _github;

        public IndexModule(IConfiguration config)
        {
            _github = new GitHubClient(new ProductHeaderValue("WhatsGoingOn"))
            {
                Credentials = new Credentials(config.GitHubUserName, config.GitHubPassword)
            };

            Get["/", true] = async (parameters, ct) =>
            {
                var repos = await _github.Repository.GetAllForOrg(config.GitHubOwner);
                var tasks = repos
                    .Where(r => !r.Private)
                    .Select(GetOpenIssuesFor);

                var results = await Task.WhenAll(tasks);
                var model = new CurrentStatus(results);
                return View["index", model];
            };
        }

        private async Task<RepoIssues> GetOpenIssuesFor(Repository repository)
        {
            var issues = await _github.Issue
                .GetForRepository(
                    repository.Owner.Login,
                    repository.Name,
                    new RepositoryIssueRequest { State = ItemState.Open });

            return new Tuple<Repository, IReadOnlyList<Issue>>(repository, issues);
        }
    }
}