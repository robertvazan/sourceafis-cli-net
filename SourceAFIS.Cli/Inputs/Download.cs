// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading;
using SourceAFIS.Cli.Config;
using SourceAFIS.Cli.Utils;

namespace SourceAFIS.Cli.Inputs
{
    static class Download
    {
        static string Url(Dataset dataset) => dataset.Code switch
        {
            DatasetCode.Fvc2000_1B => "https://cdn.machinezoo.com/h/AkBMOzR_T_0_UmxZXaubrYmwmcR1yOnByJvl3AWieMI/fvc2000-1b-gray.zip",
            DatasetCode.Fvc2000_2B => "https://cdn.machinezoo.com/h/GBo_uNlW3166tHV-_QXTCWWo6YywNycOz_n4AUQhO3Y/fvc2000-2b-gray.zip",
            DatasetCode.Fvc2000_3B => "https://cdn.machinezoo.com/h/6BXcjr6ZvCr4MrAYC5yiFioYCrepCiBfg68SrR0puxo/fvc2000-3b-gray.zip",
            DatasetCode.Fvc2000_4B => "https://cdn.machinezoo.com/h/8lbaA4LGUeNFxbLbazAG-ji76_pQV3nJpCnlY__ncAc/fvc2000-4b-gray.zip",
            DatasetCode.Fvc2002_1B => "https://cdn.machinezoo.com/h/kTJNA8M9KRnrsUPYiz4Pty5V1FPzFbdnemNqRRRsu90/fvc2002-1b-gray.zip",
            DatasetCode.Fvc2002_2B => "https://cdn.machinezoo.com/h/7ghKDoqMr2C-OFwuqRWy-1rmdYNM3f-Zu-dy4g8SN6c/fvc2002-2b-gray.zip",
            DatasetCode.Fvc2002_3B => "https://cdn.machinezoo.com/h/JTyQDvcQFE-WTeOKk8QuPAalDWvVV6SgVXIH1gNKQ8s/fvc2002-3b-gray.zip",
            DatasetCode.Fvc2002_4B => "https://cdn.machinezoo.com/h/TsMV_b91QIx-cgq-FfPRH7MdE8XYJzL6ovCNJyAgYoU/fvc2002-4b-gray.zip",
            DatasetCode.Fvc2004_1B => "https://cdn.machinezoo.com/h/3z2urqUag2AQT7m0cLmT14ofkpd6TCGlGdfagbiSScU/fvc2004-1b-gray.zip",
            DatasetCode.Fvc2004_2B => "https://cdn.machinezoo.com/h/pTR8G8tQgaYRQSz3Gip8_eDLlg4G3OgvGfqDuoNOHkQ/fvc2004-2b-gray.zip",
            DatasetCode.Fvc2004_3B => "https://cdn.machinezoo.com/h/I_jWMHnQE2J7qi3YOJbh9FwU0ObiFYOdunHYKqJW8K0/fvc2004-3b-gray.zip",
            DatasetCode.Fvc2004_4B => "https://cdn.machinezoo.com/h/elY4DqdhFK8kukU9ZHmV_H8JgL2xETg1Oz74Bg1On4s/fvc2004-4b-gray.zip",
            _ => throw new ArgumentException(),
        };
        public static string Location(Dataset dataset) => Path.Combine(Configuration.Home, "inputs", "grayscale", dataset.Name);
        static int reported;
        static readonly HttpClient client = new HttpClient();
        public static string Unpack(Dataset dataset)
        {
            var directory = Location(dataset);
            if (!Directory.Exists(directory))
            {
                var url = Url(dataset);
                var temporary = Path.Combine(Path.GetDirectoryName(directory), "tmp");
                if (Directory.Exists(temporary))
                    Directory.Delete(temporary, true);
                Directory.CreateDirectory(temporary);
                if (Interlocked.Exchange(ref reported, 1) == 0)
                    Pretty.Print("Downloading sample fingerprints...");
                var download = temporary + ".zip";
                File.WriteAllBytes(download, client.GetByteArrayAsync(url).Result);
                ZipFile.ExtractToDirectory(download, temporary);
                File.Delete(download);
                Directory.Move(temporary, directory);
            }
            return directory;
        }
    }
}
