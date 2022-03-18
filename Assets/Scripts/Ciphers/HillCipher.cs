using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HillCipher {

	public PageInfo[] encrypt(string word, string id, string log, bool invert)
	{
		Debug.LogFormat("{0} Begin Hill Cipher", log);
        string encrypt = "";
        CMTools cm = new CMTools();
        //Generate Initial Matrix
        int[] matrix = new int[4];
        matrix[1] = UnityEngine.Random.Range(0, 26);
        matrix[2] = UnityEngine.Random.Range(0, 26);
        if ((matrix[1] * matrix[2]) % 2 == 1)
            matrix[0] = UnityEngine.Random.Range(0, 12) * 2;
        else
        {
            do matrix[0] = UnityEngine.Random.Range(0, 12) * 2 + 1;
            while ((matrix[0] - matrix[1] * matrix[2]) % 13 == 0);
        }
        Debug.LogFormat("{0} [Hill Cipher] A: {1}", log, matrix[0]);
        Debug.LogFormat("{0} [Hill Cipher] B: {1}", log, matrix[1]);
        Debug.LogFormat("{0} [Hill Cipher] C: {1}", log, matrix[2]);
        
        int[] nums = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
        for (int aa = 0; aa < 26; aa++)
        {
            int num = aa * matrix[0] - matrix[1] * matrix[2];
            if (num % 2 == 0 || num % 13 == 0)
                nums = nums.Where(val => val != aa).ToArray();
        }
        matrix[3] = nums[UnityEngine.Random.Range(0, nums.Length)];
        Debug.LogFormat("{0} [Hill Cipher] D: {1}", log, matrix[3]);
        //Find the inverse of the matrix
        int a = 26;
        int b = cm.mod((matrix[0] * matrix[3]) - (matrix[1] * matrix[2]), 26);
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
        I = cm.mod(I, 26);
        int[] matrixI = { cm.mod(matrix[3] * I, 26), cm.mod(-matrix[1] * I, 26), cm.mod(-matrix[2] * I, 26), cm.mod(matrix[0] * I, 26) };
        Debug.LogFormat("{0} [Hill Cipher] I: {1}", log, I);
        Debug.LogFormat("{0} [Hill Cipher] Ai: {1}", log, matrixI[0]);
        Debug.LogFormat("{0} [Hill Cipher] Bi: {1}", log, matrixI[1]);
        Debug.LogFormat("{0} [Hill Cipher] Ci: {1}", log, matrixI[2]);
        Debug.LogFormat("{0} [Hill Cipher] Di: {1}", log, matrixI[3]);
        string alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY";
        Debug.LogFormat("{0} [Hill Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
        if(invert)
        {
            for(int i = 0; i < word.Length / 2; i++)
            {
                encrypt = encrypt + "" + alpha[cm.mod((matrixI[0] * alpha.IndexOf(word[i * 2])) + (matrixI[1] * alpha.IndexOf(word[(i * 2) + 1])), 26)];
                encrypt = encrypt + "" + alpha[cm.mod((matrixI[2] * alpha.IndexOf(word[i * 2])) + (matrixI[3] * alpha.IndexOf(word[(i * 2) + 1])), 26)];
            }
        }
        else
        {
            for (int i = 0; i < word.Length / 2; i++)
            {
                encrypt = encrypt + "" + alpha[cm.mod((matrix[0] * alpha.IndexOf(word[i * 2])) + (matrix[1] * alpha.IndexOf(word[(i * 2) + 1])), 26)];
                encrypt = encrypt + "" + alpha[cm.mod((matrix[2] * alpha.IndexOf(word[i * 2])) + (matrix[3] * alpha.IndexOf(word[(i * 2) + 1])), 26)];
            }
        }
        if (word.Length % 2 == 1)
            encrypt = encrypt + "" + word[word.Length - 1];
        Debug.LogFormat("{0} [Hill Cipher] {1} -> {2}", log, word, encrypt);
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(matrix[0] + "," + matrix[1], 30);
        screens[1] = new ScreenInfo();
        screens[2] = new ScreenInfo(matrix[2] + "," + matrix[3], 30);
        screens[8] = new ScreenInfo(id, 35);
        for (int i = 3; i < 8; i++)
            screens[i] = new ScreenInfo();
        return (new PageInfo[] { new PageInfo(new ScreenInfo[] { new ScreenInfo(encrypt, 35) }), new PageInfo(screens, invert) });
    }
}
