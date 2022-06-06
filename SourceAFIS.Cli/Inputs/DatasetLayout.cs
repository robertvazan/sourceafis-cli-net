// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DirectoryApi = System.IO.Directory;

namespace SourceAFIS.Cli.Inputs
{
    class DatasetLayout
    {
        public readonly string directory;
        readonly ushort[] offsets;
        readonly ushort[] fingers;
        readonly string[] names;
        readonly string[] filenames;
        readonly string[] prefixes;
        public int Fingers => offsets.Length - 1;
        public int Impressions(int finger) => offsets[finger + 1] - offsets[finger];
        public int Fingerprints => fingers.Length;
        public ushort Fingerprint(int finger, int impression)
        {
            if (impression < 0 || impression >= Impressions(finger))
                throw new ArgumentOutOfRangeException();
            return (ushort)(offsets[finger] + impression);
        }
        public ushort Finger(int fp) => fingers[fp];
        public int Impression(int fp) => fp - offsets[Finger(fp)];
        public string Name(int fp) => names[fp];
        public string Filename(int fp) => filenames[fp];
        public string Prefix(int finger) => prefixes[finger];
        static readonly Regex Pattern = new Regex(@"^(.+)_[0-9]+\.(?:tif|tiff|png|bmp|jpg|jpeg|wsq|gray)$");
        public DatasetLayout(string directory)
        {
            this.directory = directory;
            var groups = new Dictionary<string, List<string>>();
            foreach (var path in DirectoryApi.GetFiles(directory))
            {
                var filename = Path.GetFileName(path);
                var match = Pattern.Match(filename);
                if (match.Success)
                {
                    var prefix = match.Groups[1].Value;
                    if (!groups.ContainsKey(prefix))
                        groups[prefix] = new List<string>();
                    groups[prefix].Add(filename);
                }
            }
            if (groups.Count == 0)
                throw new Exception("Empty dataset.");
            if (groups.Count == 1)
                throw new Exception("Found only one finger in the dataset.");
            if (!groups.Values.Any(l => l.Count > 1))
                throw new Exception("Found only one impression per finger in the dataset.");
            prefixes = new string[groups.Count];
            names = new string[groups.Values.Sum(l => l.Count)];
            filenames = new string[names.Length];
            offsets = new ushort[prefixes.Length + 1];
            fingers = new ushort[names.Length];
            ushort finger = 0;
            int fp = 0;
            foreach (var prefix in groups.Keys.OrderBy(k => k))
            {
                prefixes[finger] = prefix;
                offsets[finger + 1] = (ushort)(offsets[finger] + groups[prefix].Count);
                foreach (var filename in groups[prefix].OrderBy(f => f))
                {
                    filenames[fp] = filename;
                    names[fp] = Path.GetFileNameWithoutExtension(filename);
                    fingers[fp] = finger;
                    ++fp;
                }
                ++finger;
            }
        }
        static readonly ConcurrentDictionary<Dataset, DatasetLayout> all = new ConcurrentDictionary<Dataset, DatasetLayout>();
        public static DatasetLayout Get(Dataset dataset) => all.GetOrAdd(dataset, ds => new DatasetLayout(Download.Unpack(dataset)));
    }
}
