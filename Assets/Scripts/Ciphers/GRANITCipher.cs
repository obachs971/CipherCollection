using CipherMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class GRANITCipher : CipherBase
{
	//Create Straddling Checkerboard Screen 1, Screen A, Screen B
	//Turn word into numbers Screen 2
	//Myszkowski Transposition the numbers Screen 3
	//Turn numbers back into letters 
	// Check for Xs Screen 4
	public override string Name { get { return invert ? "Inverted GRANIT Cipher" : "GRANIT Cipher"; } }
	public override string Code { get { return "GN"; } }
	
	private readonly bool invert;
	public override bool IsInvert { get { return invert; } }
	public GRANITCipher(bool invert) { this.invert = invert; }

	private List<string> tempLog = new List<string>();

	public override ResultInfo Encrypt(string word, KMBombInfo bomb)
	{
		var logMessages = new List<string>();
		string replaceX = "";
		string alpha = "ABCDEFGHIJKLMNOPQRSTUVWYZ";
		logMessages.Add(string.Format("Before Replacing Xs: {0}", word));
		for (int i = 0; i < word.Length; i++)
		{
			if (word[i] == 'X')
			{
				word = word.Substring(0, i) + "" + alpha[UnityEngine.Random.Range(0, alpha.Length)] + "" + word.Substring(i + 1);
				replaceX = replaceX + "" + word[i];
			}
			else
				replaceX = replaceX + "" + alpha.Replace(word[i].ToString(), "")[UnityEngine.Random.Range(0, 24)];
		}
		logMessages.Add(string.Format("After Replacing Xs: {0}", word));
		logMessages.Add(string.Format("Screen 3: {0}", replaceX));
		string kw = new Data().PickWord(4, 8);
		var keyFront = CMTools.generateBoolExp(bomb);
		string key = CMTools.getKey(kw.Replace("X", ""), alpha, keyFront.Value);
		string rows = new string("123456789".ToCharArray().Shuffle()).Substring(0, 2);
		for (int i = 1; i < 10; i++)
		{
			if (rows.Contains(i + ""))
				key = key.Substring(0, i - 1) + "-" + key.Substring(i - 1);
		}
		logMessages.Add(string.Format("Keyword: {0}", kw));
		logMessages.Add(string.Format("Screen A: {0} -> {1}", keyFront.Expression, keyFront.Value));
		logMessages.Add(string.Format("Screen B: {0}", rows));
		logMessages.Add(string.Format("Key: {0}", key));
		List<int> encryptNums = new List<int>();
		foreach (char let in word)
		{
			int index = key.IndexOf(let);
			if (index < 9)
			{
				encryptNums.Add(index);
				logMessages.Add(string.Format("{0} -> {1}", let, index + 1));
			}
			else
			{
				encryptNums.Add((rows[(index / 9) - 1] - '0') - 1);
				encryptNums.Add(index % 9);
				logMessages.Add(string.Format("{0} -> {1}{2}", let, rows[(index / 9) - 1], (index % 9) + 1));
			}
		}
		Data wordGen = new Data();
		tryagain:
		tempLog = new List<string>();
		string screen2 = wordGen.PickWord(3, Math.Min(encryptNums.Count - 1, 8));
		List<int> transposedNums = performTransposition(encryptNums, screen2);
		string encrypt = getNewLetters(key, transposedNums, rows);
		if (encrypt == null || encrypt.Length != word.Length)
			goto tryagain;
		logMessages.AddRange(tempLog);
		
		//Just incase something goes wrong, we can change the if condition above to see if the length is less than the original
		string screen4 = encrypt.Substring(word.Length);
		encrypt = encrypt.Substring(0, word.Length);
		if(screen4.Length > 0)
			logMessages.Add(string.Format("Screen 4: {0}", screen4));

		return new ResultInfo
		{
			LogMessages = logMessages,
			Encrypted = encrypt,
			Pages = new[] { new PageInfo(new ScreenInfo[] { kw, keyFront.Expression, screen2, rows, replaceX, null, screen4 }, invert) },
			Score = 8
		};
	}
	private List<int> performTransposition(List<int> encryptNums, string kw)
	{
		char[] order = kw.ToCharArray();
		Array.Sort(order);
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
		tempLog.Add(string.Format("Screen 2: {0}", kw));
		tempLog.Add(string.Format("Key: {0}", string.Join("", key.Select(x => (x + 1).ToString()).ToArray())));
		List<int> newNums = new List<int>();
		if (invert)
		{
			string temp = "";
			foreach (int num in encryptNums)
				temp += "*";
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
							grid[i] = grid[i].Substring(0, j) + "" + encryptNums[cur++] + "" + grid[i].Substring(j + 1);

					}
				}
			}
			foreach (string row in grid)
			{
				string r = row.Replace("-", "");
				foreach (char num in r)
					newNums.Add(num - '0');
				tempLog.Add(getCorrectOutput(row));
			}
		}
		else
		{
			string temp = "";
			foreach (int num in encryptNums)
				temp += num;
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
						if (key[j] == z && grid[i][j] != '-')
							newNums.Add(grid[i][j] - '0');
					}
				}
			}
			for (int i = 0; i < grid.Length; i++)
				tempLog.Add(getCorrectOutput(grid[i]));
		}
		return newNums;
	}
	private string getNewLetters(string key, List<int> encryptNums, string rows)
	{
		string encrypt = "";
		while(encryptNums.Count > 0)
		{
			int num = encryptNums[0];
			encryptNums.RemoveAt(0);
			if (key[num] == '-')
			{
				if (encryptNums.Count == 0)
					return null;
				encrypt += key[((rows.IndexOf((num + 1) + "") + 1) * 9) + encryptNums[0]];
				tempLog.Add(string.Format("{0}{1} -> {2}", num + 1, encryptNums[0] + 1, encrypt[encrypt.Length - 1]));
				encryptNums.RemoveAt(0);

			}
			else
			{
				encrypt += key[num];
				tempLog.Add(string.Format("{0} -> {1}", num + 1, encrypt[encrypt.Length - 1]));
			}
		}
		return encrypt;
	}
	private string getCorrectOutput(string output)
	{
		string CO = output + "";
		string curr = "876543210";
		string conv = "987654321";
		for(int i = 0; i < 9; i++)
			CO = CO.Replace(curr[i], conv[i]);
		return CO;
	}
}
