using System.Collections.Generic;
using CipherMachine;
using Words;

public class PizzaSliceCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Pizza Slice Cipher" : "Pizza Slice Cipher"; } }
    public override int Score(int wordLength) { return 6; }
    public override string Code { get { return "PS"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public PizzaSliceCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string encrypt = "";
        string keyword = new Data().PickWord(4, 8); 
        var kwfront = CMTools.generateBoolExp(bomb);
        string k = CMTools.getKey(keyword, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront.Value);
        var rowType = CMTools.generateBoolExp(bomb);
        int startingOffset = UnityEngine.Random.Range(3, 18);
        string[] rows;
        if (rowType.Value)
            rows = new string[] { k.Substring(0, 6), k.Substring(6, 7), k.Substring(13, 6), k.Substring(19) };
        else
            rows = new string[] { k.Substring(0, 7), k.Substring(7, 6), k.Substring(13, 7), k.Substring(20) };
        string key = "";
        for (int i = 0; i < 26; i++)
            key = key + "" + k[(i * 7) % 26];
        logMessages.Add(string.Format("Keyword: {0}", keyword));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", kwfront.Expression, kwfront.Value));
        logMessages.Add(string.Format("Key: {0}", k));
        logMessages.Add(string.Format("Screen 2: {0} -> {1}", rowType.Expression, rowType.Value));
        logMessages.Add(string.Format("Screen B: {0}", startingOffset));
        var offset = startingOffset + 0;
        if (invert)
        {
            for(int i = 0; i < word.Length; i++)
            {
                for(int j = 0; j < rows.Length; j++)
                {
                    if(rows[j].Contains(word[i] + ""))
                    {
                        var l = rows[j][mod(rows[j].IndexOf(word[i]) - (offset % 7), rows[j].Length)];
                        encrypt = encrypt + "" + key[mod(key.IndexOf(l) + (offset / 7), 26)];
                        logMessages.Add(string.Format("{0}, {1} -> {2} -> {3}", word[i], offset, l, encrypt[i]));
                        offset += (encrypt[i] - 'A' + 1);
                        break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                var l = key[mod(key.IndexOf(word[i]) - (offset / 7), 26)];
                for (int j = 0; j < rows.Length; j++)
                {
                    if (rows[j].Contains(l + ""))
                    {
                        encrypt = encrypt + "" + rows[j][mod(rows[j].IndexOf(l) + (offset % 7), rows[j].Length)];
                        logMessages.Add(string.Format("{0}, {1} -> {2} -> {3}", word[i], offset, l, encrypt[i]));
                        offset += (word[i] - 'A' + 1);
                        break;
                    }
                }
            }
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { keyword, kwfront.Expression, rowType.Expression, startingOffset + "" }, invert) }
        };
    }
    private int mod(int n, int m)
    {
        while (n < 0)
            n += m;
        return (n % m);
    }
}
