namespace Nuke.Community.GitHub
{
    public class GitHubReleaseSettings : GitHubSettings
    {
        public string ReleaseName { get; set; }
        public string ReleaseBody { get; set; }
        public bool Prerelease { get; set; }
        public bool Draft { get; set; }
        public string Tag { get; set; }
        public string TargetCommitish { get; set; }
    }
}
