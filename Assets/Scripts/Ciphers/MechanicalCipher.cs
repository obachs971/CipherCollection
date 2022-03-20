using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class MechanicalCipher
{
	private string[] table = new string[]
		{
			"UFHKQIPLXNZESGBVMCWJRDOTYA",
			"IWCZYMLKJODGFSQRNBTXHUEVAP",
			"WBSMEJTUCPFAHZOQLIKNYVGXRD",
			"GRINQVWOTYAJXBMHCFKLDUSZEP",
			"DLTVSUIKWCXRFJZANYHMQOGEPB",
			"FSVCEIUJKPGNTYHBLRQOXMADWZ",
			"JOCYWFPADKHIUVTSMENGQLZBRX",
			"BPHORAKNUETDZYQIMSFJGVWCXL",
			"ANDSQWTGXKFPCOVBLMYEZHRJIU",
			"AQJPBUSGWNXZVDYLETCOFHRIMK",
			"BHFTDGERXJAMUNZVYKOSPILCWQ",
			"JHUKDMSNEBICZYWLXQFPORTAVG",
			"ASNTZDBGWYILEORCQFXJPKHMVU",
			"RPCQABVLGWFENIKYMDUTSJXOZH",
			"YIXNVWQSUHFOMZDGKJPCTBELAR",
			"IMPCZLEGJARNTWSYFQDOUBKHVX",
			"JGKOXMUBAVRTFYCNPWQZESILHD",
			"SVHDBZNMKWJIEUYFXRQPLGCATO",
			"TZXGOPNBWAIYRHQLVKJSCDUEFM",
			"DJQZYWTPKIXCVABFNUEOLHSGRM",
			"CJOEDYHBNIXZRTPWGALFKUSMVQ",
			"FEHLYOBGRXQKVZUIMJTNACDPSW",
			"MOGAPTHIZXRFKLYSVDBWUQNECJ",
			"RXMSBPWOEJADIYNQLGKCTUHZFV",
			"ZJVWFBEOTKRDHSCPIGQNAYLUXM",
			"VWFXUEKRLBQTMCHSGJOZYDAPIN"
		};
	public ResultInfo encrypt(string word, string id, string log, bool invert)
	{
		Debug.LogFormat("{0} Begin Mechanical Cipher", log);
		List<List<string>> words = new Data().allWords;
		words.Insert(0, new Data().word3);
		int length = UnityEngine.Random.Range(0, word.Length - 2);
		string kw = words[length][UnityEngine.Random.Range(0, words[length].Count())];
		string encrypt = "";
		string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		if (invert)
		{
			for (int i = 0; i < word.Length; i++)
				encrypt = encrypt + "" + alpha[table[alpha.IndexOf(kw[i % kw.Length])].IndexOf(word[i])];
		}
		else
		{
			for (int i = 0; i < word.Length; i++)
				encrypt = encrypt + "" + table[alpha.IndexOf(kw[i % kw.Length])][alpha.IndexOf(word[i])];
		}
		Debug.LogFormat("{0} [Mechanical Cipher] Keyword: {1}", log, kw);
		Debug.LogFormat("{0} [Mechanical Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
		Debug.LogFormat("{0} [Mechanical Cipher] {1} -> {2}", log, word, encrypt);

		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 35, 32, 28 }[kw.Length - 3]);
		screens[8] = new ScreenInfo(id, 35);
		for (int i = 1; i < 8; i++)
			screens[i] = new ScreenInfo();
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens, invert) }
		};
	}

}
