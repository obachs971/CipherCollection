using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class PlayfairCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Playfair Cipher" : "Playfair Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "PF"; } }

    private readonly bool invert;
    public PlayfairCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string kw = new Data().PickWord(4, 8);
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
        logMessages.Add(string.Format("Screen 2: {0}", replaceJ));
        var keyFront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw.Replace("J", "I"), alpha.ToString(), keyFront.Value);
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Keyword Front Rule: {0} -> {1}", keyFront.Expression, keyFront.Value));
        logMessages.Add(string.Format("Key: {0}", key));
        for (int i = 0; i < word.Length / 2; i++)
        {
            int r1 = key.IndexOf(word[i * 2]) / 5;
            int c1 = key.IndexOf(word[i * 2]) % 5;
            int r2 = key.IndexOf(word[(i * 2) + 1]) / 5;
            int c2 = key.IndexOf(word[(i * 2) + 1]) % 5;
            if (r1 == r2 && c1 == c2)
            {
                r1 = 4 - r1;
                c1 = 4 - c1;
                r2 = 4 - r2;
                c2 = 4 - c2;
            }
            else if (r1 == r2)
            {
                if (invert)
                {
                    c1 = CMTools.mod(c1 - 1, 5);
                    c2 = CMTools.mod(c2 - 1, 5);
                }
                else
                {
                    c1 = CMTools.mod(c1 + 1, 5);
                    c2 = CMTools.mod(c2 + 1, 5);
                }
            }
            else if (c1 == c2)
            {
                if (invert)
                {
                    r1 = CMTools.mod(r1 - 1, 5);
                    r2 = CMTools.mod(r2 - 1, 5);
                }
                else
                {
                    r1 = CMTools.mod(r1 + 1, 5);
                    r2 = CMTools.mod(r2 + 1, 5);
                }
            }
            else
            {
                int temp = c1;
                c1 = c2;
                c2 = temp;
            }
            encrypt = encrypt + "" + key[(r1 * 5) + c1] + "" + key[(r2 * 5) + c2];
            logMessages.Add(string.Format("{0}{1} -> {2}{3}", word[i * 2], word[(i * 2) + 1], encrypt[i * 2], encrypt[(i * 2) + 1]));
        }
        if (word.Length % 2 == 1)
            encrypt = encrypt + "" + word[word.Length - 1];
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
        screens[1] = new ScreenInfo(keyFront.Expression, 25);
        screens[2] = new ScreenInfo(replaceJ, new int[] { 35, 35, 35, 32, 28 }[replaceJ.Length - 4]);
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
}
