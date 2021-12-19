using CipherMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class FractionatedMorseCipher
{
	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Fractionated Morse Cipher", log);
		Data data = new Data();
		CMTools cm = new CMTools(Bomb);
		int length = UnityEngine.Random.Range(0, 5);
		string kw = data.allWords[length][UnityEngine.Random.Range(0, data.allWords[length].Count())];
		string encrypt = "", morse = "", extra;
		string[] keyFront = cm.generateBoolExp();
		string key = cm.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", keyFront[1][0] == 'T');

		//Convert the letters of the word into morse
		foreach (char c in word)
			morse = morse + "" + letterToMorse(c) + "x";
		morse = morse + "x";
		
		//Adjusting it so it has at least the same amount of letters as the initial word length as well as being divisible by 3
		while (morse.Length / 3 < word.Length)
		{
			List<int> l = AllIndexesOf(morse, ".x.");
			l.AddRange(AllIndexesOf(morse, ".x-"));
			l.AddRange(AllIndexesOf(morse, "-x."));
			l.AddRange(AllIndexesOf(morse, "-x-"));
			int index = l[UnityEngine.Random.Range(0, l.Count())] + 1;
			morse = morse.Substring(0, index) + "x" + morse.Substring(index);
		}
		morse = morse.Substring(0, morse.Length - (morse.Length % 3));
		
		//Now it gets encrypted into the letters using the key
		for (int i = 0; i < morse.Length; i+=3)
			encrypt = encrypt + "" + key[(".-x".IndexOf(morse[i]) * 9) + (".-x".IndexOf(morse[i + 1]) * 3) + ".-x".IndexOf(morse[i + 2])];
		Debug.LogFormat("{0} [Fractionated Morse Cipher] Keyword: {1}", log, kw);
		Debug.LogFormat("{0} [Fractionated Morse Cipher] Key Front Rule: {1} -> {2}", log, keyFront[0], keyFront[1]);
		Debug.LogFormat("{0} [Fractionated Morse Cipher] Key: {1}", log, key);
		Debug.LogFormat("{0} [Fractionated Morse Cipher] {1} -> {2}", log, word, morse);
		Debug.LogFormat("{0} [Fractionated Morse Cipher] {1} -> {2}", log, morse, encrypt);
		extra = encrypt.Substring(word.Length);
		encrypt = encrypt.Substring(0, word.Length);
		Debug.LogFormat("{0} [Fractionated Morse Cipher] Encrypted Word: {1}", log, encrypt);
		Debug.LogFormat("{0} [Fractionated Morse Cipher] Screen 2: {1}", log, extra);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
		screens[1] = new ScreenInfo(keyFront[0], 25);
		screens[2] = new ScreenInfo(extra, 35);
		screens[8] = new ScreenInfo(id, 35);
		for (int i = 3; i < 8; i++)
			screens[i] = new ScreenInfo();
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens) });
	}
	private string letterToMorse(char c)
	{
		switch (c)
		{
			case 'A': return ".-";
			case 'B': return "-...";
			case 'C': return "-.-.";
			case 'D': return "-..";
			case 'E': return ".";
			case 'F': return "..-.";
			case 'G': return "--.";
			case 'H': return "....";
			case 'I': return "..";
			case 'J': return ".---";
			case 'K': return "-.-";
			case 'L': return ".-..";
			case 'M': return "--";
			case 'N': return "-.";
			case 'O': return "---";
			case 'P': return ".--.";
			case 'Q': return "--.-";
			case 'R': return ".-.";
			case 'S': return "...";
			case 'T': return "-";
			case 'U': return "..-";
			case 'V': return "...-";
			case 'W': return ".--";
			case 'X': return "-..-";
			case 'Y': return "-.--";
			case 'Z': return "--..";
		}
		return "";
	}
	private static List<int> AllIndexesOf(string str, string value)
	{
		List<int> indexes = new List<int>();
		for (int index = 0; ; index += value.Length)
		{
			index = str.IndexOf(value, index);
			if (index == -1)
				return indexes;
			indexes.Add(index);
		}
	}
}
