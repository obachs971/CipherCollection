using CipherMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class HomophonicCipher 
{
	public ResultInfo encrypt(string word, string id, string log)
	{
		Debug.LogFormat("{0} Begin Homophonic Cipher", log);
		string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		string key = string.Concat(alpha[UnityEngine.Random.Range(0, 26)], alpha[UnityEngine.Random.Range(0, 26)], alpha[UnityEngine.Random.Range(0, 26)]);
		string encrypt = "";
		int[][] nums = new int[3][];
		Debug.LogFormat("{0} [Homophonic Cipher] Key: {1}", log, key);
		for (int aa = 0; aa < nums.Length; aa++)
		{
			int index = alpha.IndexOf(key[aa]);
			nums[aa] = new int[26];
			for (int bb = 0; bb < 26; bb++)
				nums[aa][(bb + index) % 26] = bb + 1 + 26 * aa;
			Debug.LogFormat("{0} [Homophonic Cipher] Row {1}: {2}", log, aa + 1, nums[aa][0]);
		}
		List<int> choices = new List<int>();
		List<int> list = new List<int>() { 0, 1, 2 };
		for (int i = 0; i < word.Length; i++)
		{
			choices.Add(list[UnityEngine.Random.Range(0, list.Count())]);
			list.Remove(choices[i]);
			if (list.Count() == 0)
				list = new List<int>() { 0, 1, 2 };
		}
		string tens = "";
		string[] alphas = { "JT", "AKU", "BLV", "CMW", "DNX", "EOY", "FPZ", "GQ", "HR", "IS" };
		for(int i = 0; i < word.Length; i++)
		{
			int index = UnityEngine.Random.Range(0, choices.Count());
			int numEnc = nums[choices[index]][alpha.IndexOf(word[i])];
			tens = tens + "" + (numEnc / 10);
			encrypt = encrypt + "" + alphas[numEnc % 10][UnityEngine.Random.Range(0, alphas[numEnc % 10].Length)];
			choices.RemoveAt(index);
		}
		Debug.LogFormat("{0} [Homophonic Cipher] {1} -> {2}, {3}", log, word, tens, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(tens, new int[] { 35, 35, 35, 32, 28 }[tens.Length - 4]);
		screens[1] = new ScreenInfo(key, 25);
		screens[8] = new ScreenInfo(id, 35);
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens) }
		};
	}
}
