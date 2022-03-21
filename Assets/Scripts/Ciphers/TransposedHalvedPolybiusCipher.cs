using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class TransposedHalvedPolybiusCipher
{
    public ResultInfo encrypt(string word, string id, string log, KMBombInfo Bomb, bool invert)
    {
        Debug.LogFormat("{0} Begin Transposed Halved Polybius Cipher", log);
        var wordList = new Data();
        var kwa = wordList.PickWord(4, 8);
        var kwb = wordList.PickWord(12 - word.Length);
        string[] coords = { "", "", "" }, kwfront = CMTools.generateBoolExp(Bomb);
        string key = CMTools.getKey(kwa, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront[1][0] == 'T');
        key = key.Substring(0, 5) + key.Substring(13, 5) + key.Substring(5, 5) + key.Substring(18, 5) + key.Substring(10, 3) + "##" + key.Substring(23) + "##";
        for (int i = 0; i < word.Length; i++)
        {
            int index = key.IndexOf(word[i]);
            coords[0] = coords[0] + "" + ("LR"[(index % 10) / 5]);
            coords[1] = coords[1] + "" + (index / 10 + 1);
            coords[2] = coords[2] + "" + ((index % 5) + 1);
        }
        Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] Screen 1: {1}", log, kwa);
        Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] Screen 2: {1}", log, kwb);
        Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] Key: {1} -> {2} -> {3}", log, kwfront[0], kwfront[1], key);
        Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt"); ;
        Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] {1}", log, coords[0]);
        Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] {1}", log, coords[1]);
        Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] {1}", log, coords[2]);
        Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] --------", log);
        string encrypt = "";
        if (invert)
        {
            for (int i = 0; i < kwb.Length; i++)
            {
                int index = key.IndexOf(kwb[i]);
                int row = index / 10;
                int col = index % 5;
                if (i % 2 == 0)
                    coords[row] = shiftLets(coords[row], col + 1, true, new string[] { "", "3", "45" }[row]);
                else
                    coords = swapCol(coords, col % word.Length, (col + row + 1) % word.Length);
                Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] {1}", log, coords[0]);
                Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] {1}", log, coords[1]);
                Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] {1}", log, coords[2]);
                Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] --------", log);
            }
        }
        else
        {
            for (int i = kwb.Length - 1; i >= 0; i--)
            {
                int index = key.IndexOf(kwb[i]);
                int row = index / 10;
                int col = index % 5;
                if (i % 2 == 0)
                    coords[row] = shiftLets(coords[row], col + 1, false, new string[] { "", "3", "45" }[row]);
                else
                    coords = swapCol(coords, col % word.Length, (col + row + 1) % word.Length);
                Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] {1}", log, coords[0]);
                Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] {1}", log, coords[1]);
                Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] {1}", log, coords[2]);
                Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] --------", log);
            }
        }
        for (int i = 0; i < word.Length; i++)
        {
            int index = ("123".IndexOf(coords[1][i]) * 10) + ("LR".IndexOf(coords[0][i]) * 5) + ("12345".IndexOf(coords[2][i]));
            encrypt = encrypt + "" + key[index];
        }
        Debug.LogFormat("{0} [Transposed Halved Polybius Cipher] {1} -> {2}", log, word, encrypt);
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(kwa, new int[] { 35, 35, 35, 32, 28 }[kwa.Length - 4]);
        screens[1] = new ScreenInfo(kwfront[0], 25);
        screens[2] = new ScreenInfo(kwb, new int[] { 35, 35, 35, 32, 28 }[kwb.Length - 4]);
        screens[8] = new ScreenInfo(id, 35);
        return new ResultInfo
        {
            Encrypted = encrypt,
            Score = 5,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
    private string shiftLets(string lets, int shift, bool invert, string replace)
    {
        string temp = lets.ToUpperInvariant();
        for (int i = 0; i < replace.Length; i++)
            temp = temp.Replace(replace[i] + "", "");
        if (temp.Length > 1)
        {
            shift = shift % temp.Length;
            if (!(invert))
                shift = (temp.Length - shift) % temp.Length;
            temp = temp.Substring(shift) + temp.Substring(0, shift);
            int cur = 0;
            for (int i = 0; i < lets.Length; i++)
            {
                if (!(replace.Contains(lets[i] + "")))
                    lets = lets.Substring(0, i) + temp[cur++] + lets.Substring(i + 1);
            }
        }
        return lets;
    }
    private string[] swapCol(string[] coords, int c1, int c2)
    {
        string t1 = coords[0][c1] + "" + coords[1][c1] + "" + coords[2][c1];
        string t2 = coords[0][c2] + "" + coords[1][c2] + "" + coords[2][c2];
        for (int i = 0; i < 3; i++)
        {
            coords[i] = coords[i].Substring(0, c1) + t2[i] + coords[i].Substring(c1 + 1);
            coords[i] = coords[i].Substring(0, c2) + t1[i] + coords[i].Substring(c2 + 1);
        }
        return coords;
    }
}
