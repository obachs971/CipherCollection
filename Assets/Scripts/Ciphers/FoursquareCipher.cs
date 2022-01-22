using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class FoursquareCipher {

	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Being Trisquare Cipher", log);
		Data data = new Data();
		List<int> toReplace = new List<int>();
		string encrypt = "";
		string replaceJ = "";
		string alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
		Debug.LogFormat("{0} [Foursquare Cipher] Before Replacing Js: {1}", log, word);
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
		Debug.LogFormat("{0} [Foursquare Cipher] After Replacing Js: {1}", log, word);
		Debug.LogFormat("{0} [Foursquare Cipher] Screen 1 Page 2: {1}", log, replaceJ);
		CMTools cm = new CMTools();
		string[] kws = new string[4];
		string[] keys = new string[4];
		string[][] kwFronts = new string[4][];
		for (int i = 0; i < 4; i++)
		{
			int length = UnityEngine.Random.Range(0, 5);
			kws[i] = data.allWords[length][UnityEngine.Random.Range(0, data.allWords[length].Count())];
			data.allWords[length].Remove(kws[i]);
			kwFronts[i] = cm.generateBoolExp(Bomb);
			keys[i] = cm.getKey(kws[i].Replace("J", "I"), "ABCDEFGHIKLMNOPQRSTUVWXYZ", kwFronts[i][1][0] == 'T');
			Debug.LogFormat("{0} [Foursquare Cipher] Keyword #{1}: {2}", log, (i + 1), kws[i]);
			Debug.LogFormat("{0} [Foursquare Cipher] Keyword Front Rule #{1}: {2} -> {3}", log, (i + 1), kwFronts[i][0], kwFronts[i][1]);
			Debug.LogFormat("{0} [Foursquare Cipher] Key #{1}: {2}", log, (i + 1), keys[i]);
		}
		string[] invert = cm.generateBoolExp(Bomb);
		Debug.LogFormat("{0} [Foursquare Cipher] Invert Rule: {1} -> {2}", log, invert[0], invert[1]);
		if(invert[1][0] == 'T')
		{
			for (int i = 0; i < word.Length / 2; i++)
			{
				int r1 = keys[1].IndexOf(word[i * 2]) / 5;
				int c1 = keys[1].IndexOf(word[i * 2]) % 5;
				int r2 = keys[2].IndexOf(word[(i * 2) + 1]) / 5;
				int c2 = keys[2].IndexOf(word[(i * 2) + 1]) % 5;
				encrypt = encrypt + "" + keys[0][(r1 * 5) + c2] + "" + keys[3][(r2 * 5) + c1];
			}
		}
		else
		{
			for (int i = 0; i < word.Length / 2; i++)
			{
				int r1 = keys[0].IndexOf(word[i * 2]) / 5;
				int c1 = keys[0].IndexOf(word[i * 2]) % 5;
				int r2 = keys[3].IndexOf(word[(i * 2) + 1]) / 5;
				int c2 = keys[3].IndexOf(word[(i * 2) + 1]) % 5;
				encrypt = encrypt + "" + keys[1][(r1 * 5) + c2] + "" + keys[2][(r2 * 5) + c1];
			}
		}
		if (word.Length % 2 == 1)
			encrypt = encrypt + "" + word[word.Length - 1];
		Debug.LogFormat("{0} [Foursquare Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[][] screens = new ScreenInfo[2][] { new ScreenInfo[9], new ScreenInfo[9] };
		screens[0][0] = new ScreenInfo(kws[0], new int[] { 35, 35, 35, 32, 28 }[kws[0].Length - 4]);
		screens[0][1] = new ScreenInfo(kwFronts[0][0], 25);
		screens[0][2] = new ScreenInfo(kws[1], new int[] { 35, 35, 35, 32, 28 }[kws[1].Length - 4]);
		screens[0][3] = new ScreenInfo(kwFronts[1][0], 25);
		screens[0][4] = new ScreenInfo(kws[2], new int[] { 35, 35, 35, 32, 28 }[kws[2].Length - 4]);
		screens[0][5] = new ScreenInfo(kwFronts[2][0], 25);
		screens[0][6] = new ScreenInfo(kws[3], new int[] { 35, 35, 35, 32, 28 }[kws[3].Length - 4]);
		screens[0][7] = new ScreenInfo(kwFronts[3][0], 25);
		screens[0][8] = new ScreenInfo(id, 35);
		screens[1][0] = new ScreenInfo(replaceJ, new int[] { 35, 35, 35, 32, 28 }[replaceJ.Length - 4]);
		screens[1][1] = new ScreenInfo(invert[0], 25);
		screens[1][8] = new ScreenInfo(id, 35);
		for (int i = 2; i < 8; i++)
			screens[1][i] = new ScreenInfo();
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens[0]), new PageInfo(screens[1])});
	}
}
