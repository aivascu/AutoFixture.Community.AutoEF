using Nuke.Common.Tooling;

namespace Nuke.Community.GitHub
{
    public static class GitHubReleaseSettingsExtensions
    {
        public static GitHubReleaseSettings SetReleaseName(this GitHubReleaseSettings settings, string releaseName)
            => settings.NewInstance().Do(x => x.ReleaseName = releaseName);

        public static GitHubReleaseSettings SetReleaseBody(this GitHubReleaseSettings settings, string releaseBody)
            => settings.NewInstance().Do(x => x.ReleaseBody = releaseBody);

        public static GitHubReleaseSettings SetTag(this GitHubReleaseSettings settings, string tag)
            => settings.NewInstance().Do(x => x.Tag = tag);

        public static GitHubReleaseSettings SetDraft(this GitHubReleaseSettings settings, bool draft)
            => settings.NewInstance().Do(x => x.Draft = draft);

        public static GitHubReleaseSettings EnableDraft(this GitHubReleaseSettings settings)
            => settings.NewInstance().Do(x => x.Draft = true);

        public static GitHubReleaseSettings DisableDraft(this GitHubReleaseSettings settings)
            => settings.NewInstance().Do(x => x.Draft = false);

        public static GitHubReleaseSettings SetPrerelease(this GitHubReleaseSettings settings, bool prerelease)
            => settings.NewInstance().Do(x => x.Prerelease = prerelease);

        public static GitHubReleaseSettings EnablePrerelease(this GitHubReleaseSettings settings)
            => settings.NewInstance().Do(x => x.Prerelease = true);

        public static GitHubReleaseSettings DisablePrerelease(this GitHubReleaseSettings settings)
            => settings.NewInstance().Do(x => x.Prerelease = false);

        public static GitHubReleaseSettings SetTargetCommitish(this GitHubReleaseSettings settings, string commitish)
            => settings.NewInstance().Do(x => x.TargetCommitish = commitish);
    }
}
