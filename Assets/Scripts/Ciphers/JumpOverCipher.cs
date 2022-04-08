using System.Collections.Generic;
using CipherMachine;
using UnityEngine;
using Words;

public class JumpOverCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Jump-Over Cipher" : "Jump-Over Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "JO"; } }

    private readonly bool invert;
    public JumpOverCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWYZ";
        string kw = new Data().PickWord(4, 8);
        var kwfront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw, alpha, kwfront.Value);
        char letter = key[Random.Range(0, key.Length)];
        string encrypt = "";
        string replaceX = "";
        logMessages.Add(string.Format("Before Replacing Xs: {0}", word));
        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == 'X')
            {
                word = word.Substring(0, i) + "" + alpha[Random.Range(0, alpha.Length)] + "" + word.Substring(i + 1);
                replaceX = replaceX + "" + word[i];
            }
            else
                replaceX = replaceX + "" + alpha.Replace(word[i].ToString(), "")[Random.Range(0, 24)];
        }
        logMessages.Add(string.Format("After Replacing Xs: {0}", word));
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", kwfront.Expression, kwfront.Value));
        logMessages.Add(string.Format("Key: {0}", key));
        logMessages.Add(string.Format("Screen 2: {0}", replaceX));
        logMessages.Add(string.Format("Key Letter: {0}", letter));
        int[] offset = { (key.IndexOf(letter) / 5) - 2, (key.IndexOf(letter) % 5) - 2 };
        word = letter + "" + word;
        if (invert)
        {
            for (int i = 1; i < word.Length; i++)
            {
                int[] indexes = { key.IndexOf(word[i - 1]), key.IndexOf(word[i]) };
                int row = (indexes[1] / 5) - (indexes[0] / 5);
                int col = (indexes[1] % 5) - (indexes[0] % 5);
                encrypt = encrypt + "" + key[(CMTools.mod((indexes[1] / 5) + row, 5) * 5) + CMTools.mod((indexes[1] % 5) + col, 5)];
                logMessages.Add(string.Format("{0}{1} -> {2}", word[i - 1], word[i], encrypt[i - 1]));
            }
        }
        else
        {
            for (int i = 1; i < word.Length; i++)
            {
                int[] indexes = { key.IndexOf(word[i - 1]), key.IndexOf(word[i]) };
                int row = (indexes[1] / 5) - (indexes[0] / 5);
                if (row % 2 != 0)
                    row = (-System.Math.Sign(row) * 5) + row;
                int col = (indexes[1] % 5) - (indexes[0] % 5);
                if (col % 2 != 0)
                    col = (-System.Math.Sign(col) * 5) + col;
                encrypt = encrypt + "" + key[(CMTools.mod((indexes[0] / 5) + (row / 2), 5) * 5) + CMTools.mod((indexes[0] % 5) + (col / 2), 5)];
                logMessages.Add(string.Format("{0}{1} -> {2}", word[i - 1], word[i], encrypt[i - 1]));
            }
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, kwfront.Expression, replaceX, (letter + "") }, invert) }
        };
    }
}
