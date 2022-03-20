using System.Collections.Generic;
using CipherMachine;
using UnityEngine;

public class LogicCipher 
{
	public ResultInfo encrypt(string word, string id, string log, KMBombInfo Bomb)
	{
		Debug.LogFormat("{0} Begin Logic Cipher", log);
		string[] puzzle = generatePuzzle();
		Debug.LogFormat("{0} [Logic Cipher] Gate: {1}", log, puzzle[0]);
		Debug.LogFormat("{0} [Logic Cipher] {1}", log, puzzle[1]);
		string encrypt = "", key = "";
		string[] right = CMTools.generateBoolExp(Bomb);
		Debug.LogFormat("{0} [Logic Cipher] Boolean Expression: {1} -> {2}", log, right[0], right[1]);
		string[] bins = { "", "" };
		if(right[1][0] == 'T')
		{
			foreach(char c in word)
			{
				string[] temp = generateLetters(c, puzzle[0]);
				encrypt += temp[0][1];
				key += temp[0][0];
				bins[0] += temp[1][0];
				bins[1] += temp[1][1];
			}
			Debug.LogFormat("{0} [Logic Cipher] {1} -> ({2}, {3}) + ({4}, {5})", log, word, key, bins[0], encrypt, bins[1]);
		}
		else
		{
			foreach (char c in word)
			{
				string[] temp = generateLetters(c, puzzle[0]);
				encrypt += temp[0][0];
				key += temp[0][1];
				bins[0] += temp[1][0];
				bins[1] += temp[1][1];
			}
			Debug.LogFormat("{0} [Logic Cipher] {1} -> ({2}, {3}) + ({4}, {5})", log, word, encrypt, bins[0], key, bins[1]);
		}
		ScreenInfo[] screens = new ScreenInfo[9]; 
		screens[0] = new ScreenInfo(key, new int[] { 35, 35, 35, 32, 28 }[key.Length - 4]);
		screens[1] = new ScreenInfo(right[0], 25);
		screens[2] = new ScreenInfo(bins[0], new int[] { 35, 35, 35, 32, 28 }[bins[0].Length - 4]);
		screens[3] = new ScreenInfo();
		screens[4] = new ScreenInfo(bins[1], new int[] { 35, 35, 35, 32, 28 }[bins[1].Length - 4]);
		screens[5] = new ScreenInfo();
		screens[6] = new ScreenInfo(puzzle[1], new int[] { 35, 35, 32, 28 }[puzzle[1].Length - 5]);
		screens[7] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens) }
		};
	}
	private string[] generatePuzzle()
	{
		int[][] bins =
		{
			new int[]{1, 1, 1, 1, 1, 1},
			new int[]{1, 1, 1, 1, 1, 1},
			new int[]{0, 0, 0, 0, 0, 0},
		};
		string pos1 = "012345";
		string pos0 = "";
		int num0 = Random.Range(2, 5);
		for (int aa = 0; aa < num0; aa++)
		{
			pos0 = pos0 + "" + pos1[Random.Range(0, pos1.Length)];
			bins[0][pos0[aa] - '0'] = 0;
			pos1 = pos1.Replace(pos0[aa] + "", "");
		}
		for (int aa = 0; aa < 2; aa++)
		{
			int n = pos0[Random.Range(0, pos0.Length)] - '0';
			bins[1][n] = aa;
			pos0 = pos0.Replace(n + "", "");
		}
		for (int aa = 0; aa < 2; aa++)
		{
			int n = pos1[Random.Range(0, pos1.Length)] - '0';
			bins[1][n] = aa;
			pos1 = pos1.Replace(n + "", "");
		}
		string pos2 = pos0 + pos1;
		num0 = Random.Range(0, 3);
		for (int aa = 0; aa < num0; aa++)
		{
			int n = pos2[Random.Range(0, pos2.Length)] - '0';
			bins[1][n] = 0;
			pos2 = pos2.Replace(n + "", "");
		}
		string oper = new string[] { "AND", "OR", "XOR", "NAND", "NOR", "XNOR", "->", "<-", "!->", "<-!"}[Random.Range(0, 10)];
		for (int aa = 0; aa < 6; aa++)
		{
			switch (oper)
			{
				case "AND"://AND
					if (bins[0][aa] == 1 && bins[1][aa] == 1)
						bins[2][aa] = 1;
					break;
				case "OR"://OR
					if (bins[0][aa] == 1 || bins[1][aa] == 1)
						bins[2][aa] = 1;
					break;
				case "XOR"://XOR
					if (bins[0][aa] != bins[1][aa])
						bins[2][aa] = 1;
					break;
				case "NAND"://NAND
					if (!(bins[0][aa] == 1 && bins[1][aa] == 1))
						bins[2][aa] = 1;
					break;
				case "NOT"://NOR
					if (!(bins[0][aa] == 1 || bins[1][aa] == 1))
						bins[2][aa] = 1;
					break;
				case "XNOR"://XNOR
					if (bins[0][aa] == bins[1][aa])
						bins[2][aa] = 1;
					break;
				case "->"://->
					if (bins[1][aa] == 1 || bins[0][aa] == 0)
						bins[2][aa] = 1;
					break;
				case "<-"://<-
					if (bins[0][aa] == 1 || bins[1][aa] == 0)
						bins[2][aa] = 1;
					break;
				case "!->"://!->
					if (bins[0][aa] == 1 && bins[1][aa] == 0)
						bins[2][aa] = 1;
					break;
				default://<-!
					if (bins[0][aa] == 0 && bins[1][aa] == 1)
						bins[2][aa] = 1;
					break;
			}
		}
		int[] numbers = { 0, 0, 0 };
		int[] vals = { 32, 16, 8, 4, 2, 1 };
		for (int aa = 0; aa < 6; aa++)
		{
			for (int bb = 0; bb < 3; bb++)
			{
				if (bins[bb][aa] == 1)
					numbers[bb] += vals[aa];
			}
		}
		return new string[] { oper, numbers[0] + "?" + numbers[1] + "=" + numbers[2]};
	}
	private string[] generateLetters(char c, string gate)
	{
		List<string> binalpha = new List<string> {
			"00000","00001","00010","00011","00100",
			"00101","00110","00111","01000","01001",
			"01010","01011","01100","01101","01110",
			"01111","10000","10001","10010","10011",
			"10100","10101","10110","10111","11000",
			"11001"
		};
		string binlet = binalpha["ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c)];
		string[] bins = { "", "" };
		string[] options;
		foreach (char b in binlet)
		{
			switch (gate)
			{
				case "AND":
					if (b == '1')
						options = new string[] { "11" };
					else
						options = new string[] { "00", "01", "10" };
					break;
				case "OR":
					if (b == '1')
						options = new string[] { "01", "10", "11" };
					else
						options = new string[] { "00" };
					break;
				case "XOR":
					if (b == '1')
						options = new string[] { "01", "10" };
					else
						options = new string[] { "00", "11" };
					break;
				case "NAND":
					if (b == '1')
						options = new string[] { "00", "01", "10" };
					else
						options = new string[] { "11" };
					break;
				case "NOR":
					if (b == '1')
						options = new string[] { "00" };
					else
						options = new string[] { "01", "10", "11" };
					break;
				case "XNOR":
					if (b == '1')
						options = new string[] { "00", "11" };
					else
						options = new string[] { "01", "10" };
					break;
				case "->":
					if (b == '1')
						options = new string[] { "00", "01", "11" };
					else
						options = new string[] { "10" };
					break;
				case "<-":
					if (b == '1')
						options = new string[] { "00", "10", "11" };
					else
						options = new string[] { "01" };
					break;
				case "!->":
					if (b == '1')
						options = new string[] { "10" }; 
					else
						options = new string[] { "00", "01", "11" };
					break;
				default:
					if (b == '1')
						options = new string[] { "01" };
					else
						options = new string[] { "00", "10", "11" }; 
					break;
			}
			string result = options[Random.Range(0, options.Length)];
			bins[0] += result[0];
			bins[1] += result[1];
		}
		string over = "", lets = "";
		for(int i = 0; i < bins.Length; i++)
		{
			if(!(binalpha.Contains(bins[i])))
			{
				over += "1";
				bins[i] = bins[i].Replace("1", "*");
				bins[i] = bins[i].Replace("0", "1");
				bins[i] = bins[i].Replace("*", "0");
			}
			else
			{
				string temp = bins[i].Replace("1", "*");
				temp =  temp.Replace("0", "1");
				temp = temp.Replace("*", "0");
				if (binalpha.Contains(temp) && Random.Range(0, 3) == 0)
				{
					over += "1";
					bins[i] = temp.ToUpperInvariant();
				}
				else
					over += "0";
			}
			lets += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[binalpha.IndexOf(bins[i])];
		}
		return new string[] { lets, over };
	}
}
