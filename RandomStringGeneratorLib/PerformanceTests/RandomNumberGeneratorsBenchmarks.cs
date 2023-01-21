using BenchmarkDotNet.Attributes;
using RandomStringGeneratorLib.RandomNumberGenerators;

namespace PerformanceTests
{
    public class RandomNumberGeneratorsBenchmarks
    {

        private readonly CryptographicRandomNumberGenerator _cryptographicRandomNumberGenerator;
        private readonly PseudoRandomNumberGenerator _pseudoRandomNumberGenerator;

        public RandomNumberGeneratorsBenchmarks()
        {
            _cryptographicRandomNumberGenerator = new CryptographicRandomNumberGenerator();
            _pseudoRandomNumberGenerator = new PseudoRandomNumberGenerator();
        }

        [Benchmark(Baseline = true)]
        public int PseudoRandomGenerator()
        {
            return _pseudoRandomNumberGenerator.Next(int.MinValue, int.MaxValue);
        }

        [Benchmark]
        public int CryptoRandomGenerator()
        {
            return _cryptographicRandomNumberGenerator.Next(int.MinValue, int.MaxValue);
        }

    }
}
