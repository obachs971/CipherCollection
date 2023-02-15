using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class GROMARKCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted GROMARK Cipher" : "GROMARK Cipher"; } }
    public override int Score(int wordLength) { return 7; }
    public override string Code { get { return "GR"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public GROMARKCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string kw = new Data().PickWord(4, 8);
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", encrypt = "";
        var kwfront = CMTools.generateBoolExp(bomb);
        int[] key = new int[kw.Length];
        char[] order = kw.ToArray();
        Array.Sort(order);
        for (int i = 0; i < order.Length; i++)
        {
            for (int j = 0; j < kw.Length; j++)
            {
                if (order[i] == kw[j] && key[j] == 0)
                {
                    key[j] = i + 1;
                    break;
                }
            }
        }
        string temp = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront.Value);
        while (temp.Length % kw.Length > 0)
            temp += "-";
        string alphakey = "";
        for (int i = 1; i <= kw.Length; i++)
        {
            int cur = Array.IndexOf(key, i);
            for (int j = 0; j < (temp.Length / kw.Length); j++)
                alphakey = alphakey + "" + temp[(j * kw.Length) + cur];
        }
        alphakey = alphakey.Replace("-", "");
        string numkey = new string("123456789".ToCharArray().Shuffle()).Substring(0, 2 + UnityEngine.Random.Range(0, word.Length - 2));
        bool repeat = check(numkey);
        while (repeat)
        {
            numkey = new string("123456789".ToCharArray().Shuffle()).Substring(0, 2 + UnityEngine.Random.Range(0, word.Length - 2));
            repeat = check(numkey);
        }
        var len = numkey.Length;
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Columns: {0}", string.Join("", key.Select(x => x + "").ToArray())));
        logMessages.Add(string.Format("Alphabet Key: {0} -> {1} -> {2}", kwfront.Expression, kwfront.Value, alphakey));
        logMessages.Add(string.Format("Number Key: {0}", numkey));
        if (invert)
        {
            for (int i = 0; i < word.Length; i++)
            {
                encrypt = encrypt + "" + alpha[CMTools.mod(alphakey.IndexOf(word[i]) - (numkey[i] - '0'), 26)];
                int n = ((numkey[i] - '0') + (numkey[i + 1] - '0')) % 10;
                numkey = numkey + "" + n;
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                encrypt = encrypt + "" + alphakey[CMTools.mod(alpha.IndexOf(word[i]) + (numkey[i] - '0'), 26)];
                int n = ((numkey[i] - '0') + (numkey[i + 1] - '0')) % 10;
                numkey = numkey + "" + n;
            }
        }
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, kwfront.Expression, numkey.Substring(0, len) }, invert) }
        };
    }
    private bool check(string s)
    {
        for (int i = 0; i < s.Length; i++)
        {
            if ("13579".IndexOf(s[i]) >= 0)
                return false;
        }
        return true;
    }
}
