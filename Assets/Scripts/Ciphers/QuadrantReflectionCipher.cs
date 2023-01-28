using CipherMachine;
using KeepCoding;
using System.Collections.Generic;
using System.Linq;

internal class QuadrantReflectionCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Quadrant Reflection Cipher" : "Quadrant Reflection Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "QR"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public QuadrantReflectionCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        List<string> LogMessages = new List<string>();
        string encrypt = "";

        QuadrantReflectionCipherHelper CipherHelper = new QuadrantReflectionCipherHelper();

        string[] keywords = CipherHelper.GetKeywords();
        string[] keyStrings = CipherHelper.GetKeyStrings();
        string[] removedLetters = CipherHelper.GetRemovedLetters();

        encrypt = invert ? CipherHelper.Decrypt(word) : CipherHelper.Encrypt(word);

        Enumerable.Range(0, keywords.Length).ForEach(x => LogMessages.Add("Keyword #" + (x + 1).ToString() + ": " + keywords[x]));
        Enumerable.Range(0, keyStrings.Length).ForEach(x => LogMessages.Add("Keystring #" + (x + 1).ToString() + ": " + keyStrings[x]));
        LogMessages.Add("Removed letters, in order: " + removedLetters.Join(""));
        LogMessages.Add("Encrypted word: " + encrypt);

        return new ResultInfo
        {
            LogMessages = LogMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { keywords[0], removedLetters[0] + (CipherHelper.StartingQuadrant == 0 ? "*" : ""), keywords[1], removedLetters[1] + (CipherHelper.StartingQuadrant == 1 ? "*" : ""), keywords[2], removedLetters[2] + (CipherHelper.StartingQuadrant == 2 ? "*" : ""), keywords[3], removedLetters[3] + (CipherHelper.StartingQuadrant == 3 ? "*" : "") }, invert) }
        };
    }
}
