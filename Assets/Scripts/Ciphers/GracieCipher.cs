using System.Collections.Generic;
using CipherMachine;
using Words;

public class GracieCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Gracie Cipher" : "Gracie Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "GC"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public GracieCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string encrypt = "";
        string kw = new Data().PickWord(4, 8);
        var kwfront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront.Value);
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", kwfront.Expression, kwfront.Value));
        logMessages.Add(string.Format("Key: {0}", key));
        if(invert)
        {
            for (int i = 0; i < word.Length / 2; i++)
            {
                int n1 = key.IndexOf(word[i * 2]), n2 = key.IndexOf(word[i * 2 + 1]);
                int r1 = n1 / 13, c1 = n1 % 13, r2 = n2 / 13, c2 = n2 % 13;
                if (r1 == r2 && c1 == c2)
                {
                    r1 = (r1 + 1) % 2;
                    r2 = (r2 + 1) % 2;
                    c1 = 12 - c1;
                    c2 = 12 - c2;
                }
                else if (r1 == r2)
                {
                    r1 = (r1 + 1) % 2;
                    r2 = (r2 + 1) % 2;
                }
                else if (c1 == c2)
                {
                    c1 = CMTools.mod(c1 - 1, 13);
                    c2 = CMTools.mod(c2 - 1, 13);
                }
                else
                {
                    int temp = c1;
                    c1 = c2;
                    c2 = temp;
                }
                encrypt = encrypt + "" + key[r1 * 13 + c1] + "" + key[r2 * 13 + c2];
                logMessages.Add(string.Format("{0}{1} -> {2}{3}", word[i * 2], word[i * 2 + 1], encrypt[i * 2], encrypt[i * 2 + 1]));
            }
        }
        else
        {
            for (int i = 0; i < word.Length / 2; i++)
            {
                int n1 = key.IndexOf(word[i * 2]), n2 = key.IndexOf(word[i * 2 + 1]);
                int r1 = n1 / 13, c1 = n1 % 13, r2 = n2 / 13, c2 = n2 % 13;
                if(r1 == r2 && c1 == c2)
                {
                    r1 = (r1 + 1) % 2;
                    r2 = (r2 + 1) % 2;
                    c1 = 12 - c1;
                    c2 = 12 - c2;
                }
                else if (r1 == r2)
                {
                    r1 = (r1 + 1) % 2;
                    r2 = (r2 + 1) % 2;
                }
                else if(c1 == c2)
                {
                    c1 = CMTools.mod(c1 + 1, 13);
                    c2 = CMTools.mod(c2 + 1, 13);
                }
                else
                {
                    int temp = c1;
                    c1 = c2;
                    c2 = temp;
                }
                encrypt = encrypt + "" + key[r1 * 13 + c1] + "" + key[r2 * 13 + c2];
                logMessages.Add(string.Format("{0}{1} -> {2}{3}", word[i * 2], word[i * 2 + 1], encrypt[i * 2], encrypt[i * 2 + 1]));
            }
        }
        if (word.Length % 2 == 1)
            encrypt = encrypt + "" + word[word.Length - 1];

        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, kwfront.Expression }, invert) }
        };
    }
}
