using System.Collections.Generic;
using CipherMachine;

public class PortaxCipher : CipherBase
{
    public override string Name { get { return "Portax Cipher"; } }
    public override string Code { get { return "PX"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        int pos = -1;
        string sca = "";
        if(word.Length % 2 == 1)
        {
            pos = UnityEngine.Random.Range(0, word.Length);
            sca = word[pos] + "";
            word = word.Substring(0, pos) + word.Substring(pos + 1);
            logMessages.Add(string.Format("{0}{1}{2} + {3} -> {0}{2}", word.Substring(0, pos), sca, word.Substring(pos), (pos + 1)));
        }
        string key = new string("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Shuffle()).Substring(0, word.Length / 2);
        logMessages.Add(string.Format("Key: {0}", key));
        char[] c = new char[word.Length];
        for (int i = 0; i < key.Length; i++)
        {
            string top = "NOPQRSTUVWXYZ", btm = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int index = btm.IndexOf(key[i]);
            top = "ABCDEFGHIJKLM" + top.Substring(index / 2) + top.Substring(0, index / 2);
            btm = btm.Substring(index - (index % 2)) + btm.Substring(0, index - (index % 2));
            logMessages.Add(top);
            logMessages.Add(btm);
            int n1 = top.IndexOf(word[i]), n2 = btm.IndexOf(word[i + (word.Length / 2)]);
            if ((n1 % 13) == (n2 / 2))
            {
                n1 = (n1 + 13) % 26;
                n2 = (n2 % 2 == 0) ? n2 + 1 : n2 - 1;
            }
            else
            {
                int temp = (n2 / 2) + ((n1 / 13) * 13);
                n2 = ((n1 % 13) * 2) + (n2 % 2);
                n1 = temp;
            }
            c[i] = top[n1]; c[i + (c.Length / 2)] = btm[n2];
            logMessages.Add(string.Format("{0}{1} -> {2}{3}", word[i], word[i + (word.Length / 2)], c[i], c[i + (c.Length / 2)]));
        }
        string encrypt = new string(c);
        if (pos >= 0)
        {
            logMessages.Add(string.Format("{0}{2} + {3} + {1} -> {0}{1}{2}", encrypt.Substring(0, pos), sca, encrypt.Substring(pos), (pos + 1)));
            encrypt = encrypt.Substring(0, pos) + sca + encrypt.Substring(pos);
            sca = (pos + 1) + "";
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { key, sca }) },
            Score = 6
        };
    }
}
