
namespace RandomStringGeneratorTests
{
    public class RandomStringGeneratorTests :
        IClassFixture<RandomStringGeneratorFixture>
    {
        #region [ Fields ]

        private readonly RandomStringGeneratorFixture _fixture;
        private readonly ITestOutputHelper _testOutputHelper;
        private const string AlphaNumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        #endregion

        #region [ Constructor ]

        public RandomStringGeneratorTests(RandomStringGeneratorFixture fixture,
            ITestOutputHelper testOutputHelper)
        {
            _fixture = fixture;
            _testOutputHelper = testOutputHelper;
        }

        #endregion

        #region [ Function string ShuffleString(string inputString) Tests ]

        [Theory]
        [InlineData("The quick brown fox jumps over the lazy dog.")]
        [InlineData("身体力行|吃 得苦中苦,方为人上人|活到老，学到老|花有重开日，人无再少年|路遥知马力，日久见人心")]
        [InlineData(".الوقت من ذهب | .من طلب العلى سهر الليالي | .ضربت عصفورين بحجر واحد")]
        [InlineData("案ずるより産むが易しい。 | 猿も木から落ちる | 七転び八起き")]
        public void ShuffleString_RequestStringShuffleByPassingInAString_ReturnsShuffledString(string msg)
        {
            string rand;
            bool isValid;

            /* Test all classes that implement the IRandomStringGenerator interface.
               These classes are instantiated in the RandomStringGeneratorFixture class
              and can be accessed via _fixture.RandomStringGeneratorList property. */
            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                rand = randomStringGenerator.ShuffleString(msg);
                Assert.NotEqual(msg, rand);
                Assert.Equal(msg.Length, rand.Length);
                Assert.Equal(msg.Distinct().Count(), rand.Distinct().Count());
                isValid = msg.All((ch) => rand.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Fact]
        public void ShuffleString_RequestStringShuffleByPassingInANullValue_ThrowsArgumentNullException()
        {
            ArgumentNullException ex;
            const string paramName = "source";
            const string msg = $"Value cannot be null. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                /* Check for null value */
                ex = Assert.Throws<ArgumentNullException>(paramName, () =>
                {
                    _ = randomStringGenerator.ShuffleString(null);
                });
                Assert.Equal(msg, ex.Message);
            }
        }

        [Fact]
        public void ShuffleString_RequestStringShuffleByPassingInStringEmptyValue_ThrowsArgumentNullException()
        {
            ArgumentNullException ex;
            const string paramName = "source";
            const string msg = $"Value cannot be null. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                /* Check for Empty string */
                ex = Assert.Throws<ArgumentNullException>(paramName, () =>
                {
                    _ = randomStringGenerator.ShuffleString(string.Empty);
                });
                Assert.Equal(msg, ex.Message);
            }
        }

        [Fact]
        public void ShuffleString_RequestStringShuffleByPassingInEmptyValue_ThrowsArgumentNullException()
        {
            ArgumentNullException ex;
            const string paramName = "source";
            const string msg = $"Value cannot be null. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                /* Check for "" string */
                ex = Assert.Throws<ArgumentNullException>(paramName, () =>
                {
                    _ = randomStringGenerator.ShuffleString("");
                });
                Assert.Equal(msg, ex.Message);
            }
        }

        [Fact]
        public void ShuffleString_RequestStringShuffleByPassingInAStringThatExceedsMaxLength_ThrowsArgumentOutOfRangeException()
        {
            ArgumentOutOfRangeException exception;
            const string paramName = "source";
            const string msg = $"To prevent memory issues the maximum length for random strings is 5000. (Parameter '{paramName}')";
            string str = _fixture.LargeTestString + '!';  /* MAX_LENGTH + 1 */

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                /* Check MAX_LENGTH + 1 */
                exception = Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _ = randomStringGenerator.ShuffleString(str);
                });
                Assert.Equal(msg, exception.Message);
            }
        }

        [Fact]
        public void ShuffleString_RequestStringShuffleByPassingInAStringThatEqualsMaxLength_ReturnsShuffledString()
        {
            bool isValid;
            string rand;
            string str = _fixture.LargeTestString;  /* MAX_LENGTH */

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                rand = randomStringGenerator.ShuffleString(str);
                Assert.NotEqual(str, rand);
                Assert.Equal(str.Length, rand.Length);
                Assert.Equal(str.Distinct().Count(), rand.Distinct().Count());
                isValid = str.All((ch) => rand.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Fact]
        public void ShuffleString_RequestStringShuffleByPassingInAStringThatEqualsMaxLengthMinusOne_ReturnsShuffledString()
        {
            bool isValid;
            string rand;
            string str = _fixture.LargeTestString.Remove(_fixture.LargeTestString.Length - 1); ;  /* MAX_LENGTH  - 1*/

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                rand = randomStringGenerator.ShuffleString(str);
                Assert.NotEqual(str, rand);
                Assert.Equal(str.Length, rand.Length);
                Assert.Equal(str.Distinct().Count(), rand.Distinct().Count());
                isValid = str.All((ch) => rand.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Fact]
        public void ShuffleString_RequestStringShuffleByPassingInStringWithOneCharacter_ReturnsStringWithOneCharacter()
        {
            string rand;
            const string oneChar = "@";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                rand = randomStringGenerator.ShuffleString(oneChar);
                Assert.Equal(oneChar, rand);
            }
        }

        [Fact]
        public void ShuffleString_RequestStringShufflesUsingOneRandomStringObjectAndTenThreads_ReturnsShuffledStrings()
        {
            /* One random object is used in different threads. */
            foreach (IRandomStringGenerator randomStringGenerator
                    in _fixture.RandomStringGeneratorList)
            {
                _fixture.StringQueue.Clear();

                Parallel.For(0, threadPoolSize, (j) =>
                {
                    string rand;
                    bool isValid;

                    for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                    {
                        rand = randomStringGenerator.ShuffleString(AlphaNumeric);
                        Assert.NotEqual(AlphaNumeric, rand);
                        Assert.Equal(AlphaNumeric.Length, rand.Length);
                        Assert.Equal(AlphaNumeric.Distinct().Count(), rand.Distinct().Count());
                        isValid = AlphaNumeric.All((ch) => rand.Contains(ch));
                        Assert.True(isValid);
                        _fixture.StringQueue.Enqueue(rand);
                    }
                });

                /* No duplicate strings should be present in queue. */
                Assert.Equal(_fixture.StringQueue.Count, _fixture.StringQueue.Distinct().Count());
                _fixture.StringQueue.Clear();
            }
        }

        [Fact]
        public void PseudoRandomStringGeneratorShuffleString_RequestStringShufflesUsingTenRandomStringObjectAndTenThreads_ReturnsShuffledStrings()
        {
            _fixture.StringQueue.Clear();

            Parallel.For(0, threadPoolSize, (j) =>
            {
                string rand;
                bool isValid;

                IRandomStringGenerator pseudoRandomStringGenerator = new PseudoRandomStringGenerator();

                for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                {
                    rand = pseudoRandomStringGenerator.ShuffleString(AlphaNumeric);
                    Assert.NotEqual(AlphaNumeric, rand);
                    Assert.Equal(AlphaNumeric.Length, rand.Length);
                    Assert.Equal(AlphaNumeric.Distinct().Count(), rand.Distinct().Count());
                    isValid = AlphaNumeric.All((ch) => rand.Contains(ch));
                    Assert.True(isValid);
                    _fixture.StringQueue.Enqueue(rand);
                }
            });

            /* No duplicate strings should be present in the queue. */
            Assert.Equal(_fixture.StringQueue.Count, _fixture.StringQueue.Distinct().Count());
            _fixture.StringQueue.Clear();
        }

        [Fact]
        public void CryptographicRandomStringGeneratorShuffleString_RequestStringShufflesUsingTenRandomStringObjectAndTenThreads_ReturnsShuffledStrings()
        {
            _fixture.StringQueue.Clear();

            Parallel.For(0, threadPoolSize, (j) =>
            {
                string rand;
                bool isValid;

                IRandomStringGenerator cryptographicRandomStringGenerator = new CryptographicRandomStringGenerator();

                for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                {
                    rand = cryptographicRandomStringGenerator.ShuffleString(AlphaNumeric);
                    Assert.NotEqual(AlphaNumeric, rand);
                    Assert.Equal(AlphaNumeric.Length, rand.Length);
                    Assert.Equal(AlphaNumeric.Distinct().Count(), rand.Distinct().Count());
                    isValid = AlphaNumeric.All((ch) => rand.Contains(ch));
                    Assert.True(isValid);
                    _fixture.StringQueue.Enqueue(rand);
                }
            });

            /* No duplicate strings should be present in the queue. */
            Assert.Equal(_fixture.StringQueue.Count, _fixture.StringQueue.Distinct().Count());
            _fixture.StringQueue.Clear();
        }

        #endregion

        #region [ Function string GenerateString(char[] allowedChars, int length) Tests ]

        [Theory]
        [InlineData(new char[] { 'A','B','C','D','E','F','G','H',
            'I','J','K','L','M','N','O','P','Q','R','S','T','U','V',
            'W','X','Y','Z','0','1','2','3','4','5','6','7','8','9'})]
        [InlineData(new char[] { '身','体','力','行','吃','得','苦',
            '中','苦',',','方','为','人','上','人','活','到','老','学',
            '到','老','花','有','重','开' })]
        [InlineData(new char[] { 'ك','ﻙ','ﻚ','ﻜ','ﻛ','س','ﺱ','ﺲ','ﺴ',
                'ﺳ','ش','ﺵ','ﺶ','ﺸ','ﺷ','ص','ﺹ','ﺺ','ﺼ','ﺻ','ض','ﺽ','ﺾ','ﻀ',
                'ة','ﺓ','ﺔ','آ','ﺁ','ﺂ','ى','ﻯ','ﻰ' })]
        [InlineData(new char[] { 'Å', 'Ä', 'Ö', 'å', 'ä', 'ö', 'ü', 'ç', '§', '¨', '¢' })]
        public void GenerateString_RequestRandomStringByPassingCharsToUse_ReturnsRandomString(char[] allowedChars)
        {
            string rand;
            bool isValid;

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                rand = randomStringGenerator.GenerateString(allowedChars, maxLength);
                Assert.Equal(maxLength, rand.Length);
                Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */
                isValid = rand.All((ch) => allowedChars.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Fact]
        public void GenerateString_RequestRandomStringByPassingEmptyCharArray_ThrowsArgumentNullException()
        {
            ArgumentNullException exception;
            const string paramName = "allowedChars";
            const string msg = $"A charset must be specified in order to create a random string. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                /* Check for ArgumentNullException */
                exception = Assert.Throws<ArgumentNullException>(paramName, () =>
                {
                    _ = randomStringGenerator.GenerateString(new char[] { }, maxLength);
                });
                Assert.Equal(msg, exception.Message);
            }
        }

        [Fact]
        public void GenerateString_RequestRandomStringByPassingNullValue_ThrowsArgumentNullException()
        {
            ArgumentNullException exception;
            const string paramName = "allowedChars";
            const string msg = $"A charset must be specified in order to create a random string. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                /* Check for ArgumentNullException */
                exception = Assert.Throws<ArgumentNullException>(paramName, () =>
                {
                    _ = randomStringGenerator.GenerateString(null, maxLength);
                });
                Assert.Equal(msg, exception.Message);
            }
        }

        [Fact]
        public void GenerateString_RequestRandomStringByPassingOneCharToUse_ThrowsArgumentOutOfRangeException()
        {
            char[] oneChar = { '@' };
            ArgumentOutOfRangeException exception;
            const string paramName = "allowedChars";
            const string msg = $"The selected charset is too small to generate a random string from. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                exception = Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _ = randomStringGenerator.GenerateString(oneChar, maxLength);
                });

                Assert.Equal(msg, exception.Message);
            }
        }

        [Fact]
        public void GenerateString_RequestRandomStringByPassingTwoCharsToUse_ReturnsRandomString()
        {
            char[] twoChars = { '@', '#' };
            bool isValid;
            string rand;

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                rand = randomStringGenerator.GenerateString(twoChars, maxLength);
                Assert.Equal(maxLength, rand.Length);
                Assert.Equal(twoChars.Distinct().Count(), rand.Distinct().Count()); /* Check if the two different chars are present in rand. */
                isValid = rand.All((ch) => twoChars.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Fact]
        public void GenerateString_RequestRandomStringByPassingTwoCharsAndLengthOfOneToUse_ReturnsRandomStringWithLengthOfOne()
        {
            char[] twoChars = { '@', '#' };
            const int length = 1;
            bool isValid;
            string rand;

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                rand = randomStringGenerator.GenerateString(twoChars, length);
                Assert.Equal(length, rand.Length);
                isValid = rand.All((ch) => twoChars.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Fact]
        public void GenerateString_RequestRandomStringByPassingTwoCharsAndLengthOfZeroToUse_ThrowsArgumentOutOfRangeException()
        {
            char[] twoChars = { '@', '#' };
            const int length = 0;
            ArgumentOutOfRangeException exception;
            const string paramName = "length";
            const string msg = $"The length of the random string must be a positive non-zero integer. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                exception = Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _ = randomStringGenerator.GenerateString(twoChars, length);
                });
                Assert.Equal(msg, exception.Message);
            }
        }

        [Fact]
        public void GenerateString_RequestRandomStringByPassingTwoCharsAndMaxLengthPlusOneToUse_ThrowsArgumentOutOfRangeException()
        {
            char[] twoChars = { '@', '#' };
            ArgumentOutOfRangeException exception;
            const string paramName = "length";
            const string msg = $"To prevent memory issues the maximum length for random strings is 5000. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                exception = Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _ = randomStringGenerator.GenerateString(twoChars, maxLength + 1);
                });
                Assert.Equal(msg, exception.Message);
            }
        }

        [Theory]
        [InlineData(maxLength)]
        [InlineData(maxLength - 1)]
        public void GenerateString_RequestRandomStringByPassingTwoCharsAndLengthToUse_ReturnsRandomString(int length)
        {
            char[] twoChars = { '@', '#' };
            bool isValid;
            string rand;

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                rand = randomStringGenerator.GenerateString(twoChars, length);
                Assert.Equal(length, rand.Length);
                Assert.Equal(twoChars.Distinct().Count(), rand.Distinct().Count());  /* Check if the two different chars are present in rand. */
                isValid = rand.All((ch) => twoChars.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Fact]
        public void GenerateString_GenerateRandomStringsUsingOneRandomStringObjectAndTenThreads_ReturnsRandomStrings()
        {
            const int stringLength = 6;

            /* One random object is used in different threads. */
            foreach (IRandomStringGenerator randomStringGenerator
                    in _fixture.RandomStringGeneratorList)
            {
                _fixture.StringQueue.Clear();

                Parallel.For(0, threadPoolSize, (j) =>
                {
                    string rand;
                    bool isValid;

                    for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                    {
                        rand = randomStringGenerator.GenerateString(AlphaNumeric.ToCharArray(), stringLength);
                        Assert.Equal(stringLength, rand.Length);
                        Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */
                        isValid = rand.All((ch) => AlphaNumeric.Contains(ch));
                        Assert.True(isValid);
                        _fixture.StringQueue.Enqueue(rand);
                    }
                });

                /* No duplicate strings should be present in queue. */
                Assert.Equal(_fixture.StringQueue.Count, _fixture.StringQueue.Distinct().Count());
                _fixture.StringQueue.Clear();
            }
        }

        [Fact]
        public void PseudoRandomStringGeneratorGenerateString_GenerateRandomStringsUsingTenRandomStringObjectAndTenThreads_ReturnsRandomStrings()
        {
            const int stringLength = 6;

            _fixture.StringQueue.Clear();

            Parallel.For(0, threadPoolSize, (j) =>
            {
                string rand;
                bool isValid;

                IRandomStringGenerator pseudoRandomStringGenerator = new PseudoRandomStringGenerator();

                for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                {
                    rand = pseudoRandomStringGenerator.GenerateString(AlphaNumeric.ToCharArray(), stringLength);
                    Assert.Equal(stringLength, rand.Length);
                    Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */
                    isValid = rand.All((ch) => AlphaNumeric.Contains(ch));
                    Assert.True(isValid);
                    _fixture.StringQueue.Enqueue(rand);
                }
            });

            /* No duplicate strings should be present in queue. */
            Assert.Equal(_fixture.StringQueue.Count, _fixture.StringQueue.Distinct().Count());
            _fixture.StringQueue.Clear();
        }

        [Fact]
        public void CryptographicRandomStringGeneratorGenerateString_GenerateRandomStringsUsingTenRandomStringObjectAndTenThreads_ReturnsRandomStrings()
        {
            const int stringLength = 6;

            _fixture.StringQueue.Clear();

            Parallel.For(0, threadPoolSize, (j) =>
            {
                string rand;
                bool isValid;

                IRandomStringGenerator cryptographicRandomStringGenerator = new CryptographicRandomStringGenerator();

                for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                {
                    rand = cryptographicRandomStringGenerator.GenerateString(AlphaNumeric.ToCharArray(), stringLength);
                    Assert.Equal(stringLength, rand.Length);
                    Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */
                    isValid = rand.All((ch) => AlphaNumeric.Contains(ch));
                    Assert.True(isValid);
                    _fixture.StringQueue.Enqueue(rand);
                }
            });

            /* No duplicate strings should be present in queue. */
            Assert.Equal(_fixture.StringQueue.Count, _fixture.StringQueue.Distinct().Count());
            _fixture.StringQueue.Clear();
        }

        #endregion

        #region [ Function string GenerateString(AllowedChars allowedChars, int length, bool excludeSimilarLookingChars) Tests ]

        [Fact]
        public void GenerateStringOverloadOne_RequestRandomStringByPassingLengthOfZeroToUse_ThrowsArgumentOutOfRangeException()
        {
            const int length = 0;
            ArgumentOutOfRangeException exception;
            const string paramName = "length";
            const string msg = $"The length of the random string must be a positive non-zero integer. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                exception = Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _ = randomStringGenerator.GenerateString(AllowedChars.All, length, false);
                });
                Assert.Equal(msg, exception.Message);
            }
        }

        [Fact]
        public void GenerateStringOverloadOne_RequestRandomStringByPassingMaxLengthPlusOneToUse_ThrowsArgumentOutOfRangeException()
        {
            ArgumentOutOfRangeException exception;
            const string paramName = "length";
            string msg = $"To prevent memory issues the maximum length for random strings is {maxLength}. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                exception = Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _ = randomStringGenerator.GenerateString(AllowedChars.All, maxLength + 1, false);
                });
                Assert.Equal(msg, exception.Message);
            }
        }

        [Fact]
        public void GenerateStringOverloadOne_RequestRandomStringByPassingNoneCharset_ThrowsArgumentOutOfRangeException()
        {
            ArgumentOutOfRangeException exception;
            const string paramName = "allowedChars";
            const string msg = $"A charset must be specified in order to create a random string. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                exception = Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _ = randomStringGenerator.GenerateString(AllowedChars.None, maxLength, false);
                });
                Assert.Equal(msg, exception.Message);
            }
        }

        [Theory]
        [InlineData(AllowedChars.Minus)]
        [InlineData(AllowedChars.Underscore)]
        [InlineData(AllowedChars.Space)]
        public void GenerateStringOverloadOne_RequestRandomStringByPassingCharsetThatIsTooSmall_ThrowsArgumentOutOfRangeException(AllowedChars allowedChars)
        {
            ArgumentOutOfRangeException exception;
            const string paramName = "allowedChars";
            const string msg = $"The selected charset is too small to generate a random string from. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                exception = Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _ = randomStringGenerator.GenerateString(allowedChars, maxLength, false);
                });
                Assert.Equal(msg, exception.Message);
            }
        }

        [Theory]
        [InlineData(maxLength)]
        [InlineData(maxLength - 1)]
        public void GenerateStringOverloadOne_RequestRandomStringByPassingLengthToUse_ReturnsRandomString(int length)
        {
            bool isValid;
            string rand;

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                rand = randomStringGenerator.GenerateString(AllowedChars.All, length, false);
                Assert.Equal(length, rand.Length);
                Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */
                isValid = rand.All((ch) => All.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Theory]
        [ClassData(typeof(GenerateStringTestData))]
        public void GenerateStringOverloadOne_RequestRandomStringByPassingDifferentCharsetsAndIncludingSimilarLookingChars_ReturnsRandomString(AllowedChars allowedChars, char[] expectedCharset)
        {
            bool isValid;
            string rand;

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                rand = randomStringGenerator.GenerateString(allowedChars, maxLength, false);
                Assert.Equal(maxLength, rand.Length);
                Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */
                isValid = rand.All((ch) => expectedCharset.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Theory]
        [ClassData(typeof(GenerateStringTestData))]
        public void GenerateStringOverloadOne_RequestRandomStringByPassingDifferentCharsetsAndExcludingSimilarLookingChars_ReturnsRandomString(AllowedChars allowedChars, char[] expectedCharset)
        {
            bool isValid;
            string rand;

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                rand = randomStringGenerator.GenerateString(allowedChars, maxLength, true);
                Assert.Equal(maxLength, rand.Length);
                Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */
                isValid = rand.All((ch) => expectedCharset.Contains(ch) && !SimilarLookingCharacters.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Fact]
        public void GenerateStringOverloadOne_GenerateRandomStringsUsingOneRandomStringObjectAndTenThreads_ReturnsRandomStrings()
        {
            const int stringLength = 6;

            /* One random object is used in different threads. */
            foreach (IRandomStringGenerator randomStringGenerator
                    in _fixture.RandomStringGeneratorList)
            {
                _fixture.StringQueue.Clear();

                Parallel.For(0, threadPoolSize, (j) =>
                {
                    string rand;
                    bool isValid;

                    for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                    {
                        rand = randomStringGenerator.GenerateString(AllowedChars.AlphaNumeric, stringLength, false);
                        Assert.Equal(stringLength, rand.Length);
                        Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */
                        isValid = rand.All((ch) => AlphaNumeric.Contains(ch));
                        Assert.True(isValid);
                        _fixture.StringQueue.Enqueue(rand);
                    }
                });

                /* No duplicate strings should be present in queue. */
                Assert.Equal(_fixture.StringQueue.Count, _fixture.StringQueue.Distinct().Count());
                _fixture.StringQueue.Clear();
            }
        }

        [Fact]
        public void PseudoRandomStringGeneratorGenerateStringOverloadOne_GenerateRandomStringsUsingTenRandomStringObjectAndTenThreads_ReturnsRandomStrings()
        {
            const int stringLength = 6;

            _fixture.StringQueue.Clear();

            Parallel.For(0, threadPoolSize, (j) =>
            {
                string rand;
                bool isValid;

                IRandomStringGenerator pseudoRandomStringGenerator = new PseudoRandomStringGenerator();

                for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                {
                    rand = pseudoRandomStringGenerator.GenerateString(AllowedChars.AlphaNumeric, stringLength, false);
                    Assert.Equal(stringLength, rand.Length);
                    Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */
                    isValid = rand.All((ch) => AlphaNumeric.Contains(ch));
                    Assert.True(isValid);
                    _fixture.StringQueue.Enqueue(rand);
                }
            });

            /* No duplicate strings should be present in queue. */
            Assert.Equal(_fixture.StringQueue.Count, _fixture.StringQueue.Distinct().Count());
            _fixture.StringQueue.Clear();
        }

        [Fact]
        public void CryptographicRandomStringGeneratorGenerateStringOverloadOne_GenerateRandomStringsUsingTenRandomStringObjectAndTenThreads_ReturnsRandomStrings()
        {
            const int stringLength = 6;

            _fixture.StringQueue.Clear();

            Parallel.For(0, threadPoolSize, (j) =>
            {
                string rand;
                bool isValid;

                IRandomStringGenerator cryptographicRandomStringGenerator = new CryptographicRandomStringGenerator();

                for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                {
                    rand = cryptographicRandomStringGenerator.GenerateString(AllowedChars.AlphaNumeric, stringLength, false);
                    Assert.Equal(stringLength, rand.Length);
                    Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */
                    isValid = rand.All((ch) => AlphaNumeric.Contains(ch));
                    Assert.True(isValid);
                    _fixture.StringQueue.Enqueue(rand);
                }
            });

            /* No duplicate strings should be present in queue. */
            Assert.Equal(_fixture.StringQueue.Count, _fixture.StringQueue.Distinct().Count());
            _fixture.StringQueue.Clear();
        }

        #endregion

        #region [ Function string GenerateString(int minUpperCaseLetters, int minLowerCaseLetters, int minDigits, int minSpecialChars, int fillRestLength = 0, AllowedChars fillRest = AllowedChars.All, bool excludeSimilarLookingChars = false) Tests ]

        [Theory]
        [InlineData(-1, maxLength / 4, maxLength / 4, maxLength / 4, maxLength / 4)]
        [InlineData(maxLength / 4, -1, maxLength / 4, maxLength / 4, maxLength / 4)]
        [InlineData(maxLength / 4, maxLength / 4, -1, maxLength / 4, maxLength / 4)]
        [InlineData(maxLength / 4, maxLength / 4, maxLength / 4, -1, maxLength / 4)]
        [InlineData(maxLength / 4, maxLength / 4, maxLength / 4, maxLength / 4, -1)]
        public void GenerateStringOverloadTwo_RequestRandomStringByPassingLengthOfMinusOneToUse_ThrowsArgumentOutOfRangeException(int minUpperCaseLetters,
            int minLowerCaseLetters, int minDigits, int minSpecialChars, int extraLength)
        {
            ArgumentOutOfRangeException exception;
            const string paramName = "Individual length parameter value.";
            const string msg = $"The individual length parameter values (such as minUpperCaseLetters) must be greater than or equal to zero. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                exception = Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _ = randomStringGenerator.GenerateString(minUpperCaseLetters, minLowerCaseLetters,
                        minDigits, minSpecialChars, false, extraLength);
                });
                Assert.Equal(msg, exception.Message);
            }
        }

        [Fact]
        public void GenerateStringOverloadTwo_RequestRandomStringByPassingLengthOfZeroToUse_ThrowsArgumentOutOfRangeException()
        {
            ArgumentOutOfRangeException exception;
            const string paramName = "Overall length of random string requested.";
            const string msg = $"The length of the random string must be a positive non-zero integer. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                exception = Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _ = randomStringGenerator.GenerateString(0, 0, 0, 0);
                });
                Assert.Equal(msg, exception.Message);
            }
        }

        [Theory]
        [InlineData(maxLength + 1, 0, 0, 0, 0)]
        [InlineData(0, maxLength + 1, 0, 0, 0)]
        [InlineData(0, 0, maxLength + 1, 0, 0)]
        [InlineData(0, 0, 0, maxLength + 1, 0)]
        [InlineData(0, 0, 0, 0, maxLength + 1)]
        [InlineData(maxLength / 5 + 1, maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5)]
        [InlineData(maxLength / 5, maxLength / 5 + 1, maxLength / 5, maxLength / 5, maxLength / 5)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5 + 1, maxLength / 5, maxLength / 5)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5 + 1, maxLength / 5)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5 + 1)]
        public void GenerateStringOverloadTwo_RequestRandomStringByPassingMaxLengthPlusOneToUse_ThrowsArgumentOutOfRangeException(int minUpperCaseLetters,
            int minLowerCaseLetters, int minDigits, int minSpecialChars, int extraLength)
        {
            ArgumentOutOfRangeException exception;
            const string paramName = "Overall length of random string requested.";
            const string msg = $"To prevent memory issues the maximum length for random strings is 5000. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                exception = Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _ = randomStringGenerator.GenerateString(minUpperCaseLetters, minLowerCaseLetters,
                        minDigits, minSpecialChars, false, extraLength);
                });
                Assert.Equal(msg, exception.Message);
            }
        }

        [Theory]
        [InlineData(maxLength, 0, 0, 0, 0)]
        [InlineData(0, maxLength, 0, 0, 0)]
        [InlineData(0, 0, maxLength, 0, 0)]
        [InlineData(0, 0, 0, maxLength, 0)]
        [InlineData(0, 0, 0, 0, maxLength)]
        [InlineData(maxLength - 1, 0, 0, 0, 0)]
        [InlineData(0, maxLength - 1, 0, 0, 0)]
        [InlineData(0, 0, maxLength - 1, 0, 0)]
        [InlineData(0, 0, 0, maxLength - 1, 0)]
        [InlineData(0, 0, 0, 0, maxLength - 1)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5)]
        [InlineData(maxLength / 5 - 1, maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5)]
        [InlineData(maxLength / 5, maxLength / 5 - 1, maxLength / 5, maxLength / 5, maxLength / 5)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5 - 1, maxLength / 5, maxLength / 5)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5 - 1, maxLength / 5)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5 - 1)]
        public void GenerateStringOverloadTwo_RequestRandomStringByPassingLengthToUseAndIncludingSimilarLookingChars_ReturnsRandomString(int minUpperCaseLetters,
            int minLowerCaseLetters, int minDigits, int minSpecialChars, int extraLength)
        {
            bool isValid;
            string rand;
            int length;

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                length = minUpperCaseLetters + minLowerCaseLetters + minDigits + minSpecialChars + extraLength;

                rand = randomStringGenerator.GenerateString(minUpperCaseLetters, minLowerCaseLetters,
                    minDigits, minSpecialChars, false, extraLength, AllowedChars.All);
                Assert.Equal(length, rand.Length);
                Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */
                isValid = rand.All((ch) => All.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Theory]
        [InlineData(maxLength, 0, 0, 0, 0)]
        [InlineData(0, maxLength, 0, 0, 0)]
        [InlineData(0, 0, maxLength, 0, 0)]
        [InlineData(0, 0, 0, maxLength, 0)]
        [InlineData(0, 0, 0, 0, maxLength)]
        [InlineData(maxLength - 1, 0, 0, 0, 0)]
        [InlineData(0, maxLength - 1, 0, 0, 0)]
        [InlineData(0, 0, maxLength - 1, 0, 0)]
        [InlineData(0, 0, 0, maxLength - 1, 0)]
        [InlineData(0, 0, 0, 0, maxLength - 1)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5)]
        [InlineData(maxLength / 5 - 1, maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5)]
        [InlineData(maxLength / 5, maxLength / 5 - 1, maxLength / 5, maxLength / 5, maxLength / 5)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5 - 1, maxLength / 5, maxLength / 5)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5 - 1, maxLength / 5)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5 - 1)]
        public void GenerateStringOverloadTwo_RequestRandomStringByPassingLengthToUseAndExcludingSimilarLookingChars_ReturnsRandomString(int minUpperCaseLetters,
     int minLowerCaseLetters, int minDigits, int minSpecialChars, int extraLength)
        {
            bool isValid;
            string rand;
            int length;

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                length = minUpperCaseLetters + minLowerCaseLetters + minDigits + minSpecialChars + extraLength;

                rand = randomStringGenerator.GenerateString(minUpperCaseLetters, minLowerCaseLetters,
                    minDigits, minSpecialChars, true, extraLength, AllowedChars.All);
                Assert.Equal(length, rand.Length);
                Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */
                isValid = rand.All((ch) => All.Contains(ch) && !SimilarLookingCharacters.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Fact]
        public void GenerateStringOverloadTwo_RequestRandomStringByPassingNoneCharset_ThrowsArgumentOutOfRangeException()
        {
            ArgumentOutOfRangeException exception;
            const string paramName = "extraAllowedChars";
            const string msg = $"A charset must be specified in order to create a random string. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                exception = Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _ = randomStringGenerator.GenerateString(0, 0, 0, 0, false, maxLength, AllowedChars.None);
                });
                Assert.Equal(msg, exception.Message);
            }
        }

        [Theory]
        [InlineData(AllowedChars.Minus)]
        [InlineData(AllowedChars.Underscore)]
        [InlineData(AllowedChars.Space)]
        public void GenerateStringOverloadTwo_RequestRandomStringByPassingCharsetThatIsTooSmall_ThrowsArgumentOutOfRangeException(AllowedChars allowedChars)
        {
            ArgumentOutOfRangeException exception;
            const string paramName = "extraAllowedChars";
            const string msg = $"The selected charset is too small to generate a random string from. (Parameter '{paramName}')";

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                exception = Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                {
                    _ = randomStringGenerator.GenerateString(0, 0, 0, 0, false, maxLength, allowedChars);
                });
                Assert.Equal(msg, exception.Message);
            }
        }

        [Theory]
        [InlineData(maxLength, 0, 0, 0)]
        [InlineData(0, maxLength, 0, 0)]
        [InlineData(0, 0, maxLength, 0)]
        [InlineData(0, 0, 0, maxLength)]
        [InlineData(maxLength / 4, maxLength / 4, maxLength / 4, maxLength / 4)]
        /* Charset occurrence variations such as minUpperCaseLetters ... , extraLength is zero to exclude its contribution. */
        public void GenerateStringOverloadTwo_RequestRandomStringByPassingCharsetOccurrenceVariationsAndIncludingSimilarLookingChars_ReturnsRandomString(
            int minUpperCaseLetters, int minLowerCaseLetters, int minDigits, int minSpecialChars)
        {
            int count, length;
            string rand;

            length = minUpperCaseLetters + minLowerCaseLetters + minDigits + minSpecialChars;

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                /* include similar looking chars */
                rand = randomStringGenerator.GenerateString(minUpperCaseLetters, minLowerCaseLetters,
                    minDigits, minSpecialChars, false);

                Assert.Equal(length, rand.Length);
                Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */

                /* Account for all chars generated. */
                /* minUpperCaseLetters */
                count = rand.Count((ch) => UpperCaseLetters.Contains(ch));
                Assert.Equal(minUpperCaseLetters, count);

                /* minLowerCaseLetters */
                count = rand.Count((ch) => LowerCaseLetters.Contains(ch));
                Assert.Equal(minLowerCaseLetters, count);

                /* minDigits */
                count = rand.Count((ch) => Digits.Contains(ch));
                Assert.Equal(minDigits, count);

                /* minSpecialChars */
                count = rand.Count((ch) => SpecialReadableAsciiLetters.Contains(ch));
                Assert.Equal(minSpecialChars, count);
            }
        }

        [Theory]
        [InlineData(maxLength, 0, 0, 0)]
        [InlineData(0, maxLength, 0, 0)]
        [InlineData(0, 0, maxLength, 0)]
        [InlineData(0, 0, 0, maxLength)]
        [InlineData(maxLength / 4, maxLength / 4, maxLength / 4, maxLength / 4)]
        /* Charset occurrence variations such as minUpperCaseLetters ... , extraLength is zero to exclude its contribution. */
        public void GenerateStringOverloadTwo_RequestRandomStringByPassingCharsetOccurrenceVariationsAndExludingSimilarLookingChars_ReturnsRandomString(
            int minUpperCaseLetters, int minLowerCaseLetters, int minDigits, int minSpecialChars)
        {
            int count, length;
            bool isValid;
            string rand;

            length = minUpperCaseLetters + minLowerCaseLetters + minDigits + minSpecialChars;

            foreach (IRandomStringGenerator randomStringGenerator
                in _fixture.RandomStringGeneratorList)
            {
                /* exclude similar looking chars */
                rand = randomStringGenerator.GenerateString(minUpperCaseLetters, minLowerCaseLetters,
                    minDigits, minSpecialChars, true);

                Assert.Equal(length, rand.Length);
                Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */

                /* Account for all chars generated. */
                /* minUpperCaseLetters */
                count = rand.Count((ch) => UpperCaseLetters.Contains(ch));
                Assert.Equal(minUpperCaseLetters, count);

                /* minLowerCaseLetters */
                count = rand.Count((ch) => LowerCaseLetters.Contains(ch));
                Assert.Equal(minLowerCaseLetters, count);

                /* minDigits */
                count = rand.Count((ch) => Digits.Contains(ch));
                Assert.Equal(minDigits, count);

                /* minSpecialChars */
                count = rand.Count((ch) => SpecialReadableAsciiLetters.Contains(ch));
                Assert.Equal(minSpecialChars, count);

                /* Make sure no similar looking char is present. */
                isValid = rand.All((ch) => !SimilarLookingCharacters.Contains(ch));
                Assert.True(isValid);
            }
        }

        [Theory]
        [InlineData(AllowedChars.All, 1, false)]
        [InlineData(AllowedChars.All, maxLength - 1, false)]
        [InlineData(AllowedChars.All, maxLength, false)]
        [InlineData(AllowedChars.All, 1, true)]
        [InlineData(AllowedChars.All, maxLength - 1, true)]
        [InlineData(AllowedChars.All, maxLength, true)]

        [InlineData(AllowedChars.UpperCaseLetters, 1, false)]
        [InlineData(AllowedChars.UpperCaseLetters, maxLength - 1, false)]
        [InlineData(AllowedChars.UpperCaseLetters, maxLength, false)]
        [InlineData(AllowedChars.UpperCaseLetters, 1, true)]
        [InlineData(AllowedChars.UpperCaseLetters, maxLength - 1, true)]
        [InlineData(AllowedChars.UpperCaseLetters, maxLength, true)]

        [InlineData(AllowedChars.LowerCaseLetters, 1, false)]
        [InlineData(AllowedChars.LowerCaseLetters, maxLength - 1, false)]
        [InlineData(AllowedChars.LowerCaseLetters, maxLength, false)]
        [InlineData(AllowedChars.LowerCaseLetters, 1, true)]
        [InlineData(AllowedChars.LowerCaseLetters, maxLength - 1, true)]
        [InlineData(AllowedChars.LowerCaseLetters, maxLength, true)]

        [InlineData(AllowedChars.Digits, 1, false)]
        [InlineData(AllowedChars.Digits, maxLength - 1, false)]
        [InlineData(AllowedChars.Digits, maxLength, false)]
        [InlineData(AllowedChars.Digits, 1, true)]
        [InlineData(AllowedChars.Digits, maxLength - 1, true)]
        [InlineData(AllowedChars.Digits, maxLength, true)]

        [InlineData(AllowedChars.SpecialChars, 1, false)]
        [InlineData(AllowedChars.SpecialChars, maxLength - 1, false)]
        [InlineData(AllowedChars.SpecialChars, maxLength, false)]
        [InlineData(AllowedChars.SpecialChars, 1, true)]
        [InlineData(AllowedChars.SpecialChars, maxLength - 1, true)]
        [InlineData(AllowedChars.SpecialChars, maxLength, true)]

        [InlineData(AllowedChars.Brackets, 1, false)]
        [InlineData(AllowedChars.Brackets, maxLength - 1, false)]
        [InlineData(AllowedChars.Brackets, maxLength, false)]
        [InlineData(AllowedChars.Brackets, 1, true)]
        [InlineData(AllowedChars.Brackets, maxLength - 1, true)]
        [InlineData(AllowedChars.Brackets, maxLength, true)]

        [InlineData(AllowedChars.FileSystemSafeChars, 1, false)]
        [InlineData(AllowedChars.FileSystemSafeChars, maxLength - 1, false)]
        [InlineData(AllowedChars.FileSystemSafeChars, maxLength, false)]
        [InlineData(AllowedChars.FileSystemSafeChars, 1, true)]
        [InlineData(AllowedChars.FileSystemSafeChars, maxLength - 1, true)]
        [InlineData(AllowedChars.FileSystemSafeChars, maxLength, true)]

        [InlineData(AllowedChars.Minus | AllowedChars.Underscore | AllowedChars.Space, 1, false)]
        [InlineData(AllowedChars.Minus | AllowedChars.Underscore | AllowedChars.Space, maxLength - 1, false)]
        [InlineData(AllowedChars.Minus | AllowedChars.Underscore | AllowedChars.Space, maxLength, false)]
        [InlineData(AllowedChars.Minus | AllowedChars.Underscore | AllowedChars.Space, 1, true)]
        [InlineData(AllowedChars.Minus | AllowedChars.Underscore | AllowedChars.Space, maxLength - 1, true)]
        [InlineData(AllowedChars.Minus | AllowedChars.Underscore | AllowedChars.Space, maxLength, true)]
        /* Charset extraAllowedChars variations such as AllowedChars.All etc. , minUpperCaseLetters .. minSpecialChars are set to zero to exclude their contributions. */
        public void GenerateStringOverloadTwo_RequestRandomStringByPassingExtraAllowedCharsVariations_ReturnsRandomString(
            AllowedChars allowedChars, int length, bool excludeSimilarLookingChars)
        {
            string rand;
            bool isValid;
            int count;

            foreach (IRandomStringGenerator randomStringGenerator
                 in _fixture.RandomStringGeneratorList)
            {
                rand = randomStringGenerator.GenerateString(0, 0, 0, 0,
                    excludeSimilarLookingChars, length, allowedChars);

                Assert.Equal(length, rand.Length);

                /* Account for all chars generated. */
                switch (allowedChars)
                {
                    case AllowedChars.UpperCaseLetters:
                        count = rand.Count((ch) => UpperCaseLetters.Contains(ch));
                        Assert.Equal(length, count);
                        break;

                    case AllowedChars.LowerCaseLetters:
                        count = rand.Count((ch) => LowerCaseLetters.Contains(ch));
                        Assert.Equal(length, count);
                        break;

                    case AllowedChars.Digits:
                        count = rand.Count((ch) => Digits.Contains(ch));
                        Assert.Equal(length, count);
                        break;

                    case AllowedChars.SpecialChars:
                        count = rand.Count((ch) => SpecialReadableAsciiLetters.Contains(ch));
                        Assert.Equal(length, count);
                        break;
                    case AllowedChars.Brackets:
                        count = rand.Count((ch) => Brackets.Contains(ch));
                        Assert.Equal(length, count);
                        break;
                    case AllowedChars.FileSystemSafeChars:
                        count = rand.Count((ch) => FileSystemSafeChars.Contains(ch));
                        Assert.Equal(length, count);
                        break;
                    default:
                        count = rand.Count((ch) => All.Contains(ch));
                        Assert.Equal(length, count);
                        break;
                }

                /* Make sure no similar looking char is present. */
                if (excludeSimilarLookingChars)
                {
                    isValid = rand.All((ch) => !SimilarLookingCharacters.Contains(ch));
                    Assert.True(isValid);
                }
            }
        }

        [Theory]
        [InlineData(maxLength / 2, 0, 0, 0, false, maxLength / 2, AllowedChars.UpperCaseLetters)]
        [InlineData(maxLength / 2, 0, 0, 0, false, maxLength / 2, AllowedChars.LowerCaseLetters)]
        [InlineData(maxLength / 2, 0, 0, 0, false, maxLength / 2, AllowedChars.Digits)]
        [InlineData(maxLength / 2, 0, 0, 0, false, maxLength / 2, AllowedChars.SpecialChars)]
        [InlineData(maxLength / 2, 0, 0, 0, false, maxLength / 2, AllowedChars.FileSystemSafeChars)]
        [InlineData(maxLength / 2, 0, 0, 0, false, maxLength / 2, AllowedChars.All)]
        [InlineData(maxLength / 2, 0, 0, 0, false, maxLength / 2, AllowedChars.Brackets)]
        [InlineData(maxLength / 2, 0, 0, 0, true, maxLength / 2, AllowedChars.UpperCaseLetters)]
        [InlineData(maxLength / 2, 0, 0, 0, true, maxLength / 2, AllowedChars.LowerCaseLetters)]
        [InlineData(maxLength / 2, 0, 0, 0, true, maxLength / 2, AllowedChars.Digits)]
        [InlineData(maxLength / 2, 0, 0, 0, true, maxLength / 2, AllowedChars.SpecialChars)]
        [InlineData(maxLength / 2, 0, 0, 0, true, maxLength / 2, AllowedChars.FileSystemSafeChars)]
        [InlineData(maxLength / 2, 0, 0, 0, true, maxLength / 2, AllowedChars.All)]
        [InlineData(maxLength / 2, 0, 0, 0, true, maxLength / 2, AllowedChars.Brackets)]

        [InlineData(0, maxLength / 2, 0, 0, false, maxLength / 2, AllowedChars.UpperCaseLetters)]
        [InlineData(0, maxLength / 2, 0, 0, false, maxLength / 2, AllowedChars.LowerCaseLetters)]
        [InlineData(0, maxLength / 2, 0, 0, false, maxLength / 2, AllowedChars.Digits)]
        [InlineData(0, maxLength / 2, 0, 0, false, maxLength / 2, AllowedChars.SpecialChars)]
        [InlineData(0, maxLength / 2, 0, 0, false, maxLength / 2, AllowedChars.FileSystemSafeChars)]
        [InlineData(0, maxLength / 2, 0, 0, false, maxLength / 2, AllowedChars.All)]
        [InlineData(0, maxLength / 2, 0, 0, false, maxLength / 2, AllowedChars.Brackets)]
        [InlineData(0, maxLength / 2, 0, 0, true, maxLength / 2, AllowedChars.UpperCaseLetters)]
        [InlineData(0, maxLength / 2, 0, 0, true, maxLength / 2, AllowedChars.LowerCaseLetters)]
        [InlineData(0, maxLength / 2, 0, 0, true, maxLength / 2, AllowedChars.Digits)]
        [InlineData(0, maxLength / 2, 0, 0, true, maxLength / 2, AllowedChars.SpecialChars)]
        [InlineData(0, maxLength / 2, 0, 0, true, maxLength / 2, AllowedChars.FileSystemSafeChars)]
        [InlineData(0, maxLength / 2, 0, 0, true, maxLength / 2, AllowedChars.All)]
        [InlineData(0, maxLength / 2, 0, 0, true, maxLength / 2, AllowedChars.Brackets)]

        [InlineData(0, 0, maxLength / 2, 0, false, maxLength / 2, AllowedChars.UpperCaseLetters)]
        [InlineData(0, 0, maxLength / 2, 0, false, maxLength / 2, AllowedChars.LowerCaseLetters)]
        [InlineData(0, 0, maxLength / 2, 0, false, maxLength / 2, AllowedChars.Digits)]
        [InlineData(0, 0, maxLength / 2, 0, false, maxLength / 2, AllowedChars.SpecialChars)]
        [InlineData(0, 0, maxLength / 2, 0, false, maxLength / 2, AllowedChars.FileSystemSafeChars)]
        [InlineData(0, 0, maxLength / 2, 0, false, maxLength / 2, AllowedChars.All)]
        [InlineData(0, 0, maxLength / 2, 0, false, maxLength / 2, AllowedChars.Brackets)]
        [InlineData(0, 0, maxLength / 2, 0, true, maxLength / 2, AllowedChars.UpperCaseLetters)]
        [InlineData(0, 0, maxLength / 2, 0, true, maxLength / 2, AllowedChars.LowerCaseLetters)]
        [InlineData(0, 0, maxLength / 2, 0, true, maxLength / 2, AllowedChars.Digits)]
        [InlineData(0, 0, maxLength / 2, 0, true, maxLength / 2, AllowedChars.SpecialChars)]
        [InlineData(0, 0, maxLength / 2, 0, true, maxLength / 2, AllowedChars.FileSystemSafeChars)]
        [InlineData(0, 0, maxLength / 2, 0, true, maxLength / 2, AllowedChars.All)]
        [InlineData(0, 0, maxLength / 2, 0, true, maxLength / 2, AllowedChars.Brackets)]

        [InlineData(0, 0, 0, maxLength / 2, false, maxLength / 2, AllowedChars.UpperCaseLetters)]
        [InlineData(0, 0, 0, maxLength / 2, false, maxLength / 2, AllowedChars.LowerCaseLetters)]
        [InlineData(0, 0, 0, maxLength / 2, false, maxLength / 2, AllowedChars.Digits)]
        [InlineData(0, 0, 0, maxLength / 2, false, maxLength / 2, AllowedChars.SpecialChars)]
        [InlineData(0, 0, 0, maxLength / 2, false, maxLength / 2, AllowedChars.FileSystemSafeChars)]
        [InlineData(0, 0, 0, maxLength / 2, false, maxLength / 2, AllowedChars.All)]
        [InlineData(0, 0, 0, maxLength / 2, false, maxLength / 2, AllowedChars.Brackets)]
        [InlineData(0, 0, 0, maxLength / 2, true, maxLength / 2, AllowedChars.UpperCaseLetters)]
        [InlineData(0, 0, 0, maxLength / 2, true, maxLength / 2, AllowedChars.LowerCaseLetters)]
        [InlineData(0, 0, 0, maxLength / 2, true, maxLength / 2, AllowedChars.Digits)]
        [InlineData(0, 0, 0, maxLength / 2, true, maxLength / 2, AllowedChars.SpecialChars)]
        [InlineData(0, 0, 0, maxLength / 2, true, maxLength / 2, AllowedChars.FileSystemSafeChars)]
        [InlineData(0, 0, 0, maxLength / 2, true, maxLength / 2, AllowedChars.All)]
        [InlineData(0, 0, 0, maxLength / 2, true, maxLength / 2, AllowedChars.Brackets)]

        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, false, maxLength / 5, AllowedChars.UpperCaseLetters)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, false, maxLength / 5, AllowedChars.LowerCaseLetters)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, false, maxLength / 5, AllowedChars.Digits)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, false, maxLength / 5, AllowedChars.SpecialChars)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, false, maxLength / 5, AllowedChars.FileSystemSafeChars)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, false, maxLength / 5, AllowedChars.All)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, false, maxLength / 5, AllowedChars.Brackets)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, true, maxLength / 5, AllowedChars.UpperCaseLetters)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, true, maxLength / 5, AllowedChars.LowerCaseLetters)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, true, maxLength / 5, AllowedChars.Digits)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, true, maxLength / 5, AllowedChars.SpecialChars)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, true, maxLength / 5, AllowedChars.FileSystemSafeChars)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, true, maxLength / 5, AllowedChars.All)]
        [InlineData(maxLength / 5, maxLength / 5, maxLength / 5, maxLength / 5, true, maxLength / 5, AllowedChars.Brackets)]
        public void GenerateStringOverloadTwo_RequestRandomStringByPassingCharsetOccurrenceVariationsAndExtraAllowedCharsVariations_ReturnsRandomString(
            int minUpperCaseLetters, int minLowerCaseLetters, int minDigits, int minSpecialChars,
            bool excludeSimilarLookingChars, int extraLength, AllowedChars extraAllowedChars)
        {
            string rand;
            bool isValid;
            int length, count;

            length = minUpperCaseLetters + minLowerCaseLetters + minDigits +
                minSpecialChars + extraLength;

            foreach (IRandomStringGenerator randomStringGenerator
                 in _fixture.RandomStringGeneratorList)
            {
                rand = randomStringGenerator.GenerateString(minUpperCaseLetters,
                    minLowerCaseLetters, minDigits, minSpecialChars,
                    excludeSimilarLookingChars, extraLength, extraAllowedChars);

                Assert.Equal(length, rand.Length);

                /* Make sure all characters are present that are specified in the 
                 * min occurrence parameters such as minUpperCaseLetters. */
                /* minUpperCaseLetters */
                count = rand.Count((ch) => UpperCaseLetters.Contains(ch));
                Assert.True(count >= minUpperCaseLetters);

                /* minLowerCaseLetters */
                count = rand.Count((ch) => LowerCaseLetters.Contains(ch));
                Assert.True(count >= minLowerCaseLetters);

                /* minDigits */
                count = rand.Count((ch) => Digits.Contains(ch));
                Assert.True(count >= minDigits);

                /* minSpecialChars */
                count = rand.Count((ch) => SpecialReadableAsciiLetters.Contains(ch));
                Assert.True(count >= minSpecialChars);

                /* Account for all the extraAllowedChars. */
                switch (extraAllowedChars)
                {
                    case AllowedChars.UpperCaseLetters:
                        count = rand.Count((ch) => UpperCaseLetters.Contains(ch));
                        Assert.Equal(extraLength + minUpperCaseLetters, count);
                        break;

                    case AllowedChars.LowerCaseLetters:
                        count = rand.Count((ch) => LowerCaseLetters.Contains(ch));
                        Assert.Equal(extraLength + minLowerCaseLetters, count);
                        break;

                    case AllowedChars.Digits:
                        count = rand.Count((ch) => Digits.Contains(ch));
                        Assert.Equal(extraLength + minDigits, count);
                        break;

                    case AllowedChars.SpecialChars:
                        count = rand.Count((ch) => SpecialReadableAsciiLetters.Contains(ch));
                        Assert.Equal(extraLength + minSpecialChars, count);
                        break;

                    case AllowedChars.Brackets:
                        count = rand.Count((ch) => Brackets.Contains(ch));
                        Assert.Equal(extraLength, count);
                        break;

                    case AllowedChars.FileSystemSafeChars:
                        count = rand.Count((ch) => FileSystemSafeChars.Contains(ch));
                        Assert.Equal(extraLength + minUpperCaseLetters +
                            minLowerCaseLetters + minDigits, count);
                        break;

                    default:
                        count = rand.Count((ch) => All.Contains(ch));
                        Assert.Equal(length, count);
                        break;
                }

                /* Make sure no similar looking char is present. */
                if (excludeSimilarLookingChars)
                {
                    isValid = rand.All((ch) => !SimilarLookingCharacters.Contains(ch));
                    Assert.True(isValid);
                }
            }
        }

        [Fact]
        public void GenerateStringOverloadTwo_GenerateRandomStringsUsingOneRandomStringObjectAndTenThreads_ReturnsRandomStrings()
        {
            const int minUpperCaseLetters = 1, minLowerCaseLetters = 1, minDigits = 1,
                minSpecialChars = 0, extraLength = 3, stringLength = 6;
            const bool excludeSimilarLookingChars = false;

            /* One random object is used in different threads. */
            foreach (IRandomStringGenerator randomStringGenerator
                    in _fixture.RandomStringGeneratorList)
            {
                _fixture.StringQueue.Clear();

                Parallel.For(0, threadPoolSize, (j) =>
                {
                    string rand;
                    bool isValid;

                    for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                    {
                        rand = randomStringGenerator.GenerateString(minUpperCaseLetters,
                            minLowerCaseLetters, minDigits, minSpecialChars, excludeSimilarLookingChars,
                            extraLength, AllowedChars.AlphaNumeric);
                        Assert.Equal(stringLength, rand.Length);
                        Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */
                        isValid = rand.All((ch) => AlphaNumeric.Contains(ch));
                        Assert.True(isValid);
                        _fixture.StringQueue.Enqueue(rand);
                    }
                });

                /* No duplicate strings should be present in queue. */
                Assert.Equal(_fixture.StringQueue.Count, _fixture.StringQueue.Distinct().Count());
                _fixture.StringQueue.Clear();
            }
        }

        [Fact]
        public void PseudoRandomStringGeneratorGenerateStringOverloadTwo_GenerateRandomStringsUsingTenRandomStringObjectAndTenThreads_ReturnsRandomStrings()
        {
            const int minUpperCaseLetters = 1, minLowerCaseLetters = 1, minDigits = 1,
                minSpecialChars = 0, extraLength = 3, stringLength = 6;
            const bool excludeSimilarLookingChars = false;

            _fixture.StringQueue.Clear();

            Parallel.For(0, threadPoolSize, (j) =>
            {
                string rand;
                bool isValid;

                IRandomStringGenerator pseudoRandomStringGenerator = new PseudoRandomStringGenerator();

                for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                {
                    rand = pseudoRandomStringGenerator.GenerateString(minUpperCaseLetters,
                            minLowerCaseLetters, minDigits, minSpecialChars, excludeSimilarLookingChars,
                            extraLength, AllowedChars.AlphaNumeric);
                    Assert.Equal(stringLength, rand.Length);
                    Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */
                    isValid = rand.All((ch) => AlphaNumeric.Contains(ch));
                    Assert.True(isValid);
                    _fixture.StringQueue.Enqueue(rand);
                }
            });

            /* No duplicate strings should be present in queue. */
            Assert.Equal(_fixture.StringQueue.Count, _fixture.StringQueue.Distinct().Count());
            _fixture.StringQueue.Clear();
        }

        [Fact]
        public void CryptographicRandomStringGeneratorGenerateStringOverloadTwo_GenerateRandomStringsUsingTenRandomStringObjectAndTenThreads_ReturnsRandomStrings()
        {
            const int minUpperCaseLetters = 1, minLowerCaseLetters = 1, minDigits = 1,
                minSpecialChars = 0, extraLength = 3, stringLength = 6;
            const bool excludeSimilarLookingChars = false;

            _fixture.StringQueue.Clear();

            Parallel.For(0, threadPoolSize, (j) =>
            {
                string rand;
                bool isValid;

                IRandomStringGenerator cryptographicRandomStringGenerator = new CryptographicRandomStringGenerator();

                for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                {
                    rand = cryptographicRandomStringGenerator.GenerateString(minUpperCaseLetters,
                         minLowerCaseLetters, minDigits, minSpecialChars, excludeSimilarLookingChars,
                         extraLength, AllowedChars.AlphaNumeric);
                    Assert.Equal(stringLength, rand.Length);
                    Assert.True(rand.Distinct().Count() > minNumberOfDistinctChars); /* Check if different chars are present in random string. */
                    isValid = rand.All((ch) => AlphaNumeric.Contains(ch));
                    Assert.True(isValid);
                    _fixture.StringQueue.Enqueue(rand);
                }
            });

            /* No duplicate strings should be present in queue. */
            Assert.Equal(_fixture.StringQueue.Count, _fixture.StringQueue.Distinct().Count());
            _fixture.StringQueue.Clear();
        }

        #endregion
    }

    #region [ Class GenerateString Function TestData ]

    public class GenerateStringTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { AllowedChars.UpperCaseLetters, UpperCaseLetters };
            yield return new object[] { AllowedChars.LowerCaseLetters, LowerCaseLetters };
            yield return new object[] { AllowedChars.Letters, Letters };
            yield return new object[] { AllowedChars.Digits, Digits };
            yield return new object[] { AllowedChars.AlphaNumeric, AlphaNumeric };
            yield return new object[] { AllowedChars.SpecialChars, SpecialReadableAsciiLetters };
            yield return new object[] { AllowedChars.Brackets, Brackets };
            yield return new object[] { AllowedChars.All, All };
            yield return new object[] { AllowedChars.FileSystemSafeChars, FileSystemSafeChars };
            yield return new object[] { AllowedChars.UrlSafeChars, UrlSafeChars };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion
}
