using System;
using System.Collections.Generic;
using CipherMachine;
using Words;

public class PancakeTransposition : CipherBase
{
    public override string Name { get { return invert ? "Inverted Pancake Transposition" : "Pancake Transposition"; } }
    public override string Code { get { return "PA"; } }
    public override bool IsTransposition { get { return true; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }

    public PancakeTransposition(bool invert) { this.invert = invert; }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string encrypt = word.ToUpperInvariant();
        string numbers = "12345678".Substring(0, word.Length);
        string arrangement = numbers;
        string[] nums = { "", "" };
        List<string> arrangementsUsed = new List<string>();

        for (int i = 0; i < 7; i++)
        {
            reshuffle:
            numbers = new string(numbers.ToCharArray().Shuffle());
            int[] n = { "12345678".IndexOf(numbers[0]), "12345678".IndexOf(numbers[1]) };
            Array.Sort(n);
            string newArrangement = arrangement.Substring(0, n[0]) + reverse(arrangement.Substring(n[0], (n[1] - n[0]) + 1)) + arrangement.Substring(n[1] + 1);
            if (arrangementsUsed.Contains(newArrangement))
                goto reshuffle;
            nums[0] = invert ? nums[0] + numbers[0] : numbers[0] + nums[0];
            nums[1] = invert ? nums[1] + numbers[1] : numbers[1] + nums[1];
            string newEncrypt = encrypt.Substring(0, n[0]) + reverse(encrypt.Substring(n[0], (n[1] - n[0]) + 1)) + encrypt.Substring(n[1] + 1);
            logMessages.Add(string.Format("{0} + {1}{2} -> {3}", encrypt, numbers[0], numbers[1], newEncrypt));
            encrypt = newEncrypt;
            arrangement = newArrangement;
            arrangementsUsed.Add(arrangement);
        }

        logMessages.Add(string.Format("Screen 1: {0}", nums[0]));
        logMessages.Add(string.Format("Screen 2: {0}", nums[1]));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { nums[0], null, nums[1] }, invert) },
            Score = 4
        };
    }
    private string reverse(string s)
    {
        char[] c = s.ToCharArray();
        Array.Reverse(c);
        return new string(c);
    }
}
