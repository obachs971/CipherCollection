using System;
using System.Collections.Generic;
using CipherMachine;
using Words;

public class RozierCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Rozier Cipher" : "Rozier Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "RZ"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public RozierCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY", encrypt = "";
        string kw = new Data().PickWord(3, Math.Min(8, word.Length + 1));
        logMessages.Add(string.Format("Keyword: {0}", kw));
        if (invert)
        {
            for (int i = 0; i < word.Length; i++)
            {
                encrypt = encrypt + "" + alpha[CMTools.mod(alpha.IndexOf(word[i]) - (alpha.IndexOf(kw[(i + 1) % kw.Length]) - alpha.IndexOf(kw[i % kw.Length])), 26)];
                logMessages.Add(string.Format("{0} - ({1} - {2}) -> {3}", word[i], kw[(i + 1) % kw.Length], kw[i % kw.Length], encrypt[i]));
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                encrypt = encrypt + "" + alpha[CMTools.mod(alpha.IndexOf(word[i]) + (alpha.IndexOf(kw[(i + 1) % kw.Length]) - alpha.IndexOf(kw[i % kw.Length])), 26)];
                logMessages.Add(string.Format("{0} + ({1} - {2}) -> {3}", word[i], kw[(i + 1) % kw.Length], kw[i % kw.Length], encrypt[i]));
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
