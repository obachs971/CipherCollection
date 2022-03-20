using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class EnigmaCipher
{
	public ResultInfo encrypt(string word, string id, string log)
	{
		Debug.LogFormat("{0} Begin Engima Cipher", log);
		string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", encrypt = "";
		string plugboard = generatePlugboard(word.Length);
		string[] rotorNums = generateRotors();
		string rotorLets = alpha[Random.Range(0, 26)] + "" + alpha[Random.Range(0, 26)] + "" + alpha[Random.Range(0, 26)];
		char reflector = alpha[Random.Range(0, 3)];
		string[][] enigma = createEnigmaMachine(plugboard, rotorNums, rotorLets, reflector);
		string[] plugboardDisplay = { plugboard.Substring(0, 1), plugboard.Substring(1, 1) };
		for (int i = 2; i < plugboard.Length; i += 2)
		{
			plugboardDisplay[0] = plugboardDisplay[0] + "" + plugboard.Substring(i, 1);
			plugboardDisplay[1] = plugboardDisplay[1] + "" + plugboard.Substring(i + 1, 1);
		}
		Debug.LogFormat("{0} [Enigma Cipher] Plugboard: {1} {2}", log, plugboardDisplay[0], plugboardDisplay[1]);
		Debug.LogFormat("{0} [Enigma Cipher] Top Rotor: {1}, {2}", log, rotorNums[0], rotorLets[2]);
		Debug.LogFormat("{0} [Enigma Cipher] Middle Rotor: {1}, {2}", log, rotorNums[1], rotorLets[1]);
		Debug.LogFormat("{0} [Enigma Cipher] Bottom Rotor: {1}, {2}", log, rotorNums[2], rotorLets[0]);
		Debug.LogFormat("{0} [Enigma Cipher] Reflector: {1}", log, reflector);
		for (int i = 0; i < word.Length; i++)
		{
			encrypt = encrypt + "" + encryptLetter(word[i], enigma, log);
			enigma = turnRotors(enigma);
		}
		//Setting up Screens
		
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(rotorLets, 35);
		screens[1] = new ScreenInfo(rotorNums[0], rotorNums[0].Length == 4 ? 20 : 25);
		screens[2] = new ScreenInfo(plugboardDisplay[0], new int[] { 35, 35, 35, 32, 28 }[plugboardDisplay[0].Length - 4]);
		screens[3] = new ScreenInfo(rotorNums[1], rotorNums[1].Length == 4 ? 20 : 25);
		screens[4] = new ScreenInfo(plugboardDisplay[1], new int[] { 35, 35, 35, 32, 28 }[plugboardDisplay[1].Length - 4]);
		screens[5] = new ScreenInfo(rotorNums[2], rotorNums[2].Length == 4 ? 20 : 25);
		screens[6] = new ScreenInfo();
		screens[7] = new ScreenInfo(reflector + "", 25);
		screens[8] = new ScreenInfo(id, 35);
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens) }
		};
	}
	//Generates the Plugboard
	private string generatePlugboard(int length)
	{
		string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", plugboard = "";
		for (int i = 0; i < (12 - length); i++)
		{
			plugboard = plugboard + "" + alpha[Random.Range(0, alpha.Length)];
			alpha = alpha.Replace(plugboard[plugboard.Length - 1] + "", "");
			plugboard = plugboard + "" + alpha[Random.Range(0, alpha.Length)];
			alpha = alpha.Replace(plugboard[plugboard.Length - 1] + "", "");
		}
		return plugboard;
	}
	//Generates the Rotors
	private string[] generateRotors()
	{
		List<string> rotorList = new List<string>() { "I", "II", "III", "IV", "V", "VI", "VII", "VIII" };
		string[] rotorNums = new string[3];
		for (int i = 0; i < 3; i++)
		{
			rotorNums[i] = rotorList[Random.Range(0, rotorList.Count)].ToUpperInvariant();
			rotorList.Remove(rotorNums[i]);
		}
		return rotorNums;
	}
	//Creating the Engima Machine
	private string[][] createEnigmaMachine(string plugboard, string[] rotorNums, string rotorLets, char reflector)
	{
		string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		List<string> rotorList = new List<string>() { "I", "II", "III", "IV", "V", "VI", "VII", "VIII" };
		string[][] rotors =
		{
			new string[]{ "EKMFLGDQVZNTOWYHXUSPAIBRCJ", alpha.ToUpperInvariant(), "DQ" },
			new string[]{ "AJDKSIRUXBLHWTMCQGZNPYFVOE", alpha.ToUpperInvariant(), "ER" },
			new string[]{ "BDFHJLCPRTXVZNYEIWGAKMUSQO", alpha.ToUpperInvariant(), "IV" },
			new string[]{ "ESOVPZJAYQUIRHXLNFTGKDCMWB", alpha.ToUpperInvariant(), "JW" },
			new string[]{ "VZBRGITYUPSDNHLXAWMJQOFECK", alpha.ToUpperInvariant(), "MZ" },
			new string[]{ "JPGVOUMFYQBENHZRDKASXLICTW", alpha.ToUpperInvariant(), "LY" },
			new string[]{ "NZJHGRCXMYSWBOUFAIVLPEKQDT", alpha.ToUpperInvariant(), "HU" },
			new string[]{ "FKQHTLXOCBJSPDZRAMEWNIUYGV", alpha.ToUpperInvariant(), "CP" }
		};
		string[] reflectors =
		{
			"LUSNPQOMJIYAHDGEFXCVBTZRKW",
			"XQUMFEPOWLTJDZHGBVYKCRIASN",
			"ESKOAQMJYHCPGTDLFUBNRXZVIW"
		};
		string keyboard = alpha.ToUpperInvariant();
		for (int i = 0; i < plugboard.Length; i += 2)
		{
			keyboard = keyboard.Replace(plugboard[i], '*');
			keyboard = keyboard.Replace(plugboard[i + 1], plugboard[i]);
			keyboard = keyboard.Replace('*', plugboard[i + 1]);
		}
		string[][] enigma =
		{
			new string[]{ keyboard.ToUpperInvariant() },
			rotors[rotorList.IndexOf(rotorNums[0])],
			rotors[rotorList.IndexOf(rotorNums[1])],
			rotors[rotorList.IndexOf(rotorNums[2])],
			new string[]{ alpha.ToUpperInvariant(), reflectors[alpha.IndexOf(reflector)] }
		};
		for (int i = 0; i < rotorLets.Length; i++)
		{
			enigma[3 - i][0] = enigma[3 - i][0].Substring(alpha.IndexOf(rotorLets[i])) + enigma[3 - i][0].Substring(0, alpha.IndexOf(rotorLets[i]));
			enigma[3 - i][1] = enigma[3 - i][1].Substring(alpha.IndexOf(rotorLets[i])) + enigma[3 - i][1].Substring(0, alpha.IndexOf(rotorLets[i]));
		}
		return enigma;
	}
	//Encrypts the Letter
	private char encryptLetter(char let, string[][] enigma, string log)
	{
		string changes = let + "";
		int index = enigma[0][0].IndexOf(let);
		for(int i = 1; i < 4; i++)
		{
			changes = changes + "->" + enigma[i][0][index];
			index = enigma[i][1].IndexOf(enigma[i][0][index]);
		}
		changes = changes + "->" + enigma[4][0][index] + "->" + enigma[4][1][index];
		index = enigma[4][0].IndexOf(enigma[4][1][index]);
		for (int i = 3; i > 0; i--)
		{
			changes = changes + "->" + enigma[i][1][index];
			index = enigma[i][0].IndexOf(enigma[i][1][index]);
		}
		changes = changes + "->" + enigma[0][0][index];
		Debug.LogFormat("{0} [Enigma Cipher] {1}", log, changes);
		return enigma[0][0][index];
	}
	//Turns the Rotors
	private string[][] turnRotors(string[][] enigma)
	{
		if(enigma[2][2].Contains(enigma[2][1][0] + ""))
		{
			enigma[2][0] = enigma[2][0].Substring(1) + "" + enigma[2][0][0];
			enigma[2][1] = enigma[2][1].Substring(1) + "" + enigma[2][1][0];
			enigma[3][0] = enigma[3][0].Substring(1) + "" + enigma[3][0][0];
			enigma[3][1] = enigma[3][1].Substring(1) + "" + enigma[3][1][0];
		}
		else if (enigma[1][2].Contains(enigma[1][1][0] + ""))
		{
			enigma[2][0] = enigma[2][0].Substring(1) + "" + enigma[2][0][0];
			enigma[2][1] = enigma[2][1].Substring(1) + "" + enigma[2][1][0];
		}
		enigma[1][0] = enigma[1][0].Substring(1) + "" + enigma[1][0][0];
		enigma[1][1] = enigma[1][1].Substring(1) + "" + enigma[1][1][0];
		return enigma;
	}
}