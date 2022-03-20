using CipherMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class ConjugatedMatrixBifidCipher 
{
	public ResultInfo encrypt(string word, string id, string log, KMBombInfo Bomb, bool invert)
	{
		Debug.LogFormat("{0} Conjugated Matrix Bifid Cipher", log);
		Data words = new Data();
		string encrypt = "";
		string replaceJ = "";
		string alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
		Debug.LogFormat("{0} [Conjugated Matrix Bifid Cipher] Before Replacing Js: {1}", log, word);
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
		Debug.LogFormat("{0} [Conjugated Matrix Bifid Cipher] After Replacing Js: {1}", log, word);
		Debug.LogFormat("{0} [Conjugated Matrix Bifid Cipher] Screen 3: {1}", log, replaceJ);
		string[] kws = new string[2];
		string[] keys = new string[2];
		string[][] kwFronts = new string[2][];
		for(int i = 0; i < 2; i++)
		{
			kws[i] = words.PickWord(4, 8);
			kwFronts[i] = CMTools.generateBoolExp(Bomb);
			keys[i] = CMTools.getKey(kws[i].Replace("J", "I"), "ABCDEFGHIKLMNOPQRSTUVWXYZ", kwFronts[i][1][0] == 'T');
			Debug.LogFormat("{0} [Conjugated Matrix Bifid Cipher] Keyword #{1}: {2}", log, (i + 1), kws[i]);
			Debug.LogFormat("{0} [Conjugated Matrix Bifid Cipher] Key #{1}: {2} -> {3} -> {4}", log, (i + 1), kwFronts[i][0], kwFronts[i][1], keys[i]);
		}
		int[][] pos = new int[2][] { new int[word.Length], new int[word.Length] };
		Debug.LogFormat("{0} [Conjugated Matrix Bifid Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
		if (invert)
		{
			for (int aa = 0; aa < word.Length; aa++)
			{
				pos[(aa * 2) / word.Length][(aa * 2) % word.Length] = keys[1].IndexOf(word[aa]) / 5;
				pos[((aa * 2) + 1) / word.Length][((aa * 2) + 1) % word.Length] = keys[1].IndexOf(word[aa]) % 5;
			}
			for (int aa = 0; aa < word.Length; aa++)
				encrypt = encrypt + "" + keys[0][(pos[0][aa] * 5) + pos[1][aa]];
		}
		else
		{
			for (int aa = 0; aa < word.Length; aa++)
			{
				pos[0][aa] = keys[0].IndexOf(word[aa]) / 5;
				pos[1][aa] = keys[0].IndexOf(word[aa]) % 5;
			}
			for (int aa = 0; aa < word.Length; aa++)
				encrypt = encrypt + "" + keys[1][((pos[(aa * 2) / word.Length][(aa * 2) % word.Length]) * 5) + pos[((aa * 2) + 1) / word.Length][((aa * 2) + 1) % word.Length]];
		}
		Debug.LogFormat("{0} [Conjugated Matrix Bifid Cipher] {1}", log, String.Join("", pos[0].Select(p => (p + 1).ToString()).ToArray()));
		Debug.LogFormat("{0} [Conjugated Matrix Bifid Cipher] {1}", log, String.Join("", pos[1].Select(p => (p + 1).ToString()).ToArray()));
		Debug.LogFormat("{0} [Conjugated Matrix Bifid Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kws[0], new int[] { 35, 35, 35, 32, 28 }[kws[0].Length - 4]);
		screens[1] = new ScreenInfo(kwFronts[0][0], 25);
		screens[2] = new ScreenInfo(kws[1], new int[] { 35, 35, 35, 32, 28 }[kws[1].Length - 4]);
		screens[3] = new ScreenInfo(kwFronts[1][0], 25);
		screens[4] = new ScreenInfo(replaceJ, new int[] { 35, 35, 35, 32, 28 }[replaceJ.Length - 4]);
		screens[8] = new ScreenInfo(id, 35);
		for (int aa = 5; aa < screens.Length - 1; aa++)
			screens[aa] = new ScreenInfo();
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens, invert) }
		};
	}
}
