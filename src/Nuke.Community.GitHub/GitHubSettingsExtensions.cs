using Nuke.Common.Tooling;

namespace Nuke.Community.GitHub
{
    public static class GitHubSettingsExtensions
    {
        public static T SetAuthToken<T>(this T settings, string token)
            where T : GitHubSettings
        {
            return settings
                .NewInstance()
                .Do(x => x.AuthToken = token);
        }

        public static T ResetAuthToken<T>(this T settings)
            where T : GitHubSettings
        {
            return settings
                .NewInstance()
                .Do(x => x.AuthToken = default);
        }
    }
}
