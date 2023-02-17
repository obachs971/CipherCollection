using System.Collections.Generic;
using CipherMachine;

public class PortaxCipher : CipherBase
{
    public override string Name { get { return "Portax Cipher"; } }
    public override string Code { get { return "PX"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
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
        if (word.Length % 2 == 1)
            c[c.Length - 1] = word[word.Length - 1];
        string encrypt = new string(c);
        logMessages.Add(string.Format("{0} - > {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { key }) },
            Score = 6
        };
    }
}
