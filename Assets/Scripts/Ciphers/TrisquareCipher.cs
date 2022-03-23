using System.Collections.Generic;
using CipherMachine;
using UnityEngine;
using Words;

public class TrisquareCipher : CipherBase
{
	public override string Name { get { return "Trisquare Cipher"; } }
	public override int Score { get { return 5; } }
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
        string[][] kwFronts = new string[3][];
        for (int i = 0; i < 3; i++)
        {
            kws[i] = data.PickWord(4, 8);
            kwFronts[i] = CMTools.generateBoolExp(bomb);
            keys[i] = CMTools.getKey(kws[i].Replace("J", "I"), "ABCDEFGHIKLMNOPQRSTUVWXYZ", kwFronts[i][1][0] == 'T');
            logMessages.Add(string.Format("Keyword #{0}: {1}", (i + 1), kws[i]));
            logMessages.Add(string.Format("Key #{0}: {1} -> {2} -> {3}", (i + 1), kwFronts[i][0], kwFronts[i][1], keys[i]));
        }
        string intersection = "";
        for (int i = 0; i < (word.Length / 2); i++)
        {
            int r1 = keys[0].IndexOf(word[i * 2]) / 5;
            int c1 = keys[0].IndexOf(word[i * 2]) % 5;
            int r2 = keys[1].IndexOf(word[(i * 2) + 1]) / 5;
            int c2 = keys[1].IndexOf(word[(i * 2) + 1]) % 5;
            intersection = intersection + "" + keys[2][(r1 * 5) + c2];
            r1 = (r1 + Random.Range(0, 4) + 1) % 5;
            c2 = (c2 + Random.Range(0, 4) + 1) % 5;
            encrypt = encrypt + "" + keys[0][(r1 * 5) + c1] + "" + keys[1][(r2 * 5) + c2];
            logMessages.Add(string.Format("{0}{1} -> {2}{3}{4}", word[i * 2], word[(i * 2) + 1], encrypt[i * 2], encrypt[(i * 2) + 1], intersection[i]));
        }
        if (word.Length % 2 == 1)
            encrypt = encrypt + "" + word[word.Length - 1];
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        logMessages.Add(string.Format("Screen D: {0}", intersection));
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(kws[0], new int[] { 35, 35, 35, 32, 28 }[kws[0].Length - 4]);
        screens[1] = new ScreenInfo(kwFronts[0][0], 25);
        screens[2] = new ScreenInfo(kws[1], new int[] { 35, 35, 35, 32, 28 }[kws[1].Length - 4]);
        screens[3] = new ScreenInfo(kwFronts[1][0], 25);
        screens[4] = new ScreenInfo(kws[2], new int[] { 35, 35, 35, 32, 28 }[kws[2].Length - 4]);
        screens[5] = new ScreenInfo(kwFronts[2][0], 25);
        screens[6] = new ScreenInfo(replaceJ, new int[] { 35, 35, 35, 32, 28 }[replaceJ.Length - 4]);
        screens[7] = new ScreenInfo(intersection, new int[] { 25, 25, 20 }[intersection.Length - 2]);
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens) }
        };
    }
}
