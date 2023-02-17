using System;
using System.Collections.Generic;
using CipherMachine;
using Words;

public class MorbitCipher : CipherBase
{
    public override string Name { get { return "Morbit Cipher"; } }
    public override string Code { get { return "MO"; } }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string keyword = new Data().PickWord(8);
        string encrypt = "";
        string alpha = "-ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int[] key = { 0, 0, 0, 0, 0, 0, 0, 0 };
        string[] morkey = { "..", ".-", ".x", "-.", "--", "-x", "x.", "x-" };
        char[] order = keyword.ToCharArray();
        Array.Sort(order);
        string temp = keyword + "";
        for (int i = 0; i < 8; i++)
        {
            int index = temp.IndexOf(order[i]);
            key[index] = i + 1;
            temp = temp.Substring(0, index) + "-" + temp.Substring(index + 1);
        }

        temp = "";
        foreach (char c in word)
            temp = temp + letterToMorse(c) + "x";
        temp = temp.Substring(0, temp.Length - 1);
        if (temp.Length % 2 == 1)
            temp = UnityEngine.Random.Range(0, 2) == 0 ? "x" + temp : temp + "x";

        string nums = "";
        for (int i = 0; i < temp.Length / 2; i++)
            nums = nums + "" + key[Array.IndexOf(morkey, temp[i * 2] + "" + temp[(i * 2) + 1])];
        for (int i = 0; i < word.Length; i++)
        {
            int n = (nums[i] - '0') + (8 * UnityEngine.Random.Range(0, 3));
            if (n > 26)
                n -= 8;
            encrypt = encrypt + "" + alpha[n];
        }
        logMessages.Add(string.Format("Keyword: {0}", keyword));
        logMessages.Add(string.Format("Key: {0}{1}{2}{3}{4}{5}{6}{7}", key[0], key[1], key[2], key[3], key[4], key[5], key[6], key[7]));
        logMessages.Add(string.Format("{0} -> {1} -> {2}", word, temp, nums));
        logMessages.Add(string.Format("{0} -> {1}", nums.Substring(0, word.Length), encrypt));
        nums = nums.Substring(word.Length);
        logMessages.Add(string.Format("Leftover digits: {0}", nums));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { keyword, null, nums.Substring(0, (nums.Length / 2) + (nums.Length % 2)), null, nums.Substring((nums.Length / 2) + (nums.Length % 2)) }) },
            Score = 3
        };
    }
    private string letterToMorse(char c)
    {
        switch (c)
        {
            case 'A': return ".-";
            case 'B': return "-...";
            case 'C': return "-.-.";
            case 'D': return "-..";
            case 'E': return ".";
            case 'F': return "..-.";
            case 'G': return "--.";
            case 'H': return "....";
            case 'I': return "..";
            case 'J': return ".---";
            case 'K': return "-.-";
            case 'L': return ".-..";
            case 'M': return "--";
            case 'N': return "-.";
            case 'O': return "---";
            case 'P': return ".--.";
            case 'Q': return "--.-";
            case 'R': return ".-.";
            case 'S': return "...";
            case 'T': return "-";
            case 'U': return "..-";
            case 'V': return "...-";
            case 'W': return ".--";
            case 'X': return "-..-";
            case 'Y': return "-.--";
            case 'Z': return "--..";
        }
        return "";
    }
}
