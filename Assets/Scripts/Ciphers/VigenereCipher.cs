using System.Collections.Generic;
using CipherMachine;
using Words;

public class VigenereCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Vigenère Cipher" : "Vigenère Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "VI"; } }

    private readonly bool invert;
    public VigenereCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string keyword = new Data().PickWord(3, word.Length);
        string encrypt = "";
        string alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY";
        logMessages.Add(string.Format("Keyword: {0}", keyword));
        if (invert)
        {
            for (int i = 0; i < word.Length; i++)
                encrypt = encrypt + "" + alpha[CMTools.mod(alpha.IndexOf(word[i]) - alpha.IndexOf(keyword[i % keyword.Length]), 26)];
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
                encrypt = encrypt + "" + alpha[CMTools.mod(alpha.IndexOf(word[i]) + alpha.IndexOf(keyword[i % keyword.Length]), 26)];
        }
        logMessages.Add(string.Format("{0} + {1} -> {2}", word, keyword, encrypt));
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(keyword, new int[] { 35, 35, 35, 35, 32, 28 }[keyword.Length - 3]);
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
}
