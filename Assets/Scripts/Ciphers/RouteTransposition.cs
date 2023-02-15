using System.Collections.Generic;
using CipherMachine;
using UnityEngine;

public class RouteTransposition : CipherBase
{
    public override string Name { get { return invert ? "Inverted Route Transposition" : "Route Transposition"; } }
    public override int Score(int wordLength) { return 4; }
    public override string Code { get { return "RT"; } }
    public override bool IsTransposition { get { return true; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public RouteTransposition(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        int number = Random.Range(0, word.Length) + 1;
        string encrypt;
        logMessages.Add(string.Format("Key Number: {0}", number));
        char[] temp = new char[word.Length];
        if (invert)
        {
            encrypt = word.Substring(number - 1) + word.Substring(0, number - 1);
            for (int i = 0; i < encrypt.Length / 2; i++)
            {
                temp[(i * 2)] = encrypt[i];
                temp[(i * 2) + 1] = encrypt[encrypt.Length - (i + 1)];
            }
            if (encrypt.Length % 2 == 1)
                temp[encrypt.Length - 1] = encrypt[encrypt.Length / 2];
            encrypt = new string(temp);
        }
        else
        {
            temp[0] = word[0];
            for (int i = 0; i < (word.Length - 1) / 2; i++)
            {
                temp[word.Length - (i + 1)] = word[(i * 2) + 1];
                temp[i + 1] = word[(i * 2) + 2];
            }
            if (word.Length % 2 == 0)
                temp[word.Length / 2] = word[word.Length - 1];
            encrypt = new string(temp);
            encrypt = encrypt.Substring(encrypt.Length - (number - 1)) + encrypt.Substring(0, encrypt.Length - (number - 1));
        }
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { number + "" }, invert) }
        };
    }
}
