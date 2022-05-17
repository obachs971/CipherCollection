enum CipherSetting
{
    Random,
    DecryptOnly,
    EncryptOnly
}

enum CipherOrder
{
    Random,
    Fixed
}

sealed class MissionSettings
{
    // Two-letter codes of ciphers to use
    public string[] Ciphers;

    // Use decrypt-only/encrypt-only/random. Must have same length as Ciphers above.
    public CipherSetting[] CipherSettings;

    // Determines whether to randomize the order in which the ciphers are used on the module
    public CipherOrder Order = CipherOrder.Random;

    // Determines the number of ciphers to pick from Ciphers above
    public int? Pick;

    // Determines which lengths of solution word can be used
    public int[] WordLengths;
}
