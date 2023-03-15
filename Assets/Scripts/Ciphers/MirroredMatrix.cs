using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;
using Rnd = UnityEngine.Random;
public class MirroredMatrixCipher : CipherBase
{
    public override string Name { get { return "Mirrored Matrix Cipher"; } }
    public override string Code { get { return "MM"; } }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var log = new List<string>();

        var wordNoJ = word.Select(ch => ch == 'J' ? randomLetter() : ch).Join("");
        var indexRemoved = -1;
        var letterRemoved = '-';
        if (wordNoJ.Length % 2 != 0)
        {
            indexRemoved = Rnd.Range(0, wordNoJ.Length);
            letterRemoved = wordNoJ[indexRemoved];
            log.Add(string.Format("{0} + {1} → {2}", wordNoJ, indexRemoved + 1, wordNoJ.Substring(0, indexRemoved) + wordNoJ.Substring(indexRemoved + 1)));
            wordNoJ = wordNoJ.Substring(0, indexRemoved) + wordNoJ.Substring(indexRemoved + 1);
        }
        log.Add(string.Format("Encrypting: {0}", wordNoJ));


        var kw = new Data().PickWord(4, 8);
        var kwExpr = CMTools.generateBoolExp(bomb);
        var key = CMTools.getKey(kw.Replace('J', 'I'), "ABCDEFGHIKLMNOPQRSTUVWXYZ", kwExpr.Value);
        log.Add(string.Format("Key ({0}; {1}={2}): {3}", kw, kwExpr.Expression, kwExpr.Value, key));

        var initialAxisExpr = CMTools.generateValue(bomb);
        var axis = initialAxisExpr.Value % 4;
        log.Add(string.Format("Initial axis: {0} → {1} → {2}", initialAxisExpr.Expression, initialAxisExpr.Value, axis));

        var direction = CMTools.generateBoolExp(bomb);
        var offset = direction.Value ? 1 : -1;
        log.Add(string.Format("Initial axis: {0} → {1} → {2}", direction.Expression, direction.Value, direction.Value ? "CW" : "CCW"));

        var encrypted = "";
        for (var i = 0; i < wordNoJ.Length; i+=2)
        {
            key = shiftKey(key, wordNoJ[i]);
            log.Add(string.Format("{0} → {1}", wordNoJ[i], key));
            encrypted = encrypted + getEncryptedLetter(key, axis, wordNoJ[i + 1]);
            log.Add(string.Format("{0} → {1}", wordNoJ[i + 1], encrypted[i]));
            key = shiftKey(key, encrypted[i]);
            log.Add(string.Format("{0} → {1}", encrypted[i], key));
            encrypted = encrypted + getEncryptedLetter(key, axis, wordNoJ[i]);
            log.Add(string.Format("{0} → {1}", wordNoJ[i], encrypted[i + 1]));
            axis = CMTools.mod(axis + offset, 4);
        }
        if(indexRemoved >= 0)
        {
            log.Add(string.Format("{0} + {1} + {2} → {3}", encrypted, indexRemoved + 1, letterRemoved, encrypted.Substring(0, indexRemoved) + letterRemoved + encrypted.Substring(indexRemoved)));
            encrypted = encrypted.Substring(0, indexRemoved) + letterRemoved + encrypted.Substring(indexRemoved);
            wordNoJ = wordNoJ.Substring(0, indexRemoved) + letterRemoved + wordNoJ.Substring(indexRemoved);
        }
        
        var replaceJ = word.Select((ch, ix) => word[ix] == 'J' ? wordNoJ[ix] : randomLetter(except: wordNoJ[ix])).Join("");
        log.Add(string.Format("String for replacing Js: {0}", replaceJ));
        
        return new ResultInfo
        {
            Encrypted = encrypted,
            LogMessages = log,
            Pages = CMTools.NewArray(new PageInfo(new ScreenInfo[] { kw, kwExpr.Expression, initialAxisExpr.Expression, direction.Expression, replaceJ, indexRemoved >= 0 ? (indexRemoved + 1) + "" : null })),
            Score = 7
        };
    }
    private static char randomLetter(char? except = null)
    {
        return (except == null ? "ABCDEFGHIKLMNOPQRSTUVWXYZ" : "ABCDEFGHIKLMNOPQRSTUVWXYZ".Except(new[] { except.Value })).PickRandom();
    }
    private string shiftKey(string key, char let)
    {
        var row = key.IndexOf(let) / 5;
        var col = key.IndexOf(let) % 5;
        for (var i = row; i < 2; i++)
            key = key.Substring(20) + key.Substring(0, 20);
        for (var i = row; i > 2; i--)
            key = key.Substring(5) + key.Substring(0, 5);
        for (var i = col; i < 2; i++)
            key = key[4] + key.Substring(0, 4) + key[9] + key.Substring(5, 4) + key[14] + key.Substring(10, 4) + key[19] + key.Substring(15, 4) + key[24] + key.Substring(20, 4);
        for (var i = col; i > 2; i--)
            key = key.Substring(1, 4) + key[0] + key.Substring(6, 4) + key[5] + key.Substring(11, 4) + key[10] + key.Substring(16, 4) + key[15] + key.Substring(21, 4) + key[20];
        return key;
    }

    private char getEncryptedLetter(string key, int axis, char let)
    {
        var row = key.IndexOf(let) / 5;
        var col = key.IndexOf(let) % 5;
        switch (axis)
        {
            case 0:
                if (col == 2)
                    row = 4 - row;
                else
                    col = 4 - col;
                break;
            case 1:
                if ((4 - row) == col)
                {
                    var temp = row;
                    row = col;
                    col = temp;
                }
                else
                {
                    var temp = row;
                    row = 4 - col;
                    col = 4 - temp;
                }
                break;
            case 2:
                if (row == 2)
                    col = 4 - col;
                else
                    row = 4 - row;
                break;
            default:
                if (row == col)
                {
                    var temp = row;
                    row = 4 - col;
                    col = 4 - temp;
                }
                else
                {
                    var temp = row;
                    row = col;
                    col = temp;
                }
                break;
        }
        return (key[row * 5 + col]);
    }
}
