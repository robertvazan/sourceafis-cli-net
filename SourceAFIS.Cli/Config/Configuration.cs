// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.IO;

namespace SourceAFIS.Cli.Config
{
    static class Configuration
    {
        public static string Home = DefaultHome();
        public static string Baseline;
        public static bool BaselineMode;
        public static bool Normalized;
        public static string Output
        {
            get
            {
                if (BaselineMode)
                    return Path.Combine(Home, Baseline);
                else
                    return Path.Combine(Home, "net", FingerprintCompatibility.Version);
            }
        }

        static string DefaultHome()
        {
            // First try XDG_* variables. Data directories may be in strange locations, for example inside flatpak.
            var configured = Environment.GetEnvironmentVariable("XDG_CACHE_HOME");
            // Fall back to XDG_* default. This will perform poorly on Windows, but it will work.
            string root = configured ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".cache");
            return Path.Combine(root, "sourceafis");
        }
    }
}
