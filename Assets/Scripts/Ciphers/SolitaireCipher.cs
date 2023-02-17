using System.Collections.Generic;
using CipherMachine;
using UnityEngine;

public class SolitaireCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Solitaire Cipher" : "Solitaire Cipher"; } }
    public override string Code { get { return "SO"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public SolitaireCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string key = new string("12ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Shuffle());
        string[] display = { key.Substring(0, 7), key.Substring(7, 7), key.Substring(14, 7), key.Substring(21) };
        string letters = new string("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Shuffle()).Substring(0, 2);
        string encrypt = "";
        logMessages.Add(string.Format("Key: {0}", key));
        logMessages.Add(string.Format("Letters: {0}", letters));
        for (int i = 0; i < word.Length; i++)
        {
            logMessages.Add(key);
            //Shift 1 and 2
            key = Shift("1", key, 1);
            key = Shift("2", key, 2);
            logMessages.Add(key);
            //Triple Cut
            string left = "", right = "";
            while (!(key[0] == '1' || key[0] == '2'))
            {
                left = left + key[0];
                key = key.Substring(1);
            }
            while (!(key[key.Length - 1] == '1' || key[key.Length - 1] == '2'))
            {
                right = key[key.Length - 1] + right;
                key = key.Substring(0, key.Length - 1);
            }
            key = right.ToUpperInvariant() + key + left.ToUpperInvariant();
            logMessages.Add(key);
            //Count Cut
            int cur = getNumber(key[key.Length - 1], letters);
            left = key.Substring(0, cur);
            key = key.Substring(cur);
            key = key.Substring(0, key.Length - 1) + left + key[key.Length - 1];
            logMessages.Add(key);
            //Find Output Value
            cur = getNumber(key[0], letters);
            if (invert)
            {
                cur = getNumber(word[i], letters) - getNumber(key[cur], letters);
                while (cur < 1)
                    cur += 26;
            }
            else
            {
                cur = getNumber(word[i], letters) + getNumber(key[cur], letters);
                while (cur > 26)
                    cur -= 26;
            }
            encrypt = encrypt + "" + "-ABCDEFGHIJKLMNOPQRSTUVWXYZ"[cur];
            logMessages.Add(string.Format("{0} -> {1}", word[i], encrypt[i]));
        }
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        var screens = new ScreenInfo[7];
        for (int i = 0; i < 8; i += 2)
            screens[i] = display[i / 2];
        screens[1] = letters;
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(screens, invert) },
            Score = 14
        };
    }
    private int getNumber(char c, string lets)
    {
        switch (c)
        {
            case '1': return "-ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(lets[0]);
            case '2': return "-ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(lets[1]);
            default: return "-ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c);
        }
    }
    string Shift(string j, string deck, int shift)
    {
        int cur = deck.IndexOf(j);
        deck = deck.Replace(j, "");
        if ((cur + shift) == 27)
            deck = deck + "" + j;
        else
            deck = deck.Substring(0, (cur + shift) % 28) + "" + j + "" + deck.Substring((cur + shift) % 28);
        return deck;
    }
}
