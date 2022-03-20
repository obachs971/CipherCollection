using System;
using System.Linq;
using CipherMachine;
using UnityEngine;
using Words;

public class MyszkowskiTransposition
{
	public ResultInfo encrypt(string word, string id, string log, bool invert)
	{
		Debug.LogFormat("{0} Begin Myszkowski Transposition", log);
		
		string encrypt = "";
		string kw = new Data().PickWord(4, word.Length);
		char[] order = kw.ToCharArray();
		Array.Sort(order);
		order = order.Distinct().ToArray();
		int[] key = new int[kw.Length];
		for(int i = 0; i < order.Length; i++)
		{
			for(int j = 0; j < kw.Length; j++)
			{
				if (order[i] == kw[j])
					key[j] = i;
			}
		}
		Debug.LogFormat("{0} [Myszkowski Transposition] Keyword: {1}", log, kw);
		Debug.LogFormat("{0} [Myszkowski Transposition] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
		Debug.LogFormat("{0} [Myszkowski Transposition] Key: {1}", log, string.Join("", key.Select(x => (x + 1).ToString()).ToArray()));
		if (invert)
		{
			string temp = "********".Substring(0, word.Length);
			while (temp.Length % kw.Length > 0)
				temp += "-";
			string[] grid = new string[temp.Length / kw.Length];
			for(int i = 0; i < grid.Length; i++)
				grid[i] = temp.Substring(i * kw.Length, kw.Length);
			int cur = 0;
			for(int z = 0; z < key.Length; z++)
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
			for(int i = 0; i < grid.Length; i++)
			{
				encrypt += grid[i];
				Debug.LogFormat("{0} [Myszkowski Transposition] {1}", log, grid[i]);
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
				Debug.LogFormat("{0} [Myszkowski Transposition] {1}", log, grid[i]);
		}
		encrypt = encrypt.Replace("-", "");
		Debug.LogFormat("{0} [Myszkowski Transposition] {1} - > {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 35, 32, 28 }[kw.Length - 3]);
		for (int i = 1; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens, invert) }
		};
	}
}