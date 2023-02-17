using System.Collections.Generic;
using CipherMachine;
using UnityEngine;
using Words;

public class GlobalOffsetCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Global Offset Cipher" : "Global Offset Cipher"; } }
    public override string Code { get { return "GO"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public GlobalOffsetCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWYZ";
        string kw = new Data().PickWord(4, 8);
        var kwfront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw, alpha, kwfront.Value);
        char letter = (key.Substring(0, 12) + key.Substring(13))[Random.Range(0, 24)];
        string encrypt = "";
        string replaceX = "";
        logMessages.Add(string.Format("Before Replacing Xs: {0}", word));
        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == 'X')
            {
                word = word.Substring(0, i) + "" + alpha[Random.Range(0, alpha.Length)] + "" + word.Substring(i + 1);
                replaceX = replaceX + "" + word[i];
            }
            else
                replaceX = replaceX + "" + alpha.Replace(word[i].ToString(), "")[Random.Range(0, 24)];
        }
        logMessages.Add(string.Format("After Replacing Xs: {0}", word));
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", kwfront.Expression, kwfront.Value));
        logMessages.Add(string.Format("Key: {0}", key));
        logMessages.Add(string.Format("Screen 2: {0}", replaceX));
        logMessages.Add(string.Format("Key Letter: {0}", letter));
        int[] offset = { (key.IndexOf(letter) / 5) - 2, (key.IndexOf(letter) % 5) - 2 };
        if (invert)
        {
            foreach (char l in word)
                encrypt = encrypt + "" + key[(CMTools.mod((key.IndexOf(l) / 5) + offset[0], 5) * 5) + CMTools.mod((key.IndexOf(l) % 5) + offset[1], 5)];
        }
        else
        {
            foreach (char l in word)
                encrypt = encrypt + "" + key[(CMTools.mod((key.IndexOf(l) / 5) - offset[0], 5) * 5) + CMTools.mod((key.IndexOf(l) % 5) - offset[1], 5)];
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, kwfront.Expression, replaceX, (letter + "") }, invert) },
            Score = 4
        };
    }
}
