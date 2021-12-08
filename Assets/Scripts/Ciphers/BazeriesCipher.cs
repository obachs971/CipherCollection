using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class BazeriesCipher {

	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Bazeries Cipher", log);
		Data data = new Data();
		int length = UnityEngine.Random.Range(0, 5);
		string kw = data.allWords[length][UnityEngine.Random.Range(0, data.allWords[length].Count)];
		string encrypt = "";
		string replaceJ = "";
		string alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
		Debug.LogFormat("{0} [Bazeries Cipher] Before Replacing Js: {1}", log, word);
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
		Debug.LogFormat("{0} [Bazeries Cipher] After Replacing Js: {1}", log, word);
		Debug.LogFormat("{0} [Bazeries Cipher] Screen 1: {1}", log, replaceJ);
		CMTools cm = new CMTools(Bomb);
		string[] keyFront = cm.generateBoolExp();
		string key = cm.getKey(kw.Replace("J", "I"), alpha.ToString(), keyFront[1][0] == 'T');
		Debug.LogFormat("{0} [Bazeries Cipher] Keyword: {1}", log, kw);
		Debug.LogFormat("{0} [Bazeries Cipher] Keyword Front Rule: {1} -> {2}", log, keyFront[0], keyFront[1]);
		Debug.LogFormat("{0} [Bazeries Cipher] Key: {1}", log, key);
		
	}
}
