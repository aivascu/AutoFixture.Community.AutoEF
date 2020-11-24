using System;
using Nuke.Common.Tooling;

namespace Nuke.Community.GitHub
{
    public class GitHubSettings : ToolSettings
    {
        public string AuthToken { get; set; }
        public string RepositoryName { get; set; }
        public string RepositoryOwner { get; set; }
        public override Action<OutputType, string> ProcessCustomLogger { get; }
    }
}
