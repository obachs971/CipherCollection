using System.Collections.Generic;
using CipherMachine;
using UnityEngine;
using Words;

public class AlbertiCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Alberti Cipher" : "Alberti Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "AL"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public AlbertiCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", encrypt = "";
        var startIndex = CMTools.generateValue(bomb);
        int adder = new int[] { 1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25 }[Random.Range(0, 12)];
        logMessages.Add(string.Format("Screen 1: {0}", adder));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", startIndex.Expression, startIndex.Value));
        int offset = startIndex.Value % 26;
        if (invert)
        {
            foreach (char letter in word)
            {
                encrypt = encrypt + "" + alpha[CMTools.mod(alpha.IndexOf(letter) - offset, 26)];
                offset = CMTools.mod(offset + adder, 26);
                logMessages.Add(string.Format("{0} -> {1}, {2}", letter, encrypt[encrypt.Length - 1], offset));
            }
        }
        else
        {
            foreach(char letter in word)
            {
                encrypt = encrypt + "" + alpha[CMTools.mod(alpha.IndexOf(letter) + offset, 26)];
                offset = CMTools.mod(offset + adder, 26);
                logMessages.Add(string.Format("{0} -> {1}, {2}", letter, encrypt[encrypt.Length - 1], offset));
            }
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { adder + "", startIndex.Expression }, invert) }
        };
    }
}
