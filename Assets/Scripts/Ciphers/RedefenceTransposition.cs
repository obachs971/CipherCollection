using System.Collections.Generic;
using CipherMachine;

public class RedefenceTransposition : CipherBase
{
    public override string Name { get { return invert ? "Inverted Redefence Transposition" : "Redefence Transposition"; } }
    public override int Score(int wordLength) { return 3; }
    public override string Code { get { return "RE"; } }
    public override bool IsTransposition { get { return true; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public RedefenceTransposition(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string key = new string("1234567".Substring(0, UnityEngine.Random.Range(2, word.Length - 1)).ToCharArray().Shuffle());
        logMessages.Add(string.Format("Key: {0}", key));
        int offset = 1, cursor = 1;
        string encrypt = "";
        string[] grid = new string[key.Length];
        for (int i = 0; i < grid.Length; i++)
            grid[i] = "";
        if (invert)
        {
            grid[0] += "*";
            for (int i = 1; i < word.Length; i++)
            {
                grid[cursor] += "*";
                if (cursor == 0 || cursor == (grid.Length - 1))
                    offset = -offset;
                cursor += offset;
            }
            cursor = 0;
            for (int i = 0; i < key.Length; i++)
            {
                int row = "1234567".IndexOf(key[i]);
                grid[row] = word.Substring(cursor, grid[row].Length);
                cursor += grid[row].Length;
            }
            for (int i = 0; i < grid.Length; i++)
                logMessages.Add(grid[i]);
            encrypt = grid[0][0] + "";
            grid[0] = grid[0].Substring(1);
            offset = 1;
            cursor = 1;
            for (int i = 1; i < word.Length; i++)
            {
                encrypt = encrypt + "" + grid[cursor][0];
                grid[cursor] = grid[cursor].Substring(1);
                if (cursor == 0 || cursor == (grid.Length - 1))
                    offset = -offset;
                cursor += offset;
            }
        }
        else
        {
            grid[0] = grid[0] + "" + word[0];
            for (int i = 1; i < word.Length; i++)
            {
                grid[cursor] = grid[cursor] + "" + word[i];
                if (cursor == 0 || cursor == (grid.Length - 1))
                    offset = -offset;
                cursor += offset;
            }
            for (int i = 0; i < key.Length; i++)
            {
                logMessages.Add(grid[i]);
                encrypt += grid["1234567".IndexOf(key[i])];
            }
        }
        logMessages.Add(string.Format("{0} - > {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { key }, invert) }
        };
    }
}
