using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class UbchiTransposition : CipherBase
{
    public override string Name { get { return invert ? "Inverted Ubchi Transposition" : "Ubchi Transposition"; } }
    public override string Code { get { return "UT"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public UbchiTransposition(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        List<string> logMessages = new List<string>();

        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", encrypt;
        char letter;
        string kw = new Data().PickWord(4, word.Length);
        char[] order = kw.ToCharArray();
        Array.Sort(order);
        order = order.Distinct().ToArray();
        int[] key = new int[kw.Length];
        for (int i = 0; i < order.Length; i++)
        {
            for (int j = 0; j < kw.Length; j++)
            {
                if (order[i] == kw[j])
                    key[j] = i;
            }
        }
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Key: {0}", string.Join("", key.Select(x => (x + 1).ToString()).ToArray())));
        if (invert)
        {
            letter = alpha[UnityEngine.Random.Range(0, alpha.Length)];
            logMessages.Add(string.Format("Added Letter: {0}", letter));
            encrypt = Transpose(word + "" + letter, key, logMessages).Replace("-", "");
            letter = encrypt[encrypt.Length - 1];
            encrypt = encrypt.Substring(0, encrypt.Length - 1);
            encrypt = Transpose(encrypt, key, logMessages).Replace("-", "");
        }
        else
        {
            encrypt = Transpose(word, key, logMessages).Replace("-", "");
            letter = alpha[UnityEngine.Random.Range(0, alpha.Length)];
            logMessages.Add(string.Format("Added Letter: {0}", letter));
            encrypt = Transpose(encrypt + "" + letter, key, logMessages).Replace("-", "");
            letter = encrypt[encrypt.Length - 1];
            encrypt = encrypt.Substring(0, encrypt.Length - 1);
        }
        logMessages.Add(string.Format("Screen A: {0}", letter));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, letter + "" }, invert) },
            Score = 5
        };
    }
    private string Transpose(string word, int[] key, List<string> logMessages)
    {
        string encrypt = "";
        if (invert)
        {
            string temp = "*********".Substring(0, word.Length);
            while (temp.Length % key.Length > 0)
                temp += "-";
            string[] grid = new string[temp.Length / key.Length];
            for (int i = 0; i < grid.Length; i++)
                grid[i] = temp.Substring(i * key.Length, key.Length);
            int cur = 0;
            for (int z = 0; z < key.Length; z++)
            {
                for (int i = 0; i < grid.Length; i++)
                {
                    for (int j = 0; j < grid[i].Length; j++)
                    {
                        if (key[j] == z && grid[i][j] != '-')
                            grid[i] = grid[i].Substring(0, j) + "" + word[cur++] + "" + grid[i].Substring(j + 1);

                    }
                }
            }
            for (int i = 0; i < grid.Length; i++)
            {
                encrypt += grid[i];
                logMessages.Add(grid[i]);
            }
        }
        else
        {
            string temp = word.ToUpperInvariant();
            while (temp.Length % key.Length > 0)
                temp += "-";
            string[] grid = new string[temp.Length / key.Length];
            for (int i = 0; i < grid.Length; i++)
                grid[i] = temp.Substring(i * key.Length, key.Length);
            for (int z = 0; z < key.Length; z++)
            {
                for (int i = 0; i < grid.Length; i++)
                {
                    for (int j = 0; j < grid[i].Length; j++)
                    {
                        if (key[j] == z)
                            encrypt = encrypt + "" + grid[i][j];
                    }
                }
            }
            for (int i = 0; i < grid.Length; i++)
                logMessages.Add(grid[i]);
        }
        return encrypt;
    }
}
