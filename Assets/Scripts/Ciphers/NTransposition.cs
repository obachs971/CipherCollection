using CipherMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTransposition : CipherBase 
{
    public override string Name { get { return invert ? "Inverted Golden Retriever Transposition" : "Golden Retriever Transposition"; } }
    public override string Code { get { return "GD"; } }
    public override bool IsTransposition { get { return true; } }
    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public NTransposition(bool invert) { this.invert = invert; }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        int[][] keys = new int[4][];
        int[] possNums = new int[word.Length];
        for (int i = 0; i < word.Length; i++)
            possNums[i] = i;
        int[] bits = new int[keys.Length];
        char[] check = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(0, word.Length).ToCharArray();
        List<string> prev = new List<string>();
        prev.Add(new string(check));
        for(int i = 0; i < keys.Length; i++)
        {
            randomizeKey:
            string before = new string(check);
            bits[i] = UnityEngine.Random.Range(0, 2);
            possNums.Shuffle();
            keys[i] = new int[] { possNums[0], possNums[1], possNums[2] };
            Array.Sort(keys[i]);
            int[] key = keys[i];
            if (bits[i] == 0)
            {
                char tc = check[key[0]];
                check[key[0]] = check[key[2]];
                check[key[2]] = check[key[1]];
                check[key[1]] = tc;
            }
            else
            {
                char tc = check[key[0]];
                check[key[0]] = check[key[1]];
                check[key[1]] = check[key[2]];
                check[key[2]] = tc;
            }
            string temp = new string(check);
            if (prev.Contains(temp))
            {
                check = before.ToCharArray();
                goto randomizeKey;
            }
            prev.Add(new string(check));
        }
        //foreach(string p in prev)
        //    logMessages.Add(string.Format("{0}", p));
        char[] encrypt = word.ToCharArray();
        if (invert)
        {
            for (int z = 0; z < keys.Length; z++)
            {

                int[] key = keys[z];
                int bit = bits[z];
                string before = new string(encrypt);
                if (bit == 0)
                {
                    char tc = encrypt[key[0]];
                    encrypt[key[0]] = encrypt[key[2]];
                    encrypt[key[2]] = encrypt[key[1]];
                    encrypt[key[1]] = tc;
                }
                else
                {
                    char tc = encrypt[key[0]];
                    encrypt[key[0]] = encrypt[key[1]];
                    encrypt[key[1]] = encrypt[key[2]];
                    encrypt[key[2]] = tc;
                }
                logMessages.Add(string.Format("{0} + {1} + {2} -> {3}", before, String.Join("", new List<int>(key).ConvertAll(i => (i + 1).ToString()).ToArray()), bit, new string(encrypt)));
            }
        }
        else
        {
            for (int z = 0; z < keys.Length; z++)
            {
                int[] key = keys[keys.Length - z - 1];
                int bit = bits[bits.Length - z - 1];
                string before = new string(encrypt);
                if (bit == 0)
                {
                    char tc = encrypt[key[0]];
                    encrypt[key[0]] = encrypt[key[1]];
                    encrypt[key[1]] = encrypt[key[2]];
                    encrypt[key[2]] = tc;
                }
                else
                {
                    char tc = encrypt[key[0]];
                    encrypt[key[0]] = encrypt[key[2]];
                    encrypt[key[2]] = encrypt[key[1]];
                    encrypt[key[1]] = tc;
                }
                logMessages.Add(string.Format("{0} + {1} + {2} -> {3}", before, String.Join("", new List<int>(key).ConvertAll(i => (i + 1).ToString()).ToArray()), bit, new string(encrypt)));
            }
        }
        
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = new string(encrypt),
            Pages = new[] { new PageInfo(new ScreenInfo[] { String.Join("", new List<int>(keys[0]).ConvertAll(i => (i + 1).ToString()).ToArray()), bits[0] + "", String.Join("", new List<int>(keys[1]).ConvertAll(i => (i + 1).ToString()).ToArray()), bits[1] + "", String.Join("", new List<int>(keys[2]).ConvertAll(i => (i + 1).ToString()).ToArray()), bits[2] + "", String.Join("", new List<int>(keys[3]).ConvertAll(i => (i + 1).ToString()).ToArray()), bits[3] + "" }, invert) },
            Score = 6
        };
    }
}
