using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using KModkit;
using Words;

public class StuntedBlindPolybiusCipher : CipherBase
{
    public override string Name { get { return _invert ? "Inverted Stunted Blind Polybius Cipher" : "Stunted Blind Polybius Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "SB"; } }

    private readonly bool _invert;
    public StuntedBlindPolybiusCipher(bool invert) { _invert = invert; }

    private static readonly string[] _brailleDots = { "1", "12", "14", "145", "15", "124", "1245", "125", "24", "245", "13", "123", "134", "1345", "135", "1234", "12345", "1235", "234", "2345", "136", "1236", "2456", "1346", "13456", "1356" };

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var bitsInt = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(bomb.GetSerialNumber().First());
        var bit0 = (bitsInt & 1) != 0;
        var logMessages = new List<string>();
        var wordList = new Data();

        if (_invert)
        {
            // To get this to work, we may need to try multiple keywords until we find one where the encoded dot patterns make valid Braille letters.
            tryAgain:
            var kw = wordList.PickWord(8);
            var colSeq = sequencing(kw.Substring(0, 4));
            var rowSeq = sequencing(kw.Substring(4));
            var polybius = (bit0 ? (kw + "ABCDEFGHIJKLMNOP") : "ABCDEFGHIJKLMNOP".Except(kw).Concat(kw)).Distinct().Where(ch => ch <= 'P').Join("");

            // To get all letters into the range A–P, we must ROT13 letters that are out of range.
            // The letters ABCNOP can optionally be ROT13ed. This loop goes through all possible combinations.
            var numInd = word.Count("ABCNOP".Contains);
            for (var rot13bits = 0; rot13bits < (1 << numInd); rot13bits++)
            {
                // Braille dots in reading order.
                // The first 4×word.Length dots are determined by the encoded dot patterns.
                // The right column of the rest is determined by which letters have been ROT13ed.
                var brailleBits = new bool?[6 * word.Length];

                // ROT13 the letters that we need to or chose to ROT13.
                // This sets the right column at the bottom of brailleBits.
                var r = 0;
                var rot13ed = "";
                for (var i = 0; i < word.Length; i++)
                {
                    if ("ABCNOP".Contains(word[i]))
                    {
                        var isBit = (rot13bits & (1 << r)) != 0;
                        rot13ed += isBit ? (char) ((word[i] - 'A' + 13) % 26 + 'A') : word[i];
                        brailleBits[4 * word.Length + 2 * i + 1] = isBit;
                        r++;
                    }
                    else if (word[i] > 'P')
                    {
                        rot13ed += (char) (word[i] - 13);
                        brailleBits[4 * word.Length + 2 * i + 1] = true;
                    }
                    else
                    {
                        rot13ed += word[i];
                        brailleBits[4 * word.Length + 2 * i + 1] = false;
                    }
                }

                // Encode the (now possibly ROT13ed) letters using the Polybius square and set all the relevant nibbles in brailleBits.
                for (int i = 0; i < rot13ed.Length; i++)
                {
                    var ch = rot13ed[i];
                    var polybiusIx = polybius.IndexOf(ch);
                    var stuntedIx = Array.IndexOf(colSeq, polybiusIx % 4) + 4 * Array.IndexOf(rowSeq, polybiusIx / 4);
                    for (var j = 0; j < 4; j++)
                        brailleBits[j + 4 * i] = (stuntedIx & (1 << j)) != 0;
                }

                // Now go through brailleBits 6 bits at a time. For each such chunk, find which Braille letters that fit that pattern.
                var encrypted = "";
                var brailleCandidates = new List<int>(8);
                for (var i = 0; i < word.Length; i++)
                {
                    brailleCandidates.Clear();
                    for (var ltr = 0; ltr < _brailleDots.Length; ltr++)
                        if (
                            (brailleBits[6 * i + 0] == null || brailleBits[6 * i + 0].Value == _brailleDots[ltr].Contains('1')) &&
                            (brailleBits[6 * i + 2] == null || brailleBits[6 * i + 2].Value == _brailleDots[ltr].Contains('2')) &&
                            (brailleBits[6 * i + 4] == null || brailleBits[6 * i + 4].Value == _brailleDots[ltr].Contains('3')) &&
                            (brailleBits[6 * i + 1] == null || brailleBits[6 * i + 1].Value == _brailleDots[ltr].Contains('4')) &&
                            (brailleBits[6 * i + 3] == null || brailleBits[6 * i + 3].Value == _brailleDots[ltr].Contains('5')) &&
                            (brailleBits[6 * i + 5] == null || brailleBits[6 * i + 5].Value == _brailleDots[ltr].Contains('6')))
                            brailleCandidates.Add(ltr);

                    if (brailleCandidates.Count == 0)
                        // No Braille letter fits this pattern — we have to try a different ROT13 combination or a different keyword.
                        goto busted;

                    encrypted += (char) ('A' + brailleCandidates.PickRandom());
                }

                logMessages.Add(string.Format("After ROT-13: {0}", rot13ed));
                logMessages.Add(string.Format("Keyword: {0}", kw));
                logMessages.Add(string.Format("Blind Polybius columns: {0}; rows: {1}", colSeq.Select(i => i + 1).Join(""), rowSeq.Select(i => i + 1).Join("")));
                logMessages.Add(string.Format("Stunted Polybius square: {0}", polybius));
                logMessages.Add(string.Format("Braille: {0}", encrypted.Select(ch => "⠁⠃⠉⠙⠑⠋⠛⠓⠊⠚⠅⠇⠍⠝⠕⠏⠟⠗⠎⠞⠥⠧⠺⠭⠽⠵"[ch - 'A']).Join("")));
                logMessages.Add(string.Format("Encrypted word: {0}", encrypted));

                return new ResultInfo
                {
                    LogMessages = logMessages,
                    Encrypted = encrypted,
                    Pages = new PageInfo[] { new PageInfo(new[] { new ScreenInfo(kw, 0) }, _invert) }
                };

                busted:;
            }
            goto tryAgain;
        }
        else
        {
            var kw = wordList.PickWord(8);
            logMessages.Add(string.Format("Braille: {0}", toBraille(word)));

            // “nibble” = 4 bits. Some nibbles are a 2×2 square within a Braille letter, some are the bottom 2 dots of one Braille letter and the top 2 dots of the next
            var brailleNibbles = new int[(word.Length * 3 + 1) / 2];
            for (int i = 0; i < word.Length; i++)
                foreach (var dotNum in _brailleDots[word[i] - 'A'])
                    brailleNibbles[((dotNum - '1') % 3 + 3 * i) / 2] |= 1 << (((dotNum - '1') % 3 + 3 * i) % 2) * 2 + (dotNum - '1') / 3;
            logMessages.Add(string.Format("Braille nibbles: {0}", brailleNibbles.Select(i => "⠀⠁⠈⠉⠂⠃⠊⠋⠐⠑⠘⠙⠒⠓⠚⠛"[i]).Join("")));
            logMessages.Add(string.Format("Keyword: {0}", kw));
            var colSeq = sequencing(kw.Substring(0, 4));
            var rowSeq = sequencing(kw.Substring(4));
            logMessages.Add(string.Format("Blind Polybius columns: {0}; rows: {1}", colSeq.Select(i => i + 1).Join(""), rowSeq.Select(i => i + 1).Join("")));

            var polybius = (bit0 ? (kw + "ABCDEFGHIJKLMNOP") : "ABCDEFGHIJKLMNOP".Except(kw).Concat(kw)).Distinct().Where(ch => ch <= 'P').Join("");
            logMessages.Add(string.Format("Stunted Polybius square: {0}", polybius));

            var encrypted = brailleNibbles.Select(nibble => polybius[colSeq[nibble % 4] + 4 * rowSeq[nibble / 4]]).Join("");
            logMessages.Add(string.Format("Full encrypted word: {0}", encrypted));

            return new ResultInfo
            {
                LogMessages = logMessages,

                // The encrypted word should be the same length as the original; the rest is shown on the module to be appended by the user
                Encrypted = encrypted.Substring(0, word.Length),
                Pages = new PageInfo[] { new PageInfo(new[] { new ScreenInfo(kw, 0), new ScreenInfo(), new ScreenInfo(encrypted.Substring(word.Length), 0) }, _invert) }
            };
        }
    }

    private string toBraille(string word)
    {
        return word.Select(ch => "⠁⠃⠉⠙⠑⠋⠛⠓⠊⠚⠅⠇⠍⠝⠕⠏⠟⠗⠎⠞⠥⠧⠺⠭⠽⠵"[ch - 'A']).Join("");
    }

    private int[] sequencing(string str)
    {
        return str.Select((ch, ix) => str.Count(c => c < ch) + str.Take(ix).Count(c => c == ch)).ToArray();
    }
}