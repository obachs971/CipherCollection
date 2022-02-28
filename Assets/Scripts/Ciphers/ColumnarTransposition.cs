using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class ColumnarTransposition
{
	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Columnar Transposition", log);
		string key = "1234567".Substring(0, 2 + UnityEngine.Random.Range(0, word.Length - 2));
		key = new string(key.ToCharArray().Shuffle());
		string[] invert = new CMTools().generateBoolExp(Bomb);
		Debug.LogFormat("{0} [Columnar Transposition] Key: {1}", log, key);
		Debug.LogFormat("{0} [Columnar Transposition] Invert Rule: {1} -> {2}", log, invert[0], invert[1]);
		string encrypt = "";
		while (word.Length % key.Length != 0)
			word += "-";
		char[][] grid = new char[word.Length / key.Length][];
		for(int i = 0; i < grid.Length; i++)
			grid[i] = new char[key.Length];
		if(invert[1][0] == 'T')
		{
			int bot = word.Length / key.Length - 1;
			word = word.Replace("-", "");
			int cur = 0, mod = word.Length % key.Length;
			for (int i = 1; i <= key.Length; i++)
			{
				for (int j = 0; j < grid.Length; j++)
				{
					if(j == bot && mod > 0 && key.IndexOf(i + "") >= mod)
						grid[j][key.IndexOf(i + "")] = '-';
					else
						grid[j][key.IndexOf(i + "")] = word[cur++];
				}
			}
			for (int i = 0; i < grid.Length; i++)
			{
				Debug.LogFormat("{0} [Columnar Transposition] {1}", log, new string(grid[i]));
				encrypt += new string(grid[i]);
			}
				
		}
		else
		{
			for(int i = 0; i < grid.Length; i++)
			{
				grid[i] = word.Substring(i * key.Length, key.Length).ToCharArray();
				Debug.LogFormat("{0} [Columnar Transposition] {1}", log, new string(grid[i]));
			}
			for (int i = 1; i <= key.Length; i++)
			{
				for (int j = 0; j < grid.Length; j++)
					encrypt = encrypt + "" + grid[j][key.IndexOf(i + "")];
			}
		}
		encrypt = encrypt.Replace("-", "");
		Debug.LogFormat("{0} [Columnar Transposition] {1} - > {2}", log, word.Replace("-", ""), encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(key, (key.Length == 7 ? 32 : 35));
		screens[1] = new ScreenInfo(invert[0], 25);
		for (int i = 2; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens) });
	}
}