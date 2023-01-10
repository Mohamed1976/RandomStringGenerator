using RandomStringGeneratorLib.RandomNumberGenerators;

namespace RandomStringGeneratorLib.RandomStringGenerators
{
    /// <summary>
    /// A random string generator that uses the <see cref="System.Security.Cryptography.RandomNumberGenerator"/> to create a random string.
    /// This is a slower generator but can be used for generating passwords.
    /// </summary>
    public sealed class CryptographicRandomStringGenerator : RandomStringGeneratorBase
    {
        /// <summary>
        /// Creates a new instance of the <see cref="CryptographicRandomStringGenerator"/> class.
        /// </summary>
        public CryptographicRandomStringGenerator(): base(new CryptographicRandomNumberGenerator())
        {
        }
    }
}
