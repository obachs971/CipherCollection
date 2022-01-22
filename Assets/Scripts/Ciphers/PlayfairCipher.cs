using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class PlayfairCipher
{
	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Playfair Cipher", log);
		Data data = new Data();
		int length = UnityEngine.Random.Range(0, 5);
		string kw = data.allWords[length][UnityEngine.Random.Range(0, data.allWords[length].Count)];
		string encrypt = "";
		string replaceJ = "";
		string alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
		Debug.LogFormat("{0} [Playfair Cipher] Before Replacing Js: {1}", log, word);
		
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
		Debug.LogFormat("{0} [Playfair Cipher] After Replacing Js: {1}", log, word);
		Debug.LogFormat("{0} [Playfair Cipher] Screen 2: {1}", log, replaceJ);
		CMTools cm = new CMTools();
		string[] invert = cm.generateBoolExp(Bomb);
		string[] keyFront = cm.generateBoolExp(Bomb);
		string key = cm.getKey(kw.Replace("J", "I"), alpha.ToString(), keyFront[1][0] == 'T');	
		Debug.LogFormat("{0} [Playfair Cipher] Keyword: {1}", log, kw);
		Debug.LogFormat("{0} [Playfair Cipher] Keyword Front Rule: {1} -> {2}", log, keyFront[0], keyFront[1]);
		Debug.LogFormat("{0} [Playfair Cipher] Key: {1}", log, key);
		Debug.LogFormat("{0} [Playfair Cipher] Invert Rule: {1} -> {2}", log, invert[0], invert[1]);
		for (int i = 0; i < word.Length / 2; i++)
		{
			int r1 = key.IndexOf(word[i * 2]) / 5;
			int c1 = key.IndexOf(word[i * 2]) % 5;
			int r2 = key.IndexOf(word[(i * 2) + 1]) / 5;
			int c2 = key.IndexOf(word[(i * 2) + 1]) % 5;
			if(r1 == r2 && c1 == c2)
			{
				r1 = 4 - r1;
				c1 = 4 - c1;
				r2 = 4 - r2;
				c2 = 4 - c2;
			}
			else if(r1 == r2)
			{
				if (invert[1][0] == 'T')
				{
					c1 = cm.mod(c1 - 1, 5);
					c2 = cm.mod(c2 - 1, 5);
				}
				else
				{
					c1 = cm.mod(c1 + 1, 5);
					c2 = cm.mod(c2 + 1, 5);
				}
			}
			else if(c1 == c2)
			{
				if (invert[1][0] == 'T')
				{
					r1 = cm.mod(r1 - 1, 5);
					r2 = cm.mod(r2 - 1, 5);
				}
				else
				{
					r1 = cm.mod(r1 + 1, 5);
					r2 = cm.mod(r2 + 1, 5);
				}
			}
			else
			{
				int temp = c1;
				c1 = c2;
				c2 = temp;
			}
			encrypt = encrypt + "" + key[(r1 * 5) + c1] + "" + key[(r2 * 5) + c2];
			Debug.LogFormat("{0} [Playfair Cipher] {1}{2} -> {3}{4}", log, word[i * 2], word[(i * 2) + 1], encrypt[i * 2], encrypt[(i * 2) + 1]);
		}
		if (word.Length % 2 == 1)
			encrypt = encrypt + "" + word[word.Length - 1];
		Debug.LogFormat("{0} [Playfair Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
		screens[1] = new ScreenInfo(keyFront[0], 25);
		screens[2] = new ScreenInfo(replaceJ, new int[] { 35, 35, 35, 32, 28 }[replaceJ.Length - 4]);
		screens[3] = new ScreenInfo(invert[0], 25);
		screens[8] = new ScreenInfo(id, 35);
		for (int i = 4; i < 8; i++)
			screens[i] = new ScreenInfo();
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens) });
	}
}
