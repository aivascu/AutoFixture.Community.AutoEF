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
    private Target Coveralls => _ => _
         .DependsOn(Cover)
         .TriggeredBy(Cover)
         .Consumes(Cover)
         .Requires(() => IsServerBuild)
         .Executes(() =>
         {
             var commit = new[] { RootDirectory / ".git" }
             .SelectMany(x => Git(
                 @"log --pretty=""%H|%an|%ae|%s""",
                 workingDirectory: RootDirectory,
                 logOutput: false))
             .Select(x => x.Text)
             .ToList()
             .Select(x => x.Split('|'))
             .ForEachLazy(x => Assert(x.Length == 4, "Unexpected number of sections"))
             .Select(x => new { Hash = x[0], Name = x[1], Email = x[2], Message = x[3] })
             .First();

             CoverallsNet(_ => _
                 .SetArgumentConfigurator(x => x.Add("--lcov"))
                 .SetInput(CoverageDirectory / "lcov.info")
                 .SetDryRun(IsLocalBuild)
                 .SetCommitBranch(GitRepository.Branch)
                 .SetCommitId(commit.Hash)
                 .SetCommitAuthor(commit.Name)
                 .SetCommitEmail(commit.Email)
                 .SetCommitMessage(commit.Message));
         });
}
