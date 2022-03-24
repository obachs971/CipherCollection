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
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, keyFront.Expression, offsetExpr.Expression }, invert) }
        };
    }
}
