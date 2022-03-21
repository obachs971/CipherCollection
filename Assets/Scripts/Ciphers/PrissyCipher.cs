using CipherMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class PrissyCipher
{
	public ResultInfo encrypt(string word, string id, string log, KMBombInfo Bomb, bool invert)
	{
		Debug.LogFormat("{0} Begin Prissy Cipher", log);
		string kw = new Data().PickWord(4, 8);
		string encrypt = "";
		string[] kwfront = CMTools.generateBoolExp(Bomb);
		int[] value = CMTools.generateValue(Bomb);
		string key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront[1][0] == 'T');
		int offset = value[1] % 13;
		Debug.LogFormat("{0} [Prissy Cipher] Keyword: {1}", log, kw);
		Debug.LogFormat("{0} [Prissy Cipher] Key: {1} -> {2} -> {3}", log, kwfront[0], kwfront[1], key);
		Debug.LogFormat("{0} [Prissy Cipher] Offset: {1} -> {2} -> {3}", log, (char)value[0], value[1], offset);
		Debug.LogFormat("{0} [Prissy Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
		if (invert)
		{
			for (int i = 0; i < word.Length; i++)
			{
				int index = key.IndexOf(word[i]);
				encrypt = encrypt + "" + key[CMTools.mod((index % 13) - offset, 13) + ((((index / 13) + 1) % 2) * 13)];
				offset = (offset + "-ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(encrypt[i])) % 13;
				Debug.LogFormat("{0} [Prissy Cipher] {1} -> {2}", log, word[i], encrypt[i]);
				Debug.LogFormat("{0} [Prissy Cipher] New Offset: {1}", log, offset, offset);
			}
		}
		else
		{
			for (int i = 0; i < word.Length; i++)
			{
				int index = key.IndexOf(word[i]);
				encrypt = encrypt + "" + key[CMTools.mod((index % 13) + offset, 13) + ((((index / 13) + 1) % 2) * 13)];
				offset = (offset + "-ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(word[i])) % 13;
				Debug.LogFormat("{0} [Prissy Cipher] {1} -> {2}", log, word[i], encrypt[i]);
				Debug.LogFormat("{0} [Prissy Cipher] New Offset: {1}", log, offset, offset);
			}
		}
		Debug.LogFormat("{0} [Prissy Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
		screens[1] = new ScreenInfo(kwfront[0], 25);
		screens[2] = new ScreenInfo(((char)value[0]) + "", 35);
		screens[8] = new ScreenInfo(id, 35);
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens, invert) }
		};
	}
}