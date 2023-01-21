
namespace RandomStringGeneratorTests
{
    public class RandomStringGeneratorFixture
    {
        private readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abc#$%^&*()-_+"; /* 50 chars */

        public RandomStringGeneratorFixture()
        {
            RandomStringGeneratorList = new List<IRandomStringGenerator>()
            {
                RandomStringGenerator.PseudoRandomizer,
                RandomStringGenerator.CryptographicRandomizer
            };

            StringQueue = new ConcurrentQueue<string>();

            LargeTestString = string.Concat(Enumerable.Repeat(chars, 100)); /* Test string 5000 chars long. */
        }

        internal List<IRandomStringGenerator> RandomStringGeneratorList { get; private set; }

        public ConcurrentQueue<string> StringQueue { get; private set; }

        public string LargeTestString { get; private set; } /* Test string 5000 chars long. */

        public void Dispose()
        {
            RandomStringGeneratorList.Clear();
            StringQueue.Clear();
        }
    }
}