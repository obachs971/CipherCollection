using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class ConjugatedMatrixBifidCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Conjugated Matrix Bifid Cipher" : "Conjugated Matrix Bifid Cipher"; } }
    public override string Code { get { return "CM"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public ConjugatedMatrixBifidCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo Bomb)
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
                word = word.Substring(0, i) + "" + alpha[UnityEngine.Random.Range(0, alpha.Length)] + "" + word.Substring(i + 1);
                replaceJ = replaceJ + "" + word[i];
            }
            else
                replaceJ = replaceJ + "" + alpha.Replace(word[i].ToString(), "")[UnityEngine.Random.Range(0, 24)];
        }
        logMessages.Add(string.Format("After Replacing Js: {0}", word));
        logMessages.Add(string.Format("Screen 3: {0}", replaceJ));
        string[] kws = new string[2];
        string[] keys = new string[2];
        var kwFronts = new ValueExpression<bool>[2];
        for (int i = 0; i < 2; i++)
        {
            kws[i] = words.PickWord(4, 8);
            kwFronts[i] = CMTools.generateBoolExp(Bomb);
            keys[i] = CMTools.getKey(kws[i].Replace("J", "I"), "ABCDEFGHIKLMNOPQRSTUVWXYZ", kwFronts[i].Value);
            logMessages.Add(string.Format("Keyword #{0}: {1}", (i + 1), kws[i]));
            logMessages.Add(string.Format("Key #{0}: {1} -> {2} -> {3}", (i + 1), kwFronts[i].Expression, kwFronts[i].Value, keys[i]));
        }
        int[][] pos = new int[2][] { new int[word.Length], new int[word.Length] };
        if (invert)
        {
            for (int aa = 0; aa < word.Length; aa++)
            {
                pos[(aa * 2) / word.Length][(aa * 2) % word.Length] = keys[1].IndexOf(word[aa]) / 5;
                pos[((aa * 2) + 1) / word.Length][((aa * 2) + 1) % word.Length] = keys[1].IndexOf(word[aa]) % 5;
            }
            for (int aa = 0; aa < word.Length; aa++)
                encrypt = encrypt + "" + keys[0][(pos[0][aa] * 5) + pos[1][aa]];
        }
        else
        {
            for (int aa = 0; aa < word.Length; aa++)
            {
                pos[0][aa] = keys[0].IndexOf(word[aa]) / 5;
                pos[1][aa] = keys[0].IndexOf(word[aa]) % 5;
            }
            for (int aa = 0; aa < word.Length; aa++)
                encrypt = encrypt + "" + keys[1][((pos[(aa * 2) / word.Length][(aa * 2) % word.Length]) * 5) + pos[((aa * 2) + 1) / word.Length][((aa * 2) + 1) % word.Length]];
        }
        logMessages.Add(String.Join("", pos[0].Select(p => (p + 1).ToString()).ToArray()));
        logMessages.Add(String.Join("", pos[1].Select(p => (p + 1).ToString()).ToArray()));
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kws[0], kwFronts[0].Expression, kws[1], kwFronts[1].Expression, replaceJ }, invert) },
            Score = 5
        };
    }
}
