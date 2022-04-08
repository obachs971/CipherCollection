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
        public static ValueExpression<bool> generateBoolExp(KMBombInfo Bomb)
        {
            string boolExp = "ABCDEFGHIJKL";
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
                    result = !isFibo(getValue(exp[1], Bomb));
                    break;
                case 'G':
                    result = (getValue(exp[1], Bomb) % 4 < 2);
                    break;
                case 'H':
                    result = (getValue(exp[1], Bomb) % 4 >= 2);
                    break;
                case 'I':
                    exp += alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)];
                    result = (getValue(exp[1], Bomb) % 2 == getValue(exp[2], Bomb) % 2);
                    break;
                case 'J':
                    exp += alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)];
                    result = (getValue(exp[1], Bomb) % 2 != getValue(exp[2], Bomb) % 2);
                    break;
                case 'K':
                    exp += alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)];
                    result = coprime(getValue(exp[1], Bomb), getValue(exp[2], Bomb));
                    break;
                case 'L':
                    exp += alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)];
                    result = !coprime(getValue(exp[1], Bomb), getValue(exp[2], Bomb));
                    break;
                case 'M':
                    exp += alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)];
                    result = (getValue(exp[0], Bomb) >= getValue(exp[1], Bomb));
                    break;
                case 'N':
                    exp += alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)];
                    result = (getValue(exp[0], Bomb) <= getValue(exp[1], Bomb));
                    break;
                case 'O':
                    exp += alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)];
                    result = ((getValue(exp[0], Bomb) % 4) / 2 == (getValue(exp[1], Bomb) % 4) / 2);
                    break;
                case 'P':
                    exp += alphaVar[UnityEngine.Random.Range(0, alphaVar.Length)];
                    result = ((getValue(exp[0], Bomb) % 4) / 2 != (getValue(exp[1], Bomb) % 4) / 2);
                    break;
            }
            return new ValueExpression<bool> { Expression = exp, Value = result };
        }
        public static ValueExpression<int> generateValue(KMBombInfo Bomb)
        {
            var character = (char) ('A' + UnityEngine.Random.Range(0, 26));
            var value = getValue(character, Bomb);
            return new ValueExpression<int> { Expression = character.ToString(), Value = value };
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
                case 'U': return Bomb.GetSerialNumberLetters().Sum(c => c - 'A' + 1);
                case 'V': return Bomb.GetSerialNumber().Sum(c => "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c));
                case 'W': return (int) DateTime.Now.Date.DayOfWeek;
                case 'X': return DateTime.Now.Date.Day;
                case 'Y': return DateTime.Now.Date.Month;
                case 'Z': return ((DateTime.Now.Date.Year % 1000 - 1) % 9) + 1;
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
            return !isPrime(n);
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

        public static T[] NewArray<T>(params T[] array)
        {
            return array;
        }
    }
}

