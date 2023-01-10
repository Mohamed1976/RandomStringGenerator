
namespace RandomStringGeneratorLib.RandomStringGenerators
{
    #region [ enum AllowedChars ]

    /// <summary>
    /// Charsets can be composed of the following characters:  
    /// </summary>
    [Flags]
    public enum AllowedChars : byte
    {
        /// <summary>
        /// No characters
        /// </summary>
        None = 0x00,
        /// <summary>
        /// Latin upper-case characters A,B,C ... (Count: 26)
        /// </summary>
        UpperCaseLetters = 0x01,
        /// <summary>
        /// Latin lower-case characters a,b,c ... (Count: 26)
        /// </summary>
        LowerCaseLetters = 0x02,
        /// <summary>
        /// All latin upper-case and lower-case characters (Count: 52)
        /// </summary>
        Letters = UpperCaseLetters | LowerCaseLetters,
        /// <summary>
        /// Digits 0,1,2,3 ... (Count: 10)
        /// </summary>
        Digits = 0x04,
        /// <summary>
        /// All latin upper-case, lower-case characters and digits from 0 to 9. (Count: 62)
        /// </summary>
        AlphaNumeric = Letters | Digits,
        /// <summary>
        /// All readable special ascii characters - '!', '&quot;', '#', '$', '%', '&amp;', ''', '*', '+', ',', '.', '/', ':', ';', '=', '?', '@', '\', '^', '´', '`', '|', '~' (Count: 23)
        /// </summary>
        SpecialChars = 0x08,
        /// <summary>
        /// The minus ('-') character (Count: 1)
        /// </summary>
        Minus = 0x10,
        /// <summary>
        /// The underscore ('_') character (Count: 1)
        /// </summary>        
        Underscore = 0x20,
        /// <summary>
        /// The space (' ') character (Count: 1)
        /// </summary>
        Space = 0x40,
        /// <summary>
        /// All bracket characters '&lt;', '&gt;', '{', '}', '[', ']', '(', ')' (Count: 8)
        /// </summary>
        Brackets = 0x80,
        /// <summary>
        /// All readable ascii characters (code 32 till code 126). (Count: 96)
        /// </summary>
        All = AlphaNumeric | Minus | Underscore | Space | Brackets | SpecialChars,
        /// <summary>
        /// 64 url safe characters (A-Z,a-z,0-9 and '-','_'). (Count: 64)
        /// </summary>
        UrlSafeChars = AlphaNumeric | Minus | Underscore,
        /// <summary>
        /// The characters usable to create names for files or directories (A-Z,a-z,0-9 and '-','_'). (Count: 64)
        /// </summary>
        FileSystemSafeChars = AlphaNumeric | Minus | Underscore
    }

    #endregion
}
