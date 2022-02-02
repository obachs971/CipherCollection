using CipherMachine;
using UnityEngine;

public class GrilleTransposition
{
	public PageInfo[] encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Grille Transposition", log);
		int number = UnityEngine.Random.Range(0, word.Length);
		string[] invert = new CMTools().generateBoolExp(Bomb);
		string encrypt = "";
		Debug.LogFormat("{0} [Grille Transposition] Key Number: {1}", log, number);
		Debug.LogFormat("{0} [Grille Transposition] Invert Rule: {1} -> {2}", log, invert[0], invert[1]);
		char[] temp = new char[word.Length];
		int n1 = number, n2 = (number + (word.Length / 2)) % word.Length;
		if (invert[1][0] == 'T')
		{
			for (int i = 0; i < (word.Length / 2); i++)
			{
				temp[(n1 + i) % temp.Length] = word[i * 2];
				temp[(n2 + i) % temp.Length] = word[(i * 2) + 1];
			}
			if (word.Length % 2 == 1)
				temp[(number + (temp.Length - 1)) % temp.Length] = word[word.Length - 1];
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
				encrypt = encrypt + "" + temp[(number + (temp.Length - 1)) % temp.Length];
		}
		Debug.LogFormat("{0} [Grille Transposition] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(number + "", 35);
		screens[1] = new ScreenInfo(invert[0], 25);
		for (int i = 2; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens) });
	}
}