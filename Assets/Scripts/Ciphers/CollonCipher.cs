using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class CollonCipher 
{

	public ResultInfo encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Collon Cipher", log);
        string kw = new Data().PickWord(4, 8);
		string replaceJ = "";
		string alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
		Debug.LogFormat("{0} [Collon Cipher] Before Replacing Js: {1}", log, word);
		for (int i = 0; i < word.Length; i++)
		{
			if (word[i] == 'J')
			{
				word = word.Substring(0, i) + "" + alpha[Random.Range(0, alpha.Length)] + "" + word.Substring(i + 1);
				replaceJ = replaceJ + "" + word[i];
			}
			else
				replaceJ = replaceJ + "" + alpha.Replace(word[i].ToString(), "")[Random.Range(0, 24)];
		}
		Debug.LogFormat("{0} [Collon Cipher] After Replacing Js: {1}", log, word);
		Debug.LogFormat("{0} [Collon Cipher] Screen 3: {1}", log, replaceJ);
		string[] keyFront = CMTools.generateBoolExp(Bomb);
		string key = CMTools.getKey(kw.Replace("J", "I"), alpha.ToString(), keyFront[1][0] == 'T');
		Debug.LogFormat("{0} [Collon Cipher] Keyword: {1}", log, kw);
		Debug.LogFormat("{0} [Collon Cipher] Keyword Front Rule: {1} -> {2}", log, keyFront[0], keyFront[1]);
		Debug.LogFormat("{0} [Collon Cipher] Key: {1}", log, key);
		string[] rc = { "", "" };
		for(int i = 0; i < word.Length; i++)
		{
			int row = key.IndexOf(word[i]) / 5;
			int col = key.IndexOf(word[i]) % 5;
			rc[0] = rc[0] + "" + key[(row * 5) + ((col + Random.Range(0, 4) + 1) % 5)];
			rc[1] = rc[1] + "" + key[(((row + Random.Range(0, 4) + 1) % 5) * 5) + col];
			Debug.LogFormat("{0} [Collon Cipher] {1} -> {2}{3}", log, word[i], rc[0][i], rc[1][i]);
		}
		Debug.LogFormat("{0} [Collon Cipher] {1} -> {2}", log, word, rc[0]);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
		screens[1] = new ScreenInfo(keyFront[0], 25);
		screens[2] = new ScreenInfo(rc[1], new int[] { 35, 35, 35, 32, 28 }[word.Length - 4]);
		screens[4] = new ScreenInfo(replaceJ, new int[] { 35, 35, 35, 32, 28 }[replaceJ.Length - 4]);
		screens[8] = new ScreenInfo(id, 35);
		return new ResultInfo
		{
			Encrypted = rc[0],
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens) }
		};
	}
}
