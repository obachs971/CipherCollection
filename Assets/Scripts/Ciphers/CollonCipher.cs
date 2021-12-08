using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class CollonCipher 
{

	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Collon Cipher", log);
		Data data = new Data();
		int length = UnityEngine.Random.Range(0, 5);
		string kw = data.allWords[length][UnityEngine.Random.Range(0, data.allWords[length].Count)];
		string encrypt = "";
		string replaceJ = "";
		string alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
		Debug.LogFormat("{0} [Collon Cipher] Before Replacing Js: {1}", log, word);
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
		Debug.LogFormat("{0} [Collon Cipher] After Replacing Js: {1}", log, word);
		Debug.LogFormat("{0} [Collon Cipher] Screen 1: {1}", log, replaceJ);
		CMTools cm = new CMTools(Bomb);
		string[] keyFront = cm.generateBoolExp();
		string key = cm.getKey(kw.Replace("J", "I"), alpha.ToString(), keyFront[1][0] == 'T');
		Debug.LogFormat("{0} [Collon Cipher] Keyword: {1}", log, kw);
		Debug.LogFormat("{0} [Collon Cipher] Keyword Front Rule: {1} -> {2}", log, keyFront[0], keyFront[1]);
		Debug.LogFormat("{0} [Collon Cipher] Key: {1}", log, key);
		for(int i = 0; i < word.Length; i++)
		{
			int row = key.IndexOf(word[i]) / 5;
			int col = key.IndexOf(word[i]) % 5;
			encrypt = encrypt + "" + key[(row * 5) + ((col + UnityEngine.Random.Range(0, 4) + 1) % 5)];
			encrypt = encrypt + "" + key[(((row + UnityEngine.Random.Range(0, 4) + 1) % 5) * 5) + col];
			Debug.LogFormat("{0} [Collon Cipher] {1} -> {2}{3}", log, word[i], encrypt[i * 2], encrypt[(i * 2) + 1]);
		}
		Debug.LogFormat("{0} [Collon Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(replaceJ, new int[] { 35, 35, 35, 32, 28 }[replaceJ.Length - 4]);
		screens[1] = new ScreenInfo();
		screens[2] = new ScreenInfo(encrypt.Substring(word.Length, word.Length), new int[] { 35, 35, 35, 32, 28 }[word.Length - 4]);
		screens[3] = new ScreenInfo();
		screens[4] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
		screens[5] = new ScreenInfo(keyFront[0], 25);
		screens[8] = new ScreenInfo(id, 35);
		for (int i = 6; i < 8; i++)
			screens[i] = new ScreenInfo();
		PageInfo[] pageInfo = new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt.Substring(0, word.Length), 35) }), new PageInfo(screens) };
		return pageInfo;
	}
}
