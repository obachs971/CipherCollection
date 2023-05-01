using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Rnd = UnityEngine.Random;
public class LogicalTernaryManipulationCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Logical Ternary Manipulation Cipher" : "Logical Ternary Manipulation Cipher"; } }
    public override string Code { get { return "LT"; } }


    private readonly bool invert;
    public override bool IsInvert { get { return invert;} }
    public LogicalTernaryManipulationCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        var TBList = new List<string>();
        var encrypt = "";
        string TA = "";
        string TB = "";
        string alpha = "-ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        foreach(char let in word)
        {
            var val = (let - 'A') + 1;
            TA += (val / 9);
            val %= 9;
            TA += (val / 3);
            TA += (val % 3);
            logMessages.Add(string.Format("{0} -> {1}{2}{3}", let, TA[TA.Length - 3], TA[TA.Length - 2], TA[TA.Length - 1]));
        }
        logMessages.Add(string.Format("TA: {0}", TA));
        
        var logTrys = new List<string>();
        if (invert)
        {
            tryagain:
            logTrys = new List<string>();
            TB = generateTB(TBList, TA.Length);
            TBList.Add(TB + "");
            string TC = "";
            encrypt = "";
            for (int i = 0; i < TA.Length; i++)
                TC += CMTools.mod((TA[i] - '0') - (TB[i] - '0'), 3);
            logTrys.Add(string.Format("TB: {0}", TB));
            logTrys.Add(string.Format("Resulting Ternary: {0}", TC));
            string[] rows = { "", "", "" };
            for(int i = 0; i < TC.Length; i+=3)
            {
                rows[0] += TC[i];
                rows[1] += TC[i + 1];
                rows[2] += TC[i + 2];
            }
            TC = rows[0] + rows[1] + rows[2];
            logTrys.Add(string.Format("{0}", rows[0]));
            logTrys.Add(string.Format("{0}", rows[1]));
            logTrys.Add(string.Format("{0}", rows[2]));
            for (int i = 0; i < TC.Length; i += 3)
            {
                int val = ((TC[i] - '0') * 9) + ((TC[i + 1] - '0') * 3) + (TC[i + 2] - '0');
                encrypt += alpha[val];
            }
            if (encrypt.Contains("-"))
            {
                goto tryagain;
            }
            else
            {
                foreach(string log in logTrys)
                    logMessages.Add(log);
            }
        }
        else
        {
            tryagain:
            logTrys = new List<string>();
            TB = generateTB(TBList, TA.Length);
            TBList.Add(TB + "");
            string TC = "";
            encrypt = "";
            string[] rows = { TA.Substring(0, word.Length), TA.Substring(word.Length, word.Length), TA.Substring(word.Length * 2) };
            for (int i = 0; i < word.Length; i++)
                TC += rows[0][i] + "" + rows[1][i] + "" + rows[2][i];
            logTrys.Add(string.Format("{0}", rows[0]));
            logTrys.Add(string.Format("{0}", rows[1]));
            logTrys.Add(string.Format("{0}", rows[2]));
            logTrys.Add(string.Format("Resulting Ternary: {0}", TC));
            logTrys.Add(string.Format("TB: {0}", TB));
            var TD = "";
            for (int i = 0; i < TC.Length; i++)
                TD += CMTools.mod((TC[i] - '0') + (TB[i] - '0'), 3);
            
            logTrys.Add(string.Format("Resulting Ternary: {0}", TD));
            for (int i = 0; i < TD.Length; i += 3)
            {
                int val = ((TD[i] - '0') * 9) + ((TD[i + 1] - '0') * 3) + (TD[i + 2] - '0');
                encrypt += alpha[val];
            }
            if (encrypt.Contains("-"))
            {
                goto tryagain;
            }
            else
            {
                foreach (string log in logTrys)
                    logMessages.Add(log);
            }
        }

        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[]
            {
                new PageInfo(new ScreenInfo[] {TB.Substring(0, word.Length), null, TB.Substring(word.Length, word.Length), null, TB.Substring(word.Length * 2) }, invert)
            },
             Score = 6
        };
    }
    
    private string generateTB(List<string> TBList, int length)
    {
        tryagain:
        string TB = "";
        for (int i = 0; i < length; i++)
            TB += Rnd.Range(0, 3);
        if (TBList.Contains(TB))
            goto tryagain;
        return TB;
    }
}
