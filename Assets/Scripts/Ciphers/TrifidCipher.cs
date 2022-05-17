using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class TrifidCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Trifid Cipher" : "Trifid Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "TF"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public TrifidCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        var words = new Data();
        var keyFront = CMTools.generateBoolExp(bomb);
        int[][] numbers = new int[3][] { new int[word.Length], new int[word.Length], new int[word.Length] };
        string key;
        string encrypt;
        string kw;
        do
        {
            encrypt = "";
            kw = words.PickWord(4, 8);
            key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", keyFront.Value) + "-";
            if (invert)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    int n = key.IndexOf(word[i]);
                    numbers[(i * 3) / word.Length][(i * 3) % word.Length] = n / 9;
                    numbers[((i * 3) + 1) / word.Length][((i * 3) + 1) % word.Length] = (n % 9) / 3;
                    numbers[((i * 3) + 2) / word.Length][((i * 3) + 2) % word.Length] = n % 3;
                }
                for (int i = 0; i < word.Length; i++)
                    encrypt = encrypt + "" + key[(numbers[0][i] * 9) + (numbers[1][i] * 3) + (numbers[2][i])];
            }
            else
            {
                for (int i = 0; i < word.Length; i++)
                {
                    int n = key.IndexOf(word[i]);
                    numbers[0][i] = n / 9;
                    numbers[1][i] = (n % 9) / 3;
                    numbers[2][i] = n % 3;
                }
                for (int i = 0; i < word.Length; i++)
                    encrypt = encrypt + "" + key[(numbers[(i * 3) / word.Length][(i * 3) % word.Length] * 9) + (numbers[((i * 3) + 1) / word.Length][((i * 3) + 1) % word.Length] * 3) + (numbers[((i * 3) + 2) / word.Length][((i * 3) + 2) % word.Length])];
            }
        }
        while (encrypt.Contains("-"));
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Key: {0}", key));
        logMessages.Add(string.Join("", numbers[0].Select(p => (p + 1).ToString()).ToArray()));
        logMessages.Add(string.Join("", numbers[1].Select(p => (p + 1).ToString()).ToArray()));
        logMessages.Add(string.Join("", numbers[2].Select(p => (p + 1).ToString()).ToArray()));
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, keyFront.Expression }, invert) }
        };
    }
}
