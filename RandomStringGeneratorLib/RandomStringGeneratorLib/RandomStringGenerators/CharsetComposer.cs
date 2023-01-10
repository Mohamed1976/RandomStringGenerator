
namespace RandomStringGeneratorLib.RandomStringGenerators
{
    internal static class CharsetComposer
    {
        #region [ Fields ]

        private static readonly char[] UpperCaseLetters =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
            'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        private static readonly char[] LowerCaseLetters =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
            'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
            'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };

        private static readonly char[] Digits =
        {
            '1','2','3','4','5','6','7','8','9','0'
        };

        private static readonly char[] SpecialReadableAsciiLetters =
        {
            '!', '"', '#', '$', '%', '&', '\'', '*',
            '+', ',', '.', '/', ':', ';', '=', '?',
            '@', '\\', '^', '´', '`', '|', '~'
        };

        private static readonly char[] Minus = { '-' };

        private static readonly char[] Underscore = { '_' };

        private static readonly char[] Space = { ' ' };

        private static readonly char[] Brackets = { '<', '>', '{', '}', '[', ']', '(', ')' };

        private static readonly char[] SimilarLookingCharacters = { '1', 'l', 'I', '|', 'o', 'O', '0' };

        #endregion

        #region [ Methods ] 

        /// <summary>
        /// Returns a character array containing the requested set of characters.
        /// </summary>
        /// <param name="allowedChars">Enum that specifies the set of characters to include in the character array that is returned.  
        /// If <see cref="AllowedChars.None"/>, then an empty character array will be returned. </param>
        /// <param name="excludeSimilarChars">When true, removes similar looking characters from the character array that is returned.</param>
        /// <returns>
        /// Character array containing the set of characters requested in the arguments <paramref name="allowedChars" /> and <paramref name="excludeSimilarChars" />.
        /// </returns>
        public static char[] GetChars(AllowedChars allowedChars, bool excludeSimilarChars)
        {
            List<char> allowedCharslist = new List<char>();

            if (allowedChars.HasFlag(AllowedChars.UpperCaseLetters))
            {
                allowedCharslist.AddRange(UpperCaseLetters);
            }

            if (allowedChars.HasFlag(AllowedChars.LowerCaseLetters))
            {
                allowedCharslist.AddRange(LowerCaseLetters);
            }

            if (allowedChars.HasFlag(AllowedChars.Digits))
            {
                allowedCharslist.AddRange(Digits);
            }

            if (allowedChars.HasFlag(AllowedChars.SpecialChars))
            {
                allowedCharslist.AddRange(SpecialReadableAsciiLetters);
            }

            if (allowedChars.HasFlag(AllowedChars.Minus))
            {
                allowedCharslist.AddRange(Minus);
            }

            if (allowedChars.HasFlag(AllowedChars.Underscore))
            {
                allowedCharslist.AddRange(Underscore);
            }

            if (allowedChars.HasFlag(AllowedChars.Space))
            {
                allowedCharslist.AddRange(Space);
            }

            if (allowedChars.HasFlag(AllowedChars.Brackets))
            {
                allowedCharslist.AddRange(Brackets);
            }

            if (excludeSimilarChars)
            {
                allowedCharslist.RemoveAll(ch => SimilarLookingCharacters.Contains(ch));
            }

            return allowedCharslist.ToArray();
        }

        #endregion
    }
}