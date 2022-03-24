using CipherMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class FractionatedMorseCipher : CipherBase
{
    public override string Name { get { return "Fractionated Morse Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "FM"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string kw = new Data().PickWord(4, 8);
        string encrypt = "", morse = "", extra;
        var keyFront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", keyFront.Value);

        //Convert the letters of the word into morse
        foreach (char c in word)
            morse += letterToMorse(c) + "x";
        morse += "x";

        //Adjusting it so it has at least the same amount of letters as the initial word length as well as being divisible by 3
        while (morse.Length / 3 < word.Length)
        {
            List<int> l = AllIndexesOf(morse, ".x.");
            l.AddRange(AllIndexesOf(morse, ".x-"));
            l.AddRange(AllIndexesOf(morse, "-x."));
            l.AddRange(AllIndexesOf(morse, "-x-"));
            int index = l[UnityEngine.Random.Range(0, l.Count())] + 1;
            morse = morse.Substring(0, index) + "x" + morse.Substring(index);
        }
        morse = morse.Substring(0, morse.Length - (morse.Length % 3));

        //Now it gets encrypted into the letters using the key
        for (int i = 0; i < morse.Length; i += 3)
            encrypt = encrypt + "" + key[(".-x".IndexOf(morse[i]) * 9) + (".-x".IndexOf(morse[i + 1]) * 3) + ".-x".IndexOf(morse[i + 2])];
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Key Front Rule: {0} -> {1}", keyFront.Expression, keyFront.Value));
        logMessages.Add(string.Format("Key: {0}", key));
        logMessages.Add(string.Format("{0} -> {1}", word, morse));
        logMessages.Add(string.Format("{0} -> {1}", morse, encrypt));
        extra = encrypt.Substring(word.Length);
        encrypt = encrypt.Substring(0, word.Length);
        logMessages.Add(string.Format("Encrypted Word: {0}", encrypt));
        logMessages.Add(string.Format("Screen 2: {0}", extra));
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
        screens[1] = new ScreenInfo(keyFront.Expression, 25);
        screens[2] = new ScreenInfo(extra, 35);
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens) }
        };
    }
    private string letterToMorse(char c)
    {
        switch (c)
        {
            case 'A': return ".-";
            case 'B': return "-...";
            case 'C': return "-.-.";
            case 'D': return "-..";
            case 'E': return ".";
            case 'F': return "..-.";
            case 'G': return "--.";
            case 'H': return "....";
            case 'I': return "..";
            case 'J': return ".---";
            case 'K': return "-.-";
            case 'L': return ".-..";
            case 'M': return "--";
            case 'N': return "-.";
            case 'O': return "---";
            case 'P': return ".--.";
            case 'Q': return "--.-";
            case 'R': return ".-.";
            case 'S': return "...";
            case 'T': return "-";
            case 'U': return "..-";
            case 'V': return "...-";
            case 'W': return ".--";
            case 'X': return "-..-";
            case 'Y': return "-.--";
            case 'Z': return "--..";
        }
        return "";
    }
    private static List<int> AllIndexesOf(string str, string value)
    {
        List<int> indexes = new List<int>();
        for (int index = 0; ; index += value.Length)
        {
            index = str.IndexOf(value, index);
            if (index == -1)
                return indexes;
            indexes.Add(index);
        }
    }
}
