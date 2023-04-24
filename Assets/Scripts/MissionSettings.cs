enum CipherSetting
{
    Random,
    DecryptOnly,
    EncryptOnly
}

enum CipherOrder
{
    Random,
    Fixed,  // completely fixed
    FixInner,   // contents of parentheses are fixed, but order of parentheses can be shuffled
    FixOuter    // contents of parentheses can be shuffled, but order of parentheses is fixed
}

struct CipherConfig
{
    // Two-letter code of cipher to use
    public string Code { get; private set; }

    // Use decrypt-only/encrypt-only/random
    public CipherSetting Setting { get; private set; }

    public CipherConfig(string code, CipherSetting setting)
    {
        Code = code;
        Setting = setting;
    }
}

sealed class MissionSettings
{
    public CipherConfig[][] Ciphers;

    // Determines whether to randomize the order in which the ciphers are used on the module
    public CipherOrder Order = CipherOrder.Random;

    // Determines the number of parentheses (groups of ciphers) to pick from Ciphers above
    public int? PickOuter;

    // Determines the number of ciphers to pick from each group (parenthesis)
    public int? PickInner;

    // Determines which lengths of solution word can be used
    public int[] WordLengths;
}
