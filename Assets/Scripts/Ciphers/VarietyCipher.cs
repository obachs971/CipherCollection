using System.Collections.Generic;
using System.IO;
using CipherMachine;
using UnityEngine;

public class VarietyCipher : CipherBase
{
    public override string Name { get { return "Variety Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "VA"; } }

    private static readonly int[] _affineMultipliers = { 1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25 };
    private static readonly int[] _affineInverses = { 1, 9, 21, 15, 3, 19, 7, 23, 11, 5, 17, 25 };
    private static readonly string[] _subCipherNames = { "Atbash", "Caesar", "Affine" };

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        ulong n = 0;
        var encrypted = "";
        for (var i = word.Length - 1; i >= 0; i--)
        {
            var numPositions = encrypted.Length + 1;
            var letterPosition = Random.Range(0, numPositions);
            var subcipher = Random.Range(0, 3);
            char enc;
            switch (subcipher)
            {
                case 0: // Atbash
                    enc = (char) (25 - (word[i] - 'A') + 'A');
                    logMessages.Add(string.Format("Atbash: {0} -> {1}", enc, word[i]));
                    break;

                case 1: // Caesar
                    var caesarShift = Random.Range(0, 25);
                    n = (n * 25) + (ulong) caesarShift;
                    enc = (char) ((word[i] - 'A' - (caesarShift + 1) + 26) % 26 + 'A');
                    logMessages.Add(string.Format("Caesar by {2}: {0} -> {1}", enc, word[i], caesarShift + 1));
                    logMessages.Add(string.Format("{0} / 25 = {1}", n, n / 25));
                    logMessages.Add(string.Format("{0} % 25 = {1}", n, caesarShift));
                    break;

                default: // Affine
                    var multiplierIx = Random.Range(0, _affineMultipliers.Length);
                    n = (n * (ulong) _affineMultipliers.Length) + (ulong) multiplierIx;
                    var encInt = ((word[i] - 'A' + 1) * _affineInverses[multiplierIx]) % 26;
                    enc = (char) (encInt == 0 ? 'Z' : encInt + 'A' - 1);
                    logMessages.Add(string.Format("Affine by {2}: {0} -> {1}", enc, word[i], _affineMultipliers[multiplierIx]));
                    logMessages.Add(string.Format("{0} / {2} = {1}", n, n / (ulong) _affineMultipliers.Length, _affineMultipliers.Length));
                    logMessages.Add(string.Format("{0} % {2} = {1}", n, multiplierIx, _affineMultipliers.Length));
                    break;
            }
            n = (n * 3) + (ulong) subcipher;
            logMessages.Add(string.Format("{0} / 3 = {1}", n, n / 3));
            logMessages.Add(string.Format("{0} % 3 = {1} ({2})", n, subcipher, _subCipherNames[subcipher]));
            n = (n * (ulong) numPositions) + (ulong) letterPosition;
            logMessages.Add(string.Format("{0} / {1} = {2}", n, numPositions, n / (ulong) numPositions));
            logMessages.Add(string.Format("{0} % {1} = {2} ({3})", n, numPositions, letterPosition, enc));

            encrypted = encrypted.Insert(letterPosition, enc.ToString());
        }

        var screens = new ScreenInfo[7];
        var nStr = n.ToString();
        var chunkSize = (float) nStr.Length / 4;
        for (var i = 0; i < 4; i++)
        {
            var c1 = Mathf.RoundToInt(chunkSize * i);
            var c2 = Mathf.RoundToInt(chunkSize * (i + 1));
            screens[2 * i] = nStr.Substring(c1, c2 - c1);
        }

        logMessages.Add("Note: this logging follows the normal decoding process (it’s not backwards like the other ciphers).");
        logMessages.Add(string.Format("Encrypted word: {0}; number: {1}", encrypted, n));
        logMessages.Reverse();

        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypted,
            Pages = new[] { new PageInfo(screens) }
        };
    }
}
