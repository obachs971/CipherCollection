using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class SemaphoreRotationCipher : CipherBase
{
    public override string Name { get { return _invert ? "Inverted Semaphore Rotation Cipher" : "Semaphore Rotation Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "SR"; } }

    private static readonly int[][] _semaphores = "45;46;47;04;14;24;34;56;57;02;05;15;25;35;67;06;16;26;36;07;17;03;12;13;27;23".Split(';').Select(str => str.Select(ch => ch - '0').ToArray()).ToArray();

    private readonly bool _invert;
    public SemaphoreRotationCipher(bool invert) { this._invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        logMessages.Add(_invert ? "Using encrypt instructions (counter-clockwise)" : "Using decrypt instructions (clockwise)");
        var wordList = new Data();
        tryAgain:
        var kw = wordList.PickWord(4, 8);
        var encrypted = semaphoreRotationCipherAttempt(word, kw, _invert);
        if (encrypted == null)
            goto tryAgain;
        logMessages.Add(string.Format("Keyword = {0}", kw));
        logMessages.Add(string.Format("Encrypted = {0}", encrypted));
        return new ResultInfo
        {
            Encrypted = encrypted,
            LogMessages = logMessages,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw }, _invert) }
        };
    }

    private string semaphoreRotationCipherAttempt(string word, string kw, bool invert)
    {
        var encrypted = "";
        for (var i = 0; i < word.Length; i++)
        {
            var rotated = _semaphores[word[i] - 'A'].Select(j => (invert ? ((j - (kw[i % kw.Length] - 'A' + 1)) % 8 + 8) % 8 : j + kw[i % kw.Length] - 'A' + 1) % 8).ToArray();
            Array.Sort(rotated);
            var letter = Array.FindIndex(_semaphores, s => s.SequenceEqual(rotated));
            if (letter == -1)
                return null;
            encrypted += (char) ('A' + letter);
        }
        return encrypted;
    }
}
