using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class CaesarCipher
{
	public ResultInfo encrypt(string word, string id, string log, KMBombInfo Bomb, bool invert)
	{
		Debug.LogFormat("{0} Begin Caesar Cipher", log);
		CMTools cm = new CMTools();
		int[] val = cm.generateValue(Bomb);
		int offset = (val[1] % 25) + 1;
		string encrypt = "", alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		if(invert)
		{
			foreach(char c in word)
				encrypt = encrypt + "" + alpha[cm.mod(alpha.IndexOf(c) + offset, 26)];
		}
		else
		{
			foreach (char c in word)
				encrypt = encrypt + "" + alpha[cm.mod(alpha.IndexOf(c) - offset, 26)];
		}
		Debug.LogFormat("{0} [Caesar Cipher] Generated Value: {1} -> {2}", log, (char)val[0], val[1]);
		Debug.LogFormat("{0} [Caesar Cipher] Offset: {1}", log, offset);
		Debug.LogFormat("{0} [Caesar Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
		Debug.LogFormat("{0} [Caesar Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(((char)(val[0])) + "", 35);
		for (int i = 1; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens, invert) }
		};
	}
}