using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Words;
using Rnd = UnityEngine.Random;
public class GridlockCipher : CipherBase
{
    public override string Name { get { return "Gridlock Cipher"; } }
    public override string Code { get { return "GL"; } }
    private int cursor = 0;
    

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ", replaceJ = "", encrypt = "";
        logMessages.Add(string.Format("Before Replacing Js: {0}", word));
        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == 'J')
            {
                word = word.Substring(0, i) + "" + alpha[Rnd.Range(0, alpha.Length)] + "" + word.Substring(i + 1);
                replaceJ = replaceJ + "" + word[i];
            }
            else
                replaceJ = replaceJ + "" + alpha.Replace(word[i].ToString(), "")[Rnd.Range(0, 24)];
        }
        logMessages.Add(string.Format("After Replacing Js: {0}", word));
        logMessages.Add(string.Format("Screen 3: {0}", replaceJ));
        string kw = new Data().PickWord(4, 8);
        var keyExp = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw.Replace("J", "I"), alpha, keyExp.Value);
        logMessages.Add(string.Format("Keyword 1: {0}", kw));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", keyExp.Expression, keyExp.Value));
        logMessages.Add(string.Format("Key: {0}", key));
        int len = Rnd.Range(0, 7) + 2;
        string numStr = "";
        for (int i = 0; i < len; i++)
            numStr += "0123456789"[Rnd.Range(0, 10)];
        bool repeat = check(numStr);
        while(repeat)
        {
            numStr = new string("0123456789".ToCharArray().Shuffle()).Substring(0, len);
            repeat = check(numStr);
        }
        logMessages.Add(string.Format("Digit String: {0}", numStr));
        List<int> digits = new List<int>();
        foreach (char d in numStr)
            digits.Add(d - '0');
        foreach (char letter in word)
            encrypt += encryptLetter(letter, key, digits, logMessages);

        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, keyExp.Expression, numStr, null, replaceJ }) },
            Score = 4
        };
    }
    private char encryptLetter(char letter, string key, List<int> digits, List<string> log)
    {
        string matrix = new string(key.ToCharArray()), letters = matrix[0] + "", numbers = "";
        bool flag = true;
        int[] rc = { 0, 0 };
        while(flag)
        {
            digits.Add((digits[cursor] + digits[cursor + 1]) % 10);
            numbers += digits[cursor];
            switch(digits[cursor])
            {
                case 8:
                    int[][] RC8 =
                    {
                        new int[]{ CMTools.mod(rc[0] - 1, 5), rc[1] },
                        new int[]{ rc[0], CMTools.mod(rc[1] + 1, 5), },
                        new int[]{ CMTools.mod(rc[0] + 1, 5), rc[1] },
                        new int[]{ rc[0], CMTools.mod(rc[1] - 1, 5), }
                    };
                    flag = false;
                    foreach(int[] pos in RC8)
                    {
                        if (matrix[pos[0] * 5 + pos[1]] != '-')
                            flag = true;
                        matrix = matrix.Replace(matrix[pos[0] * 5 + pos[1]], '-');
                    }
                    break;
                case 9:
                    int[][] RC9 =
                    {
                        new int[]{ CMTools.mod(rc[0] + 1, 5), CMTools.mod(rc[1] + 1, 5) },
                        new int[]{ CMTools.mod(rc[0] - 1, 5), CMTools.mod(rc[1] + 1, 5) },
                        new int[]{ CMTools.mod(rc[0] + 1, 5), CMTools.mod(rc[1] - 1, 5) },
                        new int[]{ CMTools.mod(rc[0] - 1, 5), CMTools.mod(rc[1] - 1, 5) }
                    };
                    flag = false;
                    foreach (int[] pos in RC9)
                    {
                        if (matrix[pos[0] * 5 + pos[1]] != '-')
                            flag = true;
                        matrix = matrix.Replace(matrix[pos[0] * 5 + pos[1]], '-');
                    }
                    break;
                default:
                    int[] newRC = getNewRC(matrix, rc, digits[cursor]);
                    if (newRC[0] == rc[0] && newRC[1] == rc[1])
                        flag = false;
                    matrix = matrix.Replace(matrix[rc[0] * 5 + rc[1]], '-');
                    rc = newRC;
                    break;
            }
            cursor++;
            letters += key[rc[0] * 5 + rc[1]];
        }
        int[] RC = { key.IndexOf(letter) / 5, key.IndexOf(letter) % 5};
        RC[0] = CMTools.mod(RC[0] - rc[0], 5);
        RC[1] = CMTools.mod(RC[1] - rc[1], 5);
        log.Add(string.Format("Starting Letter: {0}", key[RC[0] * 5 + RC[1]]));
        for(int i = 1; i < letters.Length; i++)
        {
            int[] rc1 = { key.IndexOf(letters[i - 1]) / 5, key.IndexOf(letters[i - 1]) % 5 };
            int[] rc2 = { key.IndexOf(letters[i]) / 5, key.IndexOf(letters[i]) % 5 };
            rc1[0] = CMTools.mod(rc1[0] + RC[0], 5);
            rc1[1] = CMTools.mod(rc1[1] + RC[1], 5);
            rc2[0] = CMTools.mod(rc2[0] + RC[0], 5);
            rc2[1] = CMTools.mod(rc2[1] + RC[1], 5);
            log.Add(string.Format("{0} + {1} -> {2}", key[rc1[0] * 5 + rc1[1]], numbers[i - 1], key[rc2[0] * 5 + rc2[1]]));
        }
            
        log.Add(string.Format("GRIDLOCK!"));
        return key[RC[0] * 5 + RC[1]];
    }
    private int[] getNewRC(string matrix, int[] rc, int digit)
    {
        switch(digit)
        {
            case 0: //N
                for(int i = 1; i < 5; i++)
                {
                    if (matrix[CMTools.mod(rc[0] - i, 5) * 5 + rc[1]] != '-')
                        return new int[] { CMTools.mod(rc[0] - i, 5), rc[1] };
                }
                break;
            case 1: //NE
                for (int i = 1; i < 5; i++)
                {
                    if (matrix[CMTools.mod(rc[0] - i, 5) * 5 + CMTools.mod(rc[1] + i, 5)] != '-')
                        return new int[] { CMTools.mod(rc[0] - i, 5), CMTools.mod(rc[1] + i, 5) };
                }
                break;
            case 2: //E
                for (int i = 1; i < 5; i++)
                {
                    if (matrix[rc[0] * 5 + CMTools.mod(rc[1] + i, 5)] != '-')
                        return new int[] { rc[0], CMTools.mod(rc[1] + i, 5) };
                }
                break;
            case 3: //SE
                for (int i = 1; i < 5; i++)
                {
                    if (matrix[CMTools.mod(rc[0] + i, 5) * 5 + CMTools.mod(rc[1] + i, 5)] != '-')
                        return new int[] { CMTools.mod(rc[0] + i, 5), CMTools.mod(rc[1] + i, 5) };
                }
                break;
            case 4: //S
                for (int i = 1; i < 5; i++)
                {
                    if (matrix[CMTools.mod(rc[0] + i, 5) * 5 + rc[1]] != '-')
                        return new int[] { CMTools.mod(rc[0] + i, 5), rc[1] };
                }
                break;
            case 5: //SW
                for (int i = 1; i < 5; i++)
                {
                    if (matrix[CMTools.mod(rc[0] + i, 5) * 5 + CMTools.mod(rc[1] - i, 5)] != '-')
                        return new int[] { CMTools.mod(rc[0] + i, 5), CMTools.mod(rc[1] - i, 5) };
                }
                break;
            case 6: //W
                for (int i = 1; i < 5; i++)
                {
                    if (matrix[rc[0] * 5 + CMTools.mod(rc[1] - i, 5)] != '-')
                        return new int[] { rc[0], CMTools.mod(rc[1] - i, 5) };
                }
                break;
            case 7: //NW
                for (int i = 1; i < 5; i++)
                {
                    if (matrix[CMTools.mod(rc[0] - i, 5) * 5 + CMTools.mod(rc[1] - i, 5)] != '-')
                        return new int[] { CMTools.mod(rc[0] - i, 5), CMTools.mod(rc[1] - i, 5) };
                }
                break;
        }
        return rc;
    }
    private bool check(string s)
    {
        bool flag = true;
        for (int i = 0; i < s.Length; i++)
        {
            if ("1379".IndexOf(s[i]) >= 0)
            {
                flag = false;
                break;
            }
        }
        return (flag || s.Equals("50") || s.Equals("05"));
    }

}
