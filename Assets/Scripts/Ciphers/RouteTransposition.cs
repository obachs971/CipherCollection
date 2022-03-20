using CipherMachine;
using System.Collections.Generic;
using UnityEngine;

public class RouteTransposition
{
	public ResultInfo encrypt(string word, string id, string log, bool invert)
	{
		Debug.LogFormat("{0} Begin Route Transposition", log);
		int number = Random.Range(0, word.Length) + 1;
		string encrypt;
		Debug.LogFormat("{0} [Route Transposition] Key Number: {1}", log, number);
		Debug.LogFormat("{0} [Route Transposition] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
		char[] temp = new char[word.Length];
		if (invert)
		{
			encrypt = word.Substring(word.Length - (number - 1)) + word.Substring(0, word.Length - (number - 1));
			temp[0] = encrypt[0];
			for (int i = 0; i < (encrypt.Length - 1) / 2; i++)
			{
				temp[(i * 2) + 1] = encrypt[encrypt.Length - (i + 1)];
				temp[(i * 2) + 2] = encrypt[i + 1];
			}
			if (encrypt.Length % 2 == 0)
				temp[encrypt.Length - 1] = encrypt[encrypt.Length / 2];
			encrypt = new string(temp);
		}
		else
		{
			temp[0] = word[0];
			for(int i = 0; i < (word.Length - 1) / 2; i++)
			{
				temp[word.Length - (i + 1)] = word[(i * 2) + 1];
				temp[i + 1] = word[(i * 2) + 2];
			}
			if (word.Length % 2 == 0)
				temp[word.Length / 2] = word[word.Length - 1];
			encrypt = new string(temp);
			encrypt = encrypt.Substring(encrypt.Length - (number - 1)) + encrypt.Substring(0, encrypt.Length - (number - 1));
		}
		Debug.LogFormat("{0} [Route Transposition] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(number + "", 35);
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