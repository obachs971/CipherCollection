using System.Collections.Generic;
using CipherMachine;
using UnityEngine;
using Words;

public class TrisquareCipher : CipherBase
{
    public override string Name { get { return "Trisquare Cipher"; } }
    public override string Code { get { return "TS"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        Data data = new Data();
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
        logMessages.Add(string.Format("Screen 4: {0}", replaceJ));
        string[] kws = new string[3];
        string[] keys = new string[3];
        var kwFronts = new ValueExpression<bool>[3];
        for (int i = 0; i < 3; i++)
        {
            kws[i] = data.PickWord(4, 8);
            kwFronts[i] = CMTools.generateBoolExp(bomb);
            keys[i] = CMTools.getKey(kws[i].Replace("J", "I"), "ABCDEFGHIKLMNOPQRSTUVWXYZ", kwFronts[i].Value);
            logMessages.Add(string.Format("Keyword #{0}: {1}", (i + 1), kws[i]));
            logMessages.Add(string.Format("Key #{0}: {1} -> {2} -> {3}", (i + 1), kwFronts[i].Expression, kwFronts[i].Value, keys[i]));
        }
        for (int i = 0; i < (word.Length / 2); i++)
        {
            int r1 = keys[0].IndexOf(word[i * 2]) / 5;
            int c1 = keys[0].IndexOf(word[i * 2]) % 5;
            int r2 = keys[1].IndexOf(word[(i * 2) + 1]) / 5;
            int c2 = keys[1].IndexOf(word[(i * 2) + 1]) % 5;
            char intersection = keys[2][(r1 * 5) + c2];
            r1 = (r1 + Random.Range(0, 4) + 1) % 5;
            c2 = (c2 + Random.Range(0, 4) + 1) % 5;
            encrypt = encrypt + "" + keys[0][(r1 * 5) + c1] + "" + keys[1][(r2 * 5) + c2] + "" + intersection;
            logMessages.Add(string.Format("{0}{1} -> {2}{3}{4}", word[i * 2], word[(i * 2) + 1], encrypt[i * 3], encrypt[(i * 3) + 1], encrypt[(i * 3) + 2]));
        }
        if (word.Length % 2 == 1)
        {
            char letter = word[word.Length - 1];
            int row = keys[2].IndexOf(letter) / 5;
            int col = keys[2].IndexOf(letter) % 5;
            encrypt = encrypt + "" + keys[0][(row * 5) + Random.Range(0, 5)] + "" + keys[1][(Random.Range(0, 5) * 5) + col];
            logMessages.Add(string.Format("{0} -> {1}{2}", letter, encrypt[encrypt.Length - 2], encrypt[encrypt.Length - 1]));
        }
        string screenD = encrypt.Substring(word.Length);
        encrypt = encrypt.Substring(0, word.Length);
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        logMessages.Add(string.Format("Screen D: {0}", screenD));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kws[0], kwFronts[0].Expression, kws[1], kwFronts[1].Expression, kws[2], kwFronts[2].Expression, replaceJ, screenD }) },
            Score = 9
        };
    }
}
