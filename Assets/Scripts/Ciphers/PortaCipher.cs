using System.Collections.Generic;
using CipherMachine;
using Words;

public class PortaCipher : CipherBase
{
    public override string Name { get { return "Porta Cipher"; } }
    public override int Score(int wordLength) { return 4; }
    public override string Code { get { return "PO"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string kw = new Data().PickWord(4, word.Length);
        logMessages.Add(string.Format("Keyword: {0}", kw));
        string encrypt = "", alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        for (int i = 0; i < word.Length; i++)
        {
            int index = alpha.IndexOf(kw[i % kw.Length]) / 2;
            string temp = alpha.Substring(0, 13) + alpha.Substring(13 + index) + alpha.Substring(13, index);
            encrypt = encrypt + "" + temp[(temp.IndexOf(word[i]) + 13) % 26];
        }
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));

        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw }) }
        };
    }
}
