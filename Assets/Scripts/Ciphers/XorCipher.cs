using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class XorCipher : CipherBase
{
    public override string Name { get { return "XOR Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "XO"; } }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        var wordList = new Data();
        int[] xor;
        string kw;
        do
        {
            kw = wordList.PickWord(word.Length);
            xor = Enumerable.Range(0, word.Length).Select(ix => (kw[ix] - 'A' + 1) ^ (word[ix] - 'A' + 1)).ToArray();
        }
        while (xor.Any(x => x < 1 || x > 26));
        var encrypted = xor.Select(i => (char) ('A' + i - 1)).Join("");

        logMessages.Add(string.Format("Keyword: {0}", kw));
        for (var i = 0; i < word.Length; i++)
            logMessages.Add(string.Format("{0} ({1}) xor {2} ({3}) = {4} ({5})", word[i], binary(word[i]), kw[i], binary(kw[i]), encrypted[i], binary(encrypted[i])));

        return new ResultInfo
        {
            Encrypted = encrypted,
            LogMessages = logMessages,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw }) }
        };
    }

    private string binary(char ltr)
    {
        var i = ltr - 'A' + 1;
        return Enumerable.Range(0, 5).Select(b => (i & (1 << (4 - b))) != 0 ? "1" : "0").Join("");
    }
}
