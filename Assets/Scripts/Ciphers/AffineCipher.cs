using System.Collections.Generic;
using CipherMachine;
using UnityEngine;

public class AffineCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Affine Cipher" : "Affine Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "AF"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public AffineCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        int[][] choices =
        {
            new int[]{ 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25 },
            new int[]{ 9, 21, 15, 3, 19, 7, 23, 11, 5, 17, 25 },
        };
        int e = Random.Range(0, choices[0].Length);
        var xVal = CMTools.generateValue(bomb);
        int x = (xVal.Value % 25) + 1;
        string encrypt = "";
        string alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY";
        if (invert)
        {
            foreach (char c in word)
                encrypt = encrypt + "" + alpha[CMTools.mod((alpha.IndexOf(c) - x) * choices[1][e], 26)];
        }
        else
        {
            foreach (char c in word)
                encrypt = encrypt + "" + alpha[CMTools.mod((alpha.IndexOf(c) * choices[0][e]) + x, 26)];
        }
        logMessages.Add(string.Format("E: {0}", choices[0][e]));
        logMessages.Add(string.Format("D: {0}", choices[1][e]));
        logMessages.Add(string.Format("Value Generated: {0} -> {1}", xVal.Expression, xVal.Value));
        logMessages.Add(string.Format("X: {0}", x));
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { choices[0][e] + "", xVal.Expression }, invert) }
        };
    }
}
