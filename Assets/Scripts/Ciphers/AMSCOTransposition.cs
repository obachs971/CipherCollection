using CipherMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class AMSCOTransposition
{
	public PageInfo[] encrypt(string word, string id, string log, bool invert)
	{
		Debug.LogFormat("{0} Begin AMSCO Transposition", log);
		int[] nums = { 2, 2, 3, 4, 4 };
		string key = new string("12345".Substring(0, 2 + (UnityEngine.Random.Range(0, nums[word.Length - 4]))).ToCharArray().Shuffle());
		while ("12345".Contains(key))
			key = new string(key.ToCharArray().Shuffle());
		int start = word.Length % 3;
		if(start == 0)
		{
			if (sum(word) % 2 == 0) start = 2;
			else start = 1;
		}
		start--;
		int numGroups = ((word.Length / 3) * 2) + ((word.Length % 3) / 2) + ((word.Length % 3) % 2);
		Debug.LogFormat("{0} [AMSCO Transposition] Key: {1}", log, key);
		Debug.LogFormat("{0} [AMSCO Transposition] Start Number: {1}", log, (start + 1));
		string[][] grid;
		if (key.Length % numGroups == 0)
			grid = new string[numGroups / key.Length][];
		else
			grid = new string[numGroups / key.Length + 1][];
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
		Debug.LogFormat("{0} [AMSCO Transposition] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
		if (invert)
		{
			int len = 0, cur = 0, index = 0;
			for (int i = 0; i < word.Length; i += len)
			{
				len = grid[cur / grid[0].Length][cur % grid[0].Length].Length;
				grid[cur / grid[0].Length][cur % grid[0].Length] = "**".Substring(0, len);
				cur++;
			}
			cur = 0;
			for(int i = 0; i < key.Length; i++)
			{
				index = key.IndexOf("1234"[i]);
				for(int j = 0; j < grid.Length; j++)
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
				Debug.LogFormat("{0} [AMSCO Transposition] {1}", log, string.Join(" ", grid[i]));
			}
		}
		else
		{
			int len = 0, cur = 0;
			for(int i = 0; i < word.Length; i+=len)
			{
				len = grid[cur / grid[0].Length][cur % grid[0].Length].Length;
				grid[cur / grid[0].Length][cur % grid[0].Length] = word.Substring(i, len);
				cur++;
			}
			for(int i = 0; i < key.Length; i++)
			{
				cur = key.IndexOf("1234"[i]);
				for(int j = 0; j < grid.Length; j++)
					encrypt += grid[j][cur];
			}
			for(int i = 0; i < grid.Length; i++)
				Debug.LogFormat("{0} [AMSCO Transposition] {1}", log, string.Join(" ", grid[i]));
		}
		encrypt = encrypt.Replace("-", "");
		Debug.LogFormat("{0} [AMSCO Transposition] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(key, new int[] { 35, 35, 35, 35, 35, 32 }[key.Length - 2]);
		for (int i = 1; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens, invert) });
	}
	private int sum(string s)
	{
		int sum = 0;
		foreach (char c in s)
			sum += "-ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c);
		return sum;
	}
}