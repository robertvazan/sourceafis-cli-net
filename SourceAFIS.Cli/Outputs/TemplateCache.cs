// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Inputs;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli.Outputs
{
	class TemplateCache
	{
		public static byte[] Load(Fingerprint fp)
		{
			return Cache.Get("templates", fp.Path + ".cbor", () => new FingerprintTemplate(fp.Decode()).ToByteArray());
		}
		public static FingerprintTemplate Deserialize(Fingerprint fp) => new FingerprintTemplate(Load(fp));
	}
}
