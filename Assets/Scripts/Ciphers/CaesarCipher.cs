using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class CaesarCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Caesar Cipher" : "Caesar Cipher"; } }
    public override string Code { get { return "CA"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public CaesarCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        var val = CMTools.generateValue(bomb);
        int offset = (val.Value % 25) + 1;
        string encrypt = "", alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        if (invert)
        {
            foreach (char c in word)
                encrypt = encrypt + "" + alpha[CMTools.mod(alpha.IndexOf(c) + offset, 26)];
        }
        else
        {
            foreach (char c in word)
                encrypt = encrypt + "" + alpha[CMTools.mod(alpha.IndexOf(c) - offset, 26)];
        }
        logMessages.Add(string.Format("Generated Value: {0} -> {1}", val.Expression, val.Value));
        logMessages.Add(string.Format("Offset: {0}", offset));
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { val.Expression }, invert) },
            Score = 3
        };
    }
}
