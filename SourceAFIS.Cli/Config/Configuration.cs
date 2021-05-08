// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.IO;

namespace SourceAFIS.Cli.Config
{
    static class Configuration
    {
        public static bool Normalized;
        public static string Home = DefaultHome();
        public static string Output
        {
            get
            {
                return Path.Combine(Home, "net", FingerprintCompatibility.Version);
            }
        }

        static string DefaultHome()
        {
            // First try XDG_* variables. Data directories may be in strange locations, for example inside flatpak.
            var configured = Environment.GetEnvironmentVariable("XDG_CACHE_HOME");
            string root;
            if (configured != null)
                root = configured;
            else {
                // Fall back to XDG_* default. This will perform poorly on Windows, but it will work.
                root = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".cache");
            }
            return Path.Combine(root, "sourceafis");
        }
    }
}
