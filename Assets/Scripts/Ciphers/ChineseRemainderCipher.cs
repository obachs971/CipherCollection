using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CipherMachine;
using Rnd = UnityEngine.Random;

public class ChineseRemainderCipher : CipherBase
{
    public override string Name { get { return _invert ? "Inverted Chinese Remainder Cipher" : "Chinese Remainder Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "RM"; } }

    private readonly bool _invert;
    public ChineseRemainderCipher(bool invert) { _invert = invert; }

    private static IEnumerable<int[]> recurseOnly(int[] sofar, int len, int[] goodModuli)
    {
        if (sofar.Length == len)
        {
            yield return sofar;
            yield break;
        }

        for (var i = sofar.Length == 0 ? 0 : Array.IndexOf(goodModuli, sofar.Last()) + 1; i <= goodModuli.Length - len + sofar.Length; i++)
            if (sofar.All(s => ExtendedEuclideanAlgorithm(s, goodModuli[i]).Gcd == 1))
                foreach (var solution in recurseOnly(appendItem(sofar, goodModuli[i]), len, goodModuli))
                    yield return solution;
    }

    private static IEnumerable<int[]> recurseAll(int[] sofar, int len, int[] goodModuli = null, int[] impossibleModuli = null)
    {
        if (sofar.Length == len)
        {
            yield return sofar;
            yield break;
        }

        int offset;
        if (goodModuli != null)
        {
            offset = Rnd.Range(0, goodModuli.Length);
            for (var i = 0; i < goodModuli.Length; i++)
            {
                var ri = goodModuli[(i + offset) % goodModuli.Length];
                if (sofar.All(s => ExtendedEuclideanAlgorithm(s, ri).Gcd == 1))
                    foreach (var solution in recurseAll(appendItem(sofar, ri), len, goodModuli))
                        yield return solution;
            }
        }

        offset = Rnd.Range(0, 26);
        for (var i = 0; i < 26; i++)
        {
            var ri = (i + offset) % 26 + 27;
            if ((goodModuli != null && goodModuli.Contains(ri)) || (impossibleModuli != null && impossibleModuli.Contains(ri)))
                continue;
            if (sofar.All(s => ExtendedEuclideanAlgorithm(s, ri).Gcd == 1))
                foreach (var solution in recurseAll(appendItem(sofar, ri), len, goodModuli))
                    yield return solution;
        }
    }

    private static T[] appendItem<T>(T[] src, T item)
    {
        var newArray = new T[src.Length + 1];
        Array.Copy(src, newArray, src.Length);
        newArray[src.Length] = item;
        return newArray;
    }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        return _invert ? decrypt(word) : encrypt(word);
    }

    private static ResultInfo decrypt(string word)
    {
        var logMessages = new List<string>();

        var v = 0L;
        foreach (var ch in word)
            v = (v * 26) + (ch - 'A' + 1) % 26; // Z=0, A=1..Y=25

        logMessages.Add(string.Format("Value: {0}", v));

        var goodModuli = Enumerable.Range(27, 26).Where(i => v % i >= 1 && v % i <= 26).ToArray().Shuffle();
        var moduli = goodModuli.Length < word.Length ? null : recurseOnly(new int[0], word.Length, goodModuli).FirstOrDefault();
        if (moduli == null)
        {
            var impossibleModuli = Enumerable.Range(27, 26).Where(i => v % i == 0 || v % i > 51).ToArray();
            var bestSoFar = 0;
            for (var iter = 0; moduli == null || (iter < 10 && bestSoFar < word.Length); iter++)
            {
                var moduliTmp = recurseAll(new int[0], word.Length, goodModuli, impossibleModuli).First().Shuffle();
                var remaindersTmp = Enumerable.Range(0, word.Length).Select(ix => (int) (v % moduliTmp[ix])).ToArray();
                if (remaindersTmp.Any(r => r == 0 || r > 51))
                    continue;
                var numValid = remaindersTmp.Count(r => r != 0 && r <= 26);
                if (numValid > bestSoFar)
                {
                    bestSoFar = numValid;
                    moduli = moduliTmp;
                }
            }
        }
        var remainders = Enumerable.Range(0, word.Length).Select(ix => (int) (v % moduli[ix])).ToArray();
        var moduliStr = moduli.Select(c => (char) (c - 27 + 'A')).Join("");
        var offsets = remainders.Select(r => r <= 26 ? 0 : Rnd.Range(r - 26, 27)).ToArray();
        var offsetsStr = offsets.Select(c => c == 0 ? 'Z' : (char) (c + 'A' - 1)).Join("");

        logMessages.Add(string.Format("Moduli: {0} = {1}", moduliStr, moduli.Join(", ")));
        logMessages.Add(string.Format("Offsets: {0} = {1}", offsetsStr, offsets.Join(", ")));
        logMessages.Add(string.Format("Remainders: {0}", remainders.Join(", ")));

        var encrypted = Enumerable.Range(0, word.Length).Select(ix => (char) ('A' + remainders[ix] - offsets[ix] - 1)).Join("");
        return new ResultInfo
        {
            Encrypted = encrypted,
            LogMessages = logMessages,
            Pages = new[] { new PageInfo(new ScreenInfo[] { moduliStr, null, offsetsStr }, invert: true) }
        };
    }

    private static ResultInfo encrypt(string word)
    {
        var logMessages = new List<string>();

        var moduli = recurseAll(new int[0], word.Length).First();
        var moduliStr = moduli.Select(c => (char) (c - 27 + 'A')).Join("");
        logMessages.Add(string.Format("Moduli: {0} = {1}", moduliStr, moduli.Join(", ")));
        var desires = Enumerable.Range(0, word.Length).Select(i => new Desire { Modulus = moduli[i], Value = word[i] - 'A' + 1 }).ToList();

        while (desires.Count > 1)
        {
            var d1 = desires[0];
            var d2 = desires[1];
            var result = ChineseRemainderDeduction(d1.Modulus, d1.Value, d2.Modulus, d2.Value);
            desires.RemoveRange(0, 2);
            desires.Add(new Desire { Modulus = d1.Modulus * d2.Modulus, Value = result });
        }

        var v = desires[0].Value;
        var sb = new StringBuilder();
        var vc = v;
        while (vc > 0)
        {
            sb.Insert(0, vc % 26 == 0 ? 'Z' : (char) ('A' + (vc % 26 - 1)));
            vc /= 26;
        }
        var encrypted = sb.ToString();
        logMessages.Add(string.Format("Encoded value: {0} → {1}", v, encrypted));

        return new ResultInfo
        {
            Encrypted = encrypted.Substring(0, word.Length),
            LogMessages = logMessages,
            Pages = new[] { new PageInfo(new ScreenInfo[] { moduliStr, null, encrypted.Substring(word.Length) }) }
        };
    }

    private struct Desire
    {
        public long Modulus;
        public long Value;
    }

    private struct EeaResult
    {
        public long X, Y, Gcd;
        public EeaResult(long x, long y, long gcd)
        {
            X = x;
            Y = y;
            Gcd = gcd;
        }
    }

    private static void set(out long var1, out long var2, long val1, long val2)
    {
        var1 = val1;
        var2 = val2;
    }

    private static EeaResult ExtendedEuclideanAlgorithm(long a, long b)
    {
        long r1 = a, r2 = b, s1 = 1L, s2 = 0L, t1 = 0L, t2 = 1L;

        while (r2 != 0L)
        {
            var q = r1 / r2;
            set(out r1, out r2, r2, r1 - q * r2);
            set(out s1, out s2, s2, s1 - q * s2);
            set(out t1, out t2, t2, t1 - q * t2);
        }
        return new EeaResult(s1, t1, r1);
    }

    private static long ChineseRemainderDeduction(long m1, long a1, long m2, long a2)
    {
        var result = ExtendedEuclideanAlgorithm(m1, m2);
        var n = m1 * m2;
        return ((a2 * m1 * result.X + a1 * m2 * result.Y) % n + n) % n;
    }
}
