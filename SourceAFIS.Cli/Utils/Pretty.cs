// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;

namespace SourceAFIS.Cli.Utils
{
    class Pretty
    {
        public static string Hash(byte[] data) => Convert.ToBase64String(data).TrimEnd(new[] { '=' }).Replace('+', '-').Replace('/', '_');
    }
}
