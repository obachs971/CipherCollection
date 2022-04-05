using System;
using System.Collections.Generic;
using CipherMachine;
using Words;

public class BinaryGearCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Binary Gear Cipher" : "Binary Gear Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "BG"; } }

    private readonly bool invert;
    public BinaryGearCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string[] bins = {
            "0000","0001","0010","0011","0100","0101","0110","0111",
            "1000","1001","1010","1011","1100","1101","1110","1111"
                        };
        string encrypt = "", screen2 = "";
        string kw = new Data().PickWord(4, 8);
        var kwfront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront.Value);
        char let = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Shuffle()[0];
        int[] cur = { key.IndexOf(let), 0 };
        

        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", kwfront.Expression, kwfront.Value));
        logMessages.Add(string.Format("Key: {0}", key));
        string bin = "";
        if (invert)
        {
            for (int i = 0; i < word.Length; i++)
            {
                int tempCur = key.IndexOf(word[i]);
                int iter = 0;
                do
                {
                    cur[0] = CMTools.mod(cur[0] - 1, 26);
                    cur[1] = (cur[1] + 1) % 16;
                    iter++;
                } while (cur[0] != tempCur);
                bin = bin + bins[cur[1]];
                screen2 = screen2 + "" + (iter / 16);
                logMessages.Add(string.Format("{0} -> {1}", word[i], bins[cur[1]]));
            }
            string tempBin = "";
            for (int i = 0; i < word.Length; i++)
                tempBin = tempBin + "" + bin[i] + "" + bin[word.Length + i] + "" + bin[word.Length * 2 + i] + "" + bin[word.Length * 3 + i];
            for (int i = 0; i < tempBin.Length; i += 4)
            {
                int tempCur = Array.IndexOf(bins, tempBin.Substring(i, 4));
                int iter = 0;
                do
                {
                    cur[0] = CMTools.mod(cur[0] - 1, 26);
                    cur[1] = (cur[1] + 1) % 16;
                    iter++;
                } while (cur[1] != tempCur);
                screen2 = screen2 + (iter < 10 ? ("" + UnityEngine.Random.Range(0, 2)) : "0");
                if (screen2[screen2.Length - 1] == '1')
                {
                    do
                    {
                        cur[0] = CMTools.mod(cur[0] - 1, 26);
                        cur[1] = (cur[1] + 1) % 16;
                    } while (cur[1] != tempCur);
                }
                encrypt = encrypt + "" + key[cur[0]];
                logMessages.Add(string.Format("{0} -> {1}", tempBin.Substring(i, 4), encrypt[i / 4]));
            }
            screen2 = screen2.Substring(1, word.Length);
            while (cur[1] != 0)
            {
                cur[0] = CMTools.mod(cur[0] - 1, 26);
                cur[1] = (cur[1] + 1) % 16;
            }
        }
        else
        {
            for (int i = (word.Length - 1); i >= 0; i--)
            {
                int tempCur = key.IndexOf(word[i]);
                int iter = 0;
                do
                {
                    cur[0] = (cur[0] + 1) % 26; 
                    cur[1] = CMTools.mod(cur[1] - 1, 16);
                    iter++;
                } while (cur[0] != tempCur);
                bin = bins[cur[1]] + bin;
                screen2 = screen2 + "" + (iter / 16);
                logMessages.Add(string.Format("{0} -> {1}", word[i], bins[cur[1]]));
            }
            string tempBin = "";         
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < word.Length; j++)
                    tempBin = tempBin + "" + bin[i + (j * 4)];
            }
                
            for (int i = (tempBin.Length - 4); i >= 0; i-=4)
            {
                int tempCur = Array.IndexOf(bins, tempBin.Substring(i, 4));
                int iter = 0;
                do
                {
                    cur[0] = (cur[0] + 1) % 26;
                    cur[1] = CMTools.mod(cur[1] - 1, 16);
                    iter++;
                } while (cur[1] != tempCur);
                screen2 = screen2 + (iter < 10 ? ("" + UnityEngine.Random.Range(0, 2)) : "0");
                if (screen2[screen2.Length - 1] == '1')
                {
                    do
                    {
                        cur[0] = (cur[0] + 1) % 26;
                        cur[1] = CMTools.mod(cur[1] - 1, 16);
                    } while (cur[1] != tempCur);
                }
                encrypt = key[cur[0]] + "" + encrypt;
                logMessages.Add(string.Format("{0} -> {1}", tempBin.Substring(i, 4), encrypt[0]));
            }
            char[] arr = screen2.Substring(1, word.Length).ToCharArray();
            Array.Reverse(arr);
            screen2 = new string(arr);
            while (cur[1] != 0)
            {
                cur[0] = (cur[0] + 1) % 26;
                cur[1] = CMTools.mod(cur[1] - 1, 16);
            }
        }
        
        let = key[cur[0]];
        logMessages.Add(string.Format("Screen 2: {0}", screen2));
        logMessages.Add(string.Format("Key Letter: {0}", let));

        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, kwfront.Expression, screen2, (let + "") }, invert) }
        };
    }
}
