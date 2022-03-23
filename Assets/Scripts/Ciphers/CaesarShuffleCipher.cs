using System.Collections.Generic;
using CipherMachine;
using Words;

public class CaesarShuffleCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Caesar Shuffle Cipher" : "Caesar Shuffle Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "CS"; } }

    private readonly bool invert;
    public CaesarShuffleCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "-ABCDEFGHIJKLMNOPQRSTUVWXYZ", encrypt = word.ToUpperInvariant();
        var wordList = new Data();
        string kwa = wordList.PickWord(12 - word.Length);
        string kwb = wordList.PickWord(12 - word.Length);
        logMessages.Add(string.Format("Screen 1: {0}", kwa));
        logMessages.Add(string.Format("Screen 2: {0}", kwb));
        if (invert)
        {
            for (int aa = 0; aa < kwa.Length; aa++)
            {
                int index = (alpha.IndexOf(kwa[aa]) % (word.Length - 1)) + 1;
                logMessages.Add(string.Format("{0} -> {1}", kwa[aa], index));
                string[] s = { encrypt.Substring(index), encrypt.Substring(0, index) };
                logMessages.Add(string.Format("{0}|{1}", s[1], s[0]));
                logMessages.Add(string.Format("{0}|{1}", s[0], s[1]));
                encrypt = "";
                for (int bb = 0; bb < s[0].Length; bb++)
                {
                    index = alpha.IndexOf(s[0][bb]) + alpha.IndexOf(kwb[aa]);
                    if (index > 26)
                        index -= 26;
                    encrypt += alpha[index];
                }
                encrypt += s[1];
                logMessages.Add(string.Format("{0}|{1} - {2} -> {3}", s[0], s[1], kwb[aa], encrypt));
            }
        }
        else
        {
            for (int aa = (kwa.Length - 1); aa >= 0; aa--)
            {
                int index = (word.Length - 1) - (alpha.IndexOf(kwa[aa]) % (word.Length - 1));
                logMessages.Add(string.Format("{0} -> {1}", kwa[aa], index));
                string[] s = { encrypt.Substring(index), encrypt.Substring(0, index) };
                logMessages.Add(string.Format("{0}|{1}", s[1], s[0]));
                logMessages.Add(string.Format("{0}|{1}", s[0], s[1]));
                encrypt = "";
                for (int bb = 0; bb < s[1].Length; bb++)
                {
                    index = alpha.IndexOf(s[1][bb]) - alpha.IndexOf(kwb[aa]);
                    if (index < 1)
                        index += 26;
                    encrypt += alpha[index];
                }
                encrypt = s[0] + encrypt;
                logMessages.Add(string.Format("{0}|{1} + {2} -> {3}", s[0], s[1], kwb[aa], encrypt));
            }
        }
        logMessages.Add(string.Format("{0} - > {1}", word, encrypt));
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(kwa, new int[] { 35, 35, 35, 32, 28 }[kwa.Length - 4]);
        screens[2] = new ScreenInfo(kwb, new int[] { 35, 35, 35, 32, 28 }[kwb.Length - 4]);
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
}