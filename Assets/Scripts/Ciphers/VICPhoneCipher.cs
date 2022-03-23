using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class VICPhoneCipher : CipherBase
{
    public override string Name { get { return "VIC Phone Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "VP"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string kw = new Data().PickWord(4, 8);
        string[] kwfront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront[1][0] == 'T');
        string rows = new string("0123456789".ToCharArray().Shuffle()).Substring(0, 4);
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Screen B: {0}", rows));
        for (int i = 0; i < 10; i++)
        {
            if (rows.Contains(i + ""))
                key = key.Substring(0, i) + "-" + key.Substring(i);
        }
        logMessages.Add(string.Format("Key: {0} -> {1} -> {2}", kwfront[0], kwfront[1], key));
        List<int> encryptNums = new List<int>();
        foreach (char let in word)
        {
            int index = key.IndexOf(let);
            logMessages.Add(index.ToString());
            if (index < 10)
                encryptNums.Add(index % 10);
            else
            {
                int n = ((index / 10) * 2) - UnityEngine.Random.Range(1, 3);
                encryptNums.Add("0123456789".IndexOf(rows[n]));
                encryptNums.Add(index % 10);
            }
        }
        logMessages.Add(string.Format("{0} -> {1}", word, string.Join("", encryptNums.ConvertAll(i => i.ToString()).ToArray())));
        string numKey = new string("0123456789".ToCharArray().Shuffle()).Substring(0, (encryptNums.Count / 2));
        logMessages.Add(string.Format("Number Key: {0}", numKey));
        for (int i = 0; i < encryptNums.Count; i++)
            encryptNums[i] = (encryptNums[i] + (numKey[i % numKey.Length] - '0')) % 10;
        string[] replace = {
            "111", "222", "333", "444", "555", "666",
            "11", "22", "33", "44", "55", "66", "77", "88", "99", "00",
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
        };
        string alpha = "CFILORBEHKNQTVXZADGJMPSUWY", encrypt = string.Join("", encryptNums.ConvertAll(i => i.ToString()).ToArray());
        for (int i = 0; i < replace.Length; i++)
            encrypt = encrypt.Replace(replace[i], alpha[i] + "");
        while (encrypt.Length < word.Length)
            encrypt = enlarge(encrypt);
        logMessages.Add(string.Format("{0} -> {1}", string.Join("", encryptNums.ConvertAll(i => i.ToString()).ToArray()), encrypt));
        string extra = encrypt.Substring(word.Length);
        encrypt = encrypt.Substring(0, word.Length);
        logMessages.Add(string.Format("Screen 3: {0}", extra));
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));

        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
        screens[1] = new ScreenInfo(kwfront[0], 25);
        screens[2] = new ScreenInfo(numKey, (numKey.Length == 8) ? 28 : (numKey.Length == 7) ? 32 : 35);
        screens[3] = new ScreenInfo(rows, 20);
        screens[4] = new ScreenInfo(extra, extra.Length == 8 ? 28 : extra.Length == 7 ? 32 : 35);
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens) }
        };
    }
    private string enlarge(string encrypt)
    {
        string[] replace = {
            "111", "222", "333", "444", "555", "666",
            "11", "22", "33", "44", "55", "66", "77", "88", "99", "00",
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
        };
        string alpha = "CFILORBEHKNQTVXZ";
        List<int> indexes = new List<int>();
        for (int i = 0; i < encrypt.Length; i++)
        {
            if (alpha.Contains(encrypt[i]))
                indexes.Add(i);
        }
        int index = indexes[UnityEngine.Random.Range(0, indexes.Count)];
        alpha += "ADGJMPSUWY";
        if (alpha.IndexOf(encrypt[index]) < 6)
        {
            char c = replace[alpha.IndexOf(encrypt[index])][0];
            string s = alpha[Array.IndexOf(replace, c + "" + c)] + "" + alpha[Array.IndexOf(replace, c + "")];
            encrypt = encrypt.Substring(0, index) + new string(s.ToCharArray().Shuffle()) + encrypt.Substring(index + 1);
        }
        else
        {
            char c = replace[alpha.IndexOf(encrypt[index])][0];
            string s = alpha[Array.IndexOf(replace, c + "")] + "" + alpha[Array.IndexOf(replace, c + "")];
            encrypt = encrypt.Substring(0, index) + s + encrypt.Substring(index + 1);
        }
        return encrypt;
    }
}