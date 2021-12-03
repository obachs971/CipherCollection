using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class VigenereCipher 
{
	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Vigenere Cipher", log);
		Data data = new Data();
		int kwLength = UnityEngine.Random.Range(0, word.Length - 3);
		string keyword = data.allWords[kwLength][UnityEngine.Random.Range(0, data.allWords[kwLength].Count())];
		string encrypt = "";
		string alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY";
		CMTools cm = new CMTools(Bomb);
		string[] invert = cm.generateBoolExp();
		Debug.LogFormat("{0} [Vigenere Cipher] Keyword: {1}", log, keyword);
		Debug.LogFormat("{0} [Vigenere Cipher] Bool Expression: {1} => {2}", log, invert[0], invert[1]);
		if (invert[1][0] == 'T')
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
		screens[0] = new ScreenInfo(keyword, new int[] { 35, 35, 30, 25, 25 }[keyword.Length - 4]);
		screens[1] = new ScreenInfo(invert[0], new int[] {25, 20}[invert[0].Length - 2]);
		for(int i = 2; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		PageInfo[] pageInfo = new PageInfo[2];
		pageInfo[0] = new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) });
		pageInfo[1] = new PageInfo(screens);
		return pageInfo;
	}
	
}
