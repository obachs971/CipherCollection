using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class VigenereCipher 
{
	public PageInfo[] encrypt(string word, string id, string log, bool invert)
	{
		Debug.LogFormat("{0} Begin Vigenere Cipher", log);
		List<List<string>> words = new Data().allWords;
		words.Insert(0, new Data().word3);
		int len = UnityEngine.Random.Range(0, word.Length - 2);
		string keyword = words[len][UnityEngine.Random.Range(0, words[len].Count)];
		string encrypt = "";
		string alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY";
		CMTools cm = new CMTools();
		Debug.LogFormat("{0} [Vigenere Cipher] Keyword: {1}", log, keyword);
		Debug.LogFormat("{0} [Vigenere Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
		if (invert)
		{
			for (int i = 0; i < word.Length; i++)
				encrypt = encrypt + "" + alpha[cm.mod(alpha.IndexOf(word[i]) - alpha.IndexOf(keyword[i % keyword.Length]), 26)];
		}
		else
		{
			for (int i = 0; i < word.Length; i++)
				encrypt = encrypt + "" + alpha[cm.mod(alpha.IndexOf(word[i]) + alpha.IndexOf(keyword[i % keyword.Length]), 26)];
		}
		Debug.LogFormat("{0} [Vigenere Cipher] {1} + {2} -> {3}", log, word, keyword, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(keyword, new int[] { 35, 35, 35, 35, 32, 28 }[keyword.Length - 3]);
		for(int i = 1; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens, invert) });
	}
	
}
