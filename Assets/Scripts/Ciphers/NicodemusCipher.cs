using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class NicodemusCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Nicodemus Cipher" : "Nicodemus Cipher"; } }
    public override string Code { get { return "NC"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public NicodemusCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY", encrypt = "";
        string kw = new Data().PickWord(3, word.Length);
        char[] order = kw.ToCharArray();
        System.Array.Sort(order);
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
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] != '-')
                        encrypt = encrypt + "" + alpha[CMTools.mod(alpha.IndexOf(grid[i][j]) - alpha.IndexOf(kw[j]), 26)];
                }
                logMessages.Add(grid[i]);
            }
        }
        else
        {
            while (word.Length % kw.Length > 0)
                word += "-";
            char[][] grid = new char[word.Length / kw.Length][];
            for (int i = 0; i < grid.Length; i++)
                grid[i] = new char[kw.Length];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = word.Substring(i * kw.Length, kw.Length).ToCharArray();
                logMessages.Add(new string(grid[i]));
            }
            for (int i = 0; i < key.Length; i++)
            {
                for (int j = 0; j < grid.Length; j++)
                {
                    for (int k = 0; k < grid[j].Length; k++)
                    {
                        if (key[k] == i && grid[j][k] != '-')
                            encrypt = encrypt + "" + alpha[CMTools.mod(alpha.IndexOf(grid[j][k]) + alpha.IndexOf(kw[k]), 26)];
                    }
                }
            }
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw }, invert) },
            Score = 6
        };
    }
}
