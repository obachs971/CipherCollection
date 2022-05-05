using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class AlphabeticalDisorderCipher : CipherBase
{
    public override string Name { get { return "Alphabetical Disorder Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "AD"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "-ABCDEFGHIJKLMNOPQRSTUVWXYZ", encrypt = "", substrings = "", log = "";
        while (true)
        {
            var p = Enumerable.Range(0, word.Length - 1).IndexOf(ix => word[ix] > word[ix + 1]);
            char[] chunk = p == -1 ? word.ToCharArray() : word.Substring(0, p + 1).ToCharArray();
            log += new string(chunk) + "|";
            if (chunk.Length > 1)
            {
                while (word.StartsWith(new string(chunk)))
                    chunk.Shuffle();
            }
            encrypt += new string(chunk);
            if (p != -1) substrings = substrings + "" + chunk.Length;
            if (p == -1)
                break;
            word = word.Substring(p + 1);
        }
        word = encrypt.ToUpperInvariant();
        encrypt = "";
        logMessages.Add(string.Format("{0}", log.Substring(0, log.Length - 1)));
        logMessages.Add(string.Format("New Order: {0}", word));
        logMessages.Add(string.Format("Screen 1: {0}", substrings));
        for (int i = word.Length - 1; i >= 1; i--)
        {
            int num = CMTools.mod(alpha.IndexOf(word[i - 1]) - alpha.IndexOf(word[i]), 26);
            if (num == 0) num = 26;
            encrypt = alpha[num] + "" + encrypt;
        }
        encrypt = word[0] + "" + encrypt;
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { substrings }) }
        };
    }
}
