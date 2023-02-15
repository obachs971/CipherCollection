using System.Collections.Generic;
using CipherMachine;
using KModkit;
using Words;

public class OwOCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted OwO Cipher" : "OwO Cipher"; } }
    public override int Score(int wordLength) { return 5; }
    public override string Code { get { return "OC"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public OwOCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string encrypt = "";
        string alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY";
        string screen1 = "";
        string[] OwOs = new string[word.Length];
        string sn = "";
        for (int i = 0; i < word.Length; i++)
        {
            screen1 = screen1 + "" + "CENOQU"[UnityEngine.Random.Range(0, 6)];
            int n = UnityEngine.Random.Range(0, 36);
            sn = sn + "" + "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"[n];
            OwOs[i] = "";
            for (int j = 0; j < (n % 3); j++)
                OwOs[i] = OwOs[i] + "" + screen1[i];
            OwOs[i] += "W";
            while(OwOs[i].Length < 3)
                OwOs[i] = OwOs[i] + "" + screen1[i];
        }
        logMessages.Add(string.Format("Screen 1: {0}", screen1));
        foreach(string str in OwOs)
            logMessages.Add(string.Format("{0}", str));
        string newOwOs = "";
        for(int i = 0; i < 3; i++)
        {
            foreach (string str in OwOs)
                newOwOs = newOwOs + "" + str[i];
        }
        
        if (invert)
        {
            for (int i = 0; i < newOwOs.Length; i += 3)
            {
                int sum = (alpha.IndexOf(newOwOs[i]) + alpha.IndexOf(newOwOs[i + 1]) + alpha.IndexOf(newOwOs[i + 2]) + alpha.IndexOf(word[i / 3])) % 26;
                encrypt = encrypt + "" + alpha[sum];
                logMessages.Add(string.Format("{0} + {1} + {2} + {3} -> {4}", newOwOs[i], newOwOs[i + 1], newOwOs[i + 2], word[i / 3], encrypt[i / 3]));
            }
        }
        else
        {
            for(int i = 0; i < newOwOs.Length; i+=3)
            {
                int sum = alpha.IndexOf(newOwOs[i]) + alpha.IndexOf(newOwOs[i + 1]) + alpha.IndexOf(newOwOs[i + 2]);
                sum = mod(alpha.IndexOf(word[i / 3]) - sum, 26);
                encrypt = encrypt + "" + alpha[sum];
                logMessages.Add(string.Format("{0} - ({1} + {2} + {3}) -> {4}", word[i / 3], newOwOs[i], newOwOs[i + 1], newOwOs[i + 2], encrypt[i / 3]));
            }
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { screen1, null, sn }, invert) }
        };
    }
    private int mod(int n, int m)
    {
        while (n < 0)
            n += m;
        return (n % m);
    }
}
