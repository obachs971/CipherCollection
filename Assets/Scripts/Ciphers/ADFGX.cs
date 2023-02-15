using System.Collections.Generic;
using CipherMachine;
using Words;
using UnityEngine;

public class ADFGXCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted ADFGX Cipher" : "ADFGX Cipher"; } }
    public override int Score(int wordLength) { return 5; }
    public override string Code { get { return "AX"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public ADFGXCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ", ADFGX = "ADFGX", replaceJ = "", med = "", encrypt = "";
        logMessages.Add(string.Format("Before Replacing Js: {0}", word));
        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == 'J')
            {
                word = word.Substring(0, i) + "" + alpha[Random.Range(0, alpha.Length)] + "" + word.Substring(i + 1);
                replaceJ = replaceJ + "" + word[i];
            }
            else
                replaceJ = replaceJ + "" + alpha.Replace(word[i].ToString(), "")[Random.Range(0, 24)];
        }
        logMessages.Add(string.Format("After Replacing Js: {0}", word));
        logMessages.Add(string.Format("Screen 3: {0}", replaceJ));
        string kw = new Data().PickWord(4, 8);
        var kwfront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw.Replace("J", "I"), alpha, kwfront.Value);
        logMessages.Add(string.Format("Keyword 1: {0}", kw));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", kwfront.Expression, kwfront.Value));
        logMessages.Add(string.Format("Key: {0}", key));
        string kw2 = new Data().PickWord(3, 8);
        char[] temp = kw2.ToCharArray();
        System.Array.Sort(temp);
        string order = new string(temp);
        for(int i = 0; i < order.Length; i++)
        {
            for(int j = i + 1; j < order.Length; j++)
            {
                if (order[i] == order[j])
                {
                    order = order.Substring(0, j) + order.Substring(j + 1);
                    j--;
                } 
            }
        }
        int[] keynum = new int[kw2.Length];
        int cur = 1;
        for(int i = 0; i < order.Length; i++)
        {
            for(int j = 0; j < kw2.Length; j++)
            {
                if (order[i] == kw2[j])
                    keynum[j] = cur++;
            }
        }
        foreach (char c in word)
            med = med + "" + ADFGX[key.IndexOf(c) / 5] + "" + ADFGX[key.IndexOf(c) % 5];
        logMessages.Add(string.Format("{0} -> {1}", word, med));
        while (med.Length % keynum.Length != 0)
            med += "-";
        char[][] grid = new char[med.Length / keynum.Length][];
        for (int i = 0; i < grid.Length; i++)
            grid[i] = new char[keynum.Length];
        logMessages.Add(string.Format("Keyword 2: {0}", kw2));
        logMessages.Add(string.Format("Key Number: {0}", System.String.Join("", new List<int>(keynum).ConvertAll(i => i.ToString()).ToArray())));
        if (invert)
        {
            int bot = med.Length / keynum.Length - 1;
            med = med.Replace("-", "");
            cur = 0; 
            int mod = med.Length % keynum.Length;
            for (int i = 1; i <= keynum.Length; i++)
            {
                for (int j = 0; j < grid.Length; j++)
                {
                    if (j == bot && mod > 0 && System.Array.IndexOf(keynum, i) >= mod)
                        grid[j][System.Array.IndexOf(keynum, i)] = '-';
                    else
                        grid[j][System.Array.IndexOf(keynum, i)] = med[cur++];
                }
            }
            med = "";
            for (int i = 0; i < grid.Length; i++)
            {
                logMessages.Add(new string(grid[i]));
                med += new string(grid[i]);
            }
        }
        else
        {
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = med.Substring(i * keynum.Length, keynum.Length).ToCharArray();
                logMessages.Add(new string(grid[i]));
            }
            med = "";
            for (int i = 1; i <= keynum.Length; i++)
            {
                for (int j = 0; j < grid.Length; j++)
                    med = med + "" + grid[j][System.Array.IndexOf(keynum, i)];
            }
        }
        med = med.Replace("-", "");
        for (int i = 0; i < med.Length; i += 2)
            encrypt = encrypt + "" + key[ADFGX.IndexOf(med[i]) * 5 + ADFGX.IndexOf(med[i + 1])];
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, kwfront.Expression, kw2, null, replaceJ }, invert) }
        };
    }
}
