using System.Collections.Generic;
using CipherMachine;
using Words;

public class TransposedHalvedPolybiusCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Transposed Halved Polybius Cipher" : "Transposed Halved Polybius Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "TH"; } }

    private readonly bool invert;
    public TransposedHalvedPolybiusCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        var wordList = new Data();
        var kwa = wordList.PickWord(4, 8);
        var kwb = wordList.PickWord(12 - word.Length);
        string[] coords = { "", "", "" };
        var kwfront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kwa, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront.Value);
        key = key.Substring(0, 5) + key.Substring(13, 5) + key.Substring(5, 5) + key.Substring(18, 5) + key.Substring(10, 3) + "##" + key.Substring(23) + "##";
        for (int i = 0; i < word.Length; i++)
        {
            int index = key.IndexOf(word[i]);
            coords[0] = coords[0] + "" + ("LR"[(index % 10) / 5]);
            coords[1] = coords[1] + "" + (index / 10 + 1);
            coords[2] = coords[2] + "" + ((index % 5) + 1);
        }
        logMessages.Add(string.Format("Screen 1: {0}", kwa));
        logMessages.Add(string.Format("Screen 2: {0}", kwb));
        logMessages.Add(string.Format("Key: {0} -> {1} -> {2}", kwfront.Expression, kwfront.Value, key));
        logMessages.Add(coords[0]);
        logMessages.Add(coords[1]);
        logMessages.Add(coords[2]);
        logMessages.Add(string.Format("--------"));
        string encrypt = "";
        if (invert)
        {
            for (int i = 0; i < kwb.Length; i++)
            {
                int index = key.IndexOf(kwb[i]);
                int row = index / 10;
                int col = index % 5;
                if (i % 2 == 0)
                    coords[row] = shiftLets(coords[row], col + 1, true, new string[] { "", "3", "45" }[row]);
                else
                    coords = swapCol(coords, col % word.Length, (col + row + 1) % word.Length);
                logMessages.Add(coords[0]);
                logMessages.Add(coords[1]);
                logMessages.Add(coords[2]);
                logMessages.Add(string.Format("--------"));
            }
        }
        else
        {
            for (int i = kwb.Length - 1; i >= 0; i--)
            {
                int index = key.IndexOf(kwb[i]);
                int row = index / 10;
                int col = index % 5;
                if (i % 2 == 0)
                    coords[row] = shiftLets(coords[row], col + 1, false, new string[] { "", "3", "45" }[row]);
                else
                    coords = swapCol(coords, col % word.Length, (col + row + 1) % word.Length);
                logMessages.Add(coords[0]);
                logMessages.Add(coords[1]);
                logMessages.Add(coords[2]);
                logMessages.Add(string.Format("--------"));
            }
        }
        for (int i = 0; i < word.Length; i++)
        {
            int index = ("123".IndexOf(coords[1][i]) * 10) + ("LR".IndexOf(coords[0][i]) * 5) + ("12345".IndexOf(coords[2][i]));
            encrypt = encrypt + "" + key[index];
        }
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kwa, kwfront.Expression, kwb }, invert) }
        };
    }
    private string shiftLets(string lets, int shift, bool invert, string replace)
    {
        string temp = lets.ToUpperInvariant();
        for (int i = 0; i < replace.Length; i++)
            temp = temp.Replace(replace[i] + "", "");
        if (temp.Length > 1)
        {
            shift = shift % temp.Length;
            if (!(invert))
                shift = (temp.Length - shift) % temp.Length;
            temp = temp.Substring(shift) + temp.Substring(0, shift);
            int cur = 0;
            for (int i = 0; i < lets.Length; i++)
            {
                if (!(replace.Contains(lets[i] + "")))
                    lets = lets.Substring(0, i) + temp[cur++] + lets.Substring(i + 1);
            }
        }
        return lets;
    }
    private string[] swapCol(string[] coords, int c1, int c2)
    {
        string t1 = coords[0][c1] + "" + coords[1][c1] + "" + coords[2][c1];
        string t2 = coords[0][c2] + "" + coords[1][c2] + "" + coords[2][c2];
        for (int i = 0; i < 3; i++)
        {
            coords[i] = coords[i].Substring(0, c1) + t2[i] + coords[i].Substring(c1 + 1);
            coords[i] = coords[i].Substring(0, c2) + t1[i] + coords[i].Substring(c2 + 1);
        }
        return coords;
    }
}
