using System.Collections.Generic;
using CipherMachine;
using Words;

public class MechanicalCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Mechanical Cipher" : "Mechanical Cipher"; } }
    public override string Code { get { return "ME"; } }
    private string[] table = new string[]
    {
        "UFHKQIPLXNZESGBVMCWJRDOTYA",
        "IWCZYMLKJODGFSQRNBTXHUEVAP",
        "WBSMEJTUCPFAHZOQLIKNYVGXRD",
        "GRINQVWOTYAJXBMHCFKLDUSZEP",
        "DLTVSUIKWCXRFJZANYHMQOGEPB",
        "FSVCEIUJKPGNTYHBLRQOXMADWZ",
        "JOCYWFPADKHIUVTSMENGQLZBRX",
        "BPHORAKNUETDZYQIMSFJGVWCXL",
        "ANDSQWTGXKFPCOVBLMYEZHRJIU",
        "AQJPBUSGWNXZVDYLETCOFHRIMK",
        "BHFTDGERXJAMUNZVYKOSPILCWQ",
        "JHUKDMSNEBICZYWLXQFPORTAVG",
        "ASNTZDBGWYILEORCQFXJPKHMVU",
        "RPCQABVLGWFENIKYMDUTSJXOZH",
        "YIXNVWQSUHFOMZDGKJPCTBELAR",
        "IMPCZLEGJARNTWSYFQDOUBKHVX",
        "JGKOXMUBAVRTFYCNPWQZESILHD",
        "SVHDBZNMKWJIEUYFXRQPLGCATO",
        "TZXGOPNBWAIYRHQLVKJSCDUEFM",
        "DJQZYWTPKIXCVABFNUEOLHSGRM",
        "CJOEDYHBNIXZRTPWGALFKUSMVQ",
        "FEHLYOBGRXQKVZUIMJTNACDPSW",
        "MOGAPTHIZXRFKLYSVDBWUQNECJ",
        "RXMSBPWOEJADIYNQLGKCTUHZFV",
        "ZJVWFBEOTKRDHSCPIGQNAYLUXM",
        "VWFXUEKRLBQTMCHSGJOZYDAPIN"
    };

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public MechanicalCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string kw = new Data().PickWord(word.Length);
        string encrypt = "";
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        if (invert)
        {
            for (int i = 0; i < word.Length; i++)
                encrypt = encrypt + "" + alpha[table[alpha.IndexOf(kw[i % kw.Length])].IndexOf(word[i])];
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
                encrypt = encrypt + "" + table[alpha.IndexOf(kw[i % kw.Length])][alpha.IndexOf(word[i])];
        }
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));

        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(new ScreenInfo[] { kw }, invert) },
            Score = 3
        };
    }
}
