using CipherMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class TrifidCipher
{
    public ResultInfo encrypt(string word, string id, string log, KMBombInfo Bomb, bool invert)
    {
        Debug.LogFormat("{0} Begin Trifid Cipher", log);
        var words = new Data();
        string[] keyFront = CMTools.generateBoolExp(Bomb);
        int[][] numbers = new int[3][] { new int[word.Length], new int[word.Length], new int[word.Length] };
        string key;
        string encrypt;
        string kw;
        do
        {
            encrypt = "";
            kw = words.PickWord(4, 8);
            key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", keyFront[1][0] == 'T') + "-";
            if (invert)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    int n = key.IndexOf(word[i]);
                    numbers[(i * 3) / word.Length][(i * 3) % word.Length] = n / 9;
                    numbers[((i * 3) + 1) / word.Length][((i * 3) + 1) % word.Length] = (n % 9) / 3;
                    numbers[((i * 3) + 2) / word.Length][((i * 3) + 2) % word.Length] = n % 3;
                }
                for (int i = 0; i < word.Length; i++)
                    encrypt = encrypt + "" + key[(numbers[0][i] * 9) + (numbers[1][i] * 3) + (numbers[2][i])];
            }
            else
            {
                for (int i = 0; i < word.Length; i++)
                {
                    int n = key.IndexOf(word[i]);
                    numbers[0][i] = n / 9;
                    numbers[1][i] = (n % 9) / 3;
                    numbers[2][i] = n % 3;
                }
                for (int i = 0; i < word.Length; i++)
                    encrypt = encrypt + "" + key[(numbers[(i * 3) / word.Length][(i * 3) % word.Length] * 9) + (numbers[((i * 3) + 1) / word.Length][((i * 3) + 1) % word.Length] * 3) + (numbers[((i * 3) + 2) / word.Length][((i * 3) + 2) % word.Length])];
            }
        }
        while (encrypt.Contains("-"));
        Debug.LogFormat("{0} [Trifid Cipher] Keyword: {1}", log, kw);
        Debug.LogFormat("{0} [Trifid Cipher] Key: {1}", log, key);
        Debug.LogFormat("{0} [Trifid Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
        Debug.LogFormat("{0} [Trifid Cipher] {1}", log, String.Join("", numbers[0].Select(p => (p + 1).ToString()).ToArray()));
        Debug.LogFormat("{0} [Trifid Cipher] {1}", log, String.Join("", numbers[1].Select(p => (p + 1).ToString()).ToArray()));
        Debug.LogFormat("{0} [Trifid Cipher] {1}", log, String.Join("", numbers[2].Select(p => (p + 1).ToString()).ToArray()));
        Debug.LogFormat("{0} [Trifid Cipher] {1} -> {2}", log, word, encrypt);
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
        screens[1] = new ScreenInfo(keyFront[0], 25);
        screens[8] = new ScreenInfo(id, 35);
        for (int i = 2; i < 8; i++)
            screens[i] = new ScreenInfo();
        return new ResultInfo
        {
            Encrypted = encrypt,
            Score = 5,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
}
