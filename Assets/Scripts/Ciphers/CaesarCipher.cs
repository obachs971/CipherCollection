using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class CaesarCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Caesar Cipher" : "Caesar Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "CC"; } }

    private readonly bool invert;
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
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(val.Expression, 35);
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
}