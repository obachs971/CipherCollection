using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class TridigitalCipher 
{
    public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Tridigital Cipher", log);
		Data data = new Data();
		CMTools cm = new CMTools(Bomb);
		int length = UnityEngine.Random.Range(0, 5);
		string kw = data.allWords[length][UnityEngine.Random.Range(0, data.allWords[length].Count())];
		string encrypt = "";
		string nums = "";
		string[] keyFront = cm.generateBoolExp();
		string key = cm.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", keyFront[1][0] == 'T');
        Debug.LogFormat("{0} [Tridigital Cipher] Keyword: {1}", log, kw);
		Debug.LogFormat("{0} [Tridigital Cipher] Key Front Rule: {1} -> {2}", log, keyFront[0], keyFront[1]);
		Debug.LogFormat("{0} [Tridigital Cipher] Key: {1}", log, key);
		string[] alpha = { "AJS", "BKT", "CLU", "DMV", "ENW", "FOX", "GPY", "HQZ", "IR" };
		foreach(char c in word)
		{
			int index = key.IndexOf(c);
			nums = nums + "" + ((index / 9) + 1);
			encrypt = encrypt + "" + alpha[index % 9][UnityEngine.Random.Range(0, alpha[index % 9].Length)];
			Debug.LogFormat("{0} [Tridigital Cipher] {1} -> {2}{3} -> {4}{5}", log, c, nums[nums.Length - 1], ((index % 9) + 1), nums[nums.Length - 1], encrypt[encrypt.Length - 1]);
		}
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
		screens[1] = new ScreenInfo(keyFront[0], 25);
		screens[2] = new ScreenInfo(nums, new int[] { 35, 35, 35, 32, 28 }[nums.Length - 4]);
		screens[8] = new ScreenInfo(id, 35);
		for (int i = 3; i < 8; i++)
			screens[i] = new ScreenInfo();
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens) });
	}
}
