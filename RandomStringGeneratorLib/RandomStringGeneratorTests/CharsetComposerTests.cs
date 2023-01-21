
namespace RandomStringGeneratorTests
{
    public class CharsetComposerTests
    {
        #region [ Function char[] GetChars(AllowedChars allowedChars, bool excludeSimilarChars) Tests ]

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetChars_NoneRequest_ReturnsEmptyCharArray(bool excludeSimilarChars)
        {
            char[] charset;

            charset = CharsetComposer.GetChars(AllowedChars.None, excludeSimilarChars);
            Assert.Empty(charset);
        }

        [Theory]
        [InlineData(AllowedChars.UpperCaseLetters, false, 26)]
        [InlineData(AllowedChars.UpperCaseLetters, true, 24)]
        [InlineData(AllowedChars.LowerCaseLetters, false, 26)]
        [InlineData(AllowedChars.LowerCaseLetters, true, 24)]
        [InlineData(AllowedChars.Letters, false, 52)]
        [InlineData(AllowedChars.Letters, true, 48)]
        [InlineData(AllowedChars.Digits, false, 10)]
        [InlineData(AllowedChars.Digits, true, 8)]
        [InlineData(AllowedChars.AlphaNumeric, false, 62)]
        [InlineData(AllowedChars.AlphaNumeric, true, 56)]
        [InlineData(AllowedChars.SpecialChars, false, 23)]
        [InlineData(AllowedChars.SpecialChars, true, 22)]
        [InlineData(AllowedChars.Minus, false, 1)]
        [InlineData(AllowedChars.Minus, true, 1)]
        [InlineData(AllowedChars.Underscore, false, 1)]
        [InlineData(AllowedChars.Underscore, true, 1)]
        [InlineData(AllowedChars.Space, false, 1)]
        [InlineData(AllowedChars.Space, true, 1)]
        [InlineData(AllowedChars.Brackets, false, 8)]
        [InlineData(AllowedChars.Brackets, true, 8)]
        [InlineData(AllowedChars.All, false, 96)]
        [InlineData(AllowedChars.All, true, 89)]
        /* Note that FileSystemSafeChars has the same char composition as UrlSafeChars. */
        [InlineData(AllowedChars.UrlSafeChars, false, 64)]
        [InlineData(AllowedChars.UrlSafeChars, true, 58)]
        public void GetChars_RequestSpecificCharset_ReturnsCharArrayWithCorrectLength(AllowedChars allowedChars,
            bool excludeSimilarChars, int expectedLength)
        {
            char[] charset;

            charset = CharsetComposer.GetChars(allowedChars, excludeSimilarChars);
            Assert.Equal(expectedLength, charset.Length);
        }

        [Theory]
        [ClassData(typeof(GetCharsTestData))]
        public void GetChars_RequestSpecificCharsetIncludingSimilarChars_ReturnsCorrectCharArray(
            AllowedChars allowedChars, char[] expectedCharset)
        {
            bool isValid;

            char[] charset = CharsetComposer.GetChars(allowedChars, false);
            Assert.Equal(expectedCharset.Length, charset.Length);
            Assert.Equal(expectedCharset.Distinct().Count(), charset.Distinct().Count());
            isValid = expectedCharset.All((ch) => charset.Contains(ch));
            Assert.True(isValid);
        }

        [Theory]
        [ClassData(typeof(GetCharsTestData))]
        public void GetChars_RequestSpecificCharsetExcludingSimilarChars_ReturnsCorrectCharArray(
            AllowedChars allowedChars, char[] expectedCharset)
        {
            bool isValid;

            char[] charset = CharsetComposer.GetChars(allowedChars, true);

            /* Remove similar chars from expectedCharset before comparing with returned charset. */
            List<char> expectedCharsetExcludingSimilarChars = new List<char>(expectedCharset);
            expectedCharsetExcludingSimilarChars.RemoveAll(ch => SimilarLookingCharacters.Contains(ch));

            Assert.Equal(expectedCharsetExcludingSimilarChars.Count, charset.Length);
            Assert.Equal(expectedCharsetExcludingSimilarChars.Distinct().Count(), charset.Distinct().Count());
            isValid = expectedCharsetExcludingSimilarChars.All((ch) => charset.Contains(ch));
            Assert.True(isValid);
        }

        #endregion
    }

    #region [ Class GetChars Function TestData ]

    public class GetCharsTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { AllowedChars.UpperCaseLetters, UpperCaseLetters };
            yield return new object[] { AllowedChars.LowerCaseLetters, LowerCaseLetters };
            yield return new object[] { AllowedChars.Letters, Letters };
            yield return new object[] { AllowedChars.Digits, Digits };
            yield return new object[] { AllowedChars.AlphaNumeric, AlphaNumeric };
            yield return new object[] { AllowedChars.SpecialChars, SpecialReadableAsciiLetters };
            yield return new object[] { AllowedChars.Minus, Minus };
            yield return new object[] { AllowedChars.Underscore, Underscore };
            yield return new object[] { AllowedChars.Space, Space };
            yield return new object[] { AllowedChars.Brackets, Brackets };
            yield return new object[] { AllowedChars.All, All };
            yield return new object[] { AllowedChars.FileSystemSafeChars, FileSystemSafeChars };
            yield return new object[] { AllowedChars.UrlSafeChars, UrlSafeChars };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion
}
