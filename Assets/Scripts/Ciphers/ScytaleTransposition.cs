using System.Collections.Generic;
using CipherMachine;

public class ScytaleTransposition : CipherBase
{
    public override string Name { get { return invert ? "Inverted Scytale Transposition" : "Scytale Transposition"; } }
    public override string Code { get { return "SC"; } }
    public override bool IsTransposition { get { return true; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public ScytaleTransposition(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        var key = CMTools.generateValue(bomb);
        int rows = (key.Value % (word.Length - 2)) + 2;
        logMessages.Add(string.Format("Number Rows: {0} -> {1} -> {2}", key.Expression, key.Value, rows));
        string encrypt = "";
        string[] grid = new string[rows];
        for (int i = 0; i < grid.Length; i++)
            grid[i] = "";
        if (invert)
        {
            for (int i = 0; i < word.Length; i++)
                grid[i % rows] += "*";
            int cur = 0;
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                    grid[i] = grid[i].Substring(0, j) + "" + word[cur++] + "" + grid[i].Substring(j + 1);
                logMessages.Add(grid[i]);
            }
            for (int i = 0; i < word.Length; i++)
                encrypt = encrypt + "" + grid[i % rows][i / rows];
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
                grid[i % rows] = grid[i % rows] + "" + word[i];
            for (int i = 0; i < grid.Length; i++)
            {
                logMessages.Add(grid[i]);
                encrypt += grid[i];
            }
        }

        logMessages.Add(string.Format("{0} - > {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { key.Expression }, invert) },
            Score = 3
        };
    }
}
