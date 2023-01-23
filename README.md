# Random string generator

This is a .NET Core 6 Library for generating random strings with full customizable settings to set the number of upper, lower, numeric, and special characters along with the length of the string. This library can be used for various applications such as random password generation.

## Usage

The library offers two different random string generators, allowing for control over the type of randomness used.

### Pseudo Random

The pseudo random string generator uses the `System.Random.Shared` class, which is fast but less secure for generating random strings. Therefore, it should not be used for generating passwords.

### Cryptographic Random

The random string generator using the `System.Security.Cryptography.RandomNumberGenerator` class is considered more secure due to its use of a cryptographic random number generator, which provides higher security compared to the `System.Random.Shared` class. However, its performance may not be as fast as `System.Random.Shared` class.

### Code Samples

```cs

// Get cryptographic random string generator.
IRandomStringGenerator cryptographicRandomStringGenerator = RandomStringGenerator.CryptographicRandomizer;

// Get pseudo random string generator.
IRandomStringGenerator pseudoRandomStringGenerator = RandomStringGenerator.PseudoRandomizer;

// Controlling the allowed characters in the generated random string is achieved using the enum AllowedChars.
// In this example only AlphaNumeric (Letters | Digits) characters are used.
bool excludeSimilarLookingChars = false;
int length = 6;
string rand = cryptographicRandomStringGenerator.GenerateString(AllowedChars.AlphaNumeric, length, excludeSimilarLookingChars);
Console.WriteLine(rand);

// For a more fine grained control you can always pass an array of allowed characters.
int length = 6;
string rand = cryptographicRandomStringGenerator.GenerateString(new char[] { 'w', 'o', 'r', 'l', 'd' }, length);
Console.WriteLine(rand);

//Randomly shuffle string
string rand = cryptographicRandomStringGenerator.ShuffleString("The Netherlands");
Console.WriteLine(rand);

```

Notes:

- `excludeSimilarLookingChars` is handy when you have to exclude characters
  like 'l' (lower-case L), '1' (one), '|' (pipe), 'I' (upper-case i) and others
  to improves string readability.
