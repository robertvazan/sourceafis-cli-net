// Part of SourceAFIS CLI for .NET: https://sourceafis.machinezoo.com/cli
namespace SourceAFIS.Cli.Inputs
{
    readonly record struct CrossDatasetPair(Fingerprint Probe, Fingerprint Candidate)
    {
    }
}
