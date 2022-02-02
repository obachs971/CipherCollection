﻿using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class BitSwitchCipher
{
	public PageInfo[] encrypt(string word, string id, string log)
	{
		Debug.LogFormat("{0} Begin Bit Switch Cipher", log);
		string scrambler = new string("12345".ToCharArray().Shuffle());
		while(check(scrambler))
			scrambler = new string("12345".ToCharArray().Shuffle());
		Debug.LogFormat("{0} [Bit Switch Cipher] Scrambler: {1}", log, scrambler);
		int[] puzzleNums = generateNumbers(log, scrambler);
		string puzzle = "", alpha = "-ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		for(int i = 0; i < puzzleNums.Length; i++)
		{
			if (puzzleNums[i] < 10)
				puzzle = puzzle + "0" + puzzleNums[i];
			else
				puzzle = puzzle + "" + puzzleNums[i];
		}
		string bin = "", encrypt = "";
		foreach(char c in word)
		{
			string alphaBin = numberToBin(alpha.IndexOf(c));
			string encryptBin = scramble(alphaBin, scrambler);
			string invertBin = encryptBin.Replace("0", "*").Replace("1", "0").Replace("*", "1");
			int check = binToNumber(encryptBin);
			if (check > 26)
				bin = bin + "1";
			else
			{
				check = binToNumber(invertBin);
				if (check <= 26 && UnityEngine.Random.Range(0, 3) == 0)
					bin = bin + "1";
				else
					bin = bin + "0";
			}
			string finalBin = bin[bin.Length - 1] == '1' ? invertBin.ToLowerInvariant() : encryptBin.ToLowerInvariant();
			encrypt = encrypt + "" + alpha[binToNumber(finalBin)];
			Debug.LogFormat("{0} [Bit Switch Cipher] {1} -> {2} + {3} -> {4} + {5} -> {6} -> {7}", log, c, alphaBin, scrambler, encryptBin, bin[bin.Length - 1], finalBin, encrypt[encrypt.Length - 1]);
		}
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(puzzle.Substring(0, 8), 28);
		screens[1] = new ScreenInfo();
		screens[2] = new ScreenInfo(puzzle.Substring(8), 28);
		screens[3] = new ScreenInfo();
		screens[4] = new ScreenInfo(bin, new int[] { 35, 35, 35, 32, 28 }[bin.Length - 4]);
		for (int i = 5; i < 8; i++)
			screens[i] = new ScreenInfo();
		screens[8] = new ScreenInfo(id, 35);
		return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens) });
	}
	private int[] generateNumbers(string log, string scrambler)
	{
		string order = new string("01234".ToCharArray().Shuffle());
		string[] initialBins = new string[4];
		string[] resultBins = new string[4];
		int num = UnityEngine.Random.Range(0, 2);
		initialBins[0] = "";
		for(int i = 0; i < 5; i++)
		{
			if (i == (order[0] - '0'))
				initialBins[0] = initialBins[0] + "" + ((num + 1) % 2);
			else
				initialBins[0] = initialBins[0] + "" + num;
		}
		resultBins[0] = scramble(initialBins[0], scrambler);
		for(int i = 1; i < 4; i++)
		{
			num = UnityEngine.Random.Range(0, 2);
			int pos = UnityEngine.Random.Range(0, i);
			initialBins[i] = "";
			for (int j = 0; j < 5; j++)
			{
				if (j == (order[i] - '0') || j == (order[pos] - '0'))
					initialBins[i] = initialBins[i] + "" + ((num + 1) % 2);
				else
					initialBins[i] = initialBins[i] + "" + num;
			}
			resultBins[i] = scramble(initialBins[i], scrambler);
		}
		string[] temp1 = new string[4], temp2 = new string[4];
		order = new string("0123".ToCharArray().Shuffle());
		for (int i = 0; i < 4; i++)
		{
			temp1[i] = initialBins[(order[i] - '0')];
			temp2[i] = resultBins[(order[i] - '0')];
		}
		int[] nums = new int[8];
		for(int i = 0; i < 8; i+=2)
		{
			nums[i] = binToNumber(temp1[i / 2]);
			nums[i + 1] = binToNumber(temp2[i / 2]);
			Debug.LogFormat("{0} [Bit Switch Cipher] {1} -> {2} -> {3} -> {4}", log, nums[i], temp1[i / 2], temp2[i / 2], nums[i + 1]);
		}
		return nums;
	}
	private string scramble(string bin, string scrambler)
	{
		string temp = "";
		for (int i = 0; i < scrambler.Length; i++)
			temp = temp + "" + bin["12345".IndexOf(scrambler[i])];
		return temp;
	}
	private int binToNumber(string bin)
	{
		int num = 0;
		int mult = 1;
		for(int i = (bin.Length - 1); i >= 0; i--)
		{
			if (bin[i] == '1')
				num += mult;
			mult *= 2;
		}
		return num;
	}
	private string numberToBin(int num)
	{
		string bin = "";
		for(int i = 0; i < 5; i++)
		{
			bin = (num % 2) + "" + bin;
			num = num / 2;
		}
		return bin;
	}
	private bool check(string scrambler)
	{
		for(int i = 1; i <= 5; i++)
		{
			if (i == (scrambler[i - 1] - '0'))
				return true;
		}
		return false;
	}
}