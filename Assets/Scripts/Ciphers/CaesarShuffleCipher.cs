using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class CaesarShuffleCipher
{
    public ResultInfo encrypt(string word, string id, string log, bool invert)
    {
        Debug.LogFormat("{0} Begin Caesar Shuffle Cipher", log);
        string alpha = "-ABCDEFGHIJKLMNOPQRSTUVWXYZ", encrypt = word.ToUpperInvariant();
        var wordList = new Data();
        string kwa = wordList.PickWord(12 - word.Length);
        string kwb = wordList.PickWord(12 - word.Length);
        Debug.LogFormat("{0} [Caesar Shuffle Cipher] Screen 1: {1}", log, kwa);
        Debug.LogFormat("{0} [Caesar Shuffle Cipher] Screen 2: {1}", log, kwb);
        Debug.LogFormat("{0} [Caesar Shuffle Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
        if (invert)
        {
            for (int aa = 0; aa < kwa.Length; aa++)
            {
                int index = (alpha.IndexOf(kwa[aa]) % (word.Length - 1)) + 1;
                Debug.LogFormat("{0} [Caesar Shuffle Cipher] {1} -> {2}", log, kwa[aa], index);
                string[] s = { encrypt.Substring(index), encrypt.Substring(0, index) };
                Debug.LogFormat("{0} [Caesar Shuffle Cipher] {1}|{2}", log, s[1], s[0]);
                Debug.LogFormat("{0} [Caesar Shuffle Cipher] {1}|{2}", log, s[0], s[1]);
                encrypt = "";
                for (int bb = 0; bb < s[0].Length; bb++)
                {
                    index = alpha.IndexOf(s[0][bb]) + alpha.IndexOf(kwb[aa]);
                    if (index > 26)
                        index -= 26;
                    encrypt += alpha[index];
                }
                encrypt += s[1];
                Debug.LogFormat("{0} [Caesar Shuffle Cipher] {1}|{2} - {3} -> {4}", log, s[0], s[1], kwb[aa], encrypt);
            }
        }
        else
        {
            for (int aa = (kwa.Length - 1); aa >= 0; aa--)
            {
                int index = (word.Length - 1) - (alpha.IndexOf(kwa[aa]) % (word.Length - 1));
                Debug.LogFormat("{0} [Caesar Shuffle Cipher] {1} -> {2}", log, kwa[aa], index);
                string[] s = { encrypt.Substring(index), encrypt.Substring(0, index) };
                Debug.LogFormat("{0} [Caesar Shuffle Cipher] {1}|{2}", log, s[1], s[0]);
                Debug.LogFormat("{0} [Caesar Shuffle Cipher] {1}|{2}", log, s[0], s[1]);
                encrypt = "";
                for (int bb = 0; bb < s[1].Length; bb++)
                {
                    index = alpha.IndexOf(s[1][bb]) - alpha.IndexOf(kwb[aa]);
                    if (index < 1)
                        index += 26;
                    encrypt += alpha[index];
                }
                encrypt = s[0] + encrypt;
                Debug.LogFormat("{0} [Caesar Shuffle Cipher] {1}|{2} + {3} -> {4}", log, s[0], s[1], kwb[aa], encrypt);
            }
        }
        Debug.LogFormat("{0} [Caesar Shuffle Cipher] {1} - > {2}", log, word, encrypt);
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(kwa, new int[] { 35, 35, 35, 32, 28 }[kwa.Length - 4]);
        screens[1] = new ScreenInfo();
        screens[2] = new ScreenInfo(kwb, new int[] { 35, 35, 35, 32, 28 }[kwb.Length - 4]);
        for (int i = 3; i < 8; i++)
            screens[i] = new ScreenInfo();
        screens[8] = new ScreenInfo(id, 35);
        return new ResultInfo
        {
            Encrypted = encrypt,
            Score = 5,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
}