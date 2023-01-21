
namespace RandomStringGeneratorTests
{
    public class RandomNumberGeneratorFixture : IDisposable
    {
        public RandomNumberGeneratorFixture()
        {
            RandomNumberGeneratorList = new List<IRandomNumberGenerator>()
            {
                new PseudoRandomNumberGenerator(),
                new CryptographicRandomNumberGenerator()
            };

            NumberQueue = new ConcurrentQueue<int>();
        }

        internal List<IRandomNumberGenerator> RandomNumberGeneratorList { get; private set; }

        public ConcurrentQueue<int> NumberQueue { get; private set; }

        public void Dispose()
        {
            RandomNumberGeneratorList.Clear();
            NumberQueue.Clear();
        }
    }
}
