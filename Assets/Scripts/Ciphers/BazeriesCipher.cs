using CipherMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class BazeriesCipher {

	public ResultInfo encrypt(string word, string id, string log, KMBombInfo Bomb, bool invert)
	{
		Debug.LogFormat("{0} Begin Bazeries Cipher", log);
		string encrypt = "";
		string replaceJ = "";
		string alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
		Debug.LogFormat("{0} [Bazeries Cipher] Before Replacing Js: {1}", log, word);
		for (int i = 0; i < word.Length; i++)
		{
			if (word[i] == 'J')
			{
				word = word.Substring(0, i) + "" + alpha[UnityEngine.Random.Range(0, alpha.Length)] + "" + word.Substring(i + 1);
				replaceJ = replaceJ + "" + word[i];
			}
			else
				replaceJ = replaceJ + "" + alpha.Replace(word[i].ToString(), "")[UnityEngine.Random.Range(0, 24)];
		}
		Debug.LogFormat("{0} [Bazeries Cipher] After Replacing Js: {1}", log, word);
		Debug.LogFormat("{0} [Bazeries Cipher] Screen 2: {1}", log, replaceJ);
		string kw = "";
		int[] digits = new int[4];
		int sum = 0;
		for (int i = 0; i < 4; i++)
		{
			digits[i] = UnityEngine.Random.Range(0, 10);
			kw = kw + "" + new string[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE" }[digits[i]];
			sum += digits[i];
		}
		sum = (sum % (word.Length - 1)) + 2;
		string temp = "";
		while(temp.Length < word.Length)
		{
			char[] c;
			if(temp.Length + sum > word.Length)
				c = word.Substring(temp.Length).ToCharArray();
			else
				c = word.Substring(temp.Length, sum).ToCharArray();
			Array.Reverse(c);
			temp = temp + "" + new string(c);
		}
		CMTools cm = new CMTools();
		string[] keyFront = cm.generateBoolExp(Bomb);
		string key = cm.getKey(kw, alpha, keyFront[1][0] == 'T');
		alpha = "AFLQVBGMRWCHNSXDIOTYEKPUZ";
		if(invert)
		{
			foreach (char c in temp)
				encrypt = encrypt + "" + alpha[key.IndexOf(c)];
		}
		else
		{
			foreach (char c in temp)
				encrypt = encrypt + "" + key[alpha.IndexOf(c)];
		}
		Debug.LogFormat("{0} [Bazeries Cipher] Digit Key: {1}{2}{3}{4}", log, digits[0], digits[1], digits[2], digits[3]);
		Debug.LogFormat("{0} [Bazeries Cipher] Key: {1} -> {2} -> {3}", log, keyFront[0], keyFront[1], key);
		Debug.LogFormat("{0} [Bazeries Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
		Debug.LogFormat("{0} [Bazeries Cipher] {1} -> {2} -> {3}", log, word, temp, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(digits[0] + "" + digits[1] + "" + digits[2] + "" + digits[3], 35);
		screens[1] = new ScreenInfo(keyFront[0], 25);
		screens[2] = new ScreenInfo(replaceJ, new int[] { 35, 35, 35, 32, 28 }[replaceJ.Length - 4]);
		screens[8] = new ScreenInfo(id, 35);
		for (int i = 3; i < 8; i++)
			screens[i] = new ScreenInfo();
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens, invert) }
		};
	}
}
