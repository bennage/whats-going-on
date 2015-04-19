namespace WhatsGoingOn
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Octokit;

    public class CurrentStatus
    {
        public int TotalOpenIssues { get; private set; }
        public double OldestIssueAge { get; private set; }

        public CurrentStatus(IList<Tuple<Repository, IReadOnlyList<Issue>>> state)
        {
            TotalOpenIssues = state.Aggregate(0, (current, item) => current + item.Item2.Count);

            var oldestToYoungest = state
                .SelectMany(x => x.Item2)
                .OrderBy(x => x.CreatedAt);

            OldestIssueAge = (DateTime.UtcNow - oldestToYoungest.First().CreatedAt).TotalDays;
        }
    }
}