using System.Collections.Generic;
using CipherMachine;
using UnityEngine;
using Words;

public class CompositeSpinningJumpingLeapfrogOrphanageCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Composite Spinning/Jumping Leapfrog Orphanage Cipher" : "Composite Spinning/Jumping Leapfrog Orphanage Cipher"; } }
    public override int Score(int wordLength) { return 6; }
    public override string Code { get { return "JL"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public CompositeSpinningJumpingLeapfrogOrphanageCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string replaceX = "";
        string orphanage = "ABCDEFGHIJKLMNOPQRSTUVWYZ", encrypt = "";
        logMessages.Add(string.Format("Before Replacing Xs: {0}", word));
        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == 'X')
            {
                word = word.Substring(0, i) + "" + orphanage[Random.Range(0, orphanage.Length)] + "" + word.Substring(i + 1);
                replaceX = replaceX + "" + word[i];
            }
            else
                replaceX = replaceX + "" + orphanage.Replace(word[i].ToString(), "")[Random.Range(0, 24)];
        }
        logMessages.Add(string.Format("After Replacing Xs: {0}", word));
        string orphans = new string("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Shuffle()).Substring(0, 4);
        logMessages.Add(string.Format("Orphans: {0}", orphans));
        foreach (char orphan in orphans)
        {
            int[][] swaps = getSwaps(orphan);
            foreach(int[] swap in swaps)
            {
                char c1 = orphanage[swap[0]], c2 = orphanage[swap[1]];
                orphanage = orphanage.Replace(c1, '*').Replace(c2, c1).Replace('*', c2);
            }
            logMessages.Add(string.Format("{0} -> {1}", orphan, orphanage));
        }
        if (invert)
        {
            for (int i = 0; i < word.Length / 2; i++)
            {
                if (word[i * 2] == word[i * 2 + 1])
                {
                    encrypt = encrypt + "" + orphanage[24 - orphanage.IndexOf(word[i * 2])];
                    encrypt = encrypt + "" + encrypt[encrypt.Length - 1];
                }
                else
                {
                    encrypt = encrypt + "" + JumpOver(orphanage, word[i * 2], word[i * 2 + 1]);
                    encrypt = encrypt + "" + JumpOver(orphanage, word[i * 2 + 1], encrypt[encrypt.Length - 1]);
                }
                logMessages.Add(string.Format("{0}{1} -> {2}{3}", word[i * 2], word[i * 2 + 1], encrypt[i * 2], encrypt[i * 2 + 1]));
            }
        }
        else
        {
            for(int i = 0; i < word.Length / 2; i++)
            {
                if(word[i * 2] == word[i * 2 + 1])
                {
                    encrypt = encrypt + "" + orphanage[24 - orphanage.IndexOf(word[i * 2])];
                    encrypt = encrypt + "" + encrypt[encrypt.Length - 1];
                }
                else
                {
                    encrypt = encrypt + "" + JumpOver(orphanage, word[i * 2 + 1], word[i * 2]);
                    encrypt = encrypt + "" + JumpOver(orphanage, word[i * 2], encrypt[encrypt.Length - 1]);
                    encrypt = encrypt.Substring(0, encrypt.Length - 2) + "" + encrypt[encrypt.Length - 1] + "" + encrypt[encrypt.Length - 2];
                }
                logMessages.Add(string.Format("{0}{1} -> {2}{3}", word[i * 2], word[i * 2 + 1], encrypt[i * 2], encrypt[i * 2 + 1]));
            }
        }
        if (word.Length % 2 == 1)
            encrypt = encrypt + "" + word[word.Length - 1];
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { orphans, null, replaceX }, invert) }
        };
    }
    private char JumpOver(string orphanage, char a, char b)
    {
        int aX = orphanage.IndexOf(a) % 5, aY = orphanage.IndexOf(a) / 5;
        int bX = orphanage.IndexOf(b) % 5, bY = orphanage.IndexOf(b) / 5;
        int cX, cY;
        if(a == b)
        {
            cX = 4 - aX;
            cY = 4 - bY;
        }
        else
        {
            int xChange = CMTools.mod(bX - aX, 5);
            int yChange = CMTools.mod(bY - aY, 5);
            cX = (bX + xChange) % 5;
            cY = (bY + yChange) % 5;
        }
        return orphanage[5 * cY + cX];
    }
    private int[][] getSwaps(char c)
    {
        switch(c)
        {
            case 'A':
                return new int[][] { new int[]{ 8, 15 }, new int[] { 7, 16}, new int[] { 11, 12 } };
            case 'B':
                return new int[][] { new int[] { 12, 23 }, new int[] { 13, 22 }, new int[] { 17, 18 } };
            case 'C':
                return new int[][] { new int[] { 6, 18 }, new int[] { 7, 17 }, new int[] { 8, 16 }, new int[] { 10, 14 } };
            case 'D':
                return new int[][] { new int[] { 0, 23 }, new int[] { 3, 20 } };
            case 'E':
                return new int[][] { new int[] { 3, 13 }, new int[] { 7, 9 } };
            case 'F':
                return new int[][] { new int[] { 7, 19 }, new int[] { 8, 18 } };
            case 'G':
                return new int[][] { new int[] { 6, 19 }, new int[] { 11, 14 }, new int[] { 12, 13 } };
            case 'H':
                return new int[][] { new int[] { 5, 17 }, new int[] { 7, 15 }, new int[] { 10, 12 } };
            case 'I':
                return new int[][] { new int[] { 3, 18 }, new int[] { 8, 13 } };
            case 'J':
                return new int[][] { new int[] { 1, 18 }, new int[] { 2, 17 }, new int[] { 3, 16 } };
            case 'K':
                return new int[][] { new int[] { 2, 16 }, new int[] { 5, 13 } };
            case 'L':
                return new int[][] { new int[] { 3, 16 }, new int[] { 6, 13 } };
            case 'M':
                return new int[][] { new int[] { 4, 20 }, new int[] { 7, 17 }, new int[] { 10, 14 } };
            case 'N':
                return new int[][] { new int[] { 5, 23 }, new int[] { 8, 20 }, new int[] { 11, 17 } };
            case 'O':
                return new int[][] { new int[] { 12, 24 }, new int[] { 13, 23 }, new int[] { 14, 22 }, new int[] { 17, 19 } };
            case 'P':
                return new int[][] { new int[] { 2, 13 }, new int[] { 4, 11 } };
            case 'Q':
                return new int[][] { new int[] { 5, 23 }, new int[] { 10, 18 }, new int[] { 13, 15 } };
            case 'R':
                return new int[][] { new int[] { 6, 16 }, new int[] { 7, 15 }, new int[] { 10, 12 } };
            case 'S':
                return new int[][] { new int[] { 2, 22 }, new int[] { 3, 21 }, new int[] { 6, 18 } };
            case 'T':
                return new int[][] { new int[] { 1, 21 }, new int[] { 6, 16 }, new int[] { 10, 12 } };
            case 'U':
                return new int[][] { new int[] { 1, 22 }, new int[] { 3, 20 }, new int[] { 7, 16 } };
            case 'V':
                return new int[][] { new int[] { 0, 24 } };
            case 'W':
                return new int[][] { new int[] { 11, 22 }, new int[] { 15, 18 } };
            case 'X':
                return new int[][] { new int[] { 6, 18 }, new int[] { 8, 16 } };
            case 'Y':
                return new int[][] { new int[] { 5, 13 }, new int[] { 6, 12 }, new int[] { 7, 11 } };
            case 'Z':
                return new int[][] { new int[] { 1, 23 }, new int[] { 2, 22 }, new int[] { 3, 21 }, new int[] { 4, 20 } };
        }
        return null;
    }
}
