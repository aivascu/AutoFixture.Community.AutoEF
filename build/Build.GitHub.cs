using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.GitHub;
using Nuke.Community.GitHub;
using static Nuke.Community.GitHub.GitHubTasks;

internal partial class Build
{
    [Parameter] private readonly string GitHubToken;

    Target DraftRelease => _ => _
        .Requires(() => GitRepository.IsGitHubRepository())
        .Executes(() =>
        {
            GitHubPublishRelease(s => s
                .SetAuthToken(GitHubToken)
                .EnableDraft()
                .SetTag(GitVersion.NuGetVersionV2)
                .SetReleaseName($"Version {GitVersion.NuGetVersionV2}")
                .SetReleaseBody("# Changes ")
                .SetTargetCommitish(GitVersion.Sha)
                .When(GitRepository.IsOnDevelopBranch(),
                    _ => _.EnablePrerelease())
                .When(GitRepository.IsOnMasterBranch(),
                    _ => _.DisablePrerelease()));
        });
}
