// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Collections.Generic;
using System.IO;
using SourceAFIS.Cli.Config;

namespace SourceAFIS.Cli.Utils
{
    class Pretty
    {
        public static void Print(string text)
        {
            if (!Configuration.BaselineMode)
            {
                if (text.EndsWith("\n"))
                    text = text.Substring(text.Length - 1);
                foreach (var line in text.Split('\n'))
                    Console.WriteLine(line);
            }
        }
        public static string Extension(string mime)
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
        public static string Dump(string category) => Path.Combine(Configuration.Output, category);
        static string Tag(params string[] tag)
        {
            if (tag.Length == 0)
                throw new ArgumentException();
            return String.Join("/", tag);
        }
        static readonly Dictionary<string, string> Hashes = new Dictionary<string, string>();
        public static string Hash(byte[] hash, params string[] tag)
        {
            if (tag.Length == 0)
                return Convert.ToBase64String(hash).TrimEnd(new[] { '=' }).Replace('+', '-').Replace('/', '_').Substring(0, 8);
            else if (Configuration.BaselineMode)
            {
                var formatted = Hash(hash);
                Hashes[Tag(tag)] = formatted;
                return formatted;
            }
            else
            {
                string baseline;
                var current = Hash(hash);
                if (!Hashes.TryGetValue(Tag(tag), out baseline))
                    return current;
                else if (baseline == current)
                    return current + " (=)";
                else
                    return current + " (CHANGE)";
            }
        }
        static string Percents(double value)
        {
            double scaled = 100 * value;
            double abs = Math.Abs(scaled);
            if (abs < 1)
                return $"{scaled:F3}%";
            if (abs < 10)
                return $"{scaled:F2}%";
            return $"{scaled:F1}%";
        }
        public static string Factor(double value)
        {
            if (value >= 100)
                return $"{value:F0}x";
            if (value >= 10)
                return $"{value:F1}x";
            return $"{value:F2}x";
        }
        static string Change(double value, double baseline, string more, string less)
        {
            if (value == baseline)
                return "=";
            bool positive = value >= baseline;
            var factor = positive ? value / baseline : baseline / value;
            var change = factor >= 2 ? Factor(factor) : Percents(Math.Abs(value / baseline - 1));
            if (change == Percents(0))
                return "~";
            return change + " " + (positive ? more : less);
        }
        static readonly Dictionary<string, double> measurements = new Dictionary<string, double>();
        static string Measurement(double value, string formatted, string more, string less, params string[] tag)
        {
            if (tag.Length == 0)
                return formatted;
            else if (Configuration.BaselineMode)
            {
                measurements[Tag(tag)] = value;
                return formatted;
            }
            else if (!measurements.ContainsKey(Tag(tag)))
                return formatted;
            else
                return formatted + " (" + Change(value, measurements[Tag(tag)], more, less) + ")";
        }
        public static string Percents(double value, string more, string less, params string[] tag) => Measurement(value, Percents(value), more, less, tag);
        public static string Accuracy(double value, params string[] tag) => Percents(value, "worse", "better", tag);
        static string Unit(double value, string unit)
        {
            double abs = Math.Abs(value);
            if (abs == 0)
                return string.Format("0 {0}", unit);
            if (abs < 0.000_000_1)
                return string.Format("{0:F1} n{1}", value * 1_000_000_000, unit);
            if (abs < 0.000_001)
                return string.Format("{0:F0} n{1}", value * 1_000_000_000, unit);
            if (abs < 0.000_01)
                return string.Format("{0:F2} u{1}", value * 1_000_000, unit);
            if (abs < 0.000_1)
                return string.Format("{0:F1} u{1}", value * 1_000_000, unit);
            if (abs < 0.001)
                return string.Format("{0:F0} u{1}", value * 1_000_000, unit);
            if (abs < 0.01)
                return string.Format("{0:F2} m{1}", value * 1000, unit);
            if (abs < 0.1)
                return string.Format("{0:F1} m{1}", value * 1000, unit);
            if (abs < 1)
                return string.Format("{0:F0} m{1}", value * 1000, unit);
            if (abs < 10)
                return string.Format("{0:F2} {1}", value, unit);
            if (abs < 100)
                return string.Format("{0:F1} {1}", value, unit);
            if (abs < 1000)
                return string.Format("{0:F0} {1}", value, unit);
            if (abs < 10_000)
                return string.Format("{0:F2} K{1}", value / 1000, unit);
            if (abs < 100_000)
                return string.Format("{0:F1} K{1}", value / 1000, unit);
            if (abs < 1_000_000)
                return string.Format("{0:F0} K{1}", value / 1000, unit);
            if (abs < 10_000_000)
                return string.Format("{0:F2} M{1}", value / 1_000_000, unit);
            if (abs < 100_000_000)
                return string.Format("{0:F1} M{1}", value / 1_000_000, unit);
            if (abs < 1_000_000_000)
                return string.Format("{0:F0} M{1}", value / 1_000_000, unit);
            return string.Format("{0:G} {1}", value, unit);
        }
        static string Unit(double value, string unit, string more, string less, params string[] tag) => Measurement(value, Unit(value, unit), more, less, tag);
        public static string Bytes(double value, params string[] tag) => Unit(value, "B", "larger", "smaller", tag);
        public static string Minutiae(double value, params string[] tag) => Measurement(value, value.ToString("F0"), "more", "fewer", tag);
        static readonly Dictionary<string, long> Lengths = new Dictionary<string, long>();
        public static string Length(long length, params string[] tag)
        {
            if (tag.Length == 0)
                return length.ToString("N0");
            else if (Configuration.BaselineMode)
            {
                Lengths[Tag(tag)] = length;
                return Length(length);
            }
            else if (!Lengths.ContainsKey(Tag(tag)))
                return Length(length);
            else
            {
                long baseline = Lengths[Tag(tag)];
                return Length(length) + " (" + (baseline == length ? "=" : (length - baseline).ToString("+#,#;-#,#;0")) + ")";
            }
        }
        public static string Decibans(double value, params string[] tag)
        {
            if (tag.Length == 0)
            {
                if (value < 10)
                    return $"{value:F2} dban";
                if (value < 100)
                    return $"{value:F1} dban";
                return $"{value:F0} dban";
            }
            else
                return Measurement(value, Decibans(value), "higher", "lower", tag);
        }
        public static string Speed(double value, params string[] tag) => Unit(value, "fp/s", "faster", "slower", tag);
        public static string Time(double value, params string[] tag) => Unit(value, "s", "slower", "faster", tag);
    }
}
