using System.Collections.Generic;
using CipherMachine;
using UnityEngine;
using Words;

public class ParallelogramCipher : CipherBase
{
    public override string Name { get { return "Parallelogram Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "PC"; } }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWYZ";
        Data data = new Data();
        string[] kws = { data.PickWord(4, 8), data.PickWord(3, word.Length), data.PickWord(3, word.Length) };
        var kwfront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kws[0].Replace("X", ""), alpha, kwfront.Value);
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
        logMessages.Add(string.Format("Screen 1: {0}", kws[0]));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", kwfront.Expression, kwfront.Value));
        logMessages.Add(string.Format("Key: {0}", key));
        logMessages.Add(string.Format("Screen 2: {0}", kws[1]));
        logMessages.Add(string.Format("Screen 3: {0}", kws[2]));
        logMessages.Add(string.Format("Key Letter: {0}", letter));
        logMessages.Add(string.Format("Screen 4: {0}", replaceX));
        string[] temp = { kws[1].Replace('X', letter), kws[2].Replace('X', letter) };
        for(int i = 0; i < word.Length; i++)
        {
            int[] indexes = { key.IndexOf(word[i]), key.IndexOf(temp[0][i % temp[0].Length]), key.IndexOf(temp[1][i % temp[1].Length]) };
            int row = (indexes[0] / 5) + ((indexes[1] / 5) - (indexes[0] / 5)) + ((indexes[2] / 5) - (indexes[0] / 5));
            int col = (indexes[0] % 5) + ((indexes[1] % 5) - (indexes[0] % 5)) + ((indexes[2] % 5) - (indexes[0] % 5));
            encrypt = encrypt + "" + key[(CMTools.mod(row, 5) * 5) + CMTools.mod(col, 5)];
            logMessages.Add(string.Format("{0}, {1}{2} -> {3}", word[i], temp[0][i % temp[0].Length], temp[1][i % temp[1].Length], encrypt[i]));
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kws[0], kwfront.Expression, kws[1], (letter + ""), kws[2], null, replaceX }) }
        };
    }
}
