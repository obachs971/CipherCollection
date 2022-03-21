using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class DigrafidCipher
{
    public ResultInfo encrypt(string word, string id, string log, KMBombInfo Bomb)
    {
        Debug.LogFormat("{0} Begin Digrafid Cipher", log);
        string[] kws = new string[2], keys = new string[2], choices = { "ABCDEFGHIJKLMNOPQRSTUVWXYZ#", "ABCDEFGHIJKLMNOPQRSTUVWXYZ#" }, nums;
        string[][] kwFront = new string[2][];
        var wordList = new Data();
        for (int i = 0; i < kws.Length; i++)
        {
            kws[i] = wordList.PickWord(4, 8);
            kwFront[i] = CMTools.generateBoolExp(Bomb);
            keys[i] = CMTools.getKey(kws[i], "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwFront[i][1][0] == 'T');
        }
        string encrypt, letters = choices[0][Random.Range(0, choices[0].Length)] + "", key = "123456789";
        choices[0] = choices[0].Replace(letters, "");
        Debug.LogFormat("{0} [Digrafid Cipher] KW1: {1}", log, kws[0]);
        Debug.LogFormat("{0} [Digrafid Cipher] KW2: {1}", log, kws[1]);
        while (true)
        {
            encrypt = "";
            nums = new string[3] { "", "", "" };
            letters = letters[0] + "" + choices[1][Random.Range(0, choices[1].Length)];
            choices[1] = choices[1].Replace(letters.Substring(1), "");
            string[] tempKey = { keys[0].ToUpperInvariant(), keys[1].ToUpperInvariant() };
            for (int i = 0; i < 2; i++)
            {
                if (letters[i] == '#')
                    tempKey[i] += "#";
                else
                    tempKey[i] = tempKey[i].Replace(letters[i], '#') + "" + letters[i];
            }
            for (int i = 0; i < word.Length - (word.Length % 2); i += 2)
            {
                int n1 = tempKey[0].IndexOf(word[i]), n2 = tempKey[1].IndexOf(word[i + 1]);
                nums[0] = nums[0] + "" + key[n1 % 9];
                nums[1] = nums[1] + "" + key[((n1 / 9) * 3) + (n2 / 9)];
                nums[2] = nums[2] + "" + key[n2 % 9];
            }
            string temp = nums[0] + nums[1] + nums[2];
            for (int i = 0; i < temp.Length; i += 3)
            {
                encrypt = encrypt + "" + tempKey[0][key.IndexOf(temp[i]) + ((key.IndexOf(temp[i + 1]) / 3) * 9)];
                encrypt = encrypt + "" + tempKey[1][key.IndexOf(temp[i + 2]) + ((key.IndexOf(temp[i + 1]) % 3) * 9)];
            }
            if (!(encrypt.Contains("#")))
            {
                Debug.LogFormat("{0} [Digrafid Cipher] Letters: {1}", log, letters);
                Debug.LogFormat("{0} [Digrafid Cipher] Key A: {1} -> {2} -> {3}", log, kwFront[0][0], kwFront[0][1], tempKey[0]);
                Debug.LogFormat("{0} [Digrafid Cipher] Key B: {1} -> {2} -> {3}", log, kwFront[1][0], kwFront[1][1], tempKey[1]);
                for (int i = 0; i < word.Length - (word.Length % 2); i += 2)
                    Debug.LogFormat("{0} [Digrafid Cipher] {1}{2} -> {3}{4}{5}", log, word[i], word[i + 1], nums[0][i / 2], nums[1][i / 2], nums[2][i / 2]);
                for (int i = 0; i < encrypt.Length; i += 2)
                    Debug.LogFormat("{0} [Digrafid Cipher] {1} -> {2}{3}", log, temp.Substring((i / 2) * 3, 3), encrypt[i], encrypt[i + 1]);
                break;
            }
            if (choices[1].Length == 0)
            {
                choices[1] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ#";
                letters = choices[0][Random.Range(0, choices[0].Length)] + "";
                choices[0] = choices[0].Replace(letters, "");
            }
        }
        if (word.Length % 2 == 1)
            encrypt = encrypt + "" + word[word.Length - 1];
        Debug.LogFormat("{0} [Digrafid Cipher] {1} - > {2}", log, word, encrypt);
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(kws[0], new int[] { 35, 35, 35, 32, 28 }[kws[0].Length - 4]);
        screens[1] = new ScreenInfo(kwFront[0][0], 25);
        screens[2] = new ScreenInfo(kws[1], new int[] { 35, 35, 35, 32, 28 }[kws[1].Length - 4]);
        screens[3] = new ScreenInfo(kwFront[1][0], 25);
        screens[4] = new ScreenInfo(letters, 35);
        screens[8] = new ScreenInfo(id, 35);
        return new ResultInfo
        {
            Encrypted = encrypt,
            Score = 5,
            Pages = new PageInfo[] { new PageInfo(screens) }
        };
    }

}