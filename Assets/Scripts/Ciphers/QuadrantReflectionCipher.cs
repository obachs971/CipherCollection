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
        string[] keyStrings = CipherHelper.GetKeystrings();
        string[] removedLetters = CipherHelper.GetRemovedLetters();
        bool[] keystringOrders = CipherHelper.GetKeystringOrder();

        string[] letteredScreenInfo = Enumerable.Range(0, keywords.Length).Select(x => (keystringOrders[x] ? "0" : "1") + removedLetters[x]).ToArray();
        letteredScreenInfo[CipherHelper.StartingQuadrant] += "*";

        encrypt = invert ? CipherHelper.Decrypt(word) : CipherHelper.Encrypt(word);

        Enumerable.Range(0, keywords.Length).ForEach(x => LogMessages.Add("Keyword #" + (x + 1).ToString() + ": " + keywords[x]));
        Enumerable.Range(0, keyStrings.Length).ForEach(x => LogMessages.Add("Keystring #" + (x + 1).ToString() + ": " + keyStrings[x]));
        LogMessages.Add("Removed letters, in order: " + removedLetters.Join(""));
        LogMessages.Add("Encrypted word: " + encrypt);


        return new ResultInfo
        {
            LogMessages = LogMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { keywords[0], letteredScreenInfo[0], keywords[1], letteredScreenInfo[1], keywords[2], letteredScreenInfo[2], keywords[3], letteredScreenInfo[3] }, invert) }
        };
    }
}
