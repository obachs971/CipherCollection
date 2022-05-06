using System.Collections.Generic;
using CipherMachine;
using Words;

public class BellasoCipher : CipherBase
{
    public override string Name { get { return "Bellaso Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "BE"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string encrypt = "";
        Data data = new Data();
        string[] kws = { data.PickWord(8), data.PickWord(8) };
        string key = CMTools.getKey(kws[0] + kws[1], "", true);
        if (key.Length % 2 == 1)
            key = key.Substring(0, key.Length / 2) + key.Substring(key.Length / 2 + 1);
        string[] parts = { key.Substring(0, key.Length / 2), key.Substring(key.Length / 2) };
        key = CMTools.getKey(key, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", true).Substring(key.Length);
        key = parts[0] + key.Substring(0, 13 - parts[0].Length) + parts[1] + key.Substring(13 - parts[0].Length);
        var right = CMTools.generateBoolExp(bomb);
        logMessages.Add(string.Format("Keyword 1: {0}", kws[0]));
        logMessages.Add(string.Format("Keyword 2: {0}", kws[1]));
        logMessages.Add(string.Format("Screen A: {0} -> {1} -> {2}", right.Expression, right.Value, right.Value ? "RIGHT" : "LEFT"));
        foreach(char letter in word)
        {
            logMessages.Add(string.Format("{0}", key));
            encrypt = encrypt + "" + key[(key.IndexOf(letter) + 13) % 26];
            if (right.Value)
                key = key.Substring(0, 13) + "" + key[key.Length - 1] + "" + key.Substring(13, key.Length - 14);
            else
                key = key.Substring(0, 13) + key.Substring(14) + "" + key[13];
            logMessages.Add(string.Format("{0} -> {1}", letter, encrypt[encrypt.Length - 1]));
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kws[0], right.Expression, kws[1] }) }
        };
    }
}
