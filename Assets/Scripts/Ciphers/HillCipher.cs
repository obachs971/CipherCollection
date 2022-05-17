using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using UnityEngine;

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
        var logMessages = new List<string>();
        string encrypt = "";
        //Generate Initial Matrix
        int[] matrix = new int[4];
        matrix[1] = Random.Range(0, 26);
        matrix[2] = Random.Range(0, 26);
        if ((matrix[1] * matrix[2]) % 2 == 1)
            matrix[0] = Random.Range(0, 12) * 2;
        else
        {
            do matrix[0] = Random.Range(0, 12) * 2 + 1;
            while ((matrix[0] - matrix[1] * matrix[2]) % 13 == 0);
        }
        logMessages.Add(string.Format("A: {0}", matrix[0]));
        logMessages.Add(string.Format("B: {0}", matrix[1]));
        logMessages.Add(string.Format("C: {0}", matrix[2]));

        int[] nums = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
        for (int aa = 0; aa < 26; aa++)
        {
            int num = aa * matrix[0] - matrix[1] * matrix[2];
            if (num % 2 == 0 || num % 13 == 0)
                nums = nums.Where(val => val != aa).ToArray();
        }
        matrix[3] = nums[Random.Range(0, nums.Length)];
        logMessages.Add(string.Format("D: {0}", matrix[3]));
        //Find the inverse of the matrix
        int a = 26;
        int b = CMTools.mod((matrix[0] * matrix[3]) - (matrix[1] * matrix[2]), 26);
        int r, I = 0, t2 = 1;
        do
        {
            int q = a / b;
            r = a % b;
            int t3 = I - (t2 * q);
            I = t2;
            t2 = t3;
            a = b;
            b = r;
        } while (r != 0);
        I = CMTools.mod(I, 26);
        int[] matrixI = { CMTools.mod(matrix[3] * I, 26), CMTools.mod(-matrix[1] * I, 26), CMTools.mod(-matrix[2] * I, 26), CMTools.mod(matrix[0] * I, 26) };
        logMessages.Add(string.Format("I: {0}", I));
        logMessages.Add(string.Format("Ai: {0}", matrixI[0]));
        logMessages.Add(string.Format("Bi: {0}", matrixI[1]));
        logMessages.Add(string.Format("Ci: {0}", matrixI[2]));
        logMessages.Add(string.Format("Di: {0}", matrixI[3]));
        string alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY";
        if (invert)
        {
            for (int i = 0; i < word.Length / 2; i++)
            {
                encrypt = encrypt + "" + alpha[CMTools.mod((matrixI[0] * alpha.IndexOf(word[i * 2])) + (matrixI[1] * alpha.IndexOf(word[(i * 2) + 1])), 26)];
                encrypt = encrypt + "" + alpha[CMTools.mod((matrixI[2] * alpha.IndexOf(word[i * 2])) + (matrixI[3] * alpha.IndexOf(word[(i * 2) + 1])), 26)];
            }
        }
        else
        {
            for (int i = 0; i < word.Length / 2; i++)
            {
                encrypt = encrypt + "" + alpha[CMTools.mod((matrix[0] * alpha.IndexOf(word[i * 2])) + (matrix[1] * alpha.IndexOf(word[(i * 2) + 1])), 26)];
                encrypt = encrypt + "" + alpha[CMTools.mod((matrix[2] * alpha.IndexOf(word[i * 2])) + (matrix[3] * alpha.IndexOf(word[(i * 2) + 1])), 26)];
            }
        }
        if (word.Length % 2 == 1)
            encrypt = encrypt + "" + word[word.Length - 1];
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { matrix[0] + "," + matrix[1], null, matrix[2] + "," + matrix[3] }, invert) }
        };
    }
}
