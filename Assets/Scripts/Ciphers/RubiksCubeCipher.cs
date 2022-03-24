using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class RubiksCubeCipher : CipherBase
{
    public override string Name { get { return _invert ? "Inverted Rubik’s Cube Cipher" : "Rubik’s Cube Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "RU"; } }

    private readonly bool _invert;
    public RubiksCubeCipher(bool invert) { _invert = invert; }

    // order: U F R B L D
    private static readonly int[][] _rotations = "0 9 17 18 19 11 2 1,0 1 2 5 8 7 6 3,2 11 19 22 25 16 8 5,19 18 17 20 23 24 25 22,17 9 0 3 6 14 23 20,6 7 8 16 25 24 23 14"
        .Split(',').Select(str => str.Split(' ').Select(int.Parse).ToArray()).ToArray();

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        var wordList = new Data();
        var alphaKw = wordList.PickWord(4, 8);
        var rotationsKw = wordList.PickWord(word.Length);
        var encrypted = "";

        var alphabetKey = CMTools.getKey(alphaKw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwFirst: true);
        var specialCubelets = new[] { 10, 4, 13, 21, 12, 15 };
        var cubelets = specialCubelets.Concat(Enumerable.Range(0, 26)).Distinct().ToArray();

        var cube = new char[26];
        for (var i = 0; i < 26; i++)
            cube[cubelets[i]] = alphabetKey[i];
        logMessages.Add(string.Format("Alphabet keyword = {0}; cube before rotations (front | middle | back):", alphaKw));
        for (var row = 0; row < 3; row++)
            logMessages.Add(Enumerable.Range(0, 3).Select(layer => Enumerable.Range(0, 3).Select(col => 9 * layer + 3 * row + col == 13 ? ' ' : cube[9 * layer + 3 * row + col - (9 * layer + 3 * row + col >= 13 ? 1 : 0)]).Join(" ")).Join(" | "));

        logMessages.Add(string.Format("Rotations keyword = {0}:", rotationsKw));
        for (var ix = 0; ix < rotationsKw.Length; ix++)
        {
            var ch = rotationsKw[ix];
            var face = ix / 4;
            var numRot = 2 * (ix % 4) + 1;

            var r = _rotations[face];
            for (var n = 0; n < numRot; n++)
            {
                var f = cube[r.Last()];
                for (var i = r.Length - 1; i > 0; i--)
                    cube[r[i]] = cube[r[i - 1]];
                cube[r[0]] = f;
            }
            logMessages.Add(string.Format("Cube after rotation {0} ({1}×{2}) (front | middle | back):", ch, numRot, "UFRBLD"[face]));
            for (var row = 0; row < 3; row++)
                logMessages.Add(Enumerable.Range(0, 3).Select(layer => Enumerable.Range(0, 3).Select(col => 9 * layer + 3 * row + col == 13 ? ' ' : cube[9 * layer + 3 * row + col - (9 * layer + 3 * row + col >= 13 ? 1 : 0)]).Join(" ")).Join(" | "));
            encrypted += _invert ? (char) (Array.IndexOf(cube, word[ix]) + 'A') : cube[word[ix] - 'A'];
            logMessages.Add(string.Format("Encoding {0} -> {1}", word[ix], encrypted.Last()));
        }
        return new ResultInfo
        {
            Encrypted = encrypted,
            LogMessages = logMessages,
            Pages = new[] { new PageInfo(new[] { new ScreenInfo(alphaKw, 35), new ScreenInfo(), new ScreenInfo(rotationsKw, 40) }, invert: _invert) }
        };
    }
}
