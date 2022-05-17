using System.Collections.Generic;
using CipherMachine;
using Words;

public class Chaocipher : CipherBase
{
	public override string Name { get { return invert ? "Inverted Chaocipher" : "Chaocipher"; } }
	public override int Score { get { return 5; } }
	public override string Code { get { return "CH"; } }
    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public Chaocipher(bool invert) { this.invert = invert; }
    
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string[] kws = new string[2], keys = new string[2];
        var kwfronts = new ValueExpression<bool>[2];
        string encrypt = "";
        var wordList = new Data();
        for (int i = 0; i < 2; i++)
        {
            kws[i] = wordList.PickWord(4, 8);
            kwfronts[i] = CMTools.generateBoolExp(bomb);
            keys[i] = CMTools.getKey(kws[i], "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfronts[i].Value);
            logMessages.Add(string.Format("Keyword #{0}: {1}", (i + 1), kws[i]));
            logMessages.Add(string.Format("Key #{0}: {1} -> {2} -> {3}", (i + 1), kwfronts[i].Expression, kwfronts[i].Value, keys[i]));
        }
        if (invert)
        {
            for (int i = 0; i < word.Length; i++)
            {
                logMessages.Add(keys[0]);
                logMessages.Add(keys[1]);
                int index = keys[1].IndexOf(word[i]);
                encrypt = encrypt + "" + keys[0][index];
                keys[0] = keys[0].Substring(index + 1) + keys[0].Substring(0, index + 1);
                keys[0] = keys[0].Substring(0, 2) + keys[0].Substring(3, 11) + keys[0][2] + keys[0].Substring(14);
                keys[1] = keys[1].Substring(index) + keys[1].Substring(0, index);
                keys[1] = keys[1].Substring(0, 1) + keys[1].Substring(2, 12) + keys[1][1] + keys[1].Substring(14);
                logMessages.Add(string.Format("{0} -> {1}", word[i], encrypt[i]));
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                logMessages.Add(keys[0]);
                logMessages.Add(keys[1]);
                int index = keys[0].IndexOf(word[i]);
                encrypt = encrypt + "" + keys[1][index];
                keys[0] = keys[0].Substring(index + 1) + keys[0].Substring(0, index + 1);
                keys[0] = keys[0].Substring(0, 2) + keys[0].Substring(3, 11) + keys[0][2] + keys[0].Substring(14);
                keys[1] = keys[1].Substring(index) + keys[1].Substring(0, index);
                keys[1] = keys[1].Substring(0, 1) + keys[1].Substring(2, 12) + keys[1][1] + keys[1].Substring(14);
                logMessages.Add(string.Format("{0} -> {1}", word[i], encrypt[i]));
            }
        }
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kws[0], kwfronts[0].Expression, kws[1], kwfronts[1].Expression }, invert) }
        };
    }
}
