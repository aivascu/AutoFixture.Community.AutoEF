using System;
using Nuke.Common.Tooling;

namespace Nuke.Community.GitHub
{
    internal static class SettingsEntityExtensions
    {
        public static T Do<T>(this T settingsEntity, Action<T> action)
            where T : ISettingsEntity
        {
            action(settingsEntity);
            return settingsEntity;
        }
    }
}
