using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.CoverallsNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.ControlFlow;
using static Nuke.Common.Tools.CoverallsNet.CoverallsNetTasks;
using static Nuke.Common.Tools.Git.GitTasks;

partial class Build
{
    [Parameter("Coveralls.io repo token")]
    readonly string CoverallsToken;

    Target PublishCoverage => _ => _
        .DependsOn(Cover)
        .Consumes(Cover)
        .Executes(() =>
        {
            var commit = GetLastCommit();

            CoverallsNet(_ => _
                .SetDryRun(IsLocalBuild)
                .SetRepoToken(CoverallsToken)
                .SetProcessArgumentConfigurator(x => x.Add("--lcov"))
                .SetInput(CoverageDirectory / "lcov.info")
                .SetCommitBranch(GitRepository.Branch)
                .SetCommitId(commit.Hash)
                .SetCommitAuthor(commit.Name)
                .SetCommitEmail(commit.Email)
                .SetCommitMessage(commit.Message));
        });

    CommitInfo GetLastCommit() => new[] { RootDirectory / ".git" }
        .SelectMany(x => Git(
            @"log -1 --pretty=""%H|%an|%ae|%s""",
            workingDirectory: RootDirectory,
            logOutput: false))
        .Take(1)
        .Select(x => x.Text)
        .ToList()
        .Select(x => x.Split('|'))
        .ForEachLazy(x => Assert(x.Length == 4, "Unexpected number of sections"))
        .Select(x => new CommitInfo(x[0], x[1], x[2], x[3]))
        .Single();

    class CommitInfo
    {
        public CommitInfo(string hash, string name, string email, string message)
        {
            Hash = hash;
            Name = name;
            Email = email;
            Message = message;
        }

        public string Hash { get; }
        public string Name { get; }
        public string Email { get; }
        public string Message { get; }
    }
}
