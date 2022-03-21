using CipherMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class MorbitCipher 
{
	public ResultInfo encrypt(string word, string id, string log)
	{
		Debug.LogFormat("{0} Begin Morbit Cipher", log);
        string keyword = new Data().PickWord(8);
		string encrypt = "";
		string alpha = "-ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		int[] key = { 0, 0, 0, 0, 0, 0, 0, 0 };
		string[] morkey = { "..", ".-", ".x", "-.", "--", "-x", "x.", "x-" };
		char[] order = keyword.ToCharArray();
		Array.Sort(order);
		string temp = keyword + "";
		for(int i = 0; i < 8; i++)
		{
			int index = temp.IndexOf(order[i]);
			key[index] = i + 1;
			temp = temp.Substring(0, index) + "-" + temp.Substring(index + 1);
		}
		temp = "";
		foreach (char c in word)
			temp = temp + letterToMorse(c) + "x";
		temp = temp.Substring(0, temp.Length - (temp.Length % 2));

		string nums = "";
		for(int i = 0; i < temp.Length / 2; i++)
			nums = nums + "" + key[Array.IndexOf(morkey, temp[i * 2] + "" + temp[(i * 2) + 1])];
		for(int i = 0; i < word.Length; i++)
		{
			int n = (nums[i] - '0') + (8 * UnityEngine.Random.Range(0, 3));
			if (n > 26)
				n -= 8;
			encrypt = encrypt + "" + alpha[n];
		}
		Debug.LogFormat("{0} [Morbit Cipher] Keyword: {1}", log, keyword);
		Debug.LogFormat("{0} [Morbit Cipher] Key: {1}{2}{3}{4}{5}{6}{7}{8}", log, key[0], key[1], key[2], key[3], key[4], key[5], key[6], key[7]);
		Debug.LogFormat("{0} [Morbit Cipher] {1} -> {2} -> {3}", log, word, temp, nums);
		Debug.LogFormat("{0} [Morbit Cipher] {1} -> {2}", log, nums.Substring(0, word.Length), encrypt);
		nums = nums.Substring(word.Length);
		Debug.LogFormat("{0} [Morbit Cipher] Leftover digits: {1}", log, nums);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(keyword, new int[] { 35, 35, 35, 32, 28 }[keyword.Length - 4]);
		screens[2] = new ScreenInfo(nums.Substring(0, (nums.Length / 2) + (nums.Length % 2)), 35);
		screens[4] = new ScreenInfo(nums.Substring((nums.Length / 2) + (nums.Length % 2)), 35);
		screens[8] = new ScreenInfo(id, 35);
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens) }
		};
	}
	private string letterToMorse(char c)
	{
		switch(c)
		{
			case 'A': return ".-";
			case 'B': return "-...";
			case 'C': return "-.-.";
			case 'D': return "-..";
			case 'E': return ".";
			case 'F': return "..-.";
			case 'G': return "--.";
			case 'H': return "....";
			case 'I': return "..";
			case 'J': return ".---";
			case 'K': return "-.-";
			case 'L': return ".-..";
			case 'M': return "--";
			case 'N': return "-.";
			case 'O': return "---";
			case 'P': return ".--.";
			case 'Q': return "--.-";
			case 'R': return ".-.";
			case 'S': return "...";
			case 'T': return "-";
			case 'U': return "..-";
			case 'V': return "...-";
			case 'W': return ".--";
			case 'X': return "-..-";
			case 'Y': return "-.--";
			case 'Z': return "--..";
		}
		return "";
	}
}
