using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CipherMachine;
using Words;

public class JCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Branded Pen Cipher" : "Branded Pen Cipher"; } }
    public override string Code { get { return "BP"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public JCipher(bool invert) { this.invert = invert; }

    private readonly int[] fixedLens = { 8, 8, 8, 6, 5, 4, 3, 3 };

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        var wordList = new Data();
        string kw1 = wordList.PickWord(4, 8);
        string kw2 = wordList.PickWord(fixedLens[word.Length - 1]);
        string[] key = new string[kw2.Length];
        logMessages.Add(string.Format("KW1: {0}", kw1));
        logMessages.Add(string.Format("KW2: {0}", kw2));
        for (int i = 0; i < key.Length; i++)
        {
            key[i] = CMTools.getKey(kw1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", true);
            int index = key[i].IndexOf(kw2[i]);
            key[i] = key[i].Substring(index) + key[i].Substring(0, index);
            logMessages.Add(string.Format("{0}", key[i]));
        }
       
        string encrypt = "", alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        if (invert)
        {
            for (int i = 0; i < word.Length; i++)
            {
                char let = word[i];
                string logMessage = let + "";
                for (int j = 0; j < key.Length; j++)
                {
                    let = key[j][alpha.IndexOf(let)];
                    logMessage += " -> " + let;
                }
                encrypt = encrypt + "" + let;
                logMessages.Add(string.Format("{0}", logMessage));
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                char let = word[i];
                string logMessage = let + "";
                for (int j = 0; j < key.Length; j++)
                {
                    let = alpha[key[key.Length - j - 1].IndexOf(let)];
                    logMessage += " -> " + let;
                }
                encrypt = encrypt + "" + let;
                logMessages.Add(string.Format("{0}", logMessage));
            }
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw1, null, kw2 }, invert) },
            Score = 6
        };
    }
}
