using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffineCipher 
{

	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Affine Cipher", log);
		int[][] choices =
		{
			new int[]{ 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25 },
			new int[]{ 9, 21, 15, 3, 19, 7, 23, 11, 5, 17, 25 },
		};
		int a = UnityEngine.Random.Range(0, choices[0].Length);
		CMTools cm = new CMTools(Bomb);
		int[] bVal = cm.generateValue();
		int b = (bVal[1] % 24) + 1;
		string encrypt = "";
		foreach (char c in word)	
			encrypt = encrypt + "" + (char)(cm.mod(((c - 65) * choices[0][a]) + b, 26) + 65);
		Debug.LogFormat("{0} [Affine Cipher] A: {1}", log, choices[0][a]);
		Debug.LogFormat("{0} [Affine Cipher] Value Generated: {1} -> {2}", log, (char)bVal[0], bVal[1]);
		Debug.LogFormat("{0} [Affine Cipher] B: {1}", log, b);
		Debug.LogFormat("{0} [Affine Cipher] {1} -> {2}", log, word, encrypt);
		Debug.LogFormat("{0} [Affine Cipher] Inverted of A: {1}", log, choices[1][a]);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(choices[0][a] + ", " + ((char)bVal[0]), 35);
		for (int i = 1; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		PageInfo[] page = { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens) };
		return page;
	}
}
