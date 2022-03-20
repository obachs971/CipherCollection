using CipherMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class GROMARKCipher
{
	public ResultInfo encrypt(string word, string id, string log, KMBombInfo Bomb, bool invert)
	{
		Debug.LogFormat("{0} Begin GROMARK Cipher", log);
		int len = UnityEngine.Random.Range(0, 5);
		List<string> words = new Data().allWords[len];
		CMTools cm = new CMTools();
		string kw = words[UnityEngine.Random.Range(0, words.Count)], alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", encrypt = "";
		string[] kwfront = cm.generateBoolExp(Bomb);
		int[] key = new int[kw.Length];
		char[] order = kw.ToArray();
		Array.Sort(order);
		for (int i = 0; i < order.Length; i++)
		{
			for (int j = 0; j < kw.Length; j++)
			{
				if (order[i] == kw[j] && key[j] == 0)
				{
					key[j] = i + 1;
					break;
				}
			}
		}
		string temp = cm.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront[1][0] == 'T');
		while (temp.Length % kw.Length > 0)
			temp += "-";
		string alphakey = "";
		for (int i = 1; i <= kw.Length; i++)
		{
			int cur = Array.IndexOf(key, i);
			for (int j = 0; j < (temp.Length / kw.Length); j++)
				alphakey = alphakey + "" + temp[(j * kw.Length) + cur];
		}
		alphakey = alphakey.Replace("-", "");
		string numkey = new string("123456789".ToCharArray().Shuffle()).Substring(0, 2 + UnityEngine.Random.Range(0, word.Length - 2));
		bool repeat = check(numkey);
		while(repeat)
		{
			numkey = new string("123456789".ToCharArray().Shuffle()).Substring(0, 2 + UnityEngine.Random.Range(0, word.Length - 2));
			repeat = check(numkey);
		}
		len = numkey.Length;
		Debug.LogFormat("{0} [GROMARK Cipher] Keyword: {1}", log, kw);
		Debug.LogFormat("{0} [GROMARK Cipher] Columns: {1}", log, string.Join("", key.Select(x => x + "").ToArray()));
		Debug.LogFormat("{0} [GROMARK Cipher] Alphabet Key: {1} -> {2} -> {3}", log, kwfront[0], kwfront[1], alphakey);
		Debug.LogFormat("{0} [GROMARK Cipher] Number Key: {1}", log, numkey);
		Debug.LogFormat("{0} [GROMARK Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
		if (invert)
		{
			for (int i = 0; i < word.Length; i++)
			{
				encrypt = encrypt + "" + alpha[cm.mod(alphakey.IndexOf(word[i]) - (numkey[i] - '0'), 26)];
				int n = ((numkey[i] - '0') + (numkey[i + 1] - '0')) % 10;
				numkey = numkey + "" + n;
			}
		}
		else
		{
			for(int i = 0; i < word.Length; i++)
			{
				encrypt = encrypt + "" + alphakey[cm.mod(alpha.IndexOf(word[i]) + (numkey[i] - '0'), 26)];
				int n = ((numkey[i] - '0') + (numkey[i + 1] - '0')) % 10;
				numkey = numkey + "" + n;
			}
		}
		Debug.LogFormat("{0} [GROMARK Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[key.Length - 4]);
		screens[1] = new ScreenInfo(kwfront[0], 25);
		screens[2] = new ScreenInfo(numkey.Substring(0, len), new int[] { 35, 35, 35, 35, 35, 32 }[len - 2]);
		for (int i = 3; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens, invert) }
		};
	}
	private bool check(string s)
	{
		for(int i = 0; i < s.Length; i++)
		{
			if ("13579".IndexOf(s[i]) >= 0)
				return false;
		}
		return true;
	}
}