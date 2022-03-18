using CipherMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class Chaocipher
{
	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb, bool invert)
	{
		Debug.LogFormat("{0} Begin Chaocipher", log);
		List<List<string>> words = new Data().allWords;
		CMTools cm = new CMTools();
		string[] kws = new string[2], keys = new string[2];
		string[][] kwfronts = new string[2][];
		string encrypt = "";
		for(int i = 0; i < 2; i++)
		{
			int len = UnityEngine.Random.Range(0, words.Count);
			kws[i] = words[len][UnityEngine.Random.Range(0, words[len].Count)];
			words[len].Remove(kws[i]);
			kwfronts[i] = cm.generateBoolExp(Bomb);
			keys[i] = cm.getKey(kws[i], "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfronts[i][1][0] == 'T');
			Debug.LogFormat("{0} [Chaocipher] Keyword #{1}: {2}", log, (i + 1), kws[i]);
			Debug.LogFormat("{0} [Chaocipher] Key #{1}: {2} -> {3} -> {4}", log, (i + 1), kwfronts[i][0], kwfronts[i][1], keys[i]);
		}
		Debug.LogFormat("{0} [Chaocipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
		if (invert)
		{
			for (int i = 0; i < word.Length; i++)
			{
				Debug.LogFormat("{0} [Chaocipher] {1}", log, keys[0]);
				Debug.LogFormat("{0} [Chaocipher] {1}", log, keys[1]);
				int index = keys[1].IndexOf(word[i]);
				encrypt = encrypt + "" + keys[0][index];
				keys[0] = keys[0].Substring(index + 1) + keys[0].Substring(0, index + 1);
				keys[0] = keys[0].Substring(0, 2) + keys[0].Substring(3, 11) + keys[0][2] + keys[0].Substring(14);
				keys[1] = keys[1].Substring(index) + keys[1].Substring(0, index);
				keys[1] = keys[1].Substring(0, 1) + keys[1].Substring(2, 12) + keys[1][1] + keys[1].Substring(14);
				Debug.LogFormat("{0} [Chaocipher] {1} -> {2}", log, word[i], encrypt[i]);
			}
		}
		else
		{
			for (int i = 0; i < word.Length; i++)
			{
				Debug.LogFormat("{0} [Chaocipher] {1}", log, keys[0]);
				Debug.LogFormat("{0} [Chaocipher] {1}", log, keys[1]);
				int index = keys[0].IndexOf(word[i]);
				encrypt = encrypt + "" + keys[1][index];
				keys[0] = keys[0].Substring(index + 1) + keys[0].Substring(0, index + 1);
				keys[0] = keys[0].Substring(0, 2) + keys[0].Substring(3, 11) + keys[0][2] + keys[0].Substring(14);
				keys[1] = keys[1].Substring(index) + keys[1].Substring(0, index);
				keys[1] = keys[1].Substring(0, 1) + keys[1].Substring(2, 12) + keys[1][1] + keys[1].Substring(14);
				Debug.LogFormat("{0} [Chaocipher] {1} -> {2}", log, word[i], encrypt[i]);
			}
		}
		Debug.LogFormat("{0} [Chaocipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kws[0], new int[] { 35, 35, 35, 32, 28 }[kws[0].Length - 4]);
		screens[1] = new ScreenInfo(kwfronts[0][0], 25);
		screens[2] = new ScreenInfo(kws[1], new int[] { 35, 35, 35, 32, 28 }[kws[1].Length - 4]);
		screens[3] = new ScreenInfo(kwfronts[1][0], 25);
		for (int i = 4; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens, invert) });
	}
}