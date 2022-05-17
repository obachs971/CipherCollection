using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class IncrementalPolyalphabeticCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Incremental Polyalphabetic Cipher" : "Incremental Polyalphabetic Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "IP"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public IncrementalPolyalphabeticCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string[] kws = generateKeywords();
        string key = CMTools.getKey(kws.Join(""), "", false);
        for (int i = 0; i < kws.Length; i++)
            logMessages.Add(string.Format("KW{0}: {1}", (i + 1), kws[i]));
        logMessages.Add(string.Format("Key: {0}", key));
        string encrypted = "";
        for (var ix = 0; ix < word.Length; ix++)
        {
            var c = word[ix];
            var l = new List<char> { c };
            for (var j = 0; j <= ix; j++)
            {
                c = invert ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[key.IndexOf(c)] : key["ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c)];
                l.Add(c);
            }
            logMessages.Add(l.Join(" -> "));
            encrypted += c;
        }
        ScreenInfo[] screens = new ScreenInfo[7];
        for (int i = 0; i < 8; i += 2)
            screens[i] = kws[i / 2];
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypted,
            Pages = new PageInfo[] { new PageInfo(screens, invert), new PageInfo(new ScreenInfo[] { kws[4], null, kws[5] }) }
        };
    }

    // Finds a set of 6 keywords that contain all of the letters A–Z.
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