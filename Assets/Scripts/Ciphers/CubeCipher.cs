using System;
using System.Collections.Generic;
using CipherMachine;
using Words;

public class CubeCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Cube Cipher" : "Cube Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "CU"; } }

    private readonly bool invert;
    public CubeCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        Data data = new Data();
        int initLen = word.Length;
        while (word.Length % 3 != 0)
        {
            int num = UnityEngine.Random.Range(0, word.Length);
            word = word.Substring(0, num) + "#" + word.Substring(num);
        }
        if(initLen % 3 != 0)
            logMessages.Add(string.Format("Added #s: {0}", word));
        string[] kws = { data.PickWord(4, 8), data.PickWord(4, 8) };
        var screenA = CMTools.generateBoolExp(bomb);
        var screenB = CMTools.generateBoolExp(bomb);
        string[] keys = { CMTools.getKey(kws[0], "ABCDEFGHIJKLMNOPQRSTUVWXYZ", screenA.Value) + "#", CMTools.getKey(kws[1], "ABCDEFGHIJKLMNOPQRSTUVWXYZ", screenB.Value) + "#" };
        string[] poss = { "ABCDEFGHIJKLMNOPQRSTUVWXYZ#", "ABCDEFGHIJKLMNOPQRSTUVWXYZ#" };
        char[] letters = { poss[0][UnityEngine.Random.Range(0, poss[0].Length)], '!' };
        poss[0] = poss[0].Replace(letters[0] + "", "");
        
        logMessages.Add(string.Format("Screen 1: {0}", kws[0]));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", screenA.Expression, screenA.Value));
        logMessages.Add(string.Format("Screen 2: {0}", kws[1]));
        logMessages.Add(string.Format("Screen B: {0} -> {1}", screenB.Expression, screenB.Value));
        var tempLogs = new List<string>();
        string encrypt;
        if (invert)
        {
        tryagain:
            encrypt = "";
            tempLogs.Clear();
            if (poss[1].Length == 0)
            {
                letters[0] = poss[0][UnityEngine.Random.Range(0, poss[0].Length)];
                poss[0] = poss[0].Replace(letters[0] + "", "");
                poss[1] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ#";
            }
            letters[1] = poss[1][UnityEngine.Random.Range(0, poss[1].Length)];
            poss[1] = poss[1].Replace(letters[1] + "", "");
            string[] tempkeys = { keys[0].Replace(letters[0], '*').Replace('#', letters[0]).Replace('*', '#'), keys[1].Replace(letters[1], '*').Replace('#', letters[1]).Replace('*', '#') };
            tempLogs.Add(string.Format("Cube A: {0}", tempkeys[0]));
            tempLogs.Add(string.Format("Cube B: {0}", tempkeys[1]));
            for (int i = 0; i < word.Length; i += 3)
            {
                string temp = word.Substring(i, 3);
                //tempLogs.Add(string.Format("{0}", temp));
                for (int j = 0; j < temp.Length; j++)
                {
                    int[] indexes = { tempkeys[0].IndexOf(temp[j]), tempkeys[0].IndexOf(temp[(j + 1) % temp.Length]), tempkeys[0].IndexOf(temp[(j + 2) % temp.Length]) };
                    //tempLogs.Add(string.Format("{0} {1} {2}", indexes[0], indexes[1], indexes[2]));
                    indexes[0] = ((indexes[0] / 3) % 3) * 3;
                    indexes[1] = indexes[1] % 3;
                    indexes[2] = (indexes[2] / 9) * 9;
                    //tempLogs.Add(string.Format("{0} {1} {2}", indexes[0], indexes[1], indexes[2]));
                    encrypt = encrypt + "" + tempkeys[1][indexes[0] + indexes[1] + indexes[2]];
                }
            }
            if (encrypt.Substring(0, initLen).Contains("#")) goto tryagain;
        }
        else
        {
        tryagain:
            encrypt = "";
            tempLogs.Clear();
            if (poss[1].Length == 0)
            {
                letters[0] = poss[0][UnityEngine.Random.Range(0, poss[0].Length)];
                poss[0] = poss[0].Replace(letters[0] + "", "");
                poss[1] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ#";
            }
            letters[1] = poss[1][UnityEngine.Random.Range(0, poss[1].Length)];
            poss[1] = poss[1].Replace(letters[1] + "", "");
            string[] tempkeys = { keys[0].Replace(letters[0], '*').Replace('#', letters[0]).Replace('*', '#'), keys[1].Replace(letters[1], '*').Replace('#', letters[1]).Replace('*', '#') };
            tempLogs.Add(string.Format("Cube A: {0}", tempkeys[0]));
            tempLogs.Add(string.Format("Cube B: {0}", tempkeys[1]));
            for (int i = 0; i < word.Length; i += 3)
            {
                string temp = word.Substring(i, 3);
                //tempLogs.Add(string.Format("{0}", temp));
                for (int j = 0; j < temp.Length; j++)
                {
                    int[] indexes = { tempkeys[1].IndexOf(temp[j]), tempkeys[1].IndexOf(temp[(j + 1) % temp.Length]), tempkeys[1].IndexOf(temp[(j + 2) % temp.Length]) };
                    //tempLogs.Add(string.Format("{0} {1} {2}", indexes[0], indexes[1], indexes[2]));
                    indexes[0] = ((indexes[0] / 3) % 3) * 3;
                    indexes[1] = (indexes[1] / 9) * 9;
                    indexes[2] = indexes[2] % 3;
                    //tempLogs.Add(string.Format("{0} {1} {2}", indexes[0], indexes[1], indexes[2]));
                    encrypt = encrypt + "" + tempkeys[0][indexes[0] + indexes[1] + indexes[2]];
                }
            }
            if (encrypt.Substring(0, initLen).Contains("#")) goto tryagain;
        }
        logMessages.Add(string.Format("Screen 3: {0}{1}", letters[0], letters[1]));
        logMessages.AddRange(tempLogs);
        if (initLen % 3 != 0)
            logMessages.Add(string.Format("Screen C: {0}", encrypt.Substring(initLen)));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt.Substring(0, initLen),
            Pages = new[] { new PageInfo(new ScreenInfo[] { kws[0], screenA.Expression, kws[1], screenB.Expression, new string(letters), encrypt.Substring(initLen) }, invert) }
        };
    }
}
