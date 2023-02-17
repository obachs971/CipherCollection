using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using KModkit;
using Words;

public class PingPongStraddlingCheckerboardCipher : CipherBase
{
    public override string Name { get { return "Ping-Pong Straddling Checkerboard Cipher"; } }
    public override string Code { get { return "PP"; } }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var wordList = new Data();

    tryAgain:
        var kw1 = wordList.PickWord(4, 8);
        var kw2 = wordList.PickWord(4, 8);

        var d1E = CMTools.generateValue(bomb);
        var d2E = CMTools.generateValue(bomb);
        var d1 = d1E.Value % 6;
        var d2 = d2E.Value % 6;
        if (d2 == d1)
            d2 = (d2 + 1) % 6;

        var d3E = CMTools.generateValue(bomb);
        var d4E = CMTools.generateValue(bomb);
        var d3 = d3E.Value % 6;
        var d4 = d4E.Value % 6;
        if (d4 == d3)
            d4 = (d4 + 1) % 6;

        var fwKwFront = CMTools.generateBoolExp(bomb);
        var fwColumnOrder = CMTools.generateBoolExp(bomb);
        var bkKwFront = CMTools.generateBoolExp(bomb);
        var bkColumnOrder = CMTools.generateBoolExp(bomb);

        // Backward Straddling Checkerboard Cipher
        var rowDigits2 = Enumerable.Range(0, 6).Where(d => d != d3 && d != d4).ToArray();
        var straddlingCheckerboard2 = MakeStraddlingCheckerboard(bkKwFront.Value, bkColumnOrder.Value, kw2, rowDigits2);

        var encryptedDigits = new List<int>();
        foreach (var ch in word)
        {
            var ix = straddlingCheckerboard2.IndexOf(ch);
            if (ix >= 6)
                encryptedDigits.Add(rowDigits2[ix / 6 - 1]);
            encryptedDigits.Add(ix % 6);
        }

        // Forward Straddling Checkerboard Cipher
        var rowDigits1 = Enumerable.Range(0, 6).Where(d => d != d1 && d != d2).ToArray();
        var straddlingCheckerboard1 = MakeStraddlingCheckerboard(fwKwFront.Value, fwColumnOrder.Value, kw1, rowDigits1);

        var encrypted = "";
        for (var i = 0; i < encryptedDigits.Count; i++)
        {
            if (encryptedDigits[i] == d1 || encryptedDigits[i] == d2)
                encrypted += straddlingCheckerboard1[encryptedDigits[i]];
            else
            {
                if (i == encryptedDigits.Count - 1)
                    encryptedDigits.Add(rowDigits2.Where(d => d != d1 && d != d2).First());
                encrypted += straddlingCheckerboard1[(Array.IndexOf(rowDigits1, encryptedDigits[i]) + 1) * 6 + encryptedDigits[i + 1]];
                i++;
            }
        }

        if (encrypted.Length != word.Length)
            goto tryAgain;

        var logMessages = new List<string>();
        logMessages.Add(string.Format("Backward Straddling Checkerboard Cipher: KW2: {0}, D3: {1} -> {2}, D4: {3} -> {4}, {5}/{6} -> {7}/{8}", kw2, d3E.Expression, d3, d4E.Expression, d4, bkKwFront.Expression, bkColumnOrder.Expression, bkKwFront.Value, bkColumnOrder.Value));
        for (var i = 0; i < 5; i++)
            logMessages.Add(string.Format("Backward Straddling Checkerboard Cipher: Row [{0}] = [{1}]", i == 0 ? " " : rowDigits2[i - 1].ToString(), straddlingCheckerboard2.Substring(6 * i, 6).Join(" ")));
        logMessages.Add(string.Format("Backward Straddling Checkerboard result: {0}", encryptedDigits.Join("")));
        logMessages.Add(string.Format("Forward Straddling Checkerboard Cipher: KW1: {0}, D1: {1} -> {2}, D2: {3} -> {4}, {5}/{6} -> {7}/{8}", kw1, d1E.Expression, d1, d2E.Expression, d2, fwKwFront.Expression, fwColumnOrder.Expression, fwKwFront.Value, fwColumnOrder.Value));
        for (var i = 0; i < 5; i++)
            logMessages.Add(string.Format("Forward Straddling Checkerboard Cipher: Row [{0}] = [{1}]", i == 0 ? " " : rowDigits1[i - 1].ToString(), straddlingCheckerboard1.Substring(6 * i, 6).Join(" ")));
        logMessages.Add(string.Format("Forward Straddling Checkerboard result: {0}", encrypted));

        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypted,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw1, d1E.Expression, kw2, d2E.Expression, fwKwFront.Expression + "/" + fwColumnOrder.Expression, d3E.Expression, bkKwFront.Expression + "/" + bkColumnOrder.Expression, d4E.Expression }) },
            Score = 8
        };
    }

    private static string MakeStraddlingCheckerboard(bool keywordFirst, bool inColumns, string kw, int[] rowDigits)
    {
        var alphabet = (keywordFirst ? (kw + "ABCDEFGHIJKLMNOPQRSTUVWXYZ") : "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Except(kw).Concat(kw)).Distinct().Join("");
        for (var i = 0; i < 6; i++)
            if (rowDigits.Contains(i))
                alphabet = alphabet.Insert(inColumns ? 5 * i : i, ".");
        if (inColumns)
            alphabet = Enumerable.Range(0, 30).Select(i => alphabet[(i / 6) + 5 * (i % 6)]).Join("");
        return alphabet;
    }
}
