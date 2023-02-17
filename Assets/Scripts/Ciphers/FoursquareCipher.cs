using System.Collections.Generic;
using CipherMachine;
using UnityEngine;
using Words;

public class FoursquareCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Foursquare Cipher" : "Foursquare Cipher"; } }
    public override string Code { get { return "FO"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public FoursquareCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        Data words = new Data();
        string encrypt = "";
        string replaceJ = "";
        string alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
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
        logMessages.Add(string.Format("Screen 1 Page 2: {0}", replaceJ));
        string[] kws = new string[4];
        string[] keys = new string[4];
        var kwFronts = new ValueExpression<bool>[4];
        for (int i = 0; i < 4; i++)
        {
            kws[i] = words.PickWord(4, 8);
            kwFronts[i] = CMTools.generateBoolExp(bomb);
            keys[i] = CMTools.getKey(kws[i].Replace("J", "I"), "ABCDEFGHIKLMNOPQRSTUVWXYZ", kwFronts[i].Value);
            logMessages.Add(string.Format("Keyword #{0}: {1}", (i + 1), kws[i]));
            logMessages.Add(string.Format("Key #{0}: {1} -> {2} -> {3}", (i + 1), kwFronts[i].Expression, kwFronts[i].Value, keys[i]));
        }
        if (invert)
        {
            for (int i = 0; i < word.Length / 2; i++)
            {
                int r1 = keys[1].IndexOf(word[i * 2]) / 5;
                int c1 = keys[1].IndexOf(word[i * 2]) % 5;
                int r2 = keys[2].IndexOf(word[(i * 2) + 1]) / 5;
                int c2 = keys[2].IndexOf(word[(i * 2) + 1]) % 5;
                encrypt = encrypt + "" + keys[0][(r1 * 5) + c2] + "" + keys[3][(r2 * 5) + c1];
            }
        }
        else
        {
            for (int i = 0; i < word.Length / 2; i++)
            {
                int r1 = keys[0].IndexOf(word[i * 2]) / 5;
                int c1 = keys[0].IndexOf(word[i * 2]) % 5;
                int r2 = keys[3].IndexOf(word[(i * 2) + 1]) / 5;
                int c2 = keys[3].IndexOf(word[(i * 2) + 1]) % 5;
                encrypt = encrypt + "" + keys[1][(r1 * 5) + c2] + "" + keys[2][(r2 * 5) + c1];
            }
        }
        if (word.Length % 2 == 1)
            encrypt = encrypt + "" + word[word.Length - 1];
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] {
                new PageInfo(new ScreenInfo[] { kws[0], kwFronts[0].Expression, kws[1], kwFronts[1].Expression, kws[2], kwFronts[2].Expression, kws[3], kwFronts[3].Expression }, invert),
                new PageInfo(new ScreenInfo[] { replaceJ }, invert)
            },
            Score = 8
        };
    }
}
