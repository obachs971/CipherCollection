using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class MalespinCipher : CipherBase
{
    public override string Name { get { return "Malespín Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "MS"; } }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string encrypt = "";
        string[] kws = generateKeywords();
        string key = CMTools.getKey(kws[0] + kws[1] + kws[2] + kws[3] + kws[4] + kws[5], "", true);
        for(int i = 0; i < kws.Length; i++)
            logMessages.Add(string.Format("Keyword #{0}: {1}", (i + 1), kws[0]));
        logMessages.Add(string.Format("Key: {0}", key));
        foreach (char letter in word)
            encrypt = encrypt + "" + key[(key.IndexOf(letter) + 13) % 26];
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { 
                new PageInfo(new ScreenInfo[] { kws[0], null, kws[1], null, kws[2], null, kws[3] }),
                new PageInfo(new ScreenInfo[] { kws[4], null, kws[5] }),
            }
        };
    }
    private string[] generateKeywords()
    {
    tryAgain:
        var alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
        string[] kws = new string[6];
        int[] order = { 4, 5, 6, 7, 8 };
        order.Shuffle();
        var words = new Data();
        for (int i = 0; i < order.Length; i++)
        {
            kws[i] = words.PickBestWord(order[i], w => alpha.Count(ch => w.Contains(ch)));
            alpha.RemoveAll(ch => kws[i].Contains(ch));
        }
        kws[5] = words.PickBestWord(4, 8, w => w.Distinct().Count(ch => alpha.Contains(ch)));
        alpha.RemoveAll(ch => kws[5].Contains(ch));
        if (alpha.Count > 0)
            goto tryAgain;
        return kws.Shuffle();
    }
}
