using CipherMachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class MonoalphabeticCipher
{
    public ResultInfo encrypt(string word, string id, string log, bool invert)
    {
        Debug.LogFormat("{0} Begin Monoalphabetic Cipher", log);
        string[] kws = generateKeywords();
        string key = CMTools.getKey(kws.Join(""), "", false);
        for (int i = 0; i < kws.Length; i++)
            Debug.LogFormat("{0} [Monoalphabetic Cipher] KW{1}: {2}", log, (i + 1), kws[i]);
        Debug.LogFormat("{0} [Monoalphabetic Cipher] Key: {1}", log, key);
        Debug.LogFormat("{0} [Monoalphabetic Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
        string encrypt = "";
        if (invert)
        {
            foreach (char c in word)
                encrypt = encrypt + "" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[key.IndexOf(c)];
        }
        else
        {
            foreach (char c in word)
                encrypt = encrypt + "" + key["ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c)];
        }
        Debug.LogFormat("{0} [Monoalphabetic Cipher] {1} - > {2}", log, word, encrypt);
        ScreenInfo[][] screens = { new ScreenInfo[9], new ScreenInfo[9] };
        for (int i = 0; i < 8; i += 2)
            screens[0][i] = new ScreenInfo(kws[i / 2], new int[] { 35, 35, 35, 32, 28 }[kws[i / 2].Length - 4]);
        screens[0][8] = new ScreenInfo(id, 35);
        screens[1][0] = new ScreenInfo(kws[4], new int[] { 35, 35, 35, 32, 28 }[kws[4].Length - 4]);
        screens[1][2] = new ScreenInfo(kws[5], new int[] { 35, 35, 35, 32, 28 }[kws[5].Length - 4]);
        screens[1][8] = new ScreenInfo(id, 35);
        return new ResultInfo
        {
            Encrypted = encrypt,
            Score = 5,
            Pages = new PageInfo[] { new PageInfo(screens[0], invert), new PageInfo(screens[1]) }
        };
    }

    // Finds a set of 6 keywords that contain all of the letters A–Z.
    private string[] generateKeywords()
    {
        tryAgain:
        var alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
        string[] kws = new string[6];
        int[] order = { 4, 5, 6, 7, 8 };
        order.Shuffle();
        var words = new Data();
        for (int i = 0; i < order.Length; i++)
        {
            kws[i] = words.PickBestWord(order[i], w => alpha.Count(ch => w.Contains(ch)));
            alpha.RemoveAll(ch => kws[i].Contains(ch));
        }
        kws[5] = words.PickBestWord(4, 8, w => w.Distinct().Count(ch => alpha.Contains(ch)));
        alpha.RemoveAll(ch => kws[5].Contains(ch));
        if (alpha.Count > 0)
            goto tryAgain;
        return kws.Shuffle();
    }
}