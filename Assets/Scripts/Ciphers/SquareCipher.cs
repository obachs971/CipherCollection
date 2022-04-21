using UnityEngine;
using System.Collections.Generic;
using CipherMachine;
using Words;

public class SquareCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Square Cipher" : "Square Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "SQ"; } }

    private readonly bool invert;
    public SquareCipher(bool invert) { this.invert = invert; }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ", replaceJ = "", encrypt = "";
        logMessages.Add(string.Format("Before Replacing Js: {0}", word));
        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == 'J')
            {
                word = word.Substring(0, i) + "" + alpha[Random.Range(0, alpha.Length)] + "" + word.Substring(i + 1);
                replaceJ = replaceJ + "" + word[i];
            }
            else
                replaceJ = replaceJ + "" + alpha.Replace(word[i].ToString(), "")[Random.Range(0, 24)];
        }
        logMessages.Add(string.Format("After Replacing Js: {0}", word));
        logMessages.Add(string.Format("Screen 2: {0}", replaceJ));
        string kw = new Data().PickWord(4, 8);
        var kwfront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw.Replace("J", "I"), alpha, kwfront.Value);
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", kwfront.Expression, kwfront.Value));
        logMessages.Add(string.Format("Key: {0}", key));
        for (int i = 0; i < word.Length / 2; i++)
        {
            int n1 = key.IndexOf(word[i * 2]), n2 = key.IndexOf(word[i * 2 + 1]), r3, c3, r4, c4;
            if (n1 == n2)
            {
                r3 = 4 - (n1 / 5);
                c3 = 4 - (n1 % 5);
                r4 = 4 - (n2 / 5);
                c4 = 4 - (n2 % 5);
            }
            else
            {
                int r1 = n1 / 5, c1 = n1 % 5, r2 = n2 / 5, c2 = n2 % 5;
                r3 = r1 - (r2 - r1);
                c3 = c1 - (c2 - c1);
                r4 = r2 - (r1 - r2);
                c4 = c2 - (c1 - c2);
            }
            if(invert)
                encrypt = encrypt + "" + key[CMTools.mod(r4, 5) * 5 + CMTools.mod(c4, 5)] + "" + key[CMTools.mod(r3, 5) * 5 + CMTools.mod(c3, 5)];
            else
                encrypt = encrypt + "" + key[CMTools.mod(r3, 5) * 5 + CMTools.mod(c3, 5)] + "" + key[CMTools.mod(r4, 5) * 5 + CMTools.mod(c4, 5)];
        }
        if (word.Length % 2 == 1)
            encrypt = encrypt + "" + word[word.Length - 1];
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, kwfront.Expression, replaceJ }, invert)}
        };
    }
}
