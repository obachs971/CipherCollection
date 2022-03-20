using System;
using System.Collections.Generic;
using System.Linq;
using KModkit;

namespace CipherMachine
{
    public static class CMTools
    {
        public static int mod(int n, int m)
        {
            return (n % m + m) % m;
        }
        public static string getKey(string kw, string alphabet, bool kwFirst)
        {
            return (kwFirst ? (kw + alphabet) : alphabet.Except(kw).Concat(kw)).Distinct().Join("");
        }
        public static string[] generateBoolExp(KMBombInfo Bomb)
        {
            string boolExp = "ABCDEFGHIJ";
            string alphaVar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string exp = boolExp[UnityEngine.Random.Range(0, boolExp.Length)] + "" + alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)];
            alphaVar = alphaVar.Replace(exp[1].ToString(), "");
            bool result = false;
            switch (exp[0])
            {
                case 'A':
                    result = (getValue(exp[1], Bomb) % 2 == 0);
                    break;
                case 'B':
                    result = (getValue(exp[1], Bomb) % 2 == 1);
                    break;
                case 'C':
                    result = isPrime(getValue(exp[1], Bomb));
                    break;
                case 'D':
                    result = isComposite(getValue(exp[1], Bomb));
                    break;
                case 'E':
                    result = isFibo(getValue(exp[1], Bomb));
                    break;
                case 'F':
                    result = !(isFibo(getValue(exp[1], Bomb)));
                    break;
                case 'G':
                    exp = exp + alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)];
                    result = (getValue(exp[1], Bomb) % 2 == getValue(exp[2], Bomb) % 2);
                    break;
                case 'H':
                    exp = exp + alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)];
                    result = (getValue(exp[1], Bomb) % 2 != getValue(exp[2], Bomb) % 2);
                    break;
                case 'I':
                    exp = exp + alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)];
                    result = coprime(getValue(exp[1], Bomb), getValue(exp[2], Bomb));
                    break;
                case 'J':
                    exp = exp + alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)];
                    result = !(coprime(getValue(exp[1], Bomb), getValue(exp[2], Bomb)));
                    break;
            }
            return new string[] { exp, result + "" };
        }
        public static int[] generateValue(KMBombInfo Bomb)
        {
            int[] vals = new int[2];
            vals[0] = UnityEngine.Random.Range(0, 26) + 65;
            vals[1] = getValue((char) vals[0], Bomb);
            return vals;
        }
        private static int getValue(char l, KMBombInfo Bomb)
        {
            switch (l)
            {
                case 'A': return Bomb.GetBatteryCount();
                case 'B': return Bomb.GetBatteryHolderCount();
                case 'C': return Bomb.GetBatteryCount(Battery.D);
                case 'D': return ((Bomb.GetBatteryCount(Battery.AA) / 2) + (Bomb.GetBatteryCount(Battery.AA) % 2)); //In the case that people have the widgets mod, round up the number
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
                case 'W': return (int) DateTime.Now.Date.DayOfWeek;
                case 'X': return DateTime.Now.Date.Day;
                case 'Y': return DateTime.Now.Date.Month;
                case 'Z':
                    int year = DateTime.Now.Date.Year % 1000;
                    return ((year - 1) % 9) + 1;
            }
            return 0;
        }
        private static bool isPrime(int n)
        {
            if (n < 2)
                return false;
            if (n == 2)
                return true;
            if (n % 2 == 0)
                return false;
            for (int aa = 3; aa < n; aa += 2)
                if (n % aa == 0)
                    return false;
            return true;
        }
        private static bool isComposite(int n)
        {
            if (n < 4)
                return false;
            return !(isPrime(n));
        }
        private static bool isFibo(int n)
        {
            List<int> vals = new List<int>() { 1, 1 };
            while (vals[vals.Count - 1] < n)
                vals.Add(vals[vals.Count - 2] + vals[vals.Count - 1]);
            return (vals[vals.Count - 1] == n);
        }
        private static bool coprime(int a, int b)
        {
            if (a == 1 || b == 1)
                return true;
            else if (a == 0 || b == 0)
                return false;
            int c;
            if (a < b)
            {
                c = a;
                a = b;
                b = c;
            }
            c = a % b;
            while (c > 0)
            {
                a = b;
                b = c;
                c = a % b;
            }
            return (b == 1);
        }
    }
}

