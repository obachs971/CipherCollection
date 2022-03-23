using System.Collections.Generic;
using CipherMachine;
using Words;

public class AutokeyCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Autokey Cipher" : "Autokey Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "AK"; } }

    private readonly bool invert;
    public AutokeyCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY", encrypt = "";
        string kw = new Data().PickWord(word.Length);
        logMessages.Add(string.Format("Keyword: {0}", kw));
        if (invert)
        {
            string key = kw.ToUpperInvariant();
            for (int i = 0; i < word.Length; i++)
            {
                encrypt = encrypt + "" + alpha[CMTools.mod(alpha.IndexOf(word[i]) - alpha.IndexOf(key[i]), 26)];
                key = key + "" + encrypt[i];
                logMessages.Add(string.Format("{0} - {1} -> {2}", word[i], key[i], encrypt[i]));
            }
        }
        else
        {
            string key = kw + word;
            for (int i = 0; i < word.Length; i++)
            {
                encrypt = encrypt + "" + alpha[CMTools.mod(alpha.IndexOf(word[i]) + alpha.IndexOf(key[i]), 26)];
                logMessages.Add(string.Format("{0} + {1} -> {2}", word[i], key[i], encrypt[i]));
            }

        }
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 35, 32, 28 }[kw.Length - 3]);
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
}