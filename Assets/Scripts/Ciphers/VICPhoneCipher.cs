using CipherMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class VICPhoneCipher
{
	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin VIC Phone Cipher", log);
		List<List<string>> words = new Data().allWords;
		CMTools cm = new CMTools();
		int len = UnityEngine.Random.Range(0, words.Count);
		string kw = words[len][UnityEngine.Random.Range(0, words[len].Count)];
		string[] kwfront = cm.generateBoolExp(Bomb);
		string key = cm.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront[1][0] == 'T');
		string rows = new string("0123456789".ToCharArray().Shuffle()).Substring(0, 4);
		Debug.LogFormat("{0} [VIC Phone Cipher] Keyword: {1}", log, kw);
		Debug.LogFormat("{0} [VIC Phone Cipher] Screen B: {1}", log, rows);
		for(int i = 0; i < 10; i++)
		{
			if(rows.Contains(i + ""))
				key = key.Substring(0, i) + "-" + key.Substring(i);
		}
		Debug.LogFormat("{0} [VIC Phone Cipher] Key: {1} -> {2} -> {3}", log, kwfront[0], kwfront[1], key);
		List<int> encryptNums = new List<int>();
		foreach(char let in word)
		{
			int index = key.IndexOf(let);
			Debug.LogFormat("{0} [VIC Phone Cipher] {1}", log, index);
			if (index < 10) 
				encryptNums.Add(index % 10);
			else
			{
				int n = ((index / 10) * 2) - UnityEngine.Random.Range(1, 3);
				encryptNums.Add("0123456789".IndexOf(rows[n]));
				encryptNums.Add(index % 10);
			}
		}
		Debug.LogFormat("{0} [VIC Phone Cipher] {1} -> {2}", log, word, string.Join("", encryptNums.ConvertAll(i => i.ToString()).ToArray()));
		string numKey = new string("0123456789".ToCharArray().Shuffle()).Substring(0, (encryptNums.Count / 2));
		Debug.LogFormat("{0} [VIC Phone Cipher] Number Key: {1}", log, numKey);
		for(int i = 0; i < encryptNums.Count; i++)
			encryptNums[i] = (encryptNums[i] + (numKey[i % numKey.Length] - '0')) % 10;
		string[] replace = {
			"111", "222", "333", "444", "555", "666",
			"11", "22", "33", "44", "55", "66", "77", "88", "99", "00",
			"1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
		};
		string alpha = "CFILORBEHKNQTVXZADGJMPSUWY", encrypt = string.Join("", encryptNums.ConvertAll(i => i.ToString()).ToArray());
		for (int i = 0; i < replace.Length; i++)
			encrypt = encrypt.Replace(replace[i], alpha[i] + "");
		while (encrypt.Length < word.Length)
			encrypt = enlarge(encrypt);
		Debug.LogFormat("{0} [VIC Phone Cipher] {1} -> {2}", log, string.Join("", encryptNums.ConvertAll(i => i.ToString()).ToArray()), encrypt);
		string extra = encrypt.Substring(word.Length);
		encrypt = encrypt.Substring(0, word.Length);
		Debug.LogFormat("{0} [VIC Phone Cipher] Screen 3: {1}", log, extra);
		Debug.LogFormat("{0} [VIC Phone Cipher] {1} -> {2}", log, word, encrypt);

		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
		screens[1] = new ScreenInfo(kwfront[0], 25);
		screens[2] = new ScreenInfo(numKey, (numKey.Length == 8) ? 28 : (numKey.Length == 7) ? 32 : 35);
		screens[3] = new ScreenInfo(rows, 20);
		screens[4] = new ScreenInfo(extra, extra.Length == 8 ? 28 : extra.Length == 7 ? 32 : 35);
		for (int i = 5; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens) });
	}
	private string enlarge(string encrypt)
	{
		string[] replace = {
			"111", "222", "333", "444", "555", "666",
			"11", "22", "33", "44", "55", "66", "77", "88", "99", "00",
			"1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
		};
		string alpha = "CFILORBEHKNQTVXZ";
		List<int> indexes = new List<int>();
		for(int i = 0; i < encrypt.Length; i++)
		{
			if (alpha.Contains(encrypt[i]))
				indexes.Add(i);
		}
		int index = indexes[UnityEngine.Random.Range(0, indexes.Count)];
		alpha += "ADGJMPSUWY";
		if (alpha.IndexOf(encrypt[index]) < 6)
		{
			char c = replace[alpha.IndexOf(encrypt[index])][0];
			string s = alpha[Array.IndexOf(replace, c + "" + c)] + "" + alpha[Array.IndexOf(replace, c + "")];
			encrypt = encrypt.Substring(0, index) + new string(s.ToCharArray().Shuffle()) + encrypt.Substring(index + 1);
		}
		else
		{
			char c = replace[alpha.IndexOf(encrypt[index])][0];
			string s = alpha[Array.IndexOf(replace, c + "")] + "" + alpha[Array.IndexOf(replace, c + "")];
			encrypt = encrypt.Substring(0, index) + s + encrypt.Substring(index + 1);
		}
		return encrypt;
	}
}