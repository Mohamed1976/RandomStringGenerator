
namespace RandomStringGeneratorLib.RandomStringGenerators
{
    /// <summary>
    /// This class can be used to create random strings.
    /// </summary>
    public sealed class RandomStringGenerator
    {
        private static IRandomStringGenerator? _pseudoRandomStringGenerator = null;
        private static IRandomStringGenerator? _cryptographicRandomStringGenerator = null;
        private static readonly object _lockPseudoRandomizer = new object();
        private static readonly object _lockCryptographicRandomizer = new object();

        /// <summary>
        /// Gets a singleton instance that uses the <see cref="System.Random"/> class to create a random string.
        /// This is a fast generator but should not be used to generate passwords.
        /// </summary>
        public static IRandomStringGenerator PseudoRandomizer
        {
            get
            {
                lock (_lockPseudoRandomizer)
                {
                    if (_pseudoRandomStringGenerator is null)
                    {
                        _pseudoRandomStringGenerator = new PseudoRandomStringGenerator();
                    }
                    return _pseudoRandomStringGenerator;
                }
            }
        }

        /// <summary>
        /// Gets a singleton instance that uses the <see cref="System.Security.Cryptography.RandomNumberGenerator"/> to create a random string.
        /// This is a slower generator but can be used for generating passwords.
        /// </summary>
        public static IRandomStringGenerator CryptographicRandomizer
        {
            get
            {
                lock (_lockCryptographicRandomizer)
                {
                    if (_cryptographicRandomStringGenerator is null)
                    {
                        _cryptographicRandomStringGenerator = new CryptographicRandomStringGenerator();
                    }
                    return _cryptographicRandomStringGenerator;
                }
            }
        }
    }
}
