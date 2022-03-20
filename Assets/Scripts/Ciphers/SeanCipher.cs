using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class SeanCipher
{
	public ResultInfo encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Sean Cipher", log);
		string[] cw = CMTools.generateBoolExp(Bomb);
		string[] kwfront = CMTools.generateBoolExp(Bomb);
		string kw = new Data().PickWord(4, 8);
		string encrypt = "", key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront[1][0] == 'T');
		Debug.LogFormat("{0} [Sean Cipher] Keyword: {1}", log, kw);
		Debug.LogFormat("{0} [Sean Cipher] Key Front Rule: {1} -> {2}", log, kwfront[0], kwfront[1]);
		Debug.LogFormat("{0} [Sean Cipher] Clockwise Rule: {1} -> {2}", log, cw[0], cw[1]);
		
		if (cw[1][0] == 'T')
		{
			foreach (char c in word)
			{
				encrypt = encrypt + "" + key[(key.IndexOf(c) + 13) % 26];
				Debug.LogFormat("{0} [Sean Cipher] \n{1}\n{2}", log, key.Substring(0, 13), key.Substring(13));
				Debug.LogFormat("{0} [Sean Cipher] {1} -> {2}", log, c, encrypt[encrypt.Length - 1]);
				key = key[13] + key.Substring(0, 12) + key.Substring(14, 12) + key[12];
			}
				
		}
		else
		{
			foreach (char c in word)
			{
				encrypt = encrypt + "" + key[(key.IndexOf(c) + 13) % 26];
				Debug.LogFormat("{0} [Sean Cipher] \n{1}\n{2}", log, key.Substring(0, 13), key.Substring(13));
				Debug.LogFormat("{0} [Sean Cipher] {1} -> {2}", log, c, encrypt[encrypt.Length - 1]);
				key = key.Substring(1, 12) + key[25] + "" + key[0] + key.Substring(13, 12);
			}
		}
		Debug.LogFormat("{0} [Sean Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
		screens[1] = new ScreenInfo(kwfront[0], 25);
		screens[2] = new ScreenInfo(cw[0], 35);
		for (int i = 3; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens) }
		};
	}
}