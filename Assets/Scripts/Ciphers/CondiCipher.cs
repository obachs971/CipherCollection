using System.Collections.Generic;
using CipherMachine;
using Words;

public class CondiCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Condi Cipher" : "Condi Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "CD"; } }

    private readonly bool invert;
    public CondiCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string kw = new Data().PickWord(4, 8);
        string encrypt = "";
        var keyFront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", keyFront.Value);
        var offsetExpr = CMTools.generateValue(bomb);
        var offset = offsetExpr.Value;
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Key: {0} -> {1} -> {2}", keyFront.Expression, keyFront.Value, key));
        logMessages.Add(string.Format("Starting Offset: {0} -> {1}", offsetExpr.Expression, offset));
        if (invert)
        {
            foreach (char c in word)
            {
                encrypt = encrypt + "" + key[CMTools.mod(key.IndexOf(c) - offset, 26)];
                offset = key.IndexOf(encrypt[encrypt.Length - 1]) + 1;
            }
        }
        else
        {
            foreach (char c in word)
            {
                encrypt = encrypt + "" + key[CMTools.mod(key.IndexOf(c) + offset, 26)];
                offset = key.IndexOf(c) + 1;
            }
        }
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
        screens[1] = new ScreenInfo(keyFront.Expression, 25);
        screens[2] = new ScreenInfo(offsetExpr.Expression, 35);
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
}
