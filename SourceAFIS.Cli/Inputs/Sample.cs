// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;

namespace SourceAFIS.Cli.Inputs
{
    enum Sample
    {
        Fvc2000_1B,
        Fvc2000_2B,
        Fvc2000_3B,
        Fvc2000_4B,
        Fvc2002_1B,
        Fvc2002_2B,
        Fvc2002_3B,
        Fvc2002_4B,
        Fvc2004_1B,
        Fvc2004_2B,
        Fvc2004_3B,
        Fvc2004_4B,
    }

    static class Samples
    {
        public static string Name(this Sample sample)
        {
            switch (sample)
            {
                case Sample.Fvc2000_1B:
                    return "fvc2000-1b";
                case Sample.Fvc2000_2B:
                    return "fvc2000-2b";
                case Sample.Fvc2000_3B:
                    return "fvc2000-3b";
                case Sample.Fvc2000_4B:
                    return "fvc2000-4b";
                case Sample.Fvc2002_1B:
                    return "fvc2002-1b";
                case Sample.Fvc2002_2B:
                    return "fvc2002-2b";
                case Sample.Fvc2002_3B:
                    return "fvc2002-3b";
                case Sample.Fvc2002_4B:
                    return "fvc2002-4b";
                case Sample.Fvc2004_1B:
                    return "fvc2004-1b";
                case Sample.Fvc2004_2B:
                    return "fvc2004-2b";
                case Sample.Fvc2004_3B:
                    return "fvc2004-3b";
                case Sample.Fvc2004_4B:
                    return "fvc2004-4b";
                default:
                    throw new ArgumentException();
            }
        }
        public static double Dpi(this Sample sample)
        {
            switch (sample)
            {
                case Sample.Fvc2002_2B:
                    return 569;
                case Sample.Fvc2004_3B:
                    return 512;
                default:
                    return 500;
            }
        }
        public static Sample[] All() => (Sample[])Enum.GetValues(typeof(Sample));
    }
}
