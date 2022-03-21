using CipherMachine;
using UnityEngine;

public class GrilleTransposition
{
	public ResultInfo encrypt(string word, string id, string log, KMBombInfo Bomb, bool invert)
	{
		Debug.LogFormat("{0} Begin Grille Transposition", log);
		int[] value = CMTools.generateValue(Bomb);
		string encrypt = "";
		Debug.LogFormat("{0} [Grille Transposition] Key Number: {1} -> {2}", log, (char)value[0], value[1]);
		Debug.LogFormat("{0} [Grille Transposition] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
		char[] temp = new char[word.Length];
		int n1 = value[1] % word.Length, n2 = (value[1] + (word.Length / 2)) % word.Length;
		if (invert)
		{
			for (int i = 0; i < (word.Length / 2); i++)
			{
				temp[(n1 + i) % temp.Length] = word[i * 2];
				temp[(n2 + i) % temp.Length] = word[(i * 2) + 1];
			}
			if (word.Length % 2 == 1)
				temp[(value[1] + (temp.Length - 1)) % temp.Length] = word[word.Length - 1];
			encrypt = encrypt + "" + temp[0];
			for (int i = 0; i < (word.Length - 1) / 2; i++)
			{
				encrypt = encrypt + "" + temp[temp.Length - (i + 1)];
				encrypt = encrypt + "" + temp[i + 1];
			}
			if (word.Length % 2 == 0)
				encrypt = encrypt + "" + temp[temp.Length / 2];
		}
		else
		{
			temp[0] = word[0];
			for (int i = 0; i < (word.Length - 1) / 2; i++)
			{
				temp[temp.Length - (i + 1)] = word[(i * 2) + 1];
				temp[i + 1] = word[(i * 2) + 2];
			}
			if (word.Length % 2 == 0)
				temp[word.Length / 2] = word[word.Length - 1];
			encrypt = "";
			for(int i = 0; i < (temp.Length / 2); i++)
				encrypt = encrypt + "" + temp[(n1 + i) % temp.Length] + "" + temp[(n2 + i) % temp.Length];
			if (word.Length % 2 == 1)
				encrypt = encrypt + "" + temp[(value[1] + (temp.Length - 1)) % temp.Length];
		}
		Debug.LogFormat("{0} [Grille Transposition] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(((char)value[0]) + "", 35);
		screens[8] = new ScreenInfo(id, 35);
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens, invert) }
		};
	}
}