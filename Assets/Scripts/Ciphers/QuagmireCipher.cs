using System.Collections.Generic;
using CipherMachine;
using Words;

public class QuagmireCipher : CipherBase
{
	public override string Name { get { return invert ? "Inverted Quagmire Cipher" : "Quagmire Cipher"; } }
	public override int Score { get { return 5; } }
	public override string Code { get { return "QU"; } }
    
    private readonly bool invert;
    public QuagmireCipher(bool invert) { this.invert = invert; }
    
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        var wordList = new Data();
        string kw1 = wordList.PickWord(4, 8);
        string kw2 = wordList.PickWord(4, word.Length);
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
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(kw1, new int[] { 35, 35, 35, 32, 28 }[kw1.Length - 4]);
        screens[2] = new ScreenInfo(kw2, new int[] { 35, 35, 35, 32, 28 }[kw2.Length - 4]);
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
}