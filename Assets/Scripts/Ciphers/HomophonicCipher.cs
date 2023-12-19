using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using UnityEngine;

public class HomophonicCipher : CipherBase
{
    public override string Name { get { return "Homophonic Cipher"; } }
    public override string Code { get { return "HO"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string key = string.Concat(alpha[Random.Range(0, 26)], alpha[Random.Range(0, 26)], alpha[Random.Range(0, 26)]);
        logMessages.Add(string.Format("Screen A: {0}", key));
        int[][] nums = new int[key.Length][];
        for(int i = 0; i < key.Length; i++)
        {
            nums[i] = new int[alpha.Length];
            int start = key[i] - 'A';
            for (int j = 0; j < nums[i].Length; j++)
                nums[i][(start + j) % nums[i].Length] = (i * 26) + (j + 1);
        }
        int[] randRows = new int[word.Length];
        int[] encryptNums = new int[word.Length];
        for(int i = 0; i < nums.Length; i++)
            randRows[i] = i;
        for (int i = nums.Length; i < randRows.Length; i++)
            randRows[i] = Random.Range(0, nums.Length);
        randRows.Shuffle();
        for(int i = 0; i < word.Length; i++)
        {
            encryptNums[i] = nums[randRows[i]][word[i] - 'A'];
            logMessages.Add(string.Format("{0} -> {1}{2}", word[i], encryptNums[i] / 10, encryptNums[i] % 10));
        }
        string encrypt = "", sc1 = "", sc2 = "";
        foreach(int encryptNum in encryptNums)
        {
            encrypt = encrypt + alpha[Random.Range(0, alpha.Length)];
            sc1 = sc1 + getScreenLetter(encrypt[encrypt.Length - 1], encryptNum / 10);
            sc2 = sc2 + getScreenLetter(encrypt[encrypt.Length - 1], encryptNum % 10);
        }
        logMessages.Add(string.Format("Screen 1: {0}", sc1));
        logMessages.Add(string.Format("Screen 2: {0}", sc2));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { sc1, key, sc2 }) },
            Score = 4
        };
    }
    private char getScreenLetter(char encryptLet, int encryptNum)
    {
        int[] choices = { (encryptLet - 'A') - encryptNum, (encryptLet - 'A') + encryptNum };
        choices.Shuffle();
        foreach (int choice in choices)
        {
            if (choice >= 0 && choice <= 25)
                return "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[choice];
        }
        return '-';
    }
}
