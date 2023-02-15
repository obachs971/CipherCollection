using System.Collections.Generic;
using CipherMachine;
using Words;
using UnityEngine;
public class LorenzCipher : CipherBase
{
    public override string Name { get { return "Lorenz Cipher"; } }
    public override int Score(int wordLength) { return 10; }
    public override string Code { get { return "LZ"; } }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        List<string> binalpha = new List<string> {
            "00000","00001","00010","00011","00100",
            "00101","00110","00111","01000","01001",
            "01010","01011","01100","01101","01110",
            "01111","10000","10001","10010","10011",
            "10100","10101","10110","10111","11000",
            "11001"
        };
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", rotorLets = "", bin = "", encrypt = "";
        string[][] key = generatePins(logMessages, word.Length);
        for (int i = 0; i < 5; i++)
        {
            rotorLets = rotorLets + "" + alpha[UnityEngine.Random.Range(0, key[1][i].Length)];
            key[1][i] = key[1][i].Substring(alpha.IndexOf(rotorLets[i])) + key[1][i].Substring(0, alpha.IndexOf(rotorLets[i]));
        }
        logMessages.Add(string.Format("Rotor Letter Config: {0}", rotorLets));
        string rotorShifter = "0101010".Substring(0, word.Length - 1);
        if (rotorShifter.Length % 2 == 1 && Random.Range(0, 2) == 0)
            rotorShifter = rotorShifter.Substring(1) + "1";
        rotorShifter = new string(rotorShifter.ToCharArray().Shuffle());
        logMessages.Add(string.Format("Key Shifting Rotor: {0}", rotorLets));
        var boolExp = CMTools.generateBoolExp(bomb);
        logMessages.Add(string.Format("Boolean Expression: {0} -> {1} -> Using {2} Gate", boolExp.Expression, boolExp.Value, boolExp.Value ? "XOR" : "XNOR"));
        for (int i = 0; i < word.Length; i++)
        {
            string[] result = getResult(binalpha[alpha.IndexOf(word[i])], key[1][0][0] + "" + key[1][1][0] + "" + key[1][2][0] + "" + key[1][3][0] + "" + key[1][4][0], boolExp.Value);
            encrypt += result[0];
            bin += result[1];
            if(rotorShifter[i % rotorShifter.Length] == '1')
            {
                for(int j = 0; j < 5; j++)
                    key[1][j] = key[1][j].Substring(1) + "" + key[1][j][0];
            }
            logMessages.Add(string.Format("{0} + {1}{2}{3}{4}{5} + {6} -> {7}", word[i], key[1][0][0], key[1][1][0], key[1][2][0], key[1][3][0], key[1][4][0], bin[i], encrypt[i]));
        }
        //logMessages.Add(string.Format("Keyword: {0}", kw));



        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { 
                new PageInfo(new ScreenInfo[] { key[0][0],  rotorLets[0] + "", key[0][1], rotorLets[1] + "", key[0][2], rotorLets[2] + "", key[0][3], rotorLets[3] + "" }),
                new PageInfo(new ScreenInfo[] { key[0][4],  rotorLets[4] + "", rotorShifter, boolExp.Expression, bin}),  
            }
        };
    }
    private string[][] generatePins(List<string> logMessages, int length)
    {
        string alpha = "ABCDEFGHIJKLMNOP";
        string[] b = { "", "" };
        for (int i = 0; i < (length / 2 + 1); i++)
        {
            b[0] += "0";
            b[1] += "1";
        }
        string[] bins =
        {
            "1111111100000000",
            "111111000000" + UnityEngine.Random.Range(0, 2),
            "1111100000" + UnityEngine.Random.Range(0, 2),
            "11110000" + UnityEngine.Random.Range(0, 2),
            "111000" + UnityEngine.Random.Range(0, 2)
        };
        string[] pins = { "", "", "", "", "", "" };
        for (int i = 0; i < bins.Length; i++)
        {
            do bins[i] = new string(bins[i].ToCharArray().Shuffle());
            while ((bins[i] + bins[i]).IndexOf(b[0]) >= 0 || (bins[i] + bins[i]).IndexOf(b[1]) >= 0);
            for (int j = 0; j < bins[i].Length; j++)
            {
                if (bins[i][j] == '1')
                    pins[i] = pins[i] + "" + alpha[j];
            }
            logMessages.Add(string.Format("Active Pins for Rotor #{0}: {1}", (i + 1), pins[i]));
        }
        return new string[][] { pins, bins };
    }
    private string[] getResult(string b1, string b2, bool xor)
    {
        List<string> binalpha = new List<string> {
            "00000","00001","00010","00011","00100",
            "00101","00110","00111","01000","01001",
            "01010","01011","01100","01101","01110",
            "01111","10000","10001","10010","10011",
            "10100","10101","10110","10111","11000",
            "11001"
        };
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string bins = "";
        if(xor)
        {
            for(int i = 0; i < 5; i++)
                bins += b1[i] != b2[i] ? "1" : "0";
        }
        else
        {
            for (int i = 0; i < 5; i++)
                bins += b1[i] == b2[i] ? "1" : "0";
        }
        if(binalpha.IndexOf(bins) < 0)
            return new string[] { alpha[binalpha.IndexOf(bins.Replace("0", "*").Replace("1", "0").Replace("*", "1"))] + "", "1" };
        else
        {
            string temp = bins.Replace("0", "*").Replace("1", "0").Replace("*", "1");
            if (binalpha.IndexOf(temp) >= 0 && Random.Range(0, 3) == 0)
                return new string[] { alpha[binalpha.IndexOf(temp)] + "", "1" };
            return new string[] { alpha[binalpha.IndexOf(bins)] + "", "0" };
        }
    }
}
