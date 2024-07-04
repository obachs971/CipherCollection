using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class VCipher : CipherBase 
{
    public override string Name { get { return invert ? "Inverted Bubble Cipher" : "Bubble Cipher"; } }
    public override string Code { get { return "BU"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public VCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        var wordNoZ = word.Select(ch => ch == 'Z' ? randomLetter() : ch).Join("");
        logMessages.Add(string.Format("Before replacing Zs: {0}", word));
        logMessages.Add(string.Format("After replacing Zs: {0}", wordNoZ));
        var replaceZ = word.Select((ch, ix) => word[ix] == 'Z' ? wordNoZ[ix] : randomLetter(except: wordNoZ[ix])).Join("");
        logMessages.Add(string.Format("String for replacing Zs: {0}", replaceZ));

        var wordList = new Data();
        string kw1 = wordList.PickWord(4, 8);
        string kw2 = wordList.PickWord(3, word.Length);
        var kwExpr = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw1.Replace("Z", "X"), "ABCDEFGHIJKLMNOPQRSTUVWXY", kwExpr.Value);
        logMessages.Add(string.Format("Key ({0}; {1}={2}): {3}", kw1, kwExpr.Expression, kwExpr.Value, key));

        logMessages.Add(string.Format("KW2: {0}", kw2));
        string kw2NoZ = kw2.Replace("Z", "X");
        string encrypt = "";
        for(int i = 0; i < wordNoZ.Length; i++)
        {
            key = shiftKey(key, kw2NoZ[i % kw2NoZ.Length]);
            logMessages.Add(string.Format("{0} → {1}", kw2NoZ[i % kw2NoZ.Length], key));
            encrypt += getEncryptedLetter(key, wordNoZ[i]);
            logMessages.Add(string.Format("{0} → {1}", wordNoZ[i], encrypt[i]));
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw1, kwExpr.Expression, kw2, null, replaceZ }, invert) },
            Score = 6
        };
    }
    private static char randomLetter(char? except = null)
    {
        return (except == null ? "ABCDEFGHIJKLMNOPQRSTUVWXY" : "ABCDEFGHIJKLMNOPQRSTUVWXY".Except(new[] { except.Value })).PickRandom();
    }
    private string shiftKey(string key, char let)
    {
        var row = key.IndexOf(let) / 5;
        var col = key.IndexOf(let) % 5;
        for (var i = row; i < 2; i++)
            key = key.Substring(20) + key.Substring(0, 20);
        for (var i = row; i > 2; i--)
            key = key.Substring(5) + key.Substring(0, 5);
        for (var i = col; i < 2; i++)
            key = key[4] + key.Substring(0, 4) + key[9] + key.Substring(5, 4) + key[14] + key.Substring(10, 4) + key[19] + key.Substring(15, 4) + key[24] + key.Substring(20, 4);
        for (var i = col; i > 2; i--)
            key = key.Substring(1, 4) + key[0] + key.Substring(6, 4) + key[5] + key.Substring(11, 4) + key[10] + key.Substring(16, 4) + key[15] + key.Substring(21, 4) + key[20];
        return key;
    }
    private char getEncryptedLetter(string key, char let)
    {
        var row = key.IndexOf(let) / 5;
        var col = key.IndexOf(let) % 5;
        
        if(invert)
        {
            var newRow = 4 - col;
            var newCol = row;
            return key[newRow * 5 + newCol];
        }
        else
        {
            var newRow = col;
            var newCol = 4 - row;
            return key[newRow * 5 + newCol];
        }
    }
}
