using System.Collections.Generic;
using CipherMachine;
using Words;

public class PortaCipher : CipherBase
{
    public override string Name { get { return "Porta Cipher"; } }
    public override int Score { get { return 5; } }
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

        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens) }
        };
    }
}