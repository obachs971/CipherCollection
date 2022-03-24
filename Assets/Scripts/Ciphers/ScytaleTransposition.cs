using System.Collections.Generic;
using CipherMachine;

public class ScytaleTransposition : CipherBase
{
    public override string Name { get { return invert ? "Inverted Scytale Transposition" : "Scytale Transposition"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "SC"; } }

    private readonly bool invert;
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
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(key.Expression, 35);
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
}