using CipherMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class PrissyCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Prissy Cipher" : "Prissy Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "PR"; } }

    private readonly bool invert;
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
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
        screens[1] = new ScreenInfo(kwfront.Expression, 25);
        screens[2] = new ScreenInfo(value.Expression, 35);
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
}