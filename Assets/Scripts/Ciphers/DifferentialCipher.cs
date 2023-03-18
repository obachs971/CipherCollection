using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;
using Rnd = UnityEngine.Random;

public class DifferentialCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Differential Cipher" : "Differential Cipher"; } }
    public override string Code { get { return "DF"; } }

    public override bool IsInvert { get { return invert; } }
    
    private readonly bool invert;
    public DifferentialCipher(bool invert) { this.invert = invert; }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var log = new List<string>();
        
        var startIndexExpr = CMTools.generateValue(bomb);
        var startIndex = startIndexExpr.Value % word.Length;
        log.Add(string.Format("Screen A: {0} → {1} → {2}", startIndexExpr.Expression, startIndexExpr.Value, startIndex));
        
        var encrypted = new char[word.Length];
        var nums = "";
        var numRows = new string[] { "", "" };
        var correction = "";
        if(invert)
        {
            for (var i = 0; i < word.Length - 1; i++)
            {
                var diff = (word[(i + 1 + startIndex) % word.Length]) - (word[(i + startIndex) % word.Length]);
                diff = CMTools.mod(diff, 26);
                List<int> choices = new List<int>();
                while (diff < 100)
                {
                    choices.Add(diff);
                    diff += 26;
                }
                choices.Shuffle();
                diff = choices[0];
                numRows[0] = numRows[0] + (diff / 10);
                numRows[1] = numRows[1] + (diff % 10);
                log.Add(string.Format("{0} - {1} → {2}", word[(i + 1 + startIndex) % word.Length], word[(i + startIndex) % word.Length], diff));
            }
            nums = numRows[0] + numRows[1];
            encrypted[startIndex] = word[startIndex];
            for (var i = 0; i < word.Length - 1; i++)
            {
                var num = ((nums[i * 2] - '0') * 10) + (nums[i * 2 + 1] - '0');
                var result = (encrypted[(i + startIndex) % encrypted.Length] - 'A') + num;
                correction = correction + (num / 26);
                encrypted[(i + 1 + startIndex) % encrypted.Length] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[CMTools.mod(result, 26)];
                log.Add(string.Format("{0} + {1}{2} → {3}", encrypted[(i + startIndex) % word.Length], nums[i * 2], nums[i * 2 + 1], encrypted[(i + 1 + startIndex) % encrypted.Length]));
            }
        }
        else
        {
            for (var i = 0; i < word.Length - 1; i++)
            {
                var diff = (word[(i + 1 + startIndex) % word.Length]) - (word[(i + startIndex) % word.Length]);
                diff = CMTools.mod(diff, 26);
                List<int> choices = new List<int>();
                while (diff < 100)
                {
                    choices.Add(diff);
                    diff += 26;
                }
                choices.Shuffle();
                diff = choices[0];
                nums = nums + (diff / 10) + "" + (diff % 10);
                log.Add(string.Format("{0} - {1} → {2}", word[(i + 1 + startIndex) % word.Length], word[(i + startIndex) % word.Length], diff));
            }
            numRows[0] = nums.Substring(0, nums.Length / 2);
            numRows[1] = nums.Substring(nums.Length / 2);
            encrypted[startIndex] = word[startIndex];
            for (var i = 0; i < word.Length - 1; i++)
            {
                var num = ((numRows[0][i] - '0') * 10) + (numRows[1][i] - '0');
                var result = (encrypted[(i + startIndex) % encrypted.Length] - 'A') + num;
                correction = correction + (num / 26);
                encrypted[(i + 1 + startIndex) % encrypted.Length] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[CMTools.mod(result, 26)];
                log.Add(string.Format("{0} + {1}{2} → {3}", encrypted[(i + startIndex) % word.Length], numRows[0][i], numRows[1][i], encrypted[(i + 1 + startIndex) % encrypted.Length]));
            }
        }
        
        log.Add(string.Format("Screen 1: {0}", correction));
        log.Add(string.Format("Encrypted word: {0}", new string(encrypted)));
        return new ResultInfo
        {
            Encrypted = new string(encrypted),
            LogMessages = log,
            Pages = CMTools.NewArray(new PageInfo(new ScreenInfo[] { correction, startIndexExpr.Expression }, invert)),
            Score = 8
        };
    }

    private static char randomLetter(char? except = null)
    {
        return (except == null ? "ABCDEFGHIKLMNOPQRSTUVWXYZ" : "ABCDEFGHIKLMNOPQRSTUVWXYZ".Except(new[] { except.Value })).PickRandom();
    }
}
