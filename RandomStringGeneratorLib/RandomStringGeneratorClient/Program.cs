// See https://aka.ms/new-console-template for more information
using RandomStringGeneratorLib.RandomNumberGenerators;
using RandomStringGeneratorLib.RandomStringGenerators;
using System.Collections.Concurrent;
using System.Diagnostics;

/* Usage examples  */
bool excludeSimilarLookingChars;
string rand;
int length;

//Get cryptographic random string generator.
IRandomStringGenerator cryptographicRandomStringGenerator = RandomStringGenerator.CryptographicRandomizer;

//Get pseudo random string generator.
IRandomStringGenerator pseudoRandomStringGenerator = RandomStringGenerator.PseudoRandomizer;

// Controlling the allowed characters in the generated random string is achieved using the enum AllowedChars.
// In this example only AlphaNumeric (Letters | Digits) characters are used.
excludeSimilarLookingChars = false;
length = 6;
rand = cryptographicRandomStringGenerator.GenerateString(AllowedChars.AlphaNumeric, length, excludeSimilarLookingChars);
Console.WriteLine(rand);

// For a more advanced scenario you can specify the minimum number of uppercase, lowercase, digits and special characters in the generated random string.  
// The extraLength parameter in combination with the extraAllowedChars parameter allow you to add extra characters to the random string. 
int minUpperCaseLetters = 1;
int minLowerCaseLetters = 2;
int minDigits = 3;
int minSpecialChars = 4;
excludeSimilarLookingChars = false;
int extraLength = 3;
AllowedChars extraAllowedChars = AllowedChars.All;
rand = cryptographicRandomStringGenerator.GenerateString(
    minUpperCaseLetters,
    minLowerCaseLetters,
    minDigits,
    minSpecialChars,
    excludeSimilarLookingChars,
    extraLength,
    extraAllowedChars);
Console.WriteLine(rand);

length = 6;
// For a more fine grained control you can always pass an array of allowed characters.
rand = cryptographicRandomStringGenerator.GenerateString(new char[] { 'w', 'o', 'r', 'l', 'd' }, length);
Console.WriteLine(rand);

//Randomly shuffle string
rand = cryptographicRandomStringGenerator.ShuffleString("The Netherlands");
Console.WriteLine(rand);

/* Distributions trails */
Console.WriteLine("PseudoRandomNumberGenerator");
RandomNumberGeneratorComparisonDistributions(
    new PseudoRandomNumberGenerator(), 1_000_000, 0, 100);
Console.WriteLine("CryptographicRandomNumberGenerator");
RandomNumberGeneratorComparisonDistributions(
    new CryptographicRandomNumberGenerator(), 1_000_000, 0, 100);

/* Timing trails */
RandomNumberGeneratorComparisonTimings(
    new PseudoRandomNumberGenerator(), 1_000_000);
RandomNumberGeneratorComparisonTimings(
    new CryptographicRandomNumberGenerator(), 1_000_000);

/* Uniqueness trails 
 * Reference: https://hanson.io/dotnet-random-string-generator/ */
const string AlphaNumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

Console.WriteLine("RandomStringGenerator.PseudoRandomizer:");
UniquenessCheck(RandomStringGenerator.PseudoRandomizer,
    100, 100_000, 4, AlphaNumeric.ToCharArray(), 6);

Console.WriteLine("RandomStringGenerator.CryptographicRandomizer:");
UniquenessCheck(RandomStringGenerator.CryptographicRandomizer,
    100, 100_000, 4, AlphaNumeric.ToCharArray(), 6);

/* Testing implementations of the Fisher-Yates shuffling algorithm. 
 * Reference: https://peaku.co/questions/4722-%C2%BFerror-en-la-clase-%60random%60-de-net */
const int shuffles = 10_000_000;
int[] sum = new int[100]; /* The elements of the array are initialized to the default value of the element type, 0 for integers. */
/* 10_000_000 shuffles using the two Fisher-Yates shuffling algorithms. */
for (int i = 0; i < shuffles; i++)
{
    int[] sequence = Enumerable.Range(1, 100).ToArray();
    //sequence = FisherYatesBad(sequence);
    sequence = FisherYates(sequence);
    /* Sum all values. */
    for (int j = 0; j < sequence.Length; j++)
    {
        sum[j] += sequence[j];
    }
}

for (int i = 0; i < sum.Length; i++)
{
    Console.WriteLine($"{i + 1}, {(double)sum[i] / shuffles}");
}

return 0;





static int[] FisherYates(int[] source)
{
    Random random = new Random();

    int[] output = source.ToArray();
    for (var i = 0; i < output.Length; i++)
    {
        var j = random.Next(i, output.Length);
        (output[i], output[j]) = (output[j], output[i]);
    }
    return output;
}

static int[] FisherYatesBad(int[] source)
{
    Random random = new Random();

    int[] output = source.ToArray();
    for (var i = 0; i < output.Length; i++)
    {
        var j = random.Next(0, output.Length);
        (output[i], output[j]) = (output[j], output[i]);
    }
    return output;
}

static int UniquenessCheck(IRandomStringGenerator randomStringGenerator,
    int repeat, int iterations, int threadPoolSize, char[] allowedChars, int length)
{
    ConcurrentQueue<string> stringQueue = new ConcurrentQueue<string>();
    int duplicates = 0;

    for (int j = 0; j < repeat; j++)
    {
        Parallel.For(0, threadPoolSize, (k) =>
        {
            for (int i = 0; i < (iterations / threadPoolSize); i++)
            {
                string rand = randomStringGenerator.GenerateString(allowedChars, length);
                stringQueue.Enqueue(rand);
            }
        });

        /* Count the number of duplicate strings in collection. */
        if (stringQueue.Count != stringQueue.Distinct().Count())
        {
            Console.WriteLine("Duplicates detected: idx[{0}], Count[{1}], Distinct[{2}]", j, stringQueue.Count, stringQueue.Distinct().Count());
            duplicates++;
        }

        stringQueue.Clear();
    }

    Console.WriteLine("Total number of duplicates: {0}, {1}%", duplicates, (double)duplicates * 100 / repeat);

    return 0;
}

static int RandomNumberGeneratorComparisonDistributions(
    IRandomNumberGenerator randomNumberGenerator,
    int iterations, int min, int max)
{
    int value;

    /* Generate random numbers in range and check frequency. */
    int[] range = new int[max - min];

    for (int i = 0; i < iterations; i++)
    {
        value = randomNumberGenerator.Next(min, max);
        range[value]++;
    }

    for (int j = 0; j < range.Length; j++)
    {
        Console.WriteLine($"{j}, {range[j]}, {(double)range[j] * 100 / iterations}%");
    }

    return 0;
}

static int RandomNumberGeneratorComparisonTimings(
    IRandomNumberGenerator randomNumberGenerator, int iterations)
{
    var watch = new Stopwatch();
    watch.Start();
    for (int i = 0; i < iterations; i++)
    {
        randomNumberGenerator.Next(int.MinValue, int.MaxValue);
    }
    watch.Stop();

    Console.WriteLine($"{randomNumberGenerator.GetType()}, Random took {watch.Elapsed.TotalMilliseconds} ms");

    return 0;
}