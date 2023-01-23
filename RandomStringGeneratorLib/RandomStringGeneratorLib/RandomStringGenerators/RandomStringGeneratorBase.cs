using RandomStringGeneratorLib.RandomNumberGenerators;
using System.Text;

namespace RandomStringGeneratorLib.RandomStringGenerators
{
    /// <summary>
    /// The base class for generating a random string.
    /// </summary>
    public abstract class RandomStringGeneratorBase : IRandomStringGenerator
    {
        #region [ Fields ]

        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private const int MaxLength = 5000;

        #endregion

        #region [ Constructor ]

        internal RandomStringGeneratorBase(IRandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator 
                ?? throw new ArgumentNullException(nameof(randomNumberGenerator));
        }

        #endregion

        #region [ IRandomStringGenerator interface members ]

        public virtual string GenerateString(AllowedChars allowedChars, 
            int length, bool excludeSimilarLookingChars)
        {
            char[] chars;
            string rand;

            if (length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(length),
                    ExceptionResources.LENGTH_MUST_BE_POSITIVE);
            }

            if (length > MaxLength)
            {
                throw new ArgumentOutOfRangeException(nameof(length),
                    string.Format(ExceptionResources.MAX_LENGTH_EXCEEDED, MaxLength));
            }

            if (allowedChars == AllowedChars.None)
            {
                throw new ArgumentOutOfRangeException(nameof(allowedChars),
                    ExceptionResources.CHARSET_NOT_SPECIFIED);
            }

            if (allowedChars == AllowedChars.Minus || 
                allowedChars == AllowedChars.Underscore ||
                allowedChars == AllowedChars.Space)
            {
                throw new ArgumentOutOfRangeException(nameof(allowedChars),
                    ExceptionResources.CHARSET_TOO_SMALL);
            }

            chars = CharsetComposer.GetChars(allowedChars, excludeSimilarLookingChars);
            rand = GenerateString(chars, length);

            return rand;
        }

        public virtual string GenerateString(char[] allowedChars, int length)
        {
            int rand;
            StringBuilder sb = new StringBuilder();

            if (allowedChars is null || allowedChars.Length == 0)
            {
                throw new ArgumentNullException(nameof(allowedChars),
                    ExceptionResources.CHARSET_NOT_SPECIFIED);
            }

            if (allowedChars.Length < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(allowedChars),
                    ExceptionResources.CHARSET_TOO_SMALL);
            }

            if (length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(length),
                    ExceptionResources.LENGTH_MUST_BE_POSITIVE);
            }

            if (length > MaxLength)
            {
                throw new ArgumentOutOfRangeException(nameof(length),
                    string.Format(ExceptionResources.MAX_LENGTH_EXCEEDED, MaxLength));
            }

            for (int i = 0; i < length; i++)
            {
                rand = _randomNumberGenerator.Next(allowedChars.Length);
                sb.Append(allowedChars[rand]);
            }

            return sb.ToString();
        }

        public virtual string GenerateString(int minUpperCaseLetters, int minLowerCaseLetters, 
            int minDigits, int minSpecialChars, bool excludeSimilarLookingChars = false,
            int extraLength = 0, AllowedChars extraAllowedChars = AllowedChars.All)
        {
            string rand = string.Empty;
            int totalLength;
            char[] chars;
            
            totalLength = minUpperCaseLetters + minLowerCaseLetters + minDigits + 
                minSpecialChars + extraLength;

            if(extraLength < 0 || minUpperCaseLetters < 0 ||
                minLowerCaseLetters < 0 || minDigits < 0 || minSpecialChars < 0)
            {
                throw new ArgumentOutOfRangeException("Individual length parameter value.",
                    ExceptionResources.LENGTH_MUST_BE_GREATER_OR_EQUAL_TO_ZERO);
            }

            if(totalLength < 1)
            {
                throw new ArgumentOutOfRangeException("Overall length of random string requested.",
                    ExceptionResources.LENGTH_MUST_BE_POSITIVE);
            }

            if (totalLength > MaxLength)
            {
                throw new ArgumentOutOfRangeException("Overall length of random string requested.",
                    string.Format(ExceptionResources.MAX_LENGTH_EXCEEDED, MaxLength));
            }

            if (extraLength > 0 && extraAllowedChars == AllowedChars.None)
            {
                throw new ArgumentOutOfRangeException(nameof(extraAllowedChars),
                    ExceptionResources.CHARSET_NOT_SPECIFIED);          
            }

            if (totalLength == extraLength &&
                (extraAllowedChars == AllowedChars.Minus ||
                extraAllowedChars == AllowedChars.Underscore ||
                extraAllowedChars == AllowedChars.Space))
            {
                throw new ArgumentOutOfRangeException(nameof(extraAllowedChars),
                    ExceptionResources.CHARSET_TOO_SMALL);
            }

            if (minUpperCaseLetters > 0)
            {
                chars = CharsetComposer.GetChars(AllowedChars.UpperCaseLetters, excludeSimilarLookingChars);
                rand += GenerateString(chars, minUpperCaseLetters);
            }

            if (minLowerCaseLetters > 0)
            {
                chars = CharsetComposer.GetChars(AllowedChars.LowerCaseLetters, excludeSimilarLookingChars);
                rand += GenerateString(chars, minLowerCaseLetters);
            }

            if (minDigits > 0)
            {
                chars = CharsetComposer.GetChars(AllowedChars.Digits, excludeSimilarLookingChars);
                rand += GenerateString(chars, minDigits);
            }

            if (minSpecialChars > 0)
            {
                chars = CharsetComposer.GetChars(AllowedChars.SpecialChars, excludeSimilarLookingChars);
                rand += GenerateString(chars, minSpecialChars);
            }

            if (extraLength > 0)
            {
                chars = CharsetComposer.GetChars(extraAllowedChars, excludeSimilarLookingChars);
                rand += GenerateString(chars, extraLength);
            }

            /* Randomly shuffle chars in string. */
            rand = ShuffleString(rand);

            return rand;
        }

        public virtual string ShuffleString(string source)
        {
            if(string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.Length > MaxLength)
            {
                throw new ArgumentOutOfRangeException(nameof(source),
                    string.Format(ExceptionResources.MAX_LENGTH_EXCEEDED, MaxLength));
            }

            /* Copies the chars in this instance to a Unicode char array. */
            char[] output = source.ToCharArray();

            for (int i = 0; i < output.Length; i++)
            {
                int j = _randomNumberGenerator.Next(i, output.Length);
                (output[i], output[j]) = (output[j], output[i]);
            }

            return new string(output);
        }

        #endregion
    }
}
