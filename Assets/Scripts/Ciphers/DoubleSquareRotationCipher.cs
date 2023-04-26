using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using rnd = UnityEngine.Random;
using Words;

public class DoubleSquareRotationCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Double Square Rotation Cipher" : "Double Square Rotation Cipher"; } }
    public override string Code { get { return "DS"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public DoubleSquareRotationCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        Data words = new Data();
        var encrypt = "";
        var replaceJ = "";
        var alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
        var alpha2 = "-ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var idxesOuterSquare = new[] { 0, 5, 10, 15, 20, 21, 22, 23, 24, 19, 14, 9, 4, 3, 2, 1 };
        var idxesInnerSquare = new[] { 6, 11, 16, 17, 18, 13, 8, 7 };

        logMessages.Add(string.Format("Before Replacing Js: {0}", word));
        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == 'J')
            {
                word = word.Substring(0, i) + "" + alpha[rnd.Range(0, alpha.Length)] + "" + word.Substring(i + 1);
                replaceJ = replaceJ + "" + word[i];
            }
            else
            {
                replaceJ = replaceJ + "" + alpha.Replace(word[i].ToString(), "")[rnd.Range(0, 24)];
            }
        }
        logMessages.Add(string.Format("After Replacing Js: {0}", word));
        logMessages.Add(string.Format("Screen 1 Page 2: {0}", replaceJ));

        string[] kws = new string[2];
        string[] keys = new string[2];
        string rotatingValues = words.PickWord(word.Length);
        var rotatingValuesMod10 = rotatingValues.Select(x => CMTools.mod(alpha2.IndexOf(x), 10));
        var kwFronts = new ValueExpression<bool>[2];
        for (int i = 0; i < 2; i++)
        {
            kws[i] = words.PickWord(4, 8);
            kwFronts[i] = CMTools.generateBoolExp(bomb);
            keys[i] = CMTools.getKey(kws[i].Replace("J", "I"), "ABCDEFGHIKLMNOPQRSTUVWXYZ", kwFronts[i].Value);
            logMessages.Add(string.Format("Keyword #{0}: {1}", (i + 1), kws[i]));
            logMessages.Add(string.Format("Key #{0}: {1} -> {2} -> {3}", (i + 1), kwFronts[i].Expression, kwFronts[i].Value, keys[i]));
        }

        logMessages.Add(string.Format("Each letter in {0} mod 10: {1}", rotatingValues, rotatingValuesMod10.Join("")));

        if (invert)
        {
            for (int i = 0; i < word.Length; i++)
            {
                var curLetter = word[i];
                var ixCurLetterInGrid = (i % 2 == 0 ? keys[0] : keys[1]).IndexOf(curLetter);
                var replacementLetter = (i % 2 == 0 ? keys[0] : keys[1])[12];

                if (idxesInnerSquare.Contains(ixCurLetterInGrid))
                {
                    var ixFromInner = idxesInnerSquare.IndexOf(x => x == ixCurLetterInGrid);
                    replacementLetter = (i % 2 == 0 ? keys[0] : keys[1])[idxesInnerSquare[CMTools.mod(ixFromInner - rotatingValuesMod10.ElementAt(i), 8)]];
                }
                else if (idxesOuterSquare.Contains(ixCurLetterInGrid))
                {
                    var ixFromOuter = idxesOuterSquare.IndexOf(x => x == ixCurLetterInGrid);
                    replacementLetter = (i % 2 == 0 ? keys[0] : keys[1])[idxesOuterSquare[CMTools.mod(ixFromOuter - rotatingValuesMod10.ElementAt(i), 16)]];
                }
                encrypt = encrypt + "" + replacementLetter;
                logMessages.Add(string.Format("{0} -> {1}", word[i], replacementLetter));
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                var curLetter = word[i];
                var ixCurLetterInGrid = (i % 2 == 0 ? keys[0] : keys[1]).IndexOf(curLetter);
                var replacementLetter = (i % 2 == 0 ? keys[0] : keys[1])[12];

                if (idxesInnerSquare.Contains(ixCurLetterInGrid))
                {
                    var ixFromInner = idxesInnerSquare.IndexOf(x => x == ixCurLetterInGrid);
                    replacementLetter = (i % 2 == 0 ? keys[0] : keys[1])[idxesInnerSquare[CMTools.mod(ixFromInner + rotatingValuesMod10.ElementAt(i), 8)]];
                }
                else if (idxesOuterSquare.Contains(ixCurLetterInGrid))
                {
                    var ixFromOuter = idxesOuterSquare.IndexOf(x => x == ixCurLetterInGrid);
                    replacementLetter = (i % 2 == 0 ? keys[0] : keys[1])[idxesOuterSquare[CMTools.mod(ixFromOuter + rotatingValuesMod10.ElementAt(i), 16)]];
                }
                encrypt = encrypt + "" + replacementLetter;
                logMessages.Add(string.Format("{0} -> {1}", word[i], replacementLetter));
            }
        }

        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[]
            {
                new PageInfo(new ScreenInfo[] {kws[0], kwFronts[0].Expression, kws[1], kwFronts[1].Expression, rotatingValues, null, replaceJ }, invert)
            },
            // Score = ?
            // Sean or Timwi, please determine the score for this cipher. - Kilo
        };
    }
}
