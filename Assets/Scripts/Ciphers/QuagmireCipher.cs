﻿using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class QuagmireCipher
{
	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Quagmire Cipher", log);
		CMTools cm = new CMTools();
		List<List<string>> words = new Data().allWords;
		int length = UnityEngine.Random.Range(0, words.Count);
		string kw1 = words[length][UnityEngine.Random.Range(0, words[length].Count)];
		length = UnityEngine.Random.Range(0, word.Length - 3);
		string kw2 = words[length][UnityEngine.Random.Range(0, words[length].Count)];
		string[] invert = cm.generateBoolExp(Bomb);
		string[] key = new string[kw2.Length];
		for(int i = 0; i < key.Length; i++)
		{
			key[i] = cm.getKey(kw1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", true);
			int index = key[i].IndexOf(kw2[i]);
			key[i] = key[i].Substring(index) + key[i].Substring(0, index);
		}
		Debug.LogFormat("{0} [Quagmire Cipher] KW1: {1}", log, kw1);
		Debug.LogFormat("{0} [Quagmire Cipher] KW2: {1}", log, kw2);
		Debug.LogFormat("{0} [Quagmire Cipher] Invert Rule: {1} -> {2}", log, invert[0], invert[1]);
		string encrypt = "", alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		if(invert[1][0] == 'T')
		{
			for (int i = 0; i < word.Length; i++)
				encrypt = encrypt + "" + alpha[key[i % key.Length].IndexOf(word[i])];
		}
		else
		{
			for (int i = 0; i < word.Length; i++)
				encrypt = encrypt + "" + key[i % key.Length][alpha.IndexOf(word[i])];
		}
		Debug.LogFormat("{0} [Quagmire Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw1, new int[] { 35, 35, 35, 32, 28 }[kw1.Length - 4]);
		screens[1] = new ScreenInfo(invert[0], 25);
		screens[2] = new ScreenInfo(kw2, new int[] { 35, 35, 35, 32, 28 }[kw2.Length - 4]);
		for (int i = 3; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens) });
	}
}