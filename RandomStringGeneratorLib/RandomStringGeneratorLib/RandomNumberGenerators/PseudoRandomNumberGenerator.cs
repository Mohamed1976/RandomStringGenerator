
namespace RandomStringGeneratorLib.RandomNumberGenerators
{
    internal class PseudoRandomNumberGenerator : IRandomNumberGenerator
    {
        #region [ IRandomNumberGenerator interface ]

        public int Next()
        {
            return Random.Shared.Next();
        }

        public int Next(int toExclusive)
        {
            if(toExclusive < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(toExclusive),
                    ExceptionResources.TO_EXCLUSIVE_VALUE_MUST_BE_GREATER_OR_EQUAL_TO_ZERO);
            }

            return Random.Shared.Next(toExclusive);
        }

        public int Next(int fromInclusive, int toExclusive)
        {
            if(fromInclusive > toExclusive)
            {
                throw new ArgumentOutOfRangeException(nameof(fromInclusive),
                    ExceptionResources.FROM_INCLUSIVE_VALUE_MUST_BE_LESS_THAN_TO_EXCLUSIVE_VALUE);
            }

            return Random.Shared.Next(fromInclusive, toExclusive);
        }

        #endregion
    }
}
