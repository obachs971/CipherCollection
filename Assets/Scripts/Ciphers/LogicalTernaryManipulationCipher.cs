using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;

public class LogicalTernaryManipulationCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Logical Ternary Manipulation Cipher" : "Logical Ternary Manipulation Cipher"; } }
    public override string Code { get { return "LT"; } }


    private readonly bool invert;
    public override bool IsInvert { get { return invert;} }
    public LogicalTernaryManipulationCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        var encrypt = "";
        var expression = CMTools.generateBoolExp(bomb);
        var values = new ValueExpression<int>[2];
        string[] binary = new string[2];
        string[] ternaryValues = new string[word.Length];
        string binaryOutput = "";
        var modValue = 0;
        var realValues = new int[2];
        var alpha = "-ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        switch (word.Length)
        {
            case 4:
                modValue = 16;
                break;
            case 5:
                modValue = 32;
                break;
            case 6:
                modValue = 64;
                break;
            case 7:
                modValue = 128;
                break;
            case 8:
                modValue = 256;
                break;
        }

        int[][] convertNum = new int[word.Length][];

        for (int i = 0; i < word.Length; i++)
        {
            ternaryValues[i] = letterToTernary(word[i]);
            logMessages.Add(string.Format("{0} -> {1}", word[i], ternaryValues[i]));
        }

        for (int i = 0; i < word.Length; i++)
        {
            convertNum[i] = new int[3];

            for (int j = 0; j < 3; j++)
            {
                int.TryParse(ternaryValues[i][j].ToString(), out convertNum[i][j]);
            }
        }

        for (int i = 0; i < 2; i++)
        {
            values[i] = CMTools.generateValue(bomb);
            realValues[i] = CMTools.mod(values[i].Value, modValue);
            binary[i] = Convert.ToString(realValues[i], 2).PadLeft(word.Length, '0');
            logMessages.Add(string.Format("Expression {0} -> {1} mod {2} -> {3}", values[i].Expression, values[i].Value, modValue, binary[i]));
        }

        bool exp = expression.Value;

        if (CMTools.mod(word.Length, 2) != 0)
        {
            binaryOutput = modifyBits(binary[0], binary[1], exp);
        }
        else
        {
            for (int i = 0; i < word.Length; i += word.Length / 2)
            {
                string halfBit1 = binary[0].Substring(i, word.Length / 2);
                string halfBit2 = binary[1].Substring(i, word.Length / 2);

                binaryOutput += modifyBits(halfBit1, halfBit2, exp);
                exp = !exp;
            }

        }



        logMessages.Add(string.Format("Binary Output: {0} -> {1} -> {2}", expression.Expression, expression.Value, binaryOutput));

        int[] concat = new int[word.Length];

        if (invert)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (binaryOutput[i] == '1')
                {
                    for (int j = 0; j < 3; j++)
                    {
                        convertNum[i][j]++;
                        if (convertNum[i][j] > 2)
                            convertNum[i][j] = 0;
                    }
                    if (convertNum[i].All(x => x == 0))
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            convertNum[i][j] = 1;
                        }
                    }
                }
                concat[i] = int.Parse(convertNum[i].Join(""));
                concat[i] = baseTo10(concat[i], 3);
                encrypt = encrypt + "" + alpha[concat[i]];
                logMessages.Add(string.Format("{0} -> {1} -> {2} -> {3}", ternaryValues[i], binaryOutput[i], convertNum[i].Join(""), alpha[concat[i]]));
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (binaryOutput[i] == '1')
                {
                    for (int j = 0; j < 3; j++)
                    {
                        convertNum[i][j]--;
                        if (convertNum[i][j] < 0)
                            convertNum[i][j] = 2;
                    }
                    if (convertNum[i].All(x => x == 0))
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            convertNum[i][j] = 2;
                        }
                    }
                }
                concat[i] = int.Parse(convertNum[i].Join(""));
                concat[i] = baseTo10(concat[i], 3);
                encrypt = encrypt + "" + alpha[concat[i]];
                logMessages.Add(string.Format("{0} -> {1} -> {2} -> {3}", ternaryValues[i], binaryOutput[i], convertNum[i].Join(""), alpha[concat[i]]));
            }
        }


        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[]
            {
                new PageInfo(new ScreenInfo[] {values[0].Expression, expression.Expression, values[1].Expression }, invert)
            },
            // Score = ?
            // I'll let you guys score this one. - Kilo
        };
    }
    
    private string letterToTernary(char letter)
    {
        var ternaryLetters = new[] { "001", "002", "010", "011", "012", "020", "021", "022", "100", "101", "102", "110", "111", "112", "120", "121", "122", "200", "201", "202", "210", "211", "212", "220", "221", "222" };
        return ternaryLetters["ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(letter)];
    }

    private int baseTo10 (int input, int baseToConvert)
    {
        var total = 0;
        var numLength = input.ToString().Length;

        for (int i = 0; i < numLength; i++)
        {
            total += (int)Math.Pow(baseToConvert, numLength - (i + 1)) * int.Parse(input.ToString()[i].ToString());
        }
        return total;
    }

    private string modifyBits(string bit1, string bit2, bool inv)
    {
        string output = string.Empty;

        for (int i = 0; i < bit1.Length && i < bit2.Length; i++)
        {
            if (inv)
            {
                output += bit1[i] == bit2[i] ? "0" : "1";
            }
            else
            {
                output += bit1[i] == bit2[i] ? "1" : "0";
            }
        }

        return output;
    }
}
