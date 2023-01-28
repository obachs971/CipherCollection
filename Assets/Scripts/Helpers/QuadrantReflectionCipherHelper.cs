using KeepCoding;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Words;

internal class QuadrantReflectionCipherHelper
{
    readonly Data words = new Data();

    private readonly Random _random = new Random();

    public int QuadrantSize = 5;
    public int StartingQuadrant;

    private string[][,] _quadrants;
    private string[] _keystrings, _keywords;
    private string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private char[] _removedLetters;
    private bool[] _keystringOrder;

    public QuadrantReflectionCipherHelper()
    {
        _quadrants = new string[4][,];
        StartingQuadrant = _random.Next(0, _quadrants.Length);

        for (int i = 0; i < _quadrants.Length; i++)
            _quadrants[i] = new string[QuadrantSize, QuadrantSize];

        _keystrings = new string[_quadrants.Length];
        _removedLetters = new char[_quadrants.Length];
        _keystringOrder = new bool[_quadrants.Length];

        Initialise();
    }

    private void Initialise()
    {
        // Picks keywords and ignored letters.
        _removedLetters = Enumerable.Range(0, _removedLetters.Length).Select(x => _alphabet.OrderBy(y => _random.Next()).First()).ToArray();
        _keywords = Enumerable.Range(0, _removedLetters.Length).Select(x => words.PickWord(3, 8)).ToArray();
        _keystringOrder = Enumerable.Range(0, _keystringOrder.Length).Select(x => _random.Next(0, 2)).Select(x => x == 1).ToArray();

        // Removes duplicate characters, keeping their first occurrences.
        for (int i = 0; i < _keywords.Length; i++)
        {
            string s = "";

            for (int j = 0; j < _keywords[i].Length; j++)
                if (!s.Contains(_keywords[i][j].ToString()))
                    s += _keywords[i][j].ToString();

            _keystrings[i] = s;

            // Removes ignored letter from keyword.
            _keystrings[i] = Regex.Replace(_keystrings[i], _removedLetters[i].ToString(), "");
        }

        // Creates keystrings from keywords.
        for (int i = 0; i < _keystrings.Length; i++)
        {
            string a = _alphabet;
            a = Regex.Replace(a, "[" + _keystrings[i] + _removedLetters[i] + "]", "");

            if (_keystringOrder[i])
                _keystrings[i] = a + _keystrings[i];
            else
                _keystrings[i] = _keystrings[i] + a;
        }

        // Fills quadrants with keystrings.
        for (int i = 0; i < _quadrants.Length; i++)
        {
            for (int j = 0; j < QuadrantSize; j++)
                for (int k = 0; k < QuadrantSize; k++)
                    _quadrants[i][j, k] = _keystrings[i][5 * j + k].ToString();
        }
    }

    public string Encrypt(string word)
    {
        string result = "";
        int currentQuadrant = StartingQuadrant;

        for (int i = 0; i < word.Length; i++)
        {
            int[] position = Find(_quadrants[currentQuadrant], word[i].ToString());

            if (position[0] == -1)
                result += word[i].ToString();
            else
            {
                if (currentQuadrant % 2 == 0)
                    result += _quadrants[PositiveModulo(currentQuadrant - 1, _quadrants.Length)][QuadrantSize - 1 - position[0], position[1]];
                else
                    result += _quadrants[PositiveModulo(currentQuadrant - 1, _quadrants.Length)][position[0], QuadrantSize - 1 - position[1]];
            }

            currentQuadrant = PositiveModulo(currentQuadrant + 1, _quadrants.Length);
        }

        return result;
    }

    public string Decrypt(string word)
    {
        string result = "";
        int currentQuadrant = PositiveModulo(StartingQuadrant - 1, _quadrants.Length);

        for (int i = 0; i < word.Length; i++)
        {
            int[] position = Find(_quadrants[currentQuadrant], word[i].ToString());

            if (position[0] == -1)
                result += word[i].ToString();
            else
            {
                if (currentQuadrant % 2 == 1)
                    result += _quadrants[PositiveModulo(currentQuadrant + 1, _quadrants.Length)][QuadrantSize - 1 - position[0], position[1]];
                else
                    result += _quadrants[PositiveModulo(currentQuadrant + 1, _quadrants.Length)][position[0], QuadrantSize - 1 - position[1]];
            }

            currentQuadrant = PositiveModulo(currentQuadrant - 1, _quadrants.Length);
        }

        return result;
    }

    public string[] GetQuadrants()
    {
        return _quadrants.Select(x => x.Cast<string>()).ToList().Select(x => x.Join("")).ToArray();
    }

    public string[] GetKeywords()
    {
        return _keywords;
    }

    public string[] GetKeystrings()
    {
        return _keystrings;
    }

    public string[] GetRemovedLetters()
    {
        return _removedLetters.Select(x => x.ToString()).ToArray();
    }

    public bool[] GetKeystringOrder()
    {
        return _keystringOrder;
    }

    private static int PositiveModulo(int s, int modulus)
    {
        return ((s % modulus) + modulus) % modulus;
    }

    private static int[] Find(string[,] quadrant, string letter)
    {
        for (int i = 0; i < (int)Math.Sqrt(quadrant.Length); i++)
            for (int j = 0; j < (int)Math.Sqrt(quadrant.Length); j++)
                if (quadrant[i, j] == letter)
                    return new[] { i, j };
        return new[] { -1, -1 };
    }
}
