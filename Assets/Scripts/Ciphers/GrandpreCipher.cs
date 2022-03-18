using CipherMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class GrandpreCipher
{
	public PageInfo[] encrypt(string word, string id, string log)
	{
		Debug.LogFormat("{0} Begin Grandpré Cipher", log);
		int len = UnityEngine.Random.Range(0, 3) + 2;
		string[] words = generateWords(len);
		while (words == null)
			words = generateWords(len);
		string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", encrypt = "", screenRows = "", key = "";
		string[] possLets = new string[words.Length];
		for (int i = 0; i < words.Length; i++)
		{
			key += words[i];
			possLets[i] = "";
			Debug.LogFormat("{0} [Grandpré Cipher] Keyword #{1}: {2}", log, (i+1), words[i]);
		}
		for (int i = 0; i < alpha.Length; i++)
			possLets[i % possLets.Length] = possLets[i % possLets.Length] + "" + alpha[i];
		for (int i = 0; i < word.Length; i++)
		{
			List<int> poss = new List<int>();
			for(int j = 0; j < key.Length; j++)
			{
				if (word[i] == key[j])
					poss.Add(j);
			}
			int index = poss[UnityEngine.Random.Range(0, poss.Count)];
			int row = index / words.Length, col = (index % words.Length) + 1;
			Debug.LogFormat("{0} [Grandpré Cipher] {1} -> {2}, {3}", log, word[i], (row + 1), col);
			encrypt = encrypt + "" + possLets[row].ToCharArray().Shuffle()[0];
			screenRows = screenRows + "" + col;
		}
		Debug.LogFormat("{0} [Grandpré Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[][] screens = new ScreenInfo[2][];
		for (int i = 0; i < screens.Length; i++)
			screens[i] = new ScreenInfo[9];
		for(int i = 0; i < 4; i++)
			screens[0][i * 2] = new ScreenInfo(words[i], new int[] { 35, 32, 28 }[words[i].Length - 6]);
		screens[0][1] = new ScreenInfo(screenRows.Substring(0, screenRows.Length / 2), (screenRows.Length) > 7 ? 20 : 25);
		screens[0][3] = new ScreenInfo(screenRows.Substring(screenRows.Length / 2), (screenRows.Length) > 6 ? 20 : 25);
		screens[0][5] = new ScreenInfo();
		screens[0][7] = new ScreenInfo();
		for(int i = 0; i < words.Length - 4; i++)
			screens[1][i * 2] = new ScreenInfo(words[i + 4], new int[] { 35, 32, 28 }[words[i].Length - 6]);
		for (int i = words.Length - 4; i < 4; i++)
			screens[1][i * 2] = new ScreenInfo();
		for(int i = 1; i < 8; i+=2)
			screens[1][i] = new ScreenInfo();
		screens[0][8] = new ScreenInfo(id, 35);
		screens[1][8] = new ScreenInfo(id, 35);
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens[0]), new PageInfo(screens[1])});
	}
	private string[] generateWords(int len)
	{
		List<string> wordList = new Data().allWords[len];
		string[] words = new string[len + 4];
		string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		for(int i = 0; i < words.Length; i++)
		{
			int best = 0;
			List<string> poss = new List<string>();
			for (int j = 0; j < wordList.Count; j++)
			{
				
				int score = 0;
				for(int k = 0; k < alpha.Length; k++)
				{
					if (wordList[j].IndexOf(alpha[k]) >= 0)
						score++;
				}
				if(score > best)
				{
					best = score;
					poss.Clear();
					poss.Add(wordList[j].ToUpperInvariant());
				}
				else if(score == best)
					poss.Add(wordList[j].ToUpperInvariant());
			}
			words[i] = poss[UnityEngine.Random.Range(0, poss.Count)].ToUpperInvariant();
			wordList.Remove(words[i]);
			for (int j = 0; j < words[i].Length; j++)
				alpha = alpha.Replace(words[i][j] + "", "");
		}
		if (alpha.Length > 0)
			return null;
		return words.Shuffle();
	}
}