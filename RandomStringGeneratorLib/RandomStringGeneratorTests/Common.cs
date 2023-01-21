
namespace RandomStringGeneratorTests
{
    internal class Common
    {
        public const int maxLength = 5000;
        public const int threadPoolSize = 10;
        public const int sampleSize = 100;
        public const int minNumberOfDistinctChars = 2;
        static Common()
        {
            /* All chars */
            All = new char[UpperCaseLetters.Length +
                LowerCaseLetters.Length +
                Digits.Length +
                SpecialReadableAsciiLetters.Length +
                Minus.Length +
                Underscore.Length +
                Space.Length +
                Brackets.Length];
            Array.Copy(UpperCaseLetters, All, UpperCaseLetters.Length);
            Array.Copy(LowerCaseLetters, 0, All, UpperCaseLetters.Length, LowerCaseLetters.Length);
            Array.Copy(Digits, 0, All, UpperCaseLetters.Length + LowerCaseLetters.Length, Digits.Length);
            Array.Copy(SpecialReadableAsciiLetters, 0, All, UpperCaseLetters.Length +
                LowerCaseLetters.Length + Digits.Length, SpecialReadableAsciiLetters.Length);
            Array.Copy(Minus, 0, All, UpperCaseLetters.Length + LowerCaseLetters.Length +
                Digits.Length + SpecialReadableAsciiLetters.Length, Minus.Length);
            Array.Copy(Underscore, 0, All, UpperCaseLetters.Length + LowerCaseLetters.Length +
                Digits.Length + SpecialReadableAsciiLetters.Length + Minus.Length, Underscore.Length);
            Array.Copy(Space, 0, All, UpperCaseLetters.Length + LowerCaseLetters.Length +
                Digits.Length + SpecialReadableAsciiLetters.Length + Minus.Length + Underscore.Length, Space.Length);
            Array.Copy(Brackets, 0, All, UpperCaseLetters.Length + LowerCaseLetters.Length +
                 Digits.Length + SpecialReadableAsciiLetters.Length + Minus.Length + Underscore.Length + Space.Length,
                 Brackets.Length);

            /* UrlSafeChars */
            UrlSafeChars = new char[UpperCaseLetters.Length +
                LowerCaseLetters.Length +
                Digits.Length +
                Minus.Length +
                Underscore.Length];
            Array.Copy(UpperCaseLetters, UrlSafeChars, UpperCaseLetters.Length);
            Array.Copy(LowerCaseLetters, 0, UrlSafeChars, UpperCaseLetters.Length,
                LowerCaseLetters.Length);
            Array.Copy(Digits, 0, UrlSafeChars, UpperCaseLetters.Length +
                LowerCaseLetters.Length, Digits.Length);
            Array.Copy(Minus, 0, UrlSafeChars, UpperCaseLetters.Length +
                LowerCaseLetters.Length + Digits.Length, Minus.Length);
            Array.Copy(Underscore, 0, UrlSafeChars, UpperCaseLetters.Length +
                LowerCaseLetters.Length + Digits.Length + Minus.Length, Underscore.Length);

            /* FileSystemSafeChars */
            FileSystemSafeChars = new char[UpperCaseLetters.Length +
                LowerCaseLetters.Length +
                Digits.Length +
                Minus.Length +
                Underscore.Length];
            Array.Copy(UpperCaseLetters, FileSystemSafeChars, UpperCaseLetters.Length);
            Array.Copy(LowerCaseLetters, 0, FileSystemSafeChars, UpperCaseLetters.Length,
                LowerCaseLetters.Length);
            Array.Copy(Digits, 0, FileSystemSafeChars, UpperCaseLetters.Length +
                LowerCaseLetters.Length, Digits.Length);
            Array.Copy(Minus, 0, FileSystemSafeChars, UpperCaseLetters.Length +
                LowerCaseLetters.Length + Digits.Length, Minus.Length);
            Array.Copy(Underscore, 0, FileSystemSafeChars, UpperCaseLetters.Length +
                LowerCaseLetters.Length + Digits.Length + Minus.Length, Underscore.Length);

            /* Letters */
            Letters = new char[UpperCaseLetters.Length + LowerCaseLetters.Length];
            Array.Copy(UpperCaseLetters, Letters, UpperCaseLetters.Length);
            Array.Copy(LowerCaseLetters, 0, Letters, UpperCaseLetters.Length,
                LowerCaseLetters.Length);

            /* AlphaNumeric */
            AlphaNumeric = new char[UpperCaseLetters.Length + LowerCaseLetters.Length + Digits.Length];
            Array.Copy(UpperCaseLetters, AlphaNumeric, UpperCaseLetters.Length);
            Array.Copy(LowerCaseLetters, 0, AlphaNumeric, UpperCaseLetters.Length,
                LowerCaseLetters.Length);
            Array.Copy(Digits, 0, AlphaNumeric, UpperCaseLetters.Length + LowerCaseLetters.Length, Digits.Length);
        }

        /* Reference charsets, used for validation tests in CharsetComposerTests */
        public static readonly char[] UpperCaseLetters =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
            'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        public static readonly char[] LowerCaseLetters =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
            'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
            'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };

        public static readonly char[] Letters;

        public static readonly char[] Digits =
        {
            '1','2','3','4','5','6','7','8','9','0'
        };

        public static readonly char[] AlphaNumeric;

        public static readonly char[] SpecialReadableAsciiLetters =
        {
            '!', '"', '#', '$', '%', '&', '\'', '*',
            '+', ',', '.', '/', ':', ';', '=', '?',
            '@', '\\', '^', '´', '`', '|', '~'
        };

        public static readonly char[] Minus = { '-' };

        public static readonly char[] Underscore = { '_' };

        public static readonly char[] Space = { ' ' };

        public static readonly char[] Brackets = { '<', '>', '{', '}', '[', ']', '(', ')' };

        public static readonly char[] All;

        public static readonly char[] UrlSafeChars;

        public static readonly char[] FileSystemSafeChars;

        public static readonly char[] SimilarLookingCharacters =
        {
            '1', 'l', 'I', '|', 'o', 'O', '0'
        };
    }
}
