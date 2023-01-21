
namespace RandomStringGeneratorTests
{
    public class RandomNumberGeneratorTests :
        IClassFixture<RandomNumberGeneratorFixture>
    {
        #region [ Fields ]

        private readonly RandomNumberGeneratorFixture _fixture;
        private readonly ITestOutputHelper _testOutputHelper;

        #endregion

        #region [ Constructor ]

        public RandomNumberGeneratorTests(RandomNumberGeneratorFixture fixture,
            ITestOutputHelper testOutputHelper)
        {
            _fixture = fixture;
            _testOutputHelper = testOutputHelper;
        }

        #endregion

        #region [ Function int Next() Tests ]

        [Fact]
        public void Next_RequestRandomNumbers_ReturnsUniqueRandomNumbers()
        {
            int rand;

            /* Test all classes that implement the IRandomNumberGenerator interface.
             * These classes are instantiated in the RandomNumberGeneratorFixture class
             * and can be accessed via RandomNumberGeneratorList property. */
            foreach (IRandomNumberGenerator randomNumberGenerator
                in _fixture.RandomNumberGeneratorList)
            {
                _fixture.NumberQueue.Clear();

                for (int i = 0; i < sampleSize; i++)
                {
                    rand = randomNumberGenerator.Next(); /* Returns a positive random integer. */
                    /* A 32-bit signed integer that is greater than or equal to 0 and less than int.MaxValue.*/
                    Assert.InRange(rand, 0, int.MaxValue - 1);
                    _fixture.NumberQueue.Enqueue(rand);
                }

                /* Make sure all generated random numbers are unique. */
                /* The chance of generating duplicates is very small, less than 1 %. */
                Assert.Equal(_fixture.NumberQueue.Count, _fixture.NumberQueue.Distinct().Count());
                _fixture.NumberQueue.Clear();
            }
        }

        [Fact]
        public void Next_RequestRandomNumbersUsingOneRandomObjectAndTenThreads_ReturnsUniqueRandomNumbers()
        {
            /* One random object is used in different threads. */
            foreach (IRandomNumberGenerator randomNumberGenerator
                in _fixture.RandomNumberGeneratorList)
            {
                _fixture.NumberQueue.Clear();

                Parallel.For(0, threadPoolSize, (j) =>
                {
                    int rand;

                    for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                    {
                        rand = randomNumberGenerator.Next();
                        Assert.InRange(rand, 0, int.MaxValue - 1);
                        _fixture.NumberQueue.Enqueue(rand);
                    }
                });

                Assert.Equal(_fixture.NumberQueue.Count, _fixture.NumberQueue.Distinct().Count());
                _fixture.NumberQueue.Clear();
            }
        }

        [Fact]
        public void PseudoRandomNumberGeneratorNext_RequestRandomNumbersUsingTenRandomObjectsAndTenThreads_ReturnsUniqueRandomNumbers()
        {
            _fixture.NumberQueue.Clear();

            Parallel.For(0, threadPoolSize, (j) =>
            {
                int rand;
                /*  Create Random Number Object for each thread. */
                IRandomNumberGenerator randomNumberGenerator = new PseudoRandomNumberGenerator();

                for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                {
                    rand = randomNumberGenerator.Next();
                    Assert.InRange(rand, 0, int.MaxValue - 1);
                    _fixture.NumberQueue.Enqueue(rand);
                }
            });

            Assert.Equal(_fixture.NumberQueue.Count, _fixture.NumberQueue.Distinct().Count());
            _fixture.NumberQueue.Clear();
        }

        [Fact]
        public void CryptographicRandomNumberGeneratorNext_RequestRandomNumbersUsingTenRandomObjectsAndTenThreads_ReturnsUniqueRandomNumbers()
        {
            _fixture.NumberQueue.Clear();

            Parallel.For(0, threadPoolSize, (j) =>
            {
                int rand;
                /*  Create Random Number Object for each thread. */
                IRandomNumberGenerator randomNumberGenerator = new CryptographicRandomNumberGenerator();

                for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                {
                    rand = randomNumberGenerator.Next();
                    Assert.InRange(rand, 0, int.MaxValue - 1);
                    _fixture.NumberQueue.Enqueue(rand);
                }
            });

            Assert.Equal(_fixture.NumberQueue.Count, _fixture.NumberQueue.Distinct().Count());
            _fixture.NumberQueue.Clear();
        }

        #endregion

        #region [ Function int Next(int toExclusive) Tests ]

        [Fact]
        public void NextOverloadOne_RequestRandomNumbers_ReturnsUniqueRandomNumbers()
        {
            int rand;

            foreach (IRandomNumberGenerator randomNumberGenerator
                in _fixture.RandomNumberGeneratorList)
            {
                _fixture.NumberQueue.Clear();

                for (int i = 0; i < sampleSize; i++)
                {
                    /* Returns a positive random integer that is less than the specified toExclusive. */
                    rand = randomNumberGenerator.Next(int.MaxValue);
                    /* A 32-bit signed integer that is greater than or equal to 0 and less than toExclusive. */
                    Assert.InRange(rand, 0, int.MaxValue - 1);
                    _fixture.NumberQueue.Enqueue(rand);
                }

                /* Make sure all generated random numbers are unique. */
                /* The chance of generating duplicates is very small, less than 1 %. */
                Assert.Equal(_fixture.NumberQueue.Count, _fixture.NumberQueue.Distinct().Count());
                _fixture.NumberQueue.Clear();
            }
        }

        [Fact]
        public void NextOverloadOne_RequestRandomNumberWithToExclusiveValueOfZero_ReturnsZero()
        {
            foreach (IRandomNumberGenerator randomNumberGenerator
                in _fixture.RandomNumberGeneratorList)
            {
                /* The range of return values ordinarily includes 0 but not toExclusive. 
                 * However, if toExclusive equals 0, toExclusive is returned. */
                Assert.Equal(0, randomNumberGenerator.Next(0));
            }
        }

        [Fact]
        public void NextOverloadOne_RequestRandomNumberWithToExclusiveValueOfOne_ReturnsZero()
        {
            foreach (IRandomNumberGenerator randomNumberGenerator
                in _fixture.RandomNumberGeneratorList)
            {
                Assert.Equal(0, randomNumberGenerator.Next(1));
            }
        }

        [Fact]
        /* Random values in range of [0,10] => results in Avg = 5. */
        public void NextOverloadOne_RequestRandomNumbersBetweenZeroAndTen_AverageValueReturnedIsFive()
        {
            int rand, avg;
            double average;

            foreach (IRandomNumberGenerator randomNumberGenerator
                in _fixture.RandomNumberGeneratorList)
            {
                _fixture.NumberQueue.Clear();

                for (int i = 0; i < (sampleSize * 1000); i++) /* Sufficiently large sample size. */
                {
                    rand = randomNumberGenerator.Next(11);
                    Assert.InRange(rand, 0, 10);
                    _fixture.NumberQueue.Enqueue(rand);
                }

                /* Check if boundary values were included in the generated numbers. */
                Assert.Contains(0, _fixture.NumberQueue);
                Assert.Contains(10, _fixture.NumberQueue);

                /* Validate average random value, should be five if random values 
                 * are evenly distributed. */
                average = _fixture.NumberQueue.Average();
                /* Math.Round() returns a double value that is rounded up to 
                 * the nearest integer. */
                avg = (int)Math.Round(average);
                Assert.Equal(5, avg);

                _fixture.NumberQueue.Clear();
            }
        }

        [Fact]
        /* Random values in range of [0,100] => results in Avg = 50. */
        public void NextOverloadOne_RequestRandomNumbersBetweenZeroAndHundred_AverageValueReturnedIsFifty()
        {
            int rand, avg;
            double average;

            foreach (IRandomNumberGenerator randomNumberGenerator
                in _fixture.RandomNumberGeneratorList)
            {
                _fixture.NumberQueue.Clear();

                for (int i = 0; i < (sampleSize * 1000); i++) /* Sufficiently large sample size. */
                {
                    rand = randomNumberGenerator.Next(101);
                    Assert.InRange(rand, 0, 100);
                    _fixture.NumberQueue.Enqueue(rand);
                }

                /* Check if boundary values were included in the generated numbers. */
                Assert.Contains(0, _fixture.NumberQueue);
                Assert.Contains(100, _fixture.NumberQueue);

                /* Validate average random value, should be fifty if random values  
                 * are evenly distributed. */
                average = _fixture.NumberQueue.Average();
                /* Math.Round() returns a double value that is rounded up to 
                 * the nearest integer. */
                avg = (int)Math.Round(average);
                Assert.Equal(50, avg);

                _fixture.NumberQueue.Clear();
            }
        }

        [Fact]
        /* ArgumentOutOfRangeException toExclusive value is less than zero. */
        public void NextOverloadOne_RequestRandomNumberWithToExclusiveValueOfMinusOne_ThrowsArgumentOutOfRangeException()
        {
            const string paramName = "toExclusive";
            const string msg = $"'{paramName}' must be greater than or equal to zero. (Parameter '{paramName}')";

            foreach (IRandomNumberGenerator randomNumberGenerator
                in _fixture.RandomNumberGeneratorList)
            {
                ArgumentOutOfRangeException ex =
                    Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                    {
                        randomNumberGenerator.Next(-1);
                    });

                Assert.Equal(msg, ex.Message);
            }
        }

        [Fact]
        public void NextOverloadOne_RequestRandomNumbersUsingOneRandomObjectAndTenThreads_ReturnsUniqueRandomNumbers()
        {
            foreach (IRandomNumberGenerator randomNumberGenerator
                in _fixture.RandomNumberGeneratorList)
            {
                _fixture.NumberQueue.Clear();

                Parallel.For(0, threadPoolSize, (j) =>
                {
                    int rand;

                    for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 1000 random values for duplicates. */
                    {
                        rand = randomNumberGenerator.Next(int.MaxValue);
                        Assert.InRange(rand, 0, int.MaxValue - 1);
                        _fixture.NumberQueue.Enqueue(rand);
                    }
                });

                Assert.Equal(_fixture.NumberQueue.Count, _fixture.NumberQueue.Distinct().Count());
                _fixture.NumberQueue.Clear();
            }
        }

        [Fact]
        public void PseudoRandomNumberGeneratorNextOverloadOne_RequestRandomNumbersUsingTenRandomObjectsAndTenThreads_ReturnsUniqueRandomNumbers()
        {
            _fixture.NumberQueue.Clear();

            Parallel.For(0, threadPoolSize, (j) =>
            {
                int rand;
                /*  Create Random Number Object for each thread. */
                IRandomNumberGenerator randomNumberGenerator = new PseudoRandomNumberGenerator();

                for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                {
                    rand = randomNumberGenerator.Next(int.MaxValue);
                    Assert.InRange(rand, 0, int.MaxValue - 1);
                    _fixture.NumberQueue.Enqueue(rand);
                }
            });

            Assert.Equal(_fixture.NumberQueue.Count, _fixture.NumberQueue.Distinct().Count());
            _fixture.NumberQueue.Clear();
        }

        [Fact]
        public void CryptographicRandomNumberGeneratorNextOverloadOne_RequestRandomNumbersUsingTenRandomObjectsAndTenThreads_ReturnsUniqueRandomNumbers()
        {
            _fixture.NumberQueue.Clear();

            Parallel.For(0, threadPoolSize, (j) =>
            {
                int rand;
                /*  Create Random Number Object for each thread. */
                IRandomNumberGenerator randomNumberGenerator = new CryptographicRandomNumberGenerator();

                for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                {
                    rand = randomNumberGenerator.Next(int.MaxValue);
                    Assert.InRange(rand, 0, int.MaxValue - 1);
                    _fixture.NumberQueue.Enqueue(rand);
                }
            });

            Assert.Equal(_fixture.NumberQueue.Count, _fixture.NumberQueue.Distinct().Count());
            _fixture.NumberQueue.Clear();
        }

        #endregion

        #region [ Function int Next(int fromInclusive, int toExclusive) Tests ]

        [Fact]
        public void NextOverloadTwo_RequestRandomNumbers_ReturnsUniqueRandomNumbers()
        {
            int rand;

            foreach (IRandomNumberGenerator randomNumberGenerator
                in _fixture.RandomNumberGeneratorList)
            {
                _fixture.NumberQueue.Clear();

                for (int i = 0; i < sampleSize; i++)
                {
                    /* Returns a random integer that is within a specified range. */
                    rand = randomNumberGenerator.Next(int.MinValue, int.MaxValue);
                    /* A 32-bit signed integer greater than or equal to fromInclusive and less than toExclusive. */
                    Assert.InRange(rand, int.MinValue, int.MaxValue - 1);
                    _fixture.NumberQueue.Enqueue(rand);
                }

                /* Make sure all generated random numbers are unique. */
                /* The chance of generating duplicates is very small, less than 1 %. */
                Assert.Equal(_fixture.NumberQueue.Count, _fixture.NumberQueue.Distinct().Count());
                _fixture.NumberQueue.Clear();
            }
        }

        [Fact]
        /* If fromInclusive equals toExclusive, fromInclusive is returned. */
        public void NextOverloadTwo_RequestRandomNumberWithSameToExclusiveAndFromInclusiveValue_ReturnsFromInclusiveValue()
        {
            foreach (IRandomNumberGenerator randomNumberGenerator
                     in _fixture.RandomNumberGeneratorList)
            {
                int rand;

                rand = randomNumberGenerator.Next(-1, -1);
                Assert.Equal(-1, rand);

                rand = randomNumberGenerator.Next(0, 0);
                Assert.Equal(0, rand);

                rand = randomNumberGenerator.Next(1, 1);
                Assert.Equal(1, rand);

                rand = randomNumberGenerator.Next(int.MinValue, int.MinValue);
                Assert.Equal(int.MinValue, rand);

                rand = randomNumberGenerator.Next(int.MaxValue, int.MaxValue);
                Assert.Equal(int.MaxValue, rand);
            }
        }

        [Fact]
        public void NextOverloadTwo_RequestRandomNumberWithToExclusiveAndFromInclusiveValueDifferenceOfOne_ReturnsFromInclusiveValue()
        {
            foreach (IRandomNumberGenerator randomNumberGenerator
                    in _fixture.RandomNumberGeneratorList)
            {
                int rand;

                rand = randomNumberGenerator.Next(-1, 0);
                Assert.Equal(-1, rand);

                rand = randomNumberGenerator.Next(0, 1);
                Assert.Equal(0, rand);

                rand = randomNumberGenerator.Next(1, 2);
                Assert.Equal(1, rand);

                rand = randomNumberGenerator.Next(int.MinValue, int.MinValue + 1);
                Assert.Equal(int.MinValue, rand);

                rand = randomNumberGenerator.Next(int.MaxValue - 1, int.MaxValue);
                Assert.Equal(int.MaxValue - 1, rand);
            }
        }

        [Fact]
        /* Random values in range of [-10,10] => results in Avg = 0. */
        public void NextOverloadTwo_RequestRandomNumbersBetweenMinusTenAndTen_AverageValueReturnedIsZero()
        {
            int rand, avg;
            double average;

            foreach (IRandomNumberGenerator randomNumberGenerator
                in _fixture.RandomNumberGeneratorList)
            {
                _fixture.NumberQueue.Clear();

                for (int i = 0; i < (sampleSize * 1000); i++) /* Sufficiently large sample size. */
                {
                    rand = randomNumberGenerator.Next(-10, 11);
                    Assert.InRange(rand, -10, 10);
                    _fixture.NumberQueue.Enqueue(rand);
                }

                /* Check if boundary values were included in the generated numbers. */
                Assert.Contains(-10, _fixture.NumberQueue);
                Assert.Contains(10, _fixture.NumberQueue);

                /* Validate average random value, should be zero if random values 
                 * are evenly distributed. */
                average = _fixture.NumberQueue.Average();
                /* Math.Round() returns a double value that is rounded up to 
                 * the nearest integer. */
                avg = (int)Math.Round(average);
                Assert.Equal(0, avg);

                _fixture.NumberQueue.Clear();
            }
        }

        [Fact]
        /* Random values in range of [-50,50] => results in Avg = 0. */
        public void NextOverloadTwo_RequestRandomNumbersBetweenMinusFiftyAndFifty_AverageValueReturnedIsZero()
        {
            int rand, avg;
            double average;

            foreach (IRandomNumberGenerator randomNumberGenerator
                in _fixture.RandomNumberGeneratorList)
            {
                _fixture.NumberQueue.Clear();

                for (int i = 0; i < (sampleSize * 1000); i++) /* Sufficiently large sample size. */
                {
                    rand = randomNumberGenerator.Next(-50, 51);
                    Assert.InRange(rand, -50, 50);
                    _fixture.NumberQueue.Enqueue(rand);
                }

                /* Check if boundary values were included in the generated numbers. */
                Assert.Contains(-50, _fixture.NumberQueue);
                Assert.Contains(50, _fixture.NumberQueue);

                /* Validate average random value, should be zero if random values 
                 * are evenly distributed. */
                average = _fixture.NumberQueue.Average();
                /* Math.Round() returns a double value that is rounded up to 
                 * the nearest integer. */
                avg = (int)Math.Round(average);
                Assert.Equal(0, avg);

                _fixture.NumberQueue.Clear();
            }
        }

        [Fact]
        /* Random values in range of [int.MinValue, int.MinValue + 10] => results in Avg = int.MinValue + 5. */
        public void NextOverloadTwo_RequestRandomNumbersBetweenIntMinValueAndIntMinValuePlusTen_AverageValueReturnedIsIntMinValuePlusFive()
        {
            int rand, avg;
            double average;

            foreach (IRandomNumberGenerator randomNumberGenerator
                in _fixture.RandomNumberGeneratorList)
            {
                _fixture.NumberQueue.Clear();

                for (int i = 0; i < (sampleSize * 1000); i++) /* Sufficiently large sample size. */
                {
                    rand = randomNumberGenerator.Next(int.MinValue, int.MinValue + 11);
                    Assert.InRange(rand, int.MinValue, int.MinValue + 10);
                    _fixture.NumberQueue.Enqueue(rand);
                }

                /* Check if boundary values were included in the generated numbers. */
                Assert.Contains(int.MinValue, _fixture.NumberQueue);
                Assert.Contains(int.MinValue + 10, _fixture.NumberQueue);

                /* Validate average random value, should be int.MinValue + 5 if random values 
                 * are evenly distributed. */
                average = _fixture.NumberQueue.Average();
                /* Math.Round() returns a double value that is rounded up to 
                 * the nearest integer. */
                avg = (int)Math.Round(average);
                Assert.Equal(int.MinValue + 5, avg);

                _fixture.NumberQueue.Clear();
            }
        }

        [Fact]
        /* Random values in range of [int.MaxValue - 11, int.MaxValue - 1] => results in Avg = int.MaxValue - 6. */
        public void NextOverloadTwo_RequestRandomNumbersBetweenIntMaxValueMinusElevenAndIntMaxValueMinusOne_AverageValueReturnedIsIntMaxValueMinusSix()
        {
            int rand, avg;
            double average;

            foreach (IRandomNumberGenerator randomNumberGenerator
                in _fixture.RandomNumberGeneratorList)
            {
                _fixture.NumberQueue.Clear();

                for (int i = 0; i < (sampleSize * 1000); i++) /* Sufficiently large sample size. */
                {
                    rand = randomNumberGenerator.Next(int.MaxValue - 11, int.MaxValue);
                    Assert.InRange(rand, int.MaxValue - 11, int.MaxValue - 1);
                    _fixture.NumberQueue.Enqueue(rand);
                }

                /* Check if boundary values were included in the generated numbers. */
                Assert.Contains(int.MaxValue - 11, _fixture.NumberQueue);
                Assert.Contains(int.MaxValue - 1, _fixture.NumberQueue);

                /* Validate average random value, should be int.MaxValue - 6 if random values 
                 * are evenly distributed. */
                average = _fixture.NumberQueue.Average();
                /* Math.Round() returns a double value that is rounded up to 
                 * the nearest integer. */
                avg = (int)Math.Round(average);
                Assert.Equal(int.MaxValue - 6, avg);

                _fixture.NumberQueue.Clear();
            }
        }

        [Fact]
        /* The specified range of random numbers is invalid, please specify 
         * a toExclusive value that is equal or greater than the fromInclusive value. */
        public void NextOverloadTwo_RequestRandomNumberWithToExclusiveLessThanFromInclusive_ThrowsArgumentOutOfRangeException()
        {
            const string paramName = "fromInclusive";
            const string msg = $"'{paramName}' cannot be greater than toExclusive. (Parameter '{paramName}')";

            foreach (IRandomNumberGenerator randomNumberGenerator
                 in _fixture.RandomNumberGeneratorList)
            {
                ArgumentOutOfRangeException ex =
                    Assert.Throws<ArgumentOutOfRangeException>(paramName, () =>
                    {
                        randomNumberGenerator.Next(1, 0);
                    });

                Assert.Equal(msg, ex.Message);
            }
        }

        [Fact]
        public void NextOverloadTwo_RequestRandomNumbersUsingOneRandomObjectAndTenThreads_ReturnsUniqueRandomNumbers()
        {
            foreach (IRandomNumberGenerator randomNumberGenerator
                in _fixture.RandomNumberGeneratorList)
            {
                _fixture.NumberQueue.Clear();

                Parallel.For(0, threadPoolSize, (j) =>
                {
                    int rand;

                    for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 1000 random values for duplicates. */
                    {
                        rand = randomNumberGenerator.Next(int.MinValue, int.MaxValue);
                        Assert.InRange(rand, int.MinValue, int.MaxValue - 1);
                        _fixture.NumberQueue.Enqueue(rand);
                    }
                });

                Assert.Equal(_fixture.NumberQueue.Count, _fixture.NumberQueue.Distinct().Count());
                _fixture.NumberQueue.Clear();
            }
        }

        [Fact]
        public void PseudoRandomNumberGeneratorNextOverloadTwo_RequestRandomNumbersUsingTenRandomObjectsAndTenThreads_ReturnsUniqueRandomNumbers()
        {
            _fixture.NumberQueue.Clear();

            Parallel.For(0, threadPoolSize, (j) =>
            {
                int rand;
                /*  Create Random Number Object for each thread. */
                IRandomNumberGenerator randomNumberGenerator = new PseudoRandomNumberGenerator();

                for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                {
                    rand = randomNumberGenerator.Next(int.MinValue, int.MaxValue);
                    Assert.InRange(rand, int.MinValue, int.MaxValue - 1);
                    _fixture.NumberQueue.Enqueue(rand);
                }
            });

            Assert.Equal(_fixture.NumberQueue.Count, _fixture.NumberQueue.Distinct().Count());
            _fixture.NumberQueue.Clear();
        }

        [Fact]
        public void CryptographicRandomNumberGeneratorNextOverloadTwo_RequestRandomNumbersUsingTenRandomObjectsAndTenThreads_ReturnsUniqueRandomNumbers()
        {
            _fixture.NumberQueue.Clear();

            Parallel.For(0, threadPoolSize, (j) =>
            {
                int rand;
                /*  Create Random Number Object for each thread. */
                IRandomNumberGenerator randomNumberGenerator = new CryptographicRandomNumberGenerator();

                for (int i = 0; i < (sampleSize / threadPoolSize); i++) /* Check 100 random values for duplicates. */
                {
                    rand = randomNumberGenerator.Next(int.MinValue, int.MaxValue);
                    Assert.InRange(rand, int.MinValue, int.MaxValue - 1);
                    _fixture.NumberQueue.Enqueue(rand);
                }
            });

            Assert.Equal(_fixture.NumberQueue.Count, _fixture.NumberQueue.Distinct().Count());
            _fixture.NumberQueue.Clear();
        }


        #endregion
    }
}
