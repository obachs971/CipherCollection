using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class ColumnarTransposition : CipherBase
{
    public override string Name { get { return invert ? "Inverted Columnar Transposition" : "Columnar Transposition"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "CT"; } }

    private readonly bool invert;
    public ColumnarTransposition(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string key = "12345678".Substring(0, 2 + Random.Range(0, word.Length - 1));
        key = new string(key.ToCharArray().Shuffle());
        while ("12345678".Contains(key))
            key = new string(key.ToCharArray().Shuffle());
        logMessages.Add(string.Format("Key: {0}", key));
        string encrypt = "";
        while (word.Length % key.Length != 0)
            word += "-";
        char[][] grid = new char[word.Length / key.Length][];
        for (int i = 0; i < grid.Length; i++)
            grid[i] = new char[key.Length];
        if (invert)
        {
            int bot = word.Length / key.Length - 1;
            word = word.Replace("-", "");
            int cur = 0, mod = word.Length % key.Length;
            for (int i = 1; i <= key.Length; i++)
            {
                for (int j = 0; j < grid.Length; j++)
                {
                    if (j == bot && mod > 0 && key.IndexOf(i + "") >= mod)
                        grid[j][key.IndexOf(i + "")] = '-';
                    else
                        grid[j][key.IndexOf(i + "")] = word[cur++];
                }
            }
            for (int i = 0; i < grid.Length; i++)
            {
                logMessages.Add(new string(grid[i]));
                encrypt += new string(grid[i]);
            }

        }
        else
        {
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = word.Substring(i * key.Length, key.Length).ToCharArray();
                logMessages.Add(new string(grid[i]));
            }
            for (int i = 1; i <= key.Length; i++)
            {
                for (int j = 0; j < grid.Length; j++)
                    encrypt = encrypt + "" + grid[j][key.IndexOf(i + "")];
            }
        }
        encrypt = encrypt.Replace("-", "");
        logMessages.Add(string.Format("{0} - > {1}", word.Replace("-", ""), encrypt));
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(key, (key.Length == 7 ? 32 : 35));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
}