
namespace RandomStringGeneratorLib.RandomNumberGenerators
{
    /// <summary>
    /// Interface for generating random numbers.
    /// </summary>
    internal interface IRandomNumberGenerator
    {
        /// <summary>
        /// Returns a positive random integer.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is greater than or equal to zero and less than <see cref="int.MaxValue"/>.
        /// </returns>
        public int Next();

        /// <summary>
        /// Returns a positive random integer that is greater than or equal to zero and less than the specified toExclusive.
        /// </summary>
        /// <param name="toExclusive">The exclusive upper bound of the random number to be generated. 
        /// ToExclusive must be greater than or equal to zero.</param>
        /// <returns>
        /// A 32-bit signed integer that is greater than or equal to zero and less than toExclusive. 
        /// The range of return values ordinarily includes zero but not toExclusive. However, 
        /// if toExclusive equals zero, toExclusive is returned.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="toExclusive" />Value is less than zero.
        /// </exception>
        public int Next(int toExclusive);

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="fromInclusive">The inclusive lower bound of the random number returned.</param>
        /// <param name="toExclusive">The exclusive upper bound of the random number returned. 
        /// ToExclusive must be greater than or equal to fromInclusive.</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to fromInclusive and less than toExclusive. 
        /// The range of return values includes fromInclusive but not toExclusive. 
        /// If fromInclusive equals toExclusive, fromInclusive is returned.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified range of random numbers is invalid, please specify a <paramref name="toExclusive" /> 
        /// value that is equal or greater than the <paramref name="fromInclusive" /> value.
        /// </exception>
        public int Next(int fromInclusive, int toExclusive);
    }
}
