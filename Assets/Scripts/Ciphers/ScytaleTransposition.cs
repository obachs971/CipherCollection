using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class ScytaleTransposition
{
	public ResultInfo encrypt(string word, string id, string log, KMBombInfo Bomb, bool invert)
	{
		Debug.LogFormat("{0} Begin Scytale Transposition", log);
		int[] key = CMTools.generateValue(Bomb);
		int rows = (key[1] % (word.Length - 2)) + 2;
		Debug.LogFormat("{0} [Scytale Transposition] Number Rows: {1} -> {2} -> {3}", log, (char)key[0], key[1], rows);
		Debug.LogFormat("{0} [Scytale Transposition] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
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
				Debug.LogFormat("{0} [Scytale Transposition] {1}", log, grid[i]);
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
				Debug.LogFormat("{0} [Scytale Transposition] {1}", log, grid[i]);
				encrypt += grid[i];
			}
		}

		Debug.LogFormat("{0} [Scytale Transposition] {1} - > {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(((char)key[0]) + "", 35);
		screens[8] = new ScreenInfo(id, 35);
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens, invert) }
		};
	}
}