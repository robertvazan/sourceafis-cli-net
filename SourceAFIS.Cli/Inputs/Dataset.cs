// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Linq;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Inputs
{
    readonly struct Dataset : DataIdentifier, IEquatable<Dataset>
    {
        public readonly DatasetCode Code;
        public Dataset(DatasetCode code)
        {
            Code = code;
        }
        public bool Equals(Dataset other) => Code == other.Code;
        public override bool Equals(object other) => other is Dataset && Equals((Dataset)other);
        public static bool operator ==(Dataset left, Dataset right) => left.Equals(right);
        public static bool operator !=(Dataset left, Dataset right) => !left.Equals(right);
        public override int GetHashCode() => Code.GetHashCode();
        public DatasetLayout Layout => DatasetLayout.Get(this);
        public static Dataset[] All => ((DatasetCode[])Enum.GetValues(typeof(DatasetCode))).Select(s => new Dataset(s)).ToArray();
        public Fingerprint[] Fingerprints
        {
            get
            {
                var self = this;
                return Enumerable.Range(0, Layout.Fingerprints).Select(n => new Fingerprint(self, n)).ToArray();
            }
        }
        public string Name
        {
            get
            {
                switch (Code)
                {
                    case DatasetCode.Fvc2000_1B:
                        return "fvc2000-1b";
                    case DatasetCode.Fvc2000_2B:
                        return "fvc2000-2b";
                    case DatasetCode.Fvc2000_3B:
                        return "fvc2000-3b";
                    case DatasetCode.Fvc2000_4B:
                        return "fvc2000-4b";
                    case DatasetCode.Fvc2002_1B:
                        return "fvc2002-1b";
                    case DatasetCode.Fvc2002_2B:
                        return "fvc2002-2b";
                    case DatasetCode.Fvc2002_3B:
                        return "fvc2002-3b";
                    case DatasetCode.Fvc2002_4B:
                        return "fvc2002-4b";
                    case DatasetCode.Fvc2004_1B:
                        return "fvc2004-1b";
                    case DatasetCode.Fvc2004_2B:
                        return "fvc2004-2b";
                    case DatasetCode.Fvc2004_3B:
                        return "fvc2004-3b";
                    case DatasetCode.Fvc2004_4B:
                        return "fvc2004-4b";
                    default:
                        throw new ArgumentException();
                }
            }
        }
        public string Path => Name;
        public double Dpi
        {
            get
            {
                switch (Code)
                {
                    case DatasetCode.Fvc2002_2B:
                        return 569;
                    case DatasetCode.Fvc2004_3B:
                        return 512;
                    default:
                        return 500;
                }
            }
        }
    }
}
