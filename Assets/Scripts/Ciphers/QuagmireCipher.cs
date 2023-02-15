using System.Collections.Generic;
using CipherMachine;
using Words;

public class QuagmireCipher : CipherBase
{
	public override string Name { get { return invert ? "Inverted Quagmire Cipher" : "Quagmire Cipher"; } }
	public override int Score(int wordLength) { return 6; }
	public override string Code { get { return "QU"; } }
    
    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public QuagmireCipher(bool invert) { this.invert = invert; }
    
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        var wordList = new Data();
        string kw1 = wordList.PickWord(4, 8);
        string kw2 = wordList.PickWord(3, word.Length);
        string[] key = new string[kw2.Length];
        for (int i = 0; i < key.Length; i++)
        {
            key[i] = CMTools.getKey(kw1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", true);
            int index = key[i].IndexOf(kw2[i]);
            key[i] = key[i].Substring(index) + key[i].Substring(0, index);
        }
        logMessages.Add(string.Format("KW1: {0}", kw1));
        logMessages.Add(string.Format("KW2: {0}", kw2));
        string encrypt = "", alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        if (invert)
        {
            for (int i = 0; i < word.Length; i++)
                encrypt = encrypt + "" + alpha[key[i % key.Length].IndexOf(word[i])];
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
                encrypt = encrypt + "" + key[i % key.Length][alpha.IndexOf(word[i])];
        }
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw1, null, kw2 }, invert) }
        };
    }
}
