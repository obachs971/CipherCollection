using CipherMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class PrissyCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Prissy Cipher" : "Prissy Cipher"; } }
    public override int Score(int wordLength) { return 6; }
    public override string Code { get { return "PR"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public PrissyCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string kw = new Data().PickWord(4, 8);
        string encrypt = "";
        var kwfront = CMTools.generateBoolExp(bomb);
        var value = CMTools.generateValue(bomb);
        string key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront.Value);
        int offset = value.Value % 13;
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Key: {0} -> {1} -> {2}", kwfront.Expression, kwfront.Value, key));
        logMessages.Add(string.Format("Offset: {0} -> {1} -> {2}", value.Expression, value.Value, offset));
        if (invert)
        {
            for (int i = 0; i < word.Length; i++)
            {
                int index = key.IndexOf(word[i]);
                encrypt = encrypt + "" + key[CMTools.mod((index % 13) - offset, 13) + ((((index / 13) + 1) % 2) * 13)];
                offset = (offset + "-ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(encrypt[i])) % 13;
                logMessages.Add(string.Format("{0} -> {1}", word[i], encrypt[i]));
                logMessages.Add(string.Format("New Offset: {0}", offset, offset));
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                int index = key.IndexOf(word[i]);
                encrypt = encrypt + "" + key[CMTools.mod((index % 13) + offset, 13) + ((((index / 13) + 1) % 2) * 13)];
                offset = (offset + "-ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(word[i])) % 13;
                logMessages.Add(string.Format("{0} -> {1}", word[i], encrypt[i]));
                logMessages.Add(string.Format("New Offset: {0}", offset, offset));
            }
        }
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, kwfront.Expression, value.Expression }, invert) }
        };
    }
}
