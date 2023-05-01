using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;
using Rnd = UnityEngine.Random;
public class SuperpositionCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Superposition Cipher" : "Superposition Cipher"; } }
    public override string Code { get { return "SU"; } }


    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public SuperpositionCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        Data data = new Data();
        string encrypt = "";
        string kw1 = data.PickWord(4, 8);
        var keyExpr = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", keyExpr.Value);
        string kw2 = data.PickWord(3, word.Length);
        string kw3 = data.PickWord(3, word.Length);
        logMessages.Add(string.Format("{0} + {1} -> {2}", kw1, keyExpr.Expression, key));
        logMessages.Add(string.Format("Screen 2: {0}", kw2));
        logMessages.Add(string.Format("Screen 3: {0}", kw3));
        
        
        if (invert)
        {
            for(int i = 0; i < word.Length; i++)
            {
                int offset = key.IndexOf(kw3[i % kw3.Length]) - key.IndexOf(kw2[i % kw2.Length]);
                encrypt += key[CMTools.mod(key.IndexOf(word[i]) + offset, 26)];
                logMessages.Add(string.Format("{0} + {1}{2} -> {3}", word[i], kw2[i % kw2.Length], kw3[i % kw3.Length], encrypt[i]));
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                int offset = key.IndexOf(kw2[i % kw2.Length]) - key.IndexOf(kw3[i % kw3.Length]);
                encrypt += key[CMTools.mod(key.IndexOf(word[i]) + offset, 26)];
                logMessages.Add(string.Format("{0} + {1}{2} -> {3}", word[i], kw3[i % kw3.Length], kw2[i % kw2.Length], encrypt[i]));
            }
        }

        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[]
            {
                new PageInfo(new ScreenInfo[] { kw1, keyExpr.Expression, kw2, null, kw3 }, invert)
            },
            Score = 6
        };
    }

    private string generateTB(List<string> TBList, int length)
    {
    tryagain:
        string TB = "";
        for (int i = 0; i < length; i++)
            TB += Rnd.Range(0, 3);
        if (TBList.Contains(TB))
            goto tryagain;
        return TB;
    }
}
