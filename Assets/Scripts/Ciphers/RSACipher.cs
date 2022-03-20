using CipherMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Words;

public class RSACipher
{
	public ResultInfo encrypt(string word, string id, string log)
	{
		Debug.LogFormat("{0} Begin RSA Cipher", log);
		string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		int[] vals = generateValues(word.Length);
		int N = vals[2], LN = vals[3], E = vals[4];
		Debug.LogFormat("{0} [RSA Cipher] N: {1}", log, N);
		Debug.LogFormat("{0} [RSA Cipher] λ(N): {1}", log, LN);
		Debug.LogFormat("{0} [RSA Cipher] E: {1}", log, E);
		int D = CMTools.mod(EEA(LN, E, log), LN);
		Debug.LogFormat("{0} [RSA Cipher] D: {1}", log, D);
		string[] page1 = { "", "", "", "" };
		int[] encryptSums = new int[word.Length];
		for (int i = 0; i < word.Length; i++)
		{
			int num = 1;
			for (int j = 0; j < E; j++)
				num = (num * (alpha.IndexOf(word[i]) + 2)) % N;
			string encryptNum = num + "";
			while(encryptNum.Length < 4)
				encryptNum = "0" + encryptNum;
			Debug.LogFormat("{0} [RSA Cipher] {1} -> {2} ", log, word[i], encryptNum);
			for (int j = 0; j < 4; j++)
				page1[j] = page1[j] + "" + encryptNum[j];
			encryptSums[i] = 0;
		}
		int[] inverseList = {
			343,1091,1477,1553,379,2087,2069,1471,973,761,1507,1583,49,
			317,1139,1141,1003,2231,2137,2213,1519,347,809,211,1633,1901
		}, encryptPowers = new int[word.Length];
		string encrypt = "";
		for(int i = word.Length; i < vals[5]; i++)
		{
			int num = UnityEngine.Random.Range(0, word.Length);
			while(encryptSums[num] >= 25)
				num = UnityEngine.Random.Range(0, word.Length);
			encryptSums[num]++;
		}
		for(int i = 0; i < encryptPowers.Length; i++)
		{
			encrypt = encrypt + "" + alpha[encryptSums[i]];
			encryptPowers[i] = inverseList[alpha.IndexOf(encrypt[i])];
		}
		for(int i = 0; i < encrypt.Length; i++)
		{
			int num = 1;
			for (int j = 0; j < encryptPowers[encrypt.Length - (i + 1)]; j++)
				num = (num * N) % 9797;
			Debug.LogFormat("{0} [RSA Cipher] N: {1}^{2} -> {3}", log, N, encrypt[encrypt.Length - (i + 1)], num);
			N = num + 0;
		}
		int inverse = EEA(2400, vals[5]), encryptE = 1;
		for (int i = 0; i < inverse; i++)
			encryptE = (E * encryptE) % 9797;
		Debug.LogFormat("{0} [RSA Cipher] E: {1}^{2} -> {3}", log, E, encrypt, encryptE);
		E = encryptE + 0;
		Debug.LogFormat("{0} [RSA Cipher] {1} -> {2}", log, word, encrypt);
		ScreenInfo[] screens = new ScreenInfo[9];
		for(int i = 0; i < 8; i+=2)
		{
			screens[i] = new ScreenInfo(page1[i / 2], new int[] { 35, 35, 35, 32, 28 }[word.Length - 4]);
			screens[i + 1] = new ScreenInfo();
		}
		screens[1] = new ScreenInfo(N + "", (N < 1000) ? 25 : 20);
		screens[3] = new ScreenInfo(E + "", (E < 1000) ? 25 : 20);
		screens[8] = new ScreenInfo(id, 35);
		return new ResultInfo
		{
			Encrypted = encrypt,
			Score = 5,
			Pages = new PageInfo[] { new PageInfo(screens) }
		};
	}
	private int[] generateValues(int len)
	{
		List<int> primes = new List<int>() { 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
		primes = primes.Shuffle();
		int[] p = { primes[0], primes[primes.Count - 1] };
		int ln = ((p[0] - 1) * (p[1] - 1)) / GCD(p[0] - 1, p[1] - 1);
		List<int> possE = new List<int>(), possEncrypt = new List<int>();
		for(int i = 2; i < ln; i++)
		{
			if (GCD(ln, i) == 1)
				possE.Add(i);
		}
		for(int i = len; i < (len * 26); i++)
		{
			if (GCD(2400, i) == 1)
				possEncrypt.Add(i);
		}
		return new int[] { p[0], p[1], (p[0] * p[1]), ln, possE[UnityEngine.Random.Range(0, possE.Count)], possEncrypt[UnityEngine.Random.Range(0, possEncrypt.Count)], };
	}
	private int GCD(int a, int b)
	{
		if (b > a)
		{
			int temp = a + 0;
			a = b + 0;
			b = temp + 0;
		}
		int r = a % b;
		while(r > 0)
		{
			a = b;
			b = r;
			r = a % b;
		}
		return b;
	}
	private int EEA(int A, int B, string log)
	{
		int Q = A / B;
		int R = A % B;
		int T1 = 0;
		int T2 = 1;
		int T3 = T1 - (T2 * Q);
		Debug.LogFormat("{0} [RSA Cipher] {1} {2} {3} {4} {5} {6} {7}", log, A, B, Q, R, T1, T2, T3);
		while (R > 0)
		{
			A = B;
			B = R;
			Q = A / B;
			R = A % B;
			T1 = T2;
			T2 = T3;
			T3 = T1 - (T2 * Q);
			Debug.LogFormat("{0} [RSA Cipher] {1} {2} {3} {4} {5} {6} {7}", log, A, B, Q, R, T1, T2, T3);
		}
		return T2;
	}
	private int EEA(int A, int B)
	{
		for(int i = 0; i < A; i++)
		{
			if ((i * B) % A == 1)
				return i;
		}
		return -1;
	}
}