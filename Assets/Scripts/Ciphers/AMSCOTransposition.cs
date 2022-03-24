using System.Collections.Generic;
using CipherMachine;

public class AMSCOTransposition : CipherBase
{
    public override string Name { get { return invert ? "Inverted AMSCO Transposition" : "AMSCO Transposition"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "AM"; } }

    private readonly bool invert;
    public AMSCOTransposition(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        int[] nums = { 2, 2, 3, 4, 4 };
        string key = new string("12345".Substring(0, 2 + (UnityEngine.Random.Range(0, nums[word.Length - 4]))).ToCharArray().Shuffle());
        while ("12345".Contains(key))
            key = new string(key.ToCharArray().Shuffle());
        int start = word.Length % 3;
        if (start == 0)
        {
            if (sum(word) % 2 == 0) start = 2;
            else start = 1;
        }
        start--;
        int numGroups = ((word.Length / 3) * 2) + ((word.Length % 3) / 2) + ((word.Length % 3) % 2);
        logMessages.Add(string.Format("Key: {0}", key));
        logMessages.Add(string.Format("Start Number: {0}", (start + 1)));
        string[][] grid = new string[(numGroups + key.Length - 1) / key.Length][];
        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = new string[key.Length];
            for (int j = 0; j < grid[i].Length; j++)
            {
                grid[i][j] = "--".Substring(0, start + 1);
                start = (start + 1) % 2;
            }
        }
        string encrypt = "";
        if (invert)
        {
            int cur = 0, len;
            for (int i = 0; i < word.Length; i += len)
            {
                len = grid[cur / grid[0].Length][cur % grid[0].Length].Length;
                grid[cur / grid[0].Length][cur % grid[0].Length] = "**".Substring(0, len);
                cur++;
            }
            cur = 0;
            for (int i = 0; i < key.Length; i++)
            {
                var index = key.IndexOf("1234"[i]);
                for (int j = 0; j < grid.Length; j++)
                {
                    if (grid[j][index].Contains("*"))
                    {
                        grid[j][index] = word.Substring(cur, grid[j][index].Length);
                        cur += grid[j][index].Length;
                    }
                }
            }
            for (int i = 0; i < grid.Length; i++)
            {
                encrypt += string.Join("", grid[i]);
                logMessages.Add(string.Join(" ", grid[i]));
            }
        }
        else
        {
            int cur = 0, len;
            for (int i = 0; i < word.Length; i += len)
            {
                len = grid[cur / grid[0].Length][cur % grid[0].Length].Length;
                grid[cur / grid[0].Length][cur % grid[0].Length] = word.Substring(i, len);
                cur++;
            }
            for (int i = 0; i < key.Length; i++)
            {
                cur = key.IndexOf("1234"[i]);
                for (int j = 0; j < grid.Length; j++)
                    encrypt += grid[j][cur];
            }
            for (int i = 0; i < grid.Length; i++)
                logMessages.Add(string.Join(" ", grid[i]));
        }
        encrypt = encrypt.Replace("-", "");
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(new[] { new ScreenInfo(key, 0) }, invert) }
        };
    }
    private int sum(string s)
    {
        int sum = 0;
        foreach (char c in s)
            sum += "-ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c);
        return sum;
    }
}