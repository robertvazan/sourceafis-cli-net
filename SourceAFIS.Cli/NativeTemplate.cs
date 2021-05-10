// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
using SourceAFIS.Cli.Datasets;
using SourceAFIS.Cli.Utils.Caching;

namespace SourceAFIS.Cli
{
	class NativeTemplate
	{
		public static byte[] Serialized(Fingerprint fp)
		{
			return Cache.Get("templates", fp.Path + ".cbor", () =>
			{
				return new FingerprintTemplate(fp.Decode()).ToByteArray();
			});
		}
		public static FingerprintTemplate Of(Fingerprint fp) { return new FingerprintTemplate(Serialized(fp)); }
	}
}
