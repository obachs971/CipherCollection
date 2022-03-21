using CipherMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class GrandpreCipher
{
    public ResultInfo encrypt(string word, string id, string log)
    {
        Debug.LogFormat("{0} Begin Grandpré Cipher", log);
        var words = generateWords(UnityEngine.Random.Range(6, 9));
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", encrypt = "", screenRows = "", key = "";
        string[] possLets = new string[words.Length];
        for (int i = 0; i < words.Length; i++)
        {
            key += words[i];
            possLets[i] = "";
            Debug.LogFormat("{0} [Grandpré Cipher] Keyword #{1}: {2}", log, (i + 1), words[i]);
        }
        for (int i = 0; i < alpha.Length; i++)
            possLets[i % possLets.Length] = possLets[i % possLets.Length] + "" + alpha[i];
        for (int i = 0; i < word.Length; i++)
        {
            List<int> poss = new List<int>();
            for (int j = 0; j < key.Length; j++)
            {
                if (word[i] == key[j])
                    poss.Add(j);
            }
            int index = poss[UnityEngine.Random.Range(0, poss.Count)];
            int row = index / words.Length, col = (index % words.Length) + 1;
            Debug.LogFormat("{0} [Grandpré Cipher] {1} -> {2}, {3}", log, word[i], (row + 1), col);
            encrypt = encrypt + "" + possLets[row].ToCharArray().Shuffle()[0];
            screenRows = screenRows + "" + col;
        }
        Debug.LogFormat("{0} [Grandpré Cipher] {1} -> {2}", log, word, encrypt);
        ScreenInfo[][] screens = new ScreenInfo[2][];
        for (int i = 0; i < screens.Length; i++)
            screens[i] = new ScreenInfo[9];
        for (int i = 0; i < 4; i++)
            screens[0][i * 2] = new ScreenInfo(words[i], new int[] { 35, 32, 28 }[words[i].Length - 6]);
        screens[0][1] = new ScreenInfo(screenRows.Substring(0, screenRows.Length / 2), (screenRows.Length) > 7 ? 20 : 25);
        screens[0][3] = new ScreenInfo(screenRows.Substring(screenRows.Length / 2), (screenRows.Length) > 6 ? 20 : 25);
        for (int i = 0; i < words.Length - 4; i++)
            screens[1][i * 2] = new ScreenInfo(words[i + 4], new int[] { 35, 32, 28 }[words[i].Length - 6]);
        screens[0][8] = new ScreenInfo(id, 35);
        screens[1][8] = new ScreenInfo(id, 35);
        return new ResultInfo
        {
            Encrypted = encrypt,
            Score = 5,
            Pages = new PageInfo[] { new PageInfo(screens[0]), new PageInfo(screens[1]) }
        };
    }
    private string[] generateWords(int len)
    {
        tryAgain:
        var wordList = new Data();
        // If len == 8, generate 8 words, etc., so they can form a square
        string[] words = new string[len];
        var alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
        for (int i = 0; i < words.Length; i++)
        {
            words[i] = wordList.PickBestWord(len, w => alpha.Count(ch => w.Contains(ch)));
            alpha.RemoveAll(ch => words[i].Contains(ch));
        }
        if (alpha.Count > 0)
            goto tryAgain;
        return words.Shuffle();
    }
}