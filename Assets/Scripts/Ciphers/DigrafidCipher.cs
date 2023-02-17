using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class DigrafidCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Digrafid Cipher" : "Digrafid Cipher"; } }
    public override string Code { get { return "DI"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public DigrafidCipher(bool invert) { this.invert = invert; }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string[] kws = new string[2], keys = new string[2], choices = { "ABCDEFGHIJKLMNOPQRSTUVWXYZ#", "ABCDEFGHIJKLMNOPQRSTUVWXYZ#" };
        var kwFront = new ValueExpression<bool>[2];
        var wordList = new Data();
        for (int i = 0; i < kws.Length; i++)
        {
            kws[i] = wordList.PickWord(4, 8);
            kwFront[i] = CMTools.generateBoolExp(bomb);
            keys[i] = CMTools.getKey(kws[i], "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwFront[i].Value);
        }
        string encrypt, letters = choices[0][Random.Range(0, choices[0].Length)] + "", key = "123456789", screenC = "";
        choices[0] = choices[0].Replace(letters, "");
        logMessages.Add(string.Format("KW1: {0}", kws[0]));
        logMessages.Add(string.Format("KW2: {0}", kws[1]));
        int len = word.Length;
        if (len % 2 == 1)
        {
            int num = Random.Range(0, word.Length + 1);
            word = word.Substring(0, num) + "#" + word.Substring(num);
        }
        if (invert)
        {
        tryagain:
            encrypt = "";
            string nums = "";
            letters = letters[0] + "" + choices[1][Random.Range(0, choices[1].Length)];
            choices[1] = choices[1].Replace(letters[1] + "", "");
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
                nums = nums + "" + key[n1 % 9] + "" + key[((n1 / 9) * 3) + (n2 / 9)] + "" + key[n2 % 9];
            }
            for (int i = 0; i < nums.Length / 3; i++)
            {
                encrypt = encrypt + "" + tempKey[0][key.IndexOf(nums[i]) + ((key.IndexOf(nums[i + word.Length / 2]) / 3) * 9)];
                encrypt = encrypt + "" + tempKey[1][key.IndexOf(nums[i + word.Length]) + ((key.IndexOf(nums[i + word.Length / 2]) % 3) * 9)];
            }
            if (encrypt.Substring(0, len).Contains("#"))
            {
                if (choices[1].Length == 0)
                {
                    choices[1] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ#";
                    letters = choices[0][Random.Range(0, choices[0].Length)] + "";
                    choices[0] = choices[0].Replace(letters, "");
                }
                goto tryagain;
            }
            logMessages.Add(string.Format("Letters: {0}", letters));
            logMessages.Add(string.Format("Key A: {0} -> {1} -> {2}", kwFront[0].Expression, kwFront[0].Value, tempKey[0]));
            logMessages.Add(string.Format("Key B: {0} -> {1} -> {2}", kwFront[1].Expression, kwFront[1].Value, tempKey[1]));
            for (int i = 0; i < word.Length; i += 2)
                logMessages.Add(string.Format("{0}{1} -> {2}", word[i], word[i + 1], nums.Substring(i / 2 * 3, 3)));
            for (int i = 0; i < encrypt.Length; i += 2)
                logMessages.Add(string.Format("{0}{1}{2} -> {3}{4}", nums[i / 2], nums[i / 2 + word.Length / 2], nums[i / 2 + word.Length], encrypt[i], encrypt[i + 1]));
        }
        else
        {
        tryagain:
            encrypt = "";
            string[] nums = new string[3] { "", "", "" };
            letters = letters[0] + "" + choices[1][Random.Range(0, choices[1].Length)];
            choices[1] = choices[1].Replace(letters[1] + "", "");
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
            if (encrypt.Substring(0, len).Contains("#"))
            {
                if (choices[1].Length == 0)
                {
                    choices[1] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ#";
                    letters = choices[0][Random.Range(0, choices[0].Length)] + "";
                    choices[0] = choices[0].Replace(letters, "");
                }
                goto tryagain;
            }
            logMessages.Add(string.Format("Letters: {0}", letters));
            logMessages.Add(string.Format("Key A: {0} -> {1} -> {2}", kwFront[0].Expression, kwFront[0].Value, tempKey[0]));
            logMessages.Add(string.Format("Key B: {0} -> {1} -> {2}", kwFront[1].Expression, kwFront[1].Value, tempKey[1]));
            for (int i = 0; i < word.Length; i += 2)
                logMessages.Add(string.Format("{0}{1} -> {2}{3}{4}", word[i], word[i + 1], nums[0][i / 2], nums[1][i / 2], nums[2][i / 2]));
            for (int i = 0; i < encrypt.Length; i += 2)
                logMessages.Add(string.Format("{0} -> {1}{2}", temp.Substring((i / 2) * 3, 3), encrypt[i], encrypt[i + 1]));
        }
        if (len % 2 == 1)
        {
            screenC = encrypt.Substring(len);
            encrypt = encrypt.Substring(0, len);
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kws[0], kwFront[0].Expression, kws[1], kwFront[1].Expression, letters, screenC }, invert) },
            Score = 7
        };
    }

}
