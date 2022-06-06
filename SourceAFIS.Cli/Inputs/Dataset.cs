// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.Linq;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Inputs
{
    readonly record struct Dataset(DatasetCode Code) : DataIdentifier
    {
        public DatasetLayout Layout => DatasetLayout.Get(this);
        public static Dataset[] All => (from c in (DatasetCode[])Enum.GetValues(typeof(DatasetCode)) select new Dataset(c)).ToArray();
        public Fingerprint[] Fingerprints
        {
            get
            {
                var self = this;
                return (from n in Enumerable.Range(0, Layout.Fingerprints) select new Fingerprint(self, (ushort)n)).ToArray();
            }
        }
        public string Name => Code switch
        {
            DatasetCode.Fvc2000_1B => "fvc2000-1b",
            DatasetCode.Fvc2000_2B => "fvc2000-2b",
            DatasetCode.Fvc2000_3B => "fvc2000-3b",
            DatasetCode.Fvc2000_4B => "fvc2000-4b",
            DatasetCode.Fvc2002_1B => "fvc2002-1b",
            DatasetCode.Fvc2002_2B => "fvc2002-2b",
            DatasetCode.Fvc2002_3B => "fvc2002-3b",
            DatasetCode.Fvc2002_4B => "fvc2002-4b",
            DatasetCode.Fvc2004_1B => "fvc2004-1b",
            DatasetCode.Fvc2004_2B => "fvc2004-2b",
            DatasetCode.Fvc2004_3B => "fvc2004-3b",
            DatasetCode.Fvc2004_4B => "fvc2004-4b",
            _ => throw new ArgumentException(),
        };
        public string Path => Name;
        public double Dpi => Code switch
        {
            DatasetCode.Fvc2002_2B => 569,
            DatasetCode.Fvc2004_3B => 512,
            _ => 500,
        };
    }
}
