using System.Collections.Generic;
using CipherMachine;
using UnityEngine;
using Words;

public class SlidefairCipher : CipherBase
{
    public override string Name { get { return "Slidefair Cipher"; } }
    public override string Code { get { return "SF"; } }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string[] alphas = { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ" };
        string key = "", encrypt = "";
        for (int i = 0; i < word.Length / 2; i++)
            key = key + "" + alphas[0][Random.Range(1, 26)];
        logMessages.Add(string.Format("Key: {0}", key));
        for (int i = 0; i < word.Length / 2; i++)
        {
            int n1 = alphas[1].IndexOf(key[i]);
            alphas[1] = alphas[1].Substring(n1) + alphas[1].Substring(0, n1);
            n1 = alphas[0].IndexOf(word[i * 2]);
            int n2 = alphas[1].IndexOf(word[i * 2 + 1]);
            if (n1 == n2)
            {
                n1 = 25 - n1;
                n2 = 25 - n2;
            }
            else
            {
                int temp = n1;
                n1 = n2;
                n2 = temp;
            }
            encrypt = encrypt + "" + alphas[0][n1] + "" + alphas[1][n2];
            logMessages.Add(string.Format("{0}{1} -> {2}{3}", word[i * 2], word[i * 2 + 1], encrypt[i * 2], encrypt[i * 2 + 1]));
        }
        if (word.Length % 2 == 1)
            encrypt = encrypt + "" + word[word.Length - 1];
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { key }) },
            Score = 5
        };
    }
}
