using System.Collections.Generic;
using CipherMachine;
using Words;

public class MonosodiumGlutamateCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Monosodium Glutamate Cipher" : "Monosodium Glutamate Cipher"; } }
    public override string Code { get { return "MG"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public MonosodiumGlutamateCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        Data data = new Data();
        string kwA = data.PickWord(4, 8);
        string kwB = data.PickWord(4, 8);
        string kwC = data.PickWord(3, word.Length);
        var kwfrontA = CMTools.generateBoolExp(bomb);
        var kwfrontB = CMTools.generateBoolExp(bomb);
        string[] keys = { CMTools.getKey(kwA, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfrontA.Value), CMTools.getKey(kwB, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfrontB.Value) };
        string[] vals = { "621", "926", "34" };
        vals = vals.Shuffle();
        string keyVal = vals[0] + vals[1] + vals[2];
        keyVal = keyVal.Substring(0, word.Length);
        var arrange = CMTools.generateBoolExp(bomb);


        logMessages.Add(string.Format("Screen 1: {0}", kwA));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", kwfrontA.Expression, kwfrontA.Value));
        logMessages.Add(string.Format("Key A: {0}", keys[0]));
        logMessages.Add(string.Format("Screen 2: {0}", kwB));
        logMessages.Add(string.Format("Screen B: {0} -> {1}", kwfrontB.Expression, kwfrontB.Value));
        logMessages.Add(string.Format("Key B: {0}", keys[1]));
        logMessages.Add(string.Format("Screen 3: {0}", keyVal));
        logMessages.Add(string.Format("Screen C: {0} -> {1}", arrange.Expression, arrange.Value));
        logMessages.Add(string.Format("Screen 4: {0}", kwC));



        string encrypt = "";
        if (invert)
        {
            for (int i = 0; i < word.Length; i++)
            {
                int index = keys[0].IndexOf(word[i]);
                int row = index / 13, col = index % 13;
                col = mod(col - (keyVal[i] - '0'), 13);
                if (!arrange.Value) row = (row + 1) % 2;
                char let = keys[1][col * 2 + row];
                index = keys[0].IndexOf(let);
                row = index / 13;
                col = index % 13;
                if (!arrange.Value) row = (row + 1) % 2;
                encrypt = encrypt + "" + keys[1][mod((col * 2 + row) + (kwC[i % kwC.Length] - 'A' + 1), 26)];
                logMessages.Add(string.Format("{0}, {1} -> {2}, {3} -> {4}", word[i], keyVal[i], let, kwC[i % kwC.Length], encrypt[i]));
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                int index = keys[1].IndexOf(word[i]);
                index = mod(index - (kwC[i % kwC.Length] - 'A' + 1), 26);
                int col = index / 2, row = index % 2;
                if (!arrange.Value) row = (row + 1) % 2;
                char let = keys[0][row * 13 + col];
                index = keys[1].IndexOf(let);
                col = index / 2;
                row = index % 2;
                if (!arrange.Value) row = (row + 1) % 2;
                col = mod(col + (keyVal[i] - '0'), 13);
                encrypt = encrypt + "" + keys[0][row * 13 + col];
                logMessages.Add(string.Format("{0}, {1} -> {2}, {3} -> {4}", word[i], kwC[i % kwC.Length], let, keyVal[i], encrypt[i]));
            }
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kwA, kwfrontA.Expression, kwB, kwfrontB.Expression, keyVal, arrange.Expression, kwC }, invert) },
            Score = 5
        };
    }
    private int mod(int n, int m)
    {
        while (n < 0)
            n += m;
        return (n % m);
    }
}
