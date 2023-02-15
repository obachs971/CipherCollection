using System.Collections.Generic;
using CipherMachine;
using Words;

using Rnd = UnityEngine.Random;

public class ArithmeticSequenceCipher : CipherBase
{
    public override string Name { get { return "Arithmetic Sequence Cipher"; } }
    public override int Score(int wordLength) { return 6; }
    public override string Code { get { return "AS"; } }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var kw = new Data().PickWord(4, 8);
        var be = CMTools.generateBoolExp(bomb);
        var key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", be.Value);
        var log = new List<string> { string.Format("Generated key: {0}.", key) };
        int? ignoreLetter = null;
        var wordRest = word;
        if (word.Length % 2 == 1)
        {
            ignoreLetter = Rnd.Range(0, word.Length) + 1;
            wordRest = word.Remove(ignoreLetter.Value - 1, 1);
            log.Add(string.Format("Ignoring letter #{0}. Encrypting {1}.", ignoreLetter.Value, wordRest));
        }
        var encrypted = "";
        for (var i = 0; i < wordRest.Length; i += 2)
        {
            var jump = CMTools.mod(key.IndexOf(wordRest[i + 1]) - key.IndexOf(wordRest[i]), key.Length);
            var ltr3 = CMTools.mod(key.IndexOf(wordRest[i + 1]) + jump, key.Length);
            encrypted += key[CMTools.mod(ltr3 + jump, key.Length)];
            encrypted += key[ltr3];
            log.Add(string.Format("{0}{1} → {2} + {3} → {3}{2}", wordRest[i], wordRest[i + 1], encrypted[encrypted.Length - 1], encrypted[encrypted.Length - 2]));
        }

        if (ignoreLetter != null)
        {
            encrypted = encrypted.Insert(ignoreLetter.Value - 1, word.Substring(ignoreLetter.Value - 1, 1));
            log.Add(string.Format("Putting {0} back in at position {1}.", word[ignoreLetter.Value - 1], ignoreLetter.Value));
        }

        return new ResultInfo
        {
            Encrypted = encrypted,
            LogMessages = log,
            Pages = CMTools.NewArray(new PageInfo(new ScreenInfo[] { kw, be.Expression, "", ignoreLetter.ToString() }))
        };
    }
}