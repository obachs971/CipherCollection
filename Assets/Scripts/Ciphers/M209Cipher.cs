using CipherMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class M209Cipher : CipherBase
{
    public override string Name { get { return "M-209 Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "MC"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string[][] key = generatePins(logMessages, word.Length);
        string[] lugs = generateLugs(logMessages, word.Length), tapcode = { "", "", "", "", "", "", "", "" };
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", rotorLets = "", encrypt = "";
        for (int i = 0; i < 6; i++)
        {
            rotorLets = rotorLets + "" + alpha[UnityEngine.Random.Range(0, key[1][i].Length)];
            key[1][i] = key[1][i].Substring(alpha.IndexOf(rotorLets[i])) + key[1][i].Substring(0, alpha.IndexOf(rotorLets[i]));
        }
        logMessages.Add(string.Format("Rotor Letter Config: {0}", rotorLets));
        for (int i = 0; i < word.Length; i++)
        {
            int sum = 0;
            for (int j = 0; j < 6; j++)
            {
                logMessages.Add(string.Format("{0}: {1}", lugs[j].Length < 2 ? "0" + lugs[j] : lugs[j], key[1][j]));
                if (key[1][j][0] == '1')
                    sum += Int32.Parse(lugs[j]);
                key[1][j] = key[1][j].Substring(1) + key[1][j].Substring(0, 1);
            }
            encrypt = encrypt + "" + alpha[((25 - alpha.IndexOf(word[i])) + sum) % 26];

            logMessages.Add(string.Format("{0} -> {1} + {2} -> {3}", word[i], alpha[25 - alpha.IndexOf(word[i])], sum, encrypt[i]));

        }
        for (int i = 0; i < lugs[6].Length; i++)
            tapcode[i % 8] = tapcode[i % 8] + "" + lugs[6][i];
        logMessages.Add(string.Format("{0} - > {1}", word, encrypt));
        ScreenInfo[][] screens = new ScreenInfo[2][];
        screens[0] = new ScreenInfo[9];
        screens[1] = new ScreenInfo[9];
        for (int i = 0; i < 8; i += 2)
        {
            screens[0][i] = new ScreenInfo(key[0][i / 2], new int[] { 35, 35, 35, 35, 35, 35, 32, 28 }[key[0][i / 2].Length - 1]);
            screens[0][i + 1] = new ScreenInfo(tapcode[i / 2], 25);
        }
        for (int i = 0; i < 8; i += 2)
        {
            if (i < 4)
                screens[1][i] = new ScreenInfo(key[0][(i / 2) + 4], new int[] { 35, 35, 35, 35, 35, 35, 32, 28 }[key[0][(i / 2) + 4].Length - 1]);
            screens[1][i + 1] = new ScreenInfo(tapcode[(i / 2) + 4], 25);
        }
        screens[1][4] = new ScreenInfo(rotorLets, 35);
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens[0]), new PageInfo(screens[1]) }
        };
    }
    private string[][] generatePins(List<string> logMessages, int length)
    {
        string alpha = "ABCDEFGHIJKLMNOP";
        string[] b = { "", "" };
        for (int i = 0; i < length; i++)
        {
            b[0] += "0";
            b[1] += "1";
        }
        string[] bins =
        {
            "1111111100000000",
            "111111000000" + UnityEngine.Random.Range(0, 2),
            "1111100000" + UnityEngine.Random.Range(0, 2),
            "11110000" + UnityEngine.Random.Range(0, 2),
            "111000" + UnityEngine.Random.Range(0, 2),
            "1100" + UnityEngine.Random.Range(0, 2)
        };
        string[] pins = { "", "", "", "", "", "" };
        for (int i = 0; i < bins.Length; i++)
        {
            do bins[i] = new string(bins[i].ToCharArray().Shuffle());
            while ((bins[i] + bins[i]).IndexOf(b[0]) >= 0 || (bins[i] + bins[i]).IndexOf(b[1]) >= 0);
            for (int j = 0; j < bins[i].Length; j++)
            {
                if (bins[i][j] == '1')
                    pins[i] = pins[i] + "" + alpha[j];
            }
            logMessages.Add(string.Format("Active Pins for Rotor #{0}: {1}", (i + 1), pins[i]));
        }
        return new string[][] { pins, bins };
    }
    private string[] generateLugs(List<string> logMessages, int length)
    {
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", tapCode = "", encode = "";
        int[] nums = { 1, 1, 1, 1, 1, 1 };
        for (int i = 0; i < 24; i++)
        {
            int index = UnityEngine.Random.Range(0, 6);
            nums[index]++;
            nums[(index + UnityEngine.Random.Range(0, 5)) % 6]++;
        }
        for (int i = 0; i < ((length + 7) * 2); i++)
        {
            int index = UnityEngine.Random.Range(0, 6);
            while (nums[index] < 2)
                index = UnityEngine.Random.Range(0, 6);
            nums[index]--;
        }
        for (int i = 0; i < nums.Length; i++)
        {
            for (int j = 0; j < nums[i]; j++)
                tapCode = tapCode + "" + (i + 1);
        }
        tapCode = new string(tapCode.ToCharArray().Shuffle());
        for (int i = 0; i < tapCode.Length; i += 2)
        {
            int index = UnityEngine.Random.Range(0, 2);
            string s = tapCode[i + index] + "" + tapCode[i + ((1 + index) % 2)];
            encode = encode + "" + alpha["123456".IndexOf(s[0]) * 6 + "123456".IndexOf(s[1])];
        }
        logMessages.Add(string.Format("Number of Lugs for each Rotor: {0}, {1}, {2}, {3}, {4}, {5}", nums[0], nums[1], nums[2], nums[3], nums[4], nums[5]));
        logMessages.Add(string.Format("{0} -> {1}", tapCode, encode));
        return new string[] { nums[0] + "", nums[1] + "", nums[2] + "", nums[3] + "", nums[4] + "", nums[5] + "", encode };
    }
}