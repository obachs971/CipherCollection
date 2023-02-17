using System.Collections.Generic;
using CipherMachine;
using UnityEngine;

public class McDondaldsChickenNuggetBigMacCipher : CipherBase
{
    public override string Name { get { return "McDondalds™ Chicken Nugget Big Mac Cipher"; } }
    public override string Code { get { return "MD"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY", encrypt = "";
        int[] vals = generateValues();
        int burgerPrice = vals[0], nuggetPrice = vals[1], nuggetCount = vals[2];
        string[] screens = { intToPrice(burgerPrice), intToPrice(nuggetPrice), nuggetCount + " Pc.", "" };
        for (int i = 0; i < 3; i++)
            logMessages.Add(string.Format("Screen {0}: {1}", (i + 1), screens[i]));
        for (int i = 0; i < word.Length / 2; i++)
        {
            string temp = base10To26(alpha.IndexOf(word[i * 2]) * nuggetPrice + alpha.IndexOf(word[i * 2 + 1]) * burgerPrice);
            screens[3] = screens[3] + "" + temp[0];
            encrypt = encrypt + "" + temp.Substring(1);
        }
        logMessages.Add(string.Format("Screen 4: {0}", screens[3]));
        if (word.Length % 2 == 1)
            encrypt = encrypt + "" + word[word.Length - 1];
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { screens[0], null, screens[1], null, screens[2], null, screens[3] }) },
            Score = 7
        };
    }
    private int[] generateValues()
    {
    tryagain:
        int burgerPrice = Random.Range(99, 425);
        List<int> possNuggetPrices = new List<int>();
        for (int i = (burgerPrice * 20) / 100; i < (burgerPrice * 60) / 100; i++)
        {
            if (GCD(burgerPrice, i) == 1)
                possNuggetPrices.Add(i);
        }
        if (possNuggetPrices.Count == 0)
            goto tryagain;
        int nuggetPrice = possNuggetPrices[Random.Range(0, possNuggetPrices.Count)];
        int nuggetCount = EEA(burgerPrice, nuggetPrice);
        if (nuggetCount > nuggetPrice)
        {
            var temp = nuggetCount;
            nuggetCount = nuggetPrice;
            nuggetPrice = temp;
        }
        return new int[] { burgerPrice, nuggetPrice, nuggetCount };
    }
    private int GCD(int a, int b)
    {
        if (b > a)
        {
            int temp = a + 0;
            a = b + 0;
            b = temp + 0;
        }
        int r = a % b;
        while (r > 0)
        {
            a = b;
            b = r;
            r = a % b;
        }
        return b;
    }
    private int EEA(int A, int B)
    {
        int init = A + 0;
        int Q = A / B;
        int R = A % B;
        int T1 = 0;
        int T2 = 1;
        int T3 = T1 - (T2 * Q);
        while (R > 0)
        {
            A = B;
            B = R;
            Q = A / B;
            R = A % B;
            T1 = T2;
            T2 = T3;
            T3 = T1 - (T2 * Q);
        }
        return CMTools.mod(T2, init);
    }
    private string intToPrice(int n)
    {
        string price = "$" + (n / 100);
        if (n % 100 < 10)
            price = price + ".0" + (n % 100);
        else
            price = price + "." + (n % 100);
        return price;
    }
    private string base10To26(int num)
    {
        string conv = "", alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY";
        while (num > 0)
        {
            conv = alpha[num % 26] + "" + conv;
            num /= 26;
        }
        while (conv.Length < 3)
            conv = "Z" + conv;
        return conv;
    }
    private int base26To10(string num)
    {
        string alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY";
        int conv = 0, mult = 1;
        for (int i = num.Length - 1; i >= 0; i--)
            conv += (mult * alpha.IndexOf(num[i]));
        return conv;
    }
}
