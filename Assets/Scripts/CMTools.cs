using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using System;
using System.Linq;

namespace CipherMachine
{
	public class CMTools
	{
		private KMBombInfo Bomb;
		public CMTools(KMBombInfo Bomb)
		{
			this.Bomb = Bomb;
		}
		public string getKey(string kw, string alphabet, bool kwFirst)
		{
			return (kwFirst ? (kw + alphabet) : alphabet.Except(kw).Concat(kw)).Distinct().Join("");
		}
		public string[] generateBoolExp()
		{
			string boolExp = "ABCDEFGHIJ";
			string alphaVar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			string exp = (alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)] + "" + boolExp[UnityEngine.Random.Range(0, boolExp.Length)]);
			alphaVar = alphaVar.Replace(exp[0].ToString(), "");
			bool result = false;
			switch(exp[1])
			{
				case 'A':
					result = (getValue(exp[0]) % 2 == 0);
					break;
				case 'B':
					result = (getValue(exp[0]) % 2 == 1);
					break;
				case 'C':
					result = isPrime(getValue(exp[0]));
					break;
				case 'D':
					result = !(isPrime(getValue(exp[0])));
					break;
				case 'E':
					result = isFibo(getValue(exp[0]));
					break;
				case 'F':
					result = !(isFibo(getValue(exp[0])));
					break;
				case 'G':
					exp = exp[0] + "" + alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)] + "" + exp[1];
					result = (getValue(exp[0]) % 2 == getValue(exp[1]) % 2);
					break;
				case 'H':
					exp = exp[0] + "" + alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)] + "" + exp[1];
					result = (getValue(exp[0]) % 2 != getValue(exp[1]) % 2);
					break;
				case 'I':
					exp = exp[0] + "" + alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)] + "" + exp[1];
					result = coprime(getValue(exp[0]), getValue(exp[1]));
					break;
				case 'J':
					exp = exp[0] + "" + alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)] + "" + exp[1];
					result = !(coprime(getValue(exp[0]), getValue(exp[1])));
					break;
			}
			return new string[] { exp, result + "" };
		}
		private int getValue(char l)
		{
			string s = DateTime.Now.Date.DayOfWeek.ToString();
			switch (l)
			{
				case 'A': return Bomb.GetBatteryCount();
				case 'B': return Bomb.GetBatteryHolderCount();
				case 'C': return Bomb.GetBatteryCount(Battery.D);
				case 'D': return (Bomb.GetBatteryCount(Battery.AA) / 2);
				case 'E': return Bomb.GetIndicators().Count();
				case 'F': return Bomb.GetOnIndicators().Count();
				case 'G': return Bomb.GetOffIndicators().Count();
				case 'H': return Bomb.GetPortCount();
				case 'I': return Bomb.GetPortPlateCount();
				case 'J': return "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(Bomb.GetSerialNumber()[0]);
				case 'K': return "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(Bomb.GetSerialNumber()[1]);
				case 'L': return "0123456789".IndexOf(Bomb.GetSerialNumber()[2]);
				case 'M': return "-ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(Bomb.GetSerialNumber()[3]);
				case 'N': return "-ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(Bomb.GetSerialNumber()[4]);
				case 'O': return "0123456789".IndexOf(Bomb.GetSerialNumber()[5]);
				case 'P': return "-ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(Bomb.GetSerialNumberLetters().ElementAt(0));
				case 'Q': return "-ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(Bomb.GetSerialNumberLetters().ElementAt(1));
				case 'R': return Bomb.GetSerialNumberNumbers().ElementAt(0);
				case 'S': return Bomb.GetSerialNumberNumbers().ElementAt(1);
				case 'T': return Bomb.GetSerialNumberNumbers().Sum();
				case 'U':
					int sum1 = 0;
					foreach (char c in Bomb.GetSerialNumberLetters())
						sum1 += (c - 64);
					return sum1;
				case 'V':
					int sum2 = 0;
					foreach (char c in Bomb.GetSerialNumber())
						sum2 += "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c);
					return sum2;
				case 'W': return (int)DateTime.Now.Date.DayOfWeek;
				case 'X': return DateTime.Now.Date.Day;
				case 'Y': return DateTime.Now.Date.Month;
				case 'Z':
					int year = DateTime.Now.Date.Year % 1000;
					return ((year - 1) % 9) + 1;
			}
			return 0;
		}
		private bool isPrime(int n)
		{
			if (n < 2)
				return false;
			for(int aa = 2; aa < n; aa++)
			{
				if (n % aa == 0)
					return false;
			}
			return true;
		}
		private bool isComposite(int n)
		{
			if (n < 4)
				return false;
			return !(isPrime(n));
		}
		private bool isFibo(int n)
		{
			List<int> vals = new List<int>() { 1, 1 };
			while (vals[vals.Count - 1] < n)
				vals.Add(vals[vals.Count - 2] + vals[vals.Count - 1]);
			return (vals[vals.Count - 1] == n);
		}
		private bool coprime(int a, int b)
		{
			if (a == 1 || b == 1)
				return true;
			else if (a == 0 || b == 0)
				return false;
			int c;
			if(a < b)
			{
				c = a;
				a = b;
				b = c;
			}
			c = a % b;
			while(c > 0)
			{
				a = b;
				b = c;
				c = a % b;
			}
			return (b == 1);
		}
	}
}

