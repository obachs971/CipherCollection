using System.Collections.Generic;
using CipherMachine;
using Words;

public class StrangelyElusiveLetterCipher : CipherBase
{
    public override string Name { get { return "Strangely Elusive Letter Cipher"; } }
    public override string Code { get { return "EL"; } }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        Data data = new Data();
        string kw1 = data.PickWord(4, 8);
        var kwfront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront.Value) + "#";
    tryagain:
        string encrypt = "";
        var logMessages = new List<string>();
        string kw2 = data.PickWord(3, word.Length);
        logMessages.Add(string.Format("Screen 1: {0}", kw1));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", kwfront.Expression, kwfront.Value));
        logMessages.Add(string.Format("Key: {0}", key));
        logMessages.Add(string.Format("Screen 2: {0}", kw2));
        for (int i = 0; i < word.Length; i++)
        {
            int[] indexes = { key.IndexOf(word[i]), key.IndexOf(kw2[i % kw2.Length]) }, cur = new int[3];
            cur[0] = ((indexes[0] / 9) == (indexes[1] / 9)) ? indexes[0] / 9 : 3 - ((indexes[0] / 9) + (indexes[1] / 9));
            cur[1] = (((indexes[0] / 3) % 3) == (((indexes[1] / 3) % 3))) ? ((indexes[0] / 3) % 3) : 3 - (((indexes[0] / 3) % 3) + (((indexes[1] / 3) % 3)));
            cur[2] = ((indexes[0] % 3) == (indexes[1] % 3)) ? indexes[0] % 3 : 3 - ((indexes[0] % 3) + (indexes[1] % 3));
            encrypt = encrypt + "" + key[(cur[0] * 9) + (cur[1] * 3) + cur[2]];
            encrypt = encrypt.Replace('#', word[i]);
            logMessages.Add(string.Format("{0} + {1} -> {2}", word[i], kw2[i % kw2.Length], encrypt[i]));
        }
        if (word.Equals(encrypt)) goto tryagain;
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw1, kwfront.Expression, kw2 }) },
            Score = 7
        };
    }
}
