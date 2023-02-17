using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class MyszkowskiTransposition : CipherBase
{
    public override string Name { get { return invert ? "Inverted Myszkowski Transposition" : "Myszkowski Transposition"; } }
    public override string Code { get { return "MY"; } }
    public override bool IsTransposition { get { return true; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public MyszkowskiTransposition(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();

        string encrypt = "";
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
            string temp = "********".Substring(0, word.Length);
            while (temp.Length % kw.Length > 0)
                temp += "-";
            string[] grid = new string[temp.Length / kw.Length];
            for (int i = 0; i < grid.Length; i++)
                grid[i] = temp.Substring(i * kw.Length, kw.Length);
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
            while (temp.Length % kw.Length > 0)
                temp += "-";
            string[] grid = new string[temp.Length / kw.Length];
            for (int i = 0; i < grid.Length; i++)
                grid[i] = temp.Substring(i * kw.Length, kw.Length);
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
        encrypt = encrypt.Replace("-", "");
        logMessages.Add(string.Format("{0} - > {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw }, invert) },
            Score = 4
        };
    }
}
