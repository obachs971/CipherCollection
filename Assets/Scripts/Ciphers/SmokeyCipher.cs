using System.Collections.Generic;
using CipherMachine;
using Words;

public class SmokeyCipher : CipherBase
{
    public override string Name { get { return "Smokey Cipher"; } }
    public override int Score(int wordLength) { return 7; }
    public override string Code { get { return "SM"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string encrypt = "";
        string kw = new Data().PickWord(4, 8);
        var kwfront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront.Value);
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", kwfront.Expression, kwfront.Value));
        foreach(char letter in word)
        {
            int index = key.IndexOf(letter);
            encrypt = encrypt + "" + key[(index + 13) % 26];
            logMessages.Add(string.Format("{0}", key));
            logMessages.Add(string.Format("{0} -> {1}", letter, encrypt[encrypt.Length - 1]));
            //Shifting key
            key = key.Replace(letter, '*').Replace(encrypt[encrypt.Length - 1], letter).Replace('*', encrypt[encrypt.Length - 1]);
            string k1 = key.Substring((index / 13) * 13, 13);
            k1 = k1.Substring(index % 13) + k1.Substring(0, index % 13);
            index = (index + 13) % 26;
            string k2 = key.Substring((index / 13) * 13, 13);
            k2 = k2.Substring((index % 13) + 1) + k2.Substring(0, (index % 13) + 1);
            key = index >= 13 ? k1 + k2 : k2 + k1;
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, kwfront.Expression }) }
        };
    }
}
