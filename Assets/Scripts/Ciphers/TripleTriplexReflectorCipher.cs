using System.Collections.Generic;
using CipherMachine;
using Words;

public class TripleTriplexReflectorCipher : CipherBase
{
    public override string Name { get { return "Triple Triplex Reflector Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "TT"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "-ABCDEFGHIJKLMNOPQRSTUVWXYZ", encrypt = "";
        Data data = new Data();
        string[] kws = { data.PickWord(4, 8), data.PickWord(4, 8), data.PickWord(4, 8), data.PickWord(3, word.Length - 1), };
        var kwfront1 = CMTools.generateBoolExp(bomb);
        var kwfront2 = CMTools.generateBoolExp(bomb);
        var kwfront3 = CMTools.generateBoolExp(bomb);
        string[] keys = { CMTools.getKey(kws[0], alpha.Substring(1), kwfront1.Value), CMTools.getKey(kws[1], alpha.Substring(1), kwfront2.Value), CMTools.getKey(kws[2], alpha.Substring(1), kwfront3.Value) };
        logMessages.Add(string.Format("Key: #1: {0} + {1} ({2}) -> {3}", kws[0], kwfront1.Expression, kwfront1.Value, keys[0]));
        logMessages.Add(string.Format("Key: #2: {0} + {1} ({2}) -> {3}", kws[1], kwfront2.Expression, kwfront1.Value, keys[1]));
        logMessages.Add(string.Format("Key: #3: {0} + {1} ({2}) -> {3}", kws[2], kwfront3.Expression, kwfront1.Value, keys[2]));
        logMessages.Add(string.Format("Keyword #4: {0}", kws[3]));
        for(int i = 0; i < keys.Length; i++)
            keys[i] = keys[i].Substring(0, 13) + " " + keys[i].Substring(13);
        for (int i = 0; i < word.Length; i++)
        {
            foreach(string key in keys)
            {
                logMessages.Add(string.Format("{0}", key.Substring(0, 9)));
                logMessages.Add(string.Format("{0}", key.Substring(9, 9)));
                logMessages.Add(string.Format("{0}", key.Substring(18)));
                logMessages.Add(string.Format("------------------"));
            }
            string change = word[i] + "";
            change = change + "" + keys[1][keys[0].IndexOf(change[0])];
            int index = keys[2].IndexOf(change[1]);
            change = change + "" + keys[2][((2 - (index / 9)) * 9) + (8 - (index % 9))];
            change = change + "" + keys[0][keys[1].IndexOf(change[2])];
            encrypt = encrypt + "" + change[3];
            logMessages.Add(string.Format("{0} -> {1} -> {2} -> {3}", change[0], change[1], change[2], change[3]));
            if(i < word.Length - 1)
            {
                int[] tri = { alpha.IndexOf(kws[3][i % kws[3].Length]) / 9, (alpha.IndexOf(kws[3][i % kws[3].Length]) / 3) % 3, alpha.IndexOf(kws[3][i % kws[3].Length]) % 3 };
                for (int j = 0; j < 3; j++)
                    keys[j] = shiftReflector(keys[j], tri[j]);
            }
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kws[0], kwfront1.Expression, kws[1], kwfront2.Expression, kws[2], kwfront3.Expression, kws[3]}) }
        };
    }
    private string shiftReflector(string r, int n)
    {
        switch(n)
        {
            case 0:
                return r[1] + "" + r[2] + "" + r[3] + "" + r[4] + "" + r[5] + "" + r[6] + "" + r[7] + "" + r[8] + "" + r[0] + "" + r[10] + "" + r[11] + "" + r[12] + "" + r[14] + " " + r[15] + "" + r[16] + "" + r[17] + "" + r[9] + "" + r[19] + "" + r[20] + "" + r[21] + "" + r[22] + "" + r[23] + "" + r[24] + "" + r[25] + "" + r[26] + "" + r[18];
            case 1:
                return r[18] + "" + r[19] + "" + r[20] + "" + r[21] + "" + r[22] + "" + r[23] + "" + r[24] + "" + r[25] + "" + r[26] + "" + r[0] + "" + r[1] + "" + r[2] + "" + r[3] + " " + r[5] + "" + r[6] + "" + r[7] + "" + r[8] + "" + r[9] + "" + r[10] + "" + r[11] + "" + r[12] + "" + r[4] + "" + r[14] + "" + r[15] + "" + r[16] + "" + r[17];
            default:
                return r[23] + "" + r[24] + "" + r[25] + "" + r[26] + "" + r[22] + "" + r[18] + "" + r[19] + "" + r[20] + "" + r[21] + "" + r[14] + "" + r[15] + "" + r[16] + "" + r[17] + " " + r[9] + "" + r[10] + "" + r[11] + "" + r[12] + "" + r[5] + "" + r[6] + "" + r[7] + "" + r[8] + "" + r[4] + "" + r[0] + "" + r[1] + "" + r[2] + "" + r[3];
        }
    }
}
