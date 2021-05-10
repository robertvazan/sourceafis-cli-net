// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using System.IO;
using SourceAFIS.Cli.Datasets;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli
{
    class TransparencyFile
    {
        public static string Extension(String mime)
        {
            switch (mime)
            {
                case "application/cbor":
                    return ".cbor";
                case "text/plain":
                    return ".txt";
                default:
                    return ".dat";
            }
        }
        class FileCollector : FingerprintTransparency
        {
            readonly string Key;
            public readonly List<byte[]> Files = new List<byte[]>();
            public FileCollector(string key) { Key = key; }
            public override bool Accepts(string key) { return Key == key; }
            public override void Take(string key, string mime, byte[] data) { Files.Add(data); }
        }
        static string ExtractorPath(string key, Fingerprint fp) { return fp.Path + Extension(TransparencyStats.ExtractorRow(fp, key).Mime); }
        public static byte[] Extractor(string key, Fingerprint fp)
        {
            return Cache.Get(Path.Combine("extractor-transparency-files", key), ExtractorPath(key, fp), () =>
            {
                using (var collector = new FileCollector(key))
                {
                    new FingerprintTemplate(fp.Decode());
                    return collector.Files[0];
                }
            });
        }
        public static void Extractor(string key)
        {
            foreach (var fp in Fingerprint.All)
                Extractor(key, fp);
        }
        public static byte[] ExtractorNormalized(string key, Fingerprint fp)
        {
            return Cache.Get(Path.Combine("normalized-extractor-transparency-files", key), ExtractorPath(key, fp), () =>
            {
                return SerializationUtils.Normalize(TransparencyStats.ExtractorRow(fp, key).Mime, Extractor(key, fp));
            });
        }
        public static void ExtractorNormalized(string key)
        {
            foreach (var fp in Fingerprint.All)
                ExtractorNormalized(key, fp);
        }
    }
}
