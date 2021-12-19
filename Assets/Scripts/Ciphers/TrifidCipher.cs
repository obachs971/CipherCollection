using CipherMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class TrifidCipher 
{
	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Trifid Cipher", log);
		List < List<string> > words = new Data().allWords;
		CMTools cm = new CMTools(Bomb);
		string[] invert = cm.generateBoolExp();
		string[] keyFront = cm.generateBoolExp();
		int[][] numbers = new int[3][] { new int[word.Length], new int[word.Length], new int[word.Length] };
		string key;
		string encrypt;
		string kw;
		do
		{
			encrypt = "";
			int length = UnityEngine.Random.Range(0, 5);
			kw = words[length][UnityEngine.Random.Range(0, words[length].Count())];
			words[length].Remove(kw);
			key = cm.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", keyFront[1][0] == 'T');
			key = key + "-";
			if (invert[1][0] == 'T')
			{
				for(int i = 0; i < word.Length; i++)
				{
					int n = key.IndexOf(word[i]);
					numbers[(i * 3) / word.Length][(i * 3) % word.Length] = n / 9;
					numbers[((i * 3) + 1) / word.Length][((i * 3) + 1) % word.Length] = (n % 9) / 3;
					numbers[((i * 3) + 2) / word.Length][((i * 3) + 2) % word.Length] = n % 3;
				}
				for (int i = 0; i < word.Length; i++)
					encrypt = encrypt + "" + key[(numbers[0][i] * 9) + (numbers[1][i] * 3) + (numbers[2][i])];
			}
			else
			{
				for(int i = 0; i < word.Length; i++)
				{
					int n = key.IndexOf(word[i]);
					numbers[0][i] = n / 9;
					numbers[1][i] = (n % 9) / 3;
					numbers[2][i] = n % 3;
				}
				for (int i = 0; i < word.Length; i++)
					encrypt = encrypt + "" + key[(numbers[(i * 3) / word.Length][(i * 3) % word.Length] * 9) + (numbers[((i * 3) + 1) / word.Length][((i * 3) + 1) % word.Length] * 3) + (numbers[((i * 3) + 2) / word.Length][((i * 3) + 2) % word.Length])];
			}
		} while (encrypt.Contains("-"));
		string s = String.Join(",", numbers[0].Select(p => (p + 1).ToString()).ToArray());
		Debug.LogFormat("{0} [Trifid Cipher] Keyword: {1}", log, kw);
		Debug.LogFormat("{0} [Trifid Cipher] Key: {1}", log, key);
		Debug.LogFormat("{0} [Trifid Cipher] Invert Rule: {1} -> {2}", log, invert[0], invert[1]);
		Debug.LogFormat("{0} [Trifid Cipher] {1}", log, String.Join("", numbers[0].Select(p => (p + 1).ToString()).ToArray()));
		Debug.LogFormat("{0} [Trifid Cipher] {1}", log, String.Join("", numbers[1].Select(p => (p + 1).ToString()).ToArray()));
		Debug.LogFormat("{0} [Trifid Cipher] {1}", log, String.Join("", numbers[2].Select(p => (p + 1).ToString()).ToArray()));
		Debug.LogFormat("{0} [Trifid Cipher] {1} -> {2}", log, word, encrypt);
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
