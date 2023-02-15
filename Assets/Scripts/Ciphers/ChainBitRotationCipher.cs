using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class ChainBitRotationCipher : CipherBase
{
    public override string Name { get { return _invert ? "Inverted Chain Bit-Rotation Cipher" : "Chain Bit-Rotation Cipher"; } }
    public override int Score(int wordLength) { return 6; }
    public override string Code { get { return "CB"; } }

    private readonly bool _invert;
    public override bool IsInvert { get { return _invert; } }
    public ChainBitRotationCipher(bool invert) { _invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        return _invert ? encryptInverted(word) : encryptNormal(word);
    }

    private ResultInfo encryptNormal(string word)
    {
        var wordList = new Data();
        var logMessages = new List<string>();

        var kw = wordList.PickWord(3, word.Length);
        long number = 0L;
        string encrypted = "";
        logMessages.Add(string.Format("Chain Bit-Rotation Cipher: encrypting {0} with keyword {1}; start with 0", word, kw));

        for (var i = word.Length - 1; i >= 0; i--)
        {
            // Process letter
            var letter = word[i];
            var addition = letter - 'A' + 1;
            number = (number << 5) + addition;
            logMessages.Add(string.Format("Shift in {0} ({1}) = {2}", addition, letter, Convert.ToString(number, 2)));

            // Bit rotation
            var nb = (word.Length - i) * 5;
            var amt = kw[i % kw.Length] - 'A' + 1;
            number = ((((number >> (nb - (amt % nb))) & ((1L << (amt % nb)) - 1)) | (number << (amt % nb))) & ((1L << nb) - 1)) | (number & ~((1L << nb) - 1));
            var numberStr = Convert.ToString(number, 2).PadLeft(nb, '0');
            logMessages.Add(string.Format("Rotating last {0} bits left {1} = {2}]", nb, amt, numberStr.Insert(numberStr.Length - nb, "[")));
        }
        for (var i = 0; i < word.Length; i++)
        {
            var num = number % 26;
            encrypted = (char) (num == 0 ? 'Z' : num + 'A' - 1) + encrypted;
            number /= 26;
        }

        logMessages.Add(string.Format("Chain Bit-Rotation Cipher: encrypted: {0}; number: {1}", encrypted, number));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypted,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, number.ToString() }) }
        };
    }

    private ResultInfo encryptInverted(string word)
    {
        var wordList = new Data();
        var logMessages = new List<string>();

        tryAgain2:
        long residue = 0;
        var kw = wordList.PickWord(4, 8);

        tryAgain1:
        var encrypted = "";
        logMessages.Clear();
        logMessages.Add(string.Format("Chain Bit-Rotation Cipher: encrypting {0} with keyword {1}; start with {2}", word, kw, residue));
        var number = word.Aggregate(residue, (p, n) => p * 26 + (n == 'Z' ? 0L : (n - 'A' + 1)));
        logMessages.Add(string.Format("Chain Bit-Rotation Cipher: binary: {0}", Convert.ToString(number, 2)));
        for (var i = 0; i < word.Length; i++)
        {
            // Bit rotation
            var nb = (word.Length - i) * 5;
            var amtRaw = kw[i % kw.Length] - 'A' + 1;
            var amt = amtRaw % nb;
            number = (((number >> amt) & ((1L << (nb - amt)) - 1)) | (number << (nb - amt))) & ((1L << nb) - 1) | (number & ~((1L << nb) - 1));
            var numberStr = Convert.ToString(number, 2).PadLeft(nb, '0');
            logMessages.Add(string.Format("Rotate {0} bits right by {1} (= {2}) = {3}]", nb, kw[i % kw.Length], amtRaw, numberStr.Insert(numberStr.Length - nb, "[")));
            // Extract a letter
            var extracted = (int) (number & 0x1f);
            if (extracted < 1 || extracted > 26)
            {
                residue++;
                if (residue > 16)
                    goto tryAgain2;
                goto tryAgain1;
            }
            encrypted += (char) (extracted + 'A' - 1);
            number >>= 5;
            logMessages.Add(string.Format("Extracted letter: {0}; remaining bits = {1}", encrypted.Last(), Convert.ToString(number, 2)));
        }

        logMessages.Add(string.Format("Chain Bit-Rotation Cipher: encrypted: {0}; number: {1}", encrypted, number));

        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypted,
            Pages = new PageInfo[] { new PageInfo(new ScreenInfo[] { kw, number.ToString() }, invert: true) }
        };
    }
}
