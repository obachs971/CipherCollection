using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class TemptationStairwayCipher : CipherBase
{
    public override string Name { get { return "Temptation Stairway Cipher"; } }
    public override string Code { get { return "TE"; } }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var log = new List<string>();

        var wordNoJ = word.Select(ch => ch == 'J' ? randomLetter(except: 'J') : ch).Join("");
        while (wordNoJ.Length % 3 != 0)
            wordNoJ += randomLetter();
        log.Add(string.Format("Encrypting: {0}", wordNoJ));

        var kw = new Data().PickWord(4, 8);
        var kwExpr = CMTools.generateBoolExp(bomb);
        var key = CMTools.getKey(kw.Replace('J', 'I'), "ABCDEFGHIKLMNOPQRSTUVWXYZ", kwExpr.Value);
        var pos = wordNoJ.Select(ch => key.IndexOf(ch)).ToArray();

        log.Add(string.Format("Key ({0}; {1}={2}): {3}", kw, kwExpr.Expression, kwExpr.Value, key));

        var initialAxisExpr = CMTools.generateBoolExp(bomb);
        var axisIsHoriz = initialAxisExpr.Value;
        log.Add(string.Format("Initial axis: {0} → {1} → {2}", initialAxisExpr.Expression, initialAxisExpr.Value, axisIsHoriz ? "horizontal" : "vertical"));

        var encrypted = "";
        for (var i = 0; i < pos.Length; i += 3)
        {
            var minX = Enumerable.Range(0, 3).Min(j => pos[i + j] % 5);
            var maxX = Enumerable.Range(0, 3).Max(j => pos[i + j] % 5);
            var minY = Enumerable.Range(0, 3).Min(j => pos[i + j] / 5);
            var maxY = Enumerable.Range(0, 3).Max(j => pos[i + j] / 5);
            for (var j = 0; j < 3; j++)
            {
                var newX = axisIsHoriz ? pos[i + j] % 5 : maxX - pos[i + j] % 5 + minX;
                var newY = axisIsHoriz ? maxY - pos[i + j] / 5 + minY : pos[i + j] / 5;
                encrypted += key[newX + 5 * newY];
            }
            log.Add(string.Format("{0} flipped about {1} axis: {2}", wordNoJ.Substring(i, 3), axisIsHoriz ? "horizontal" : "vertical", encrypted.Substring(i, 3)));
            axisIsHoriz = !axisIsHoriz;
        }

        var replaceJ = word.Select((ch, ix) => word[ix] == 'J' ? wordNoJ[ix] : randomLetter(except: wordNoJ[ix])).Join("");
        log.Add(string.Format("String for replacing Js: {0}", replaceJ));

        return new ResultInfo
        {
            Encrypted = encrypted.Substring(0, word.Length),
            LogMessages = log,
            Pages = CMTools.NewArray(new PageInfo(new ScreenInfo[] { kw, kwExpr.Expression, replaceJ, initialAxisExpr.Expression, encrypted.Substring(word.Length) })),
            Score = 4
        };
    }

    private static char randomLetter(char? except = null)
    {
        return (except == null ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ" : "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Except(new[] { except.Value })).PickRandom();
    }
}