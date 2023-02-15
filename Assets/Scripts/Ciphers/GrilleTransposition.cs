using System.Collections.Generic;
using CipherMachine;

public class GrilleTransposition : CipherBase
{
    public override string Name { get { return invert ? "Inverted Grille Transposition" : "Grille Transposition"; } }
    public override int Score(int wordLength) { return 4; }
    public override string Code { get { return "GT"; } }
    public override bool IsTransposition { get { return true; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public GrilleTransposition(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        var keyNumber = CMTools.generateValue(bomb);
        string encrypt = "";
        logMessages.Add(string.Format("Key Number: {0} -> {1}", keyNumber.Expression, keyNumber.Value));
        char[] temp = new char[word.Length];
        int n1 = keyNumber.Value % word.Length, n2 = (keyNumber.Value + (word.Length / 2)) % word.Length;
        if (invert)
        {
            for (int i = 0; i < (word.Length / 2); i++)
            {
                temp[(n1 + i) % temp.Length] = word[i * 2];
                temp[(n2 + i) % temp.Length] = word[(i * 2) + 1];
            }
            if (word.Length % 2 == 1)
                temp[(keyNumber.Value + (temp.Length - 1)) % temp.Length] = word[word.Length - 1];
            encrypt = encrypt + "" + temp[0];
            for (int i = 0; i < (word.Length - 1) / 2; i++)
            {
                encrypt = encrypt + "" + temp[temp.Length - (i + 1)];
                encrypt = encrypt + "" + temp[i + 1];
            }
            if (word.Length % 2 == 0)
                encrypt = encrypt + "" + temp[temp.Length / 2];
        }
        else
        {
            temp[0] = word[0];
            for (int i = 0; i < (word.Length - 1) / 2; i++)
            {
                temp[temp.Length - (i + 1)] = word[(i * 2) + 1];
                temp[i + 1] = word[(i * 2) + 2];
            }
            if (word.Length % 2 == 0)
                temp[word.Length / 2] = word[word.Length - 1];
            encrypt = "";
            for (int i = 0; i < (temp.Length / 2); i++)
                encrypt = encrypt + "" + temp[(n1 + i) % temp.Length] + "" + temp[(n2 + i) % temp.Length];
            if (word.Length % 2 == 1)
                encrypt = encrypt + "" + temp[(keyNumber.Value + (temp.Length - 1)) % temp.Length];
        }
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { keyNumber.Expression }, invert) }
        };
    }
}
