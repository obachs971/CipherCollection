using System;
using System.Collections.Generic;
using CipherMachine;
using UnityEngine;
using Words;

public class Dreamcipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Dreamcipher" : "Dreamcipher"; } }
    public override string Code { get { return "DR"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public Dreamcipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        //Setting up the grid
        string grid = "----------------------------------------------------------------";
        string keyword = new Data().PickWord(4, 8);
        var kwfront = CMTools.generateBoolExp(bomb);
        string encrypt = "", symbols = "";
        string alpha = "-ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string key = CMTools.getKey(keyword, alpha.Substring(1), kwfront.Value);
        string bins = new string("0000000011111111".ToCharArray().Shuffle());
        string base16 = convertBase(bins, 2, 16);
        while (base16.Length < 4)
            base16 = "0" + base16;
        var vals = new ValueExpression<int>[] { CMTools.generateValue(bomb), CMTools.generateValue(bomb) };
        int[] shifts = { (vals[0].Value % 10) + 1, (vals[1].Value % 10) + 1 };
        int cur = int.Parse(convertBase(bins.Substring(0, 8), 2, 10)) % 64;
        cur = (int.Parse(convertBase(bins.Substring(8, 4), 2, 10)) + cur + 1) % 64;
        grid = grid.Substring(0, cur) + "" + key[0] + "" + grid.Substring(cur + 1);
        cur = (int.Parse(convertBase(bins.Substring(12), 2, 10)) + cur + 1) % 64;
        grid = grid.Substring(0, cur) + "" + key[1] + "" + grid.Substring(cur + 1);
        logMessages.Add(string.Format("Keyword: {0}", keyword));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", kwfront.Expression, kwfront.Value));
        logMessages.Add(string.Format("Key: {0}", key));
        logMessages.Add(string.Format("Screen 2: {0}", base16));
        logMessages.Add(string.Format("Screen B: {0}{1} -> {2}, {3} -> {4}, {5}", vals[0].Expression, vals[1].Expression, vals[0].Value, vals[1].Value, shifts[0], shifts[1]));
        logMessages.Add(string.Format("{0}", bins));
        for (int i = 0; i < 8; i++)
            logMessages.Add(string.Format("{0}", grid.Substring(i * 8, 8)));
        for (int i = 2; i < 26; i += 4)
        {
            bins = XOR(bins, bins.Substring(shifts[0]) + "0000000000".Substring(0, shifts[0]));
            bins = XOR(bins, "0000000000".Substring(0, shifts[1]) + bins.Substring(0, bins.Length - shifts[1]));
            logMessages.Add(string.Format("{0}", bins));
            for (int j = 0; j < 4; j++)
            {
                cur = (int.Parse(convertBase(bins.Substring(j * 4, 4), 2, 10)) + cur + 1) % 64;
                while (grid[cur] != '-')
                    cur = (cur + 1) % 64;
                grid = grid.Substring(0, cur) + "" + key[i + j] + "" + grid.Substring(cur + 1);
            }
            for (int k = 0; k < 8; k++)
                logMessages.Add(string.Format("{0}", grid.Substring(k * 8, 8)));
        }
        //Encrypting the word
        if (invert)
        {
            for (int i = 0; i < word.Length; i++)
            {
                cur = grid.IndexOf(word[i]);
                List<int[]> poss = new List<int[]>();
                for (int j = 1; j < 27; j++)
                    poss.Add(new int[] { CMTools.mod(cur + j, 64) + 0x100, j });
                for (int j = 1; j < 27; j++)
                    poss.Add(new int[] { CMTools.mod(cur - j, 64) + 0x140, j });
                int[] choice = poss[UnityEngine.Random.Range(0, poss.Count)];
                symbols = symbols + "" + ((char)(choice[0]));
                encrypt = encrypt + "" + alpha[choice[1]];
                logMessages.Add(string.Format("{0} + {1}{2} + {3} -> {4}", word[i], "ABCDEFGH"[(choice[0] - 0x100) % 8], "1234567812345678"[(choice[0] - 0x100) / 8], choice[0] >= 0x140 ? "OUTLINED" : "FILLED", encrypt[i]));
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                cur = grid.IndexOf(word[i]);
                List<int[]> poss = new List<int[]>();
                for (int j = 1; j < 27; j++)
                    poss.Add(new int[] { CMTools.mod(cur - j, 64) + 0x100, j });
                for (int j = 1; j < 27; j++)
                    poss.Add(new int[] { CMTools.mod(cur + j, 64) + 0x140, j });
                int[] choice = poss[UnityEngine.Random.Range(0, poss.Count)];
                symbols = symbols + "" + ((char)(choice[0]));
                encrypt = encrypt + "" + alpha[choice[1]];
                logMessages.Add(string.Format("{0} + {1}{2} + {3} -> {4}", word[i], "ABCDEFGH"[(choice[0] - 0x100) % 8], "1234567812345678"[(choice[0] - 0x100) / 8], choice[0] >= 0x140 ? "OUTLINED" : "FILLED", encrypt[i]));
            }
        }
        ScreenInfo[] screenSymbols = new ScreenInfo[] {
            new ScreenInfo(symbols.Substring(0, symbols.Length / 2), CMFont.Dreamcipher),
            new ScreenInfo(symbols.Substring(symbols.Length / 2), CMFont.Dreamcipher)
        };
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { keyword, kwfront.Expression, base16, vals[0].Expression + vals[1].Expression, screenSymbols[0], null, screenSymbols[1] }, invert) },
            Score = 22
        };
    }
    private string convertBase(string num, int b1, int b2)
    {
        if (b1 > 36 || b2 > 36)
            return null;
        string alpha = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int mult = 1;
        var sum = 0;
        for (int i = num.Length - 1; i >= 0; i--)
        {
            sum += (mult * alpha.IndexOf(num[i]));
            mult *= b1;
        }
        string result = "";
        while (sum > 0)
        {
            result = alpha[(sum % b2)] + "" + result;
            sum /= b2;
        }
        if (result.Length == 0)
            result = "0";
        return result;
    }
    private string XOR(string b1, string b2)
    {
        string result = "";
        for (int i = 0; i < b1.Length && i < b2.Length; i++)
            result = result + (b1[i] == b2[i] ? "0" : "1");
        return result;
    }
}
