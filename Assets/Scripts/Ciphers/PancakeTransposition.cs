using System;
using System.Collections.Generic;
using CipherMachine;
using Words;

public class PancakeTransposition : CipherBase
{
    public override string Name { get { return invert ? "Inverted Pancake Transposition" : "Pancake Transposition"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "PA"; } }
    public override bool IsTransposition { get { return true; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }

    public PancakeTransposition(bool invert) { this.invert = invert; }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
    tryagain:
        var logMessages = new List<string>();
        string encrypt = word.ToUpperInvariant();
        string numbers = "12345678".Substring(0, word.Length);
        string[] nums = { "", "" };
        List<string> list = new List<string>();
        if(invert)
        {
            for (int i = 0; i < 7; i++)
            {
            reshuffle:
                numbers = new string(numbers.ToCharArray().Shuffle());
                int[] n = { "12345678".IndexOf(numbers[0]), "12345678".IndexOf(numbers[1]) };
                Array.Sort(n);
                string temp = encrypt.Substring(0, n[0]) + reverse(encrypt.Substring(n[0], (n[1] - n[0]) + 1)) + encrypt.Substring(n[1] + 1);
                if(list.Contains(temp))
                    goto reshuffle;
                nums[0] = nums[0] + "" + numbers[0];
                nums[1] = nums[1] + "" + numbers[1];
                logMessages.Add(string.Format("{0} + {1}{2} -> {3}", encrypt, nums[0][0], nums[1][0], temp));
                encrypt = temp.ToUpperInvariant();
                list.Add(encrypt);
            }
        }
        else
        {
            for (int i = 0; i < 7; i++)
            {
            reshuffle:
                numbers = new string(numbers.ToCharArray().Shuffle());
                int[] n = { "12345678".IndexOf(numbers[0]), "12345678".IndexOf(numbers[1]) };
                Array.Sort(n);
                string temp = encrypt.Substring(0, n[0]) + reverse(encrypt.Substring(n[0], (n[1] - n[0]) + 1)) + encrypt.Substring(n[1] + 1);
                if (list.Contains(temp))
                    goto reshuffle;
                nums[0] = numbers[0] + "" + nums[0];
                nums[1] = numbers[1] + "" + nums[1];
                logMessages.Add(string.Format("{0} + {1}{2} -> {3}", encrypt, nums[0][0], nums[1][0], temp));
                encrypt = temp.ToUpperInvariant();
                list.Add(encrypt);
            }
        }
        if (word.Equals(encrypt)) goto tryagain;
            
        logMessages.Add(string.Format("Screen 1: {0}", nums[0]));
        logMessages.Add(string.Format("Screen 2: {0}", nums[1]));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { nums[0], null, nums[1] }, invert) }
        };
    }
    private string reverse(string s)
    {
        char[] c = s.ToCharArray();
        Array.Reverse(c);
        return new string(c);
    }
}
