using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;

public class BurrowsWheelerTransform : CipherBase
{
    public override string Name { get { return "Burrows-Wheeler Transform"; } }
    public override string Code { get { return "BW"; } }
    public override bool IsTransposition { get { return true; } }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        var rotations = Enumerable.Range(0, word.Length).Select(i => word.Substring(i) + word.Substring(0, i)).ToArray();

        logMessages.Add("Rotations of the word:");
        logMessages.AddRange(rotations);

        Array.Sort(rotations);

        logMessages.Add("After sorting:");
        logMessages.AddRange(rotations);

        return new ResultInfo
        {
            Encrypted = rotations.Select(r => r.Last()).Join(""),
            LogMessages = logMessages,
            Pages = new[] { new PageInfo(new ScreenInfo[] { (Array.IndexOf(rotations, word) + 1).ToString() }) },
            Score = 3
        };
    }
}
