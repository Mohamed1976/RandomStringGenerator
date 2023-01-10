using System.Security.Cryptography;

namespace RandomStringGeneratorLib.RandomNumberGenerators
{
    internal class CryptographicRandomNumberGenerator : IRandomNumberGenerator
    {
        #region [ IRandomNumberGenerator interface ]

        public int Next()
        {
            return RandomNumberGenerator.GetInt32(int.MaxValue);
        }

        public int Next(int toExclusive)
        {            
            int value = 0; /* (toExclusive == 0 || toExclusive == 1) */

            if (toExclusive < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(toExclusive),
                     ExceptionResources.TO_EXCLUSIVE_VALUE_MUST_BE_GREATER_OR_EQUAL_TO_ZERO);
            }

            if (toExclusive > 1)
            {
                value = RandomNumberGenerator.GetInt32(toExclusive);
            }

            return value;
        }

        public int Next(int fromInclusive, int toExclusive)
        {
            int value = fromInclusive;

            if (fromInclusive > toExclusive)
            {
                throw new ArgumentOutOfRangeException(nameof(fromInclusive),
                    ExceptionResources.FROM_INCLUSIVE_VALUE_MUST_BE_LESS_THAN_TO_EXCLUSIVE_VALUE);
            }

            uint range = (uint)toExclusive - (uint)fromInclusive;

            if (range > 1)
            {
                value = RandomNumberGenerator.GetInt32(fromInclusive, toExclusive);
            }

            return value;
        }

        #endregion
    }
}
