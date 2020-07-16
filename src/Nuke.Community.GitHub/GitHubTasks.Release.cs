using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Nuke.Common.Tooling;
using Octokit;
using Octokit.Reactive;

namespace Nuke.Community.GitHub
{
    public static partial class GitHubTasks
    {
        public static void GitHubPublishRelease(Configure<GitHubReleaseSettings> configurator) =>
            GitHubPublishRelease(configurator(new GitHubReleaseSettings()));

        public static void GitHubPublishRelease(GitHubReleaseSettings settings) =>
            settings
            .CreateHeader()
            .CreateClient()
            .Repository
            .Release
            .Create(
                settings.RepositoryOwner,
                settings.RepositoryName,
                settings.CreateRelease())
            .ObserveOn(Scheduler.Immediate)
            .Subscribe();

        private static NewRelease CreateRelease(this GitHubReleaseSettings settings) =>
            new NewRelease(settings.Tag)
            {
                Name = settings.ReleaseName,
                Body = settings.ReleaseBody,
                Draft = settings.Draft,
                Prerelease = settings.Prerelease,
                TargetCommitish = settings.TargetCommitish
            };

        private static ObservableGitHubClient CreateClient(this ProductHeaderValue header) =>
            new ObservableGitHubClient(header);

        private static ProductHeaderValue CreateHeader(this GitHubReleaseSettings settings) =>
            new ProductHeaderValue(settings.RepositoryName);
    }
}
