using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class PortaCipher
{
	public PageInfo[] encrypt(string word, string id, string log)
	{
		Debug.LogFormat("{0} Begin Porta Cipher", log);
		List<string> words = new Data().allWords[UnityEngine.Random.Range(0, word.Length - 3)];
		string kw = words[UnityEngine.Random.Range(0, words.Count)];
		Debug.LogFormat("{0} [Porta Cipher] Keyword: {1}", log, kw);
		string encrypt = "", alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		for(int i = 0; i < word.Length; i++)
		{
			int index = alpha.IndexOf(kw[i % kw.Length]) / 2;
			string temp = alpha.Substring(0, 13) + alpha.Substring(13 + index) + alpha.Substring(13, index);
			encrypt = encrypt + "" + temp[(temp.IndexOf(word[i]) + 13) % 26];
		}
		Debug.LogFormat("{0} [Porta Cipher] {1} -> {2}", log, word, encrypt);

		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
		for (int i = 1; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens) });
	}
}