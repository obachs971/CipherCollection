using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class PortaxCipher
{
	public ResultInfo encrypt(string word, string id, string log)
	{
		Debug.LogFormat("{0} Begin Portax Cipher", log);
		string key = new string("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Shuffle()).Substring(0, word.Length / 2);
		Debug.LogFormat("{0} [Portax Cipher] Key: {1}", log, key);
		char[] c = new char[word.Length];
		for(int i = 0; i < key.Length; i++)
		{
			string top = "NOPQRSTUVWXYZ", btm = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			int index = btm.IndexOf(key[i]);
			top = "ABCDEFGHIJKLM" + top.Substring(index / 2) + top.Substring(0, index / 2);
			btm = btm.Substring(index - (index % 2)) + btm.Substring(0, index - (index % 2));
			Debug.LogFormat("{0} [Portax Cipher] {1}", log, top);
			Debug.LogFormat("{0} [Portax Cipher] {1}", log, btm);
			int n1 = top.IndexOf(word[i]), n2 = btm.IndexOf(word[i + (word.Length / 2)]);
			if((n1 % 13) == (n2 / 2))
			{
				n1 = (n1 + 13) % 26;
				n2 = (n2 % 2 == 0) ? n2 + 1 : n2 - 1;
			}
			else
			{
				int temp = (n2 / 2) + ((n1 / 13) * 13);
				n2 = ((n1 % 13) * 2) + (n2 % 2);
				n1 = temp;
			}
			c[i] = top[n1]; c[i + (c.Length / 2)] = btm[n2];
			Debug.LogFormat("{0} [Portax Cipher] {1}{2} -> {3}{4}", log, word[i], word[i + (word.Length / 2)], c[i], c[i + (c.Length / 2)]);
		}
		if (word.Length % 2 == 1)
			c[c.Length - 1] = word[word.Length - 1];
		string encrypt = new string(c);
		Debug.LogFormat("{0} [Portax Cipher] {1} - > {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(key, 35);
		for (int i = 1; i < 8; i++)
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