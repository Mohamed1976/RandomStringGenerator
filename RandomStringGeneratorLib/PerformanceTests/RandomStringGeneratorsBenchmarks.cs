using BenchmarkDotNet.Attributes;
using RandomStringGeneratorLib.RandomStringGenerators;

namespace PerformanceTests
{
    public class RandomStringGeneratorsBenchmarks
    {
        IRandomStringGenerator _pseudoRandomizer;
        IRandomStringGenerator _cryptographicRandomizer;

        public RandomStringGeneratorsBenchmarks()
        {
            _pseudoRandomizer = RandomStringGenerator.PseudoRandomizer;
            _cryptographicRandomizer = RandomStringGenerator.CryptographicRandomizer;
        }

        [Benchmark(Baseline = true)]
        public string PseudoRandomShuffle()
        {
            return _pseudoRandomizer.ShuffleString("ABCDEF");
        }

        [Benchmark]
        public string CryptoRandomShuffle()
        {
            return _cryptographicRandomizer.ShuffleString("ABCDEF");
        }

        [Benchmark]
        public string PseudoRandomGenerateString()
        {
            return _pseudoRandomizer.GenerateString("ABCDEF".ToCharArray(), 6);
        }

        [Benchmark]
        public string CryptoRandomGenerateString()
        {
            return _cryptographicRandomizer.GenerateString("ABCDEF".ToCharArray(), 6);
        }

    }
}
