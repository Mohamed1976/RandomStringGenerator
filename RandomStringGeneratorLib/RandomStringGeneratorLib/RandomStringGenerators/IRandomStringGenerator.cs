
namespace RandomStringGeneratorLib.RandomStringGenerators
{
    /// <summary>
    /// Interface for generating random strings
    /// </summary>
    public interface IRandomStringGenerator
    {
        /// <summary>
        /// Returns a random generated string based on the specified options.
        /// </summary>
        /// <param name="allowedChars">Enum value that specifies the characters that are allowed 
        /// in the generated string.</param>
        /// <param name="length">The desired length of the random string to generate.</param>
        /// <param name="excludeSimilarLookingChars">A bool that specifies whether characters that look similar
        /// (such as 0 and O, or 1 and l) should be excluded from the random generated string.</param>
        /// <returns>
        /// Returns the random generated string.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <paramref name="allowedChars"/> enum has the value <see cref="AllowedChars.None"/>, unable to create a random string using an empty charset.
        /// The <paramref name="allowedChars"/> enum has the value <see cref="AllowedChars.Minus"/>, <see cref="AllowedChars.Underscore"/> or <see cref="AllowedChars.Space"/>, hence the chosen charset is too small to create a random string.
        /// The specified string <paramref name="length"/> exceeds the maximum length of 5000 characters.
        /// The specified string <paramref name="length"/> is less than one.
        /// </exception>
        public string GenerateString(AllowedChars allowedChars,
            int length, bool excludeSimilarLookingChars);

        /// <summary>
        /// Returns a random generated string based on the specified options.
        /// </summary>
        /// <param name="allowedChars">The set of characters that will be used to generate a random string.</param>
        /// <param name="length">The desired length of the random string to generate.</param>
        /// <returns>
        /// Returns the random generated string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="allowedChars"/> array is null or empty.
        /// </exception>       
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified string <paramref name="length"/> exceeds the maximum length of 5000 characters.
        /// The specified string <paramref name="length"/> is less than one.
        /// The <paramref name="allowedChars"/> array contains less than two characters, hence the charset is too small to create a random string.
        /// </exception>
        public string GenerateString(char[] allowedChars, int length);

        /// <summary>
        /// Returns a random generated string based on the specified options.
        /// </summary>
        /// <param name="minUpperCaseLetters">Integer value that is used to specify the minimum number of Upper Case characters that will be included in the random generated string.</param>
        /// <param name="minLowerCaseLetters">Integer value that is used to specify the minimum number of Lower Case characters that will be included in the random generated string.</param>
        /// <param name="minDigits">Integer value that is used to specify the minimum number of digit characters that will be included in the random generated string.</param>
        /// <param name="minSpecialChars">Integer value that is used to specify the minimum number of special characters that will be included in the random generated string.</param>
        /// <param name="excludeSimilarLookingChars">A boolean value indicating whether characters that look similar (such as 0 and O, or 1 and l) should be excluded from the random generated string. This parameter is optional and defaults to false.</param>
        /// <param name="extraLength">The length of the generated string beyond the minimum number of 
        /// characters specified by the previous parameters. If the value is zero, the string will have the 
        /// exact length specified by the previous parameters. This parameter is optional and defaults to zero.</param>
        /// <param name="extraAllowedChars">The additional allowed characters (if <paramref name="extraLength"/> is greater than zero). This parameter is optional and defaults to <see cref="AllowedChars.All"/>, which includes all possible characters.</param>
        /// <returns> 
        /// Returns the random generated string.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The overall string length exceeds the maximum length of 5000 characters.
        /// The overall string length is less than one.
        /// One of the length parameters is less than zero. All length parameter values must be equal or greater than zero.
        /// The <paramref name="extraAllowedChars"/> enum has the value <see cref="AllowedChars.None"/>, unable to create a random string using an empty charset.
        /// The <paramref name="extraAllowedChars"/> enum has the value <see cref="AllowedChars.Minus"/>, <see cref="AllowedChars.Underscore"/> or <see cref="AllowedChars.Space"/>, hence the chosen charset is too small to create a random string.
        /// </exception>
        public string GenerateString(int minUpperCaseLetters, int minLowerCaseLetters,
            int minDigits, int minSpecialChars, bool excludeSimilarLookingChars = false,
            int extraLength = 0, AllowedChars extraAllowedChars = AllowedChars.All);

        /// <summary>
        /// Returns a random shuffled version of <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The input string that will be randomly shuffled.</param>
        /// <returns>
        /// Returns the random shuffled <paramref name="source"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="source"/> is null or empty.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <paramref name="source"/> exceeds the maximum length of 5000 characters.
        /// </exception>
        public string ShuffleString(string source);
    }
}
