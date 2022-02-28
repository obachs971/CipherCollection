using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class MonoalphabeticCipher
{
	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Monoalphabetic Cipher", log);
		CMTools cm = new CMTools();
		string[] invert = cm.generateBoolExp(Bomb);
		string[] kws = generateKeywords();
		while (kws == null)
			kws = generateKeywords();
		string key = new CMTools().getKey(kws[0] + kws[1] + kws[2] + kws[3] + kws[4] + kws[5], "", false);
		for (int i = 0; i < kws.Length; i++)
			Debug.LogFormat("{0} [Monoalphabetic Cipher] KW{1}: {2}", log, (i + 1), kws[i]);
		Debug.LogFormat("{0} [Monoalphabetic Cipher] Key: {1}", log, key);
		Debug.LogFormat("{0} [Monoalphabetic Cipher] Invert Rule: {1} -> {2}", log, invert[0], invert[1]);
		string encrypt = "";
		if(invert[1][0] == 'T')
		{
			foreach (char c in word)
				encrypt = encrypt + "" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[key.IndexOf(c)];
		}
		else
		{
			foreach (char c in word)
				encrypt = encrypt + "" + key["ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c)];
		}
		Debug.LogFormat("{0} [Monoalphabetic Cipher] {1} - > {2}", log, word, encrypt);
		ScreenInfo[][] screens = { new ScreenInfo[9], new ScreenInfo[9] };
		screens[0][0] = new ScreenInfo(kws[0], new int[] { 35, 35, 35, 32, 28 }[kws[0].Length - 4]);
		screens[0][1] = new ScreenInfo(invert[0], 25);
		for(int i = 2; i < 8; i+=2)
		{
			screens[0][i] = new ScreenInfo(kws[i/2], new int[] { 35, 35, 35, 32, 28 }[kws[i/2].Length - 4]);
			screens[0][i + 1] = new ScreenInfo();
		}
		screens[0][8] = new ScreenInfo(id, 35);
		screens[1][0] = new ScreenInfo(kws[4], new int[] { 35, 35, 35, 32, 28 }[kws[4].Length - 4]);
		screens[1][1] = new ScreenInfo();
		screens[1][2] = new ScreenInfo(kws[5], new int[] { 35, 35, 35, 32, 28 }[kws[5].Length - 4]);
		for (int i = 3; i < 8; i++)
			screens[1][i] = new ScreenInfo();
		screens[1][8] = new ScreenInfo(id, 35);
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens[0]), new PageInfo(screens[1])});
	}
	private string[] generateKeywords()
	{
		string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		string[] kws = new string[6];
		int[] order = { 0, 1, 2, 3, 4 };
		order.Shuffle();
		List<List<string>> words = new Data().allWords;
		for(int i = 0; i < order.Length; i++)
		{
			int best = 0;
			List<string> bestWords = new List<string>();
			for(int j = 0; j < words[order[i]].Count; j++)
			{
				int score = getScore(words[order[i]][j], alpha.ToUpperInvariant());
				if (score > best)
				{
					bestWords.Clear();
					bestWords.Add(words[order[i]][j]);
					best = score;
				}
				else if(score == best)
					bestWords.Add(words[order[i]][j]);
			}
			kws[i] = bestWords[UnityEngine.Random.Range(0, bestWords.Count)];
			words[order[i]].Remove(kws[i]);
			for (int j = 0; j < kws[i].Length; j++)
				alpha = alpha.Replace(kws[i][j] + "", "");
		}
		int lastBest = 0;
		List<string> lastBestWords = new List<string>();
		for (int i = 0; i < words.Count; i++)
		{
			for(int j = 0; j < words[i].Count; j++)
			{
				int score = getScore(words[i][j], alpha.ToUpperInvariant());
				if(score > lastBest)
				{
					lastBestWords.Clear();
					lastBestWords.Add(words[i][j]);
					lastBest = score;
				}
				else if (score == lastBest)
					lastBestWords.Add(words[i][j]);
			}
		}
		kws[5] = lastBestWords[UnityEngine.Random.Range(0, lastBestWords.Count)];
		for (int j = 0; j < kws[5].Length; j++)
			alpha = alpha.Replace(kws[5][j] + "", "");
		if (alpha.Length > 0)
			return null;
		return kws.Shuffle();
	}
	private int getScore(string word, string alpha)
	{
		int score = 0;
		for(int i = 0; i < word.Length; i++)
		{
			if(alpha.IndexOf(word[i]) >= 0)
				score++;
			alpha = alpha.Replace(word[i] + "", "");
		}
		return score;
	}
}