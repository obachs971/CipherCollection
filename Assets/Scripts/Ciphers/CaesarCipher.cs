using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class CaesarCipher
{
	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Caesar Cipher", log);
		CMTools cm = new CMTools();
		int[] val = cm.generateValue(Bomb);
		int offset = (val[1] % 25) + 1;
		string[] invert = cm.generateBoolExp(Bomb);
		string encrypt = "", alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		if(invert[1][0] == 'T')
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
		Debug.LogFormat("{0} [Caesar Cipher] Invert Rule: {1} -> {2}", log, invert[0], invert[1]);
		Debug.LogFormat("{0} [Caesar Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(((char)(val[0])) + "", 35);
		screens[1] = new ScreenInfo(invert[0], 25);
		for (int i = 2; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens) });
	}
}