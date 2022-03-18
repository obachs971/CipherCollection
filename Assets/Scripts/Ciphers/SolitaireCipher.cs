using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class SolitaireCipher
{
	public PageInfo[] encrypt(string word, string id, string log, bool invert)
	{
		Debug.LogFormat("{0} Begin Solitaire Cipher", log);
		string key = new string("12ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Shuffle());
		string[] display = { key.Substring(0, 7), key.Substring(7, 7), key.Substring(14, 7), key.Substring(21) };
		string letters = new string("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Shuffle()).Substring(0, 2);
		string encrypt = "";
		Debug.LogFormat("{0} [Solitaire Cipher] Key: {1}", log, key);
		Debug.LogFormat("{0} [Solitaire Cipher] Letters: {1}", log, letters);
		Debug.LogFormat("{0} [Solitaire Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
		for(int i = 0; i < word.Length; i++)
		{
			Debug.LogFormat("{0} [Solitaire Cipher] {1}", log, key);
			//Shift 1 and 2
			key = Shift("1", key, 1);
			key = Shift("2", key, 2);
			Debug.LogFormat("{0} [Solitaire Cipher] {1}", log, key);
			//Triple Cut
			string left = "", right = "";
			while (!(key[0] == '1' || key[0] == '2'))
			{
				left = left + key[0];
				key = key.Substring(1);
			}
			while (!(key[key.Length - 1] == '1' || key[key.Length - 1] == '2'))
			{
				right = key[key.Length - 1] + right;
				key = key.Substring(0, key.Length - 1);
			}
			key = right.ToUpperInvariant() + key + left.ToUpperInvariant();
			Debug.LogFormat("{0} [Solitaire Cipher] {1}", log, key);
			//Count Cut
			int cur = getNumber(key[key.Length - 1], letters);
			left = key.Substring(0, cur);
			key = key.Substring(cur);
			key = key.Substring(0, key.Length - 1) + left + key[key.Length - 1];
			Debug.LogFormat("{0} [Solitaire Cipher] {1}", log, key);
			//Find Output Value
			cur = getNumber(key[0], letters);
			if(invert)
			{
				cur = getNumber(word[i], letters) - getNumber(key[cur], letters);
				while (cur < 1)
					cur += 26;
			}
			else
			{
				cur = getNumber(word[i], letters) + getNumber(key[cur], letters);
				while (cur > 26)
					cur -= 26;
			}
			encrypt = encrypt + "" + "-ABCDEFGHIJKLMNOPQRSTUVWXYZ"[cur];
			Debug.LogFormat("{0} [Solitaire Cipher] {1} -> {2}", log, word[i], encrypt[i]);
		}
		Debug.LogFormat("{0} [Solitaire Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		for(int i = 0; i < 8; i += 2)
		{
			screens[i] = new ScreenInfo(display[i / 2], 32);
			screens[i + 1] = new ScreenInfo();
		}
		screens[1] = new ScreenInfo(letters, 25);
		screens[8] = new ScreenInfo(id, 35);
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens, invert) });
	}
	private int getNumber(char c, string lets)
	{
		switch(c)
		{
			case '1': return "-ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(lets[0]);
			case '2': return "-ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(lets[1]);
			default: return "-ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c);
		}
	}
	string Shift(string j, string deck, int shift)
	{
		int cur = deck.IndexOf(j);
		deck = deck.Replace(j, "");
		if ((cur + shift) == 27)
			deck = deck + "" + j;
		else
			deck = deck.Substring(0, (cur + shift) % 28) + "" + j + "" + deck.Substring((cur + shift) % 28);
		return deck;
	}
}
