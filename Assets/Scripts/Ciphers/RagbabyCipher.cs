using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class RagbabyCipher
{
	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Ragbaby Cipher", log);
		Data data = new Data();
		CMTools cm = new CMTools(Bomb);
		string[] keyFront = cm.generateBoolExp();
		string[] invert = cm.generateBoolExp();
		int length = UnityEngine.Random.Range(0, 5);
		string kw = data.allWords[length][UnityEngine.Random.Range(0, data.allWords[length].Count())];
		string key = cm.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", keyFront[1][0] == 'T');
		string encrypt = "";
		if(invert[1][0] == 'T')
		{
			for(int i = 0; i < word.Length; i++)
				encrypt = encrypt + "" + key[cm.mod(key.IndexOf(word[i]) - (i + 1), 26)];
		}
		else
		{
			for (int i = 0; i < word.Length; i++)
				encrypt = encrypt + "" + key[cm.mod(key.IndexOf(word[i]) + (i + 1), 26)];
		}
		Debug.LogFormat("{0} [Ragbaby Cipher] Keyword: {1}", log, kw);
		Debug.LogFormat("{0} [Ragbaby Cipher] Keyword Front Rule: {1} -> {2}", log, keyFront[0], keyFront[1]);
		Debug.LogFormat("{0} [Ragbaby Cipher] Key: {1}", log, key);
		Debug.LogFormat("{0} [Ragbaby Cipher] Invert Rule: {1} -> {2}", log, invert[0], invert[1]);
		Debug.LogFormat("{0} [Ragbaby Cipher] {1} -> {2}", log, word, encrypt);

		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
		screens[1] = new ScreenInfo(keyFront[0], 25);
		screens[2] = new ScreenInfo(invert[0], 35);
		screens[8] = new ScreenInfo(id, 35);
		for (int i = 3; i < 8; i++)
			screens[i] = new ScreenInfo();
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens) });
	}
}
