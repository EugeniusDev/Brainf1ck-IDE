using System.Reflection;

namespace Brainf1ck_IDE.Common
{
    public static class AppInfoHelper
    {
        public static string GetAppName()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var appName = assembly.GetName().Name;
            return appName ?? string.Empty;
        }

        public static string GetAppVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            return version is not null 
                ? version.ToString() 
                : string.Empty;
        }
    }
}
