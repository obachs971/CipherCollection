using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using KeepCoding;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class HillCipher : CipherBase
{
	public override string Name { get { return invert ? "Inverted Hill Cipher" : "Hill Cipher"; } }
	public override int Score { get { return 5; } }
	public override string Code { get { return "HI"; } }
    
    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public HillCipher(bool invert) { this.invert = invert; }
    
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        List<string> LogMessages = new List<string>();

        int size = Rnd.Range(2, 5);

        LogMessages.Add("Size of matrix picked: " + size + "x" + size);

        string normal = word;
        string encryptedWord = "";
        string alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY";

        while (normal.Length % size != 0)
            normal += alpha[Rnd.Range(0, alpha.Length)];

        int[] numbers = normal.Select(x => alpha.IndexOf(x)).ToArray();
        List<int> newNumbers = new List<int>();

        LogMessages.Add("Unencrypted word, converted to numbers, and padded: " + numbers.Join(", "));

        Matrix matrix = new Matrix(size, 26);
        do
        {
            for (int i = 0; i < Math.Pow(matrix.Size, 2); i++)
                matrix.SetEntry(i / matrix.Size, i % matrix.Size, Rnd.Range(0, alpha.Length));
        } while (Matrix.GreatestCommonDivisor(matrix.Determinant(), matrix.Modulus) != 1);

        LogMessages.Add("Matrix generated: " + matrix.ToString());

        Matrix inverseMatrix = matrix.InverseMatrix();

        LogMessages.Add("Inverse matrix calculated: " + inverseMatrix.ToString());

        if (invert)
            for (int i = 0; i < normal.Length / size; i++)
                inverseMatrix.MatrixVectorMultiplication(numbers.Skip(i * size).Take(size).ToArray()).ForEach(x => newNumbers.Add(x));
        else
            for (int i = 0; i < normal.Length / size; i++)
                matrix.MatrixVectorMultiplication(numbers.Skip(i * size).Take(size).ToArray()).ForEach(x => newNumbers.Add(x));

        LogMessages.Add("Encrypted numbers: " + newNumbers.Join(", "));

        string information = matrix.MatrixToArray().Select(x => alpha[x]).Join("");
        encryptedWord = newNumbers.Take(word.Length).Select(x => alpha[x]).Join("");
        string extras = newNumbers.TakeLast(normal.Length - word.Length).Select(x => alpha[x]).Join("");

        LogMessages.Add("Encrypted word: " + encryptedWord);
        LogMessages.Add("Extra letters: " + extras);

        PageInfo pages;
        if (size == 2)
            pages = new PageInfo(new ScreenInfo[] { information.Take(size).Join(""), extras, information.Skip(size).Take(size).Join("") }, invert);
        else if (size == 3)
            pages = new PageInfo(new ScreenInfo[] { information.Take(size).Join(""), extras, information.Skip(size).Take(size).Join(""), null, information.Skip(size * 2).Take(size).Join("") }, invert);
        else
            pages = new PageInfo(new ScreenInfo[] { information.Take(size).Join(""), extras, information.Skip(size).Take(size).Join(""), null, information.Skip(size * 2).Take(size).Join(""), null, information.Skip(size * 3).Take(size).Join("") }, invert);

        return new ResultInfo
        {
            LogMessages = LogMessages,
            Encrypted = encryptedWord,
            Pages = new[] { pages }
        };
    }
}
