using System.Collections.Generic;
using CipherMachine;

public class ExtinctionTransposition : CipherBase
{
    public override string Name { get { return "Extinction Transposition"; } }
    public override int Score(int wordLength) { return 3; }
    public override string Code { get { return "ET"; } }
    public override bool IsTransposition { get { return true; } }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var attempts = 0;

        redo:
        var log = new List<string>();

        var expressions = new ScreenInfo[8];
        var encrypted = word.ToCharArray();
        for (var i = 0; i < 4; i++)
        {
            var e1 = CMTools.generateValue(bomb);
            var v1 = CMTools.mod(e1.Value - 1, encrypted.Length);

            var tries = 0;
            tryAgain:
            var e2 = CMTools.generateValue(bomb);
            var v2 = CMTools.mod(e2.Value - 1, encrypted.Length);
            if (v2 == v1 && tries++ < 5)
                goto tryAgain;

            var t = encrypted[v1];
            encrypted[v1] = encrypted[v2];
            encrypted[v2] = t;

            log.Add(string.Format("Expressions: {0} = {1} = {2}; {3} = {4} = {5}; after swap: {6}", e1.Expression, e1.Value, v1 + 1, e2.Expression, e2.Value, v2 + 1, new string(encrypted)));
            expressions[6 - 2 * i] = e1.Expression;
            expressions[7 - 2 * i] = e2.Expression;
        }
        var encryptedStr = new string(encrypted);
        if (encryptedStr == word && attempts++ < 5)
            goto redo;

        return new ResultInfo
        {
            Encrypted = encryptedStr,
            LogMessages = log,
            Pages = CMTools.NewArray(new PageInfo(expressions))
        };
    }
}