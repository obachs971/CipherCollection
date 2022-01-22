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
	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Mechanical Cipher", log);
		Data data = new Data();
		CMTools cm = new CMTools();
		string[] invert = cm.generateBoolExp(Bomb);
		int length = UnityEngine.Random.Range(0, word.Length - 3);
		string kw = data.allWords[length][UnityEngine.Random.Range(0, data.allWords[length].Count())];
		string encrypt = "";
		string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		if (invert[1][0] == 'T')
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
		Debug.LogFormat("{0} [Mechanical Cipher] Invert Rule: {1} -> {2}", log, invert[0], invert[1]);
		Debug.LogFormat("{0} [Mechanical Cipher] {1} -> {2}", log, word, encrypt);

		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
		screens[1] = new ScreenInfo(invert[0], 25);
		screens[8] = new ScreenInfo(id, 35);
		for (int i = 2; i < 8; i++)
			screens[i] = new ScreenInfo();
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens) });
	}

}
