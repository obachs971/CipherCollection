using CipherMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class AutokeyCipher
{
	public ResultInfo encrypt(string word, string id, string log, bool invert)
	{
		Debug.LogFormat("{0} Begin Autokey Cipher", log);
		string alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY", encrypt = "";
		string kw = new Data().PickWord(word.Length);
		Debug.LogFormat("{0} [Autokey Cipher] Keyword: {1}", log, kw);
        Debug.LogFormat("{0} [Autokey Cipher] Using {1} Instructions", log, invert ? "Encrypt" : "Decrypt");
		if (invert)
		{
			string key = kw.ToUpperInvariant();
			for (int i = 0; i < word.Length; i++)
			{
				encrypt = encrypt + "" + alpha[CMTools.mod(alpha.IndexOf(word[i]) - alpha.IndexOf(key[i]), 26)];
				key = key + "" + encrypt[i];
				Debug.LogFormat("{0} [Autokey Cipher] {1} - {2} -> {3}", log, word[i], key[i], encrypt[i]);
			}
		}
		else
		{
			string key = kw + word;
			for (int i = 0; i < word.Length; i++)
			{
				encrypt = encrypt + "" + alpha[CMTools.mod(alpha.IndexOf(word[i]) + alpha.IndexOf(key[i]), 26)];
				Debug.LogFormat("{0} [Autokey Cipher] {1} + {2} -> {3}", log, word[i], key[i], encrypt[i]);
			}

		}
		Debug.LogFormat("{0} [Autokey Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 35, 32, 28 }[kw.Length - 3]);
		screens[8] = new ScreenInfo(id, 35);
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens, invert) }
		};
	}
}