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
        string kw = new Data().PickWord(3, word.Length - 1);
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
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw }, invert) }
        };
    }
}
