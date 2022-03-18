using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class CondiCipher 
{
	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb, bool invert)
	{
		Debug.LogFormat("{0} Begin Condi Cipher", log);
		Data data = new Data();
		CMTools cm = new CMTools();
		int length = UnityEngine.Random.Range(0, 5);
		string kw = data.allWords[length][UnityEngine.Random.Range(0, data.allWords[length].Count())];
		string encrypt = "";
		string[] keyFront = cm.generateBoolExp(Bomb);
		string key = cm.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", keyFront[1][0] == 'T');
		int[] vals = cm.generateValue(Bomb);
		char letter = (char)vals[0];
		int offset = vals[1];
		Debug.LogFormat("{0} [Condi Cipher] Keyword: {1}", log, kw);
		Debug.LogFormat("{0} [Condi Cipher] Key: {1} -> {2} -> {3}", log, keyFront[0], keyFront[1], key);
		Debug.LogFormat("{0} [Condi Cipher] Starting Offset: {1} -> {2}", log, letter, offset);
		Debug.LogFormat("{0} [Condi Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
		if (invert)
		{
			foreach (char c in word)
			{
				encrypt = encrypt + "" + key[cm.mod(key.IndexOf(c) - offset, 26)];
				offset = key.IndexOf(encrypt[encrypt.Length - 1]) + 1;
			}
		}
		else
		{
			foreach (char c in word)
			{
				encrypt = encrypt + "" + key[cm.mod(key.IndexOf(c) + offset, 26)];
				offset = key.IndexOf(c) + 1;
			}
		}
		Debug.LogFormat("{0} [Condi Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
		screens[1] = new ScreenInfo(keyFront[0], 25);
		screens[2] = new ScreenInfo(letter + "", 35);
		screens[8] = new ScreenInfo(id, 35);
		for (int i = 3; i < 8; i++)
			screens[i] = new ScreenInfo();
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens, invert) });
	}
}
