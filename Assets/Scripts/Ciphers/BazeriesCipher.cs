using System;
using System.Collections.Generic;
using CipherMachine;
using UnityEngine;

public class BazeriesCipher : CipherBase
{
	public override string Name { get { return invert ? "Inverted Bazeries Cipher" : "Bazeries Cipher"; } }
	public override int Score { get { return 5; } }
	public override string Code { get { return "BA"; } }
    
    private readonly bool invert;
    public BazeriesCipher(bool invert) { this.invert = invert; }
    
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string encrypt = "";
        string replaceJ = "";
        string alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
        logMessages.Add(string.Format("Before Replacing Js: {0}", word));
        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == 'J')
            {
                word = word.Substring(0, i) + "" + alpha[UnityEngine.Random.Range(0, alpha.Length)] + "" + word.Substring(i + 1);
                replaceJ = replaceJ + "" + word[i];
            }
            else
                replaceJ = replaceJ + "" + alpha.Replace(word[i].ToString(), "")[UnityEngine.Random.Range(0, 24)];
        }
        logMessages.Add(string.Format("After Replacing Js: {0}", word));
        logMessages.Add(string.Format("Screen 2: {0}", replaceJ));
        string kw = "";
        int[] digits = new int[4];
        int sum = 0;
        for (int i = 0; i < 4; i++)
        {
            digits[i] = UnityEngine.Random.Range(0, 10);
            kw = kw + "" + new string[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE" }[digits[i]];
            sum += digits[i];
        }
        sum = (sum % (word.Length - 1)) + 2;
        string temp = "";
        while (temp.Length < word.Length)
        {
            char[] c;
            if (temp.Length + sum > word.Length)
                c = word.Substring(temp.Length).ToCharArray();
            else
                c = word.Substring(temp.Length, sum).ToCharArray();
            Array.Reverse(c);
            temp = temp + "" + new string(c);
        }
        string[] keyFront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw, alpha, keyFront[1][0] == 'T');
        alpha = "AFLQVBGMRWCHNSXDIOTYEKPUZ";
        if (invert)
        {
            foreach (char c in temp)
                encrypt = encrypt + "" + alpha[key.IndexOf(c)];
        }
        else
        {
            foreach (char c in temp)
                encrypt = encrypt + "" + key[alpha.IndexOf(c)];
        }
        logMessages.Add(string.Format("Digit Key: {0}{1}{2}{3}", digits[0], digits[1], digits[2], digits[3]));
        logMessages.Add(string.Format("Key: {0} -> {1} -> {2}", keyFront[0], keyFront[1], key));
        logMessages.Add(string.Format("{0} -> {1} -> {2}", word, temp, encrypt));
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(digits[0] + "" + digits[1] + "" + digits[2] + "" + digits[3], 35);
        screens[1] = new ScreenInfo(keyFront[0], 25);
        screens[2] = new ScreenInfo(replaceJ, new int[] { 35, 35, 35, 32, 28 }[replaceJ.Length - 4]);
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
}
