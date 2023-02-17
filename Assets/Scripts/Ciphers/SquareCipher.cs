using UnityEngine;
using System.Collections.Generic;
using CipherMachine;
using Words;
using System;

public class SquareCipher : CipherBase
{
    public override string Name { get { return "Square Cipher"; } }
    public override string Code { get { return "SQ"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string encrypt = "", screen2 = "";
        List<List<string>> sumResults = new List<List<string>>();
        for (int i = 0; i < 26; i++)
            sumResults.Add(new List<string>());
        string kw = new Data().PickWord(4, 8);
        var kwfront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw.Replace("J", "I"), "ABCDEFGHIKLMNOPQRSTUVWXYZ", kwfront.Value);
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", kwfront.Expression, kwfront.Value));
        logMessages.Add(string.Format("Key: {0}", key));
        for (int i = 0; i < key.Length; i++)
        {
            for (int j = i + 1; j < key.Length; j++)
            {
                int r1 = i / 5, r2 = j / 5, c1 = Math.Min(i % 5, j % 5), c2 = Math.Max(i % 5, j % 5);
                int sum = 0;
                for (int row = r1; row <= r2; row++)
                {
                    for (int col = c1; col <= c2; col++)
                        sum += (key[row * 5 + col] - 'A' + 1);
                }
                sumResults[sum % 26].Add(key[i] + "" + key[j]);
            }
        }
        foreach (char letter in word)
        {
            int index = (letter - 'A' + 1) % 26;
            string temp;
            if (sumResults[index].Count == 0)
                temp = letter + "" + letter;
            else
                temp = new string(sumResults[index][UnityEngine.Random.Range(0, sumResults[index].Count)].ToCharArray().Shuffle());
            encrypt = encrypt + "" + temp[0];
            screen2 = screen2 + "" + temp[1];
            logMessages.Add(string.Format("{0} -> {1}{2}", letter, temp[0], temp[1]));
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, kwfront.Expression, screen2 }) },
            Score = 8
        };
    }
}
