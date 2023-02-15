using System.Collections.Generic;
using CipherMachine;
using UnityEngine;

public class ChainRotationCipher : CipherBase
{
    public override string Name { get { return _invert ? "Inverted Chain-Rotation Cipher" : "Chain-Rotation Cipher"; } }
    public override int Score(int wordLength) { return 7; }
    public override string Code { get { return "CR"; } }

    private readonly bool _invert;
    public override bool IsInvert { get { return _invert; } }
    public ChainRotationCipher(bool invert) { _invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var n = Random.Range(1, 10);

        var logMessages = new List<string>();
        logMessages.Add(string.Format("Amount: {0}", n));

        var encrypted = "";
        if (_invert)
        {
            logMessages.Add(string.Format("Before Chain Rotation Cipher: {0}", word));
            while (word.Length > 0)
            {
                var amt = n % word.Length;
                word = word.Substring(amt) + word.Substring(0, amt);
                var obt = word[0];
                word = word.Substring(1);
                if (encrypted.Length > 0)
                    obt = (char) ((obt - 'A' + encrypted[encrypted.Length - 1] - 'A' + 1) % 26 + 'A');
                encrypted += obt;
                logMessages.Add(string.Format("{0} -> {1}", word, encrypted));
            }
        }
        else
        {
            while (word.Length > 0)
            {
                var obt = word[word.Length - 1];
                word = word.Remove(word.Length - 1);
                if (word.Length > 0)
                    obt = (char) ('A' + (obt - 'A' + 52 - (word[word.Length - 1] - 'A' + 1)) % 26);
                encrypted = obt + encrypted;
                var amt = n % encrypted.Length;
                encrypted = encrypted.Substring(encrypted.Length - amt) + encrypted.Substring(0, encrypted.Length - amt);
                logMessages.Add(string.Format("{0} -> {1}", word, encrypted));
            }
        }

        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypted,
            Pages = new PageInfo[] { new PageInfo(new ScreenInfo[] { n.ToString() }, _invert) }
        };
    }
}