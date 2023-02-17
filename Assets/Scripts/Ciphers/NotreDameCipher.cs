using System;
using System.Collections.Generic;
using CipherMachine;
using Words;

public class NotreDameCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Notre-Dame Cipher" : "Notre-Dame Cipher"; } }
    public override string Code { get { return "ND"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public NotreDameCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "ABCDEFGHIJKLMNOPZRSTUVWXY", encrypt = "", replaceQ = "";
        logMessages.Add(string.Format("Before Replacing Qs: {0}", word));
        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == 'Q')
            {
                word = word.Substring(0, i) + "" + alpha[UnityEngine.Random.Range(0, alpha.Length)] + "" + word.Substring(i + 1);
                replaceQ = replaceQ + "" + word[i];
            }
            else
                replaceQ = replaceQ + "" + alpha.Replace(word[i].ToString(), "")[UnityEngine.Random.Range(0, 24)];
        }
        logMessages.Add(string.Format("After Replacing Qs: {0}", word));
        string kw = new Data().PickWord(4, 8);
        var kwfront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw.Replace("Q", "Z"), alpha, kwfront.Value);
        char[,] matrix = new char[5, 5];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
                matrix[i, j] = key[i * 5 + j];
        }
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", kwfront.Expression, kwfront.Value));
        logMessages.Add(string.Format("Key: {0}", key));
        //Rosace Cipher
        List<string> poss = new List<string>{
            "A1", "B1", "C1", "D1", "E1",
            "A2", "B2", "C2", "D2", "E2",
            "A3", "B3", "C3", "D3", "E3",
            "A4", "B4", "C4", "D4", "E4",
            "A5", "B5", "C5", "D5", "E5"
        };
        string[] rosaceCoords = new string[3], vitrailNums = new string[3], crossNums = new string[3];
        for (int i = 0; i < 3; i++)
        {
            string choice = poss[UnityEngine.Random.Range(0, poss.Count)];
            int col = "ABCDE".IndexOf(choice[0]), row = "12345".IndexOf(choice[1]);
            poss.Remove(choice);
            poss.Remove("ABCDE"[4 - row] + "" + "12345"[col]);
            poss.Remove("ABCDE"[4 - col] + "" + "12345"[4 - row]);
            poss.Remove("ABCDE"[row] + "" + "12345"[4 - col]);
            choice = choice + "" + "><"[UnityEngine.Random.Range(0, 2)];
            matrix = RosaceCipher(matrix, row - 2, col - 2, choice[2] == '>');
            logMessages.Add(string.Format("{0} -> {1}", choice, toString(matrix)));
            rosaceCoords[i] = choice.ToUpperInvariant();
        }
        for (int i = 0; i < 3; i++)
        {
            string choice = new string("12345".ToCharArray().Shuffle()).Substring(0, 2) + "" + UnityEngine.Random.Range(1, 6);
            matrix = VitrailCipher(matrix, "12345".IndexOf(choice[0]), "12345".IndexOf(choice[1]), "012345".IndexOf(choice[2]));
            logMessages.Add(string.Format("{0} -> {1}", choice, toString(matrix)));
            vitrailNums[i] = choice.ToUpperInvariant();
        }
        poss = new List<string>{
            "11", "21", "31", "41", "51",
            "12", "22", "32", "42", "52",
            "13", "23", "33", "43", "53",
            "14", "24", "34", "44", "54",
            "15", "25", "35", "45", "55"
        };
        for (int i = 0; i < 3; i++)
        {
            string choice = poss[UnityEngine.Random.Range(0, poss.Count)];
            poss.Remove(choice);
            matrix = CrossCipher(matrix, "12345".IndexOf(choice[0]), "12345".IndexOf(choice[1]));
            logMessages.Add(string.Format("{0} -> {1}", choice, toString(matrix)));
            crossNums[i] = choice.ToUpperInvariant();
        }
        string finalKey = toString(matrix);
        if (invert)
        {
            foreach (char c in word)
                encrypt = encrypt + "" + key[finalKey.IndexOf(c)];
        }
        else
        {
            foreach (char c in word)
                encrypt = encrypt + "" + finalKey[key.IndexOf(c)];
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, kwfront.Expression, rosaceCoords[0].Substring(0, 2), rosaceCoords[0].Substring(2), rosaceCoords[1].Substring(0, 2), rosaceCoords[1].Substring(2), rosaceCoords[2].Substring(0, 2), rosaceCoords[2].Substring(2) }, invert), new PageInfo(new ScreenInfo[] { vitrailNums[0], crossNums[0], vitrailNums[1], crossNums[1], vitrailNums[2], crossNums[2], replaceQ }, invert) },
            Score = 13
        };
    }
    private char[,] RosaceCipher(char[,] matrix, int rowchange, int colchange, bool clock)
    {

        if (clock)
        {
            //Clockwise
            char reserve = matrix[(2 + rowchange) % 5, (2 + colchange) % 5]; //Stpck 1
            matrix[(2 + rowchange) % 5, (2 + colchange) % 5] = matrix[(2 - colchange) % 5, (2 + rowchange) % 5]; //4 va dans 1
            matrix[(2 - colchange) % 5, (2 + rowchange) % 5] = matrix[(2 - rowchange) % 5, (2 - colchange) % 5]; //3 va dans 4
            matrix[(2 - rowchange) % 5, (2 - colchange) % 5] = matrix[(2 + colchange) % 5, (2 - rowchange) % 5]; //(2 va dans 3
            matrix[(2 + colchange) % 5, (2 - rowchange) % 5] = reserve; //Reserve dans (2
        }
        else
        {
            //Counter
            char reserve = matrix[(2 + rowchange) % 5, (2 + colchange) % 5];
            matrix[(2 + rowchange) % 5, (2 + colchange) % 5] = matrix[(2 + colchange) % 5, (2 - rowchange) % 5];
            matrix[(2 + colchange) % 5, (2 - rowchange) % 5] = matrix[(2 - rowchange) % 5, (2 - colchange) % 5];
            matrix[(2 - rowchange) % 5, (2 - colchange) % 5] = matrix[(2 - colchange) % 5, (2 + rowchange) % 5];
            matrix[(2 - colchange) % 5, (2 + rowchange) % 5] = reserve;
        }

        return matrix;
    }

    private char[,] VitrailCipher(char[,] matrix, int colstart, int colend, int overflow)
    {
        Queue<char> queuein = new Queue<char>();
        Queue<char> queueout = new Queue<char>();
        for (int i = 0; i < overflow; i++)
        {
            queuein.Enqueue(matrix[i, colstart]);
            queueout.Enqueue(matrix[4 - i, colend]);
        }
        for (int i = 4; i >= 0; i--)
        {
            matrix[i, colend] = queuein.Count == i + 1 ? queuein.Dequeue() : matrix[i - queuein.Count, colend];
            matrix[4 - i, colstart] = queueout.Count == i + 1 ? queueout.Dequeue() : matrix[4 - i + queueout.Count, colstart];
        }
        return matrix;
    }

    private char[,] CrossCipher(char[,] matrix, int colused, int rowused)
    {
        Queue<char> rowData = new Queue<char>();
        Queue<char> colData = new Queue<char>();
        for (int i = 0; i < 5; i++)
        {
            if (!(i == colused)) rowData.Enqueue(matrix[rowused, i]);
            if (!(i == rowused)) colData.Enqueue(matrix[i, colused]);
        }
        for (int i = 0; i < 5; i++)
        {
            if (!(i == colused)) matrix[rowused, i] = colData.Dequeue();
            if (!(i == rowused)) matrix[i, colused] = rowData.Dequeue();
        }
        return matrix;
    }
    private string toString(char[,] matrix)
    {
        string s = "";
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
                s = s + "" + matrix[i, j];
        }
        return s;
    }
}
