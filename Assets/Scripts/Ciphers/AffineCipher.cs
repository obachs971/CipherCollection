using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffineCipher 
{

	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb, bool invert)
	{
		Debug.LogFormat("{0} Begin Affine Cipher", log);
		int[][] choices =
		{
			new int[]{ 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25 },
			new int[]{ 9, 21, 15, 3, 19, 7, 23, 11, 5, 17, 25 },
		};
		int e = UnityEngine.Random.Range(0, choices[0].Length);
		CMTools cm = new CMTools();
		int[] xVal = cm.generateValue(Bomb);
		int x = (xVal[1] % 25) + 1;
		string encrypt = "";
		string alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY";
		if (invert)
		{
			foreach (char c in word)
				encrypt = encrypt + "" + alpha[cm.mod((alpha.IndexOf(c) - x) * choices[1][e], 26)];
		}
		else
		{
			foreach (char c in word)
				encrypt = encrypt + "" + alpha[cm.mod((alpha.IndexOf(c) * choices[0][e]) + x, 26)];
		}
		Debug.LogFormat("{0} [Affine Cipher] E: {1}", log, choices[0][e]);
		Debug.LogFormat("{0} [Affine Cipher] D: {1}", log, choices[1][e]);
		Debug.LogFormat("{0} [Affine Cipher] Value Generated: {1} -> {2}", log, (char)xVal[0], xVal[1]);
		Debug.LogFormat("{0} [Affine Cipher] X: {1}", log, x);
		Debug.LogFormat("{0} [Affine Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
		Debug.LogFormat("{0} [Affine Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(choices[0][e] + "", 30);
		screens[1] = new ScreenInfo(((char)xVal[0]) + "", 25);
		for (int i = 2; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens, invert) });
	}
}
