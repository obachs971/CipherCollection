using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtbashCipher
{

	public ResultInfo encrypt(string word, string id, string log)
	{
		Debug.LogFormat("{0} Begin Atbash Cipher", log);
		string encrypt = "";
		foreach (char c in word)
			encrypt = encrypt + "" + (char)(155 - c);
		Debug.LogFormat("{0} [Atbash Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[8] = new ScreenInfo(id, 35);
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens) }
		};
	}
}
