using CipherMachine;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class EnigmaCipher : CipherBase
{
    public override string Name { get { return "Enigma Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "EN"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", encrypt = "";
        string plugboard = generatePlugboard(word.Length);
        string[] rotorNums = generateRotors();
        string rotorLets = alpha[Random.Range(0, 26)] + "" + alpha[Random.Range(0, 26)] + "" + alpha[Random.Range(0, 26)];
        char reflector = alpha[Random.Range(0, 3)];
        string[][] enigma = createEnigmaMachine(plugboard, rotorNums, rotorLets, reflector);
        string[] plugboardDisplay = { plugboard.Substring(0, 1), plugboard.Substring(1, 1) };
        for (int i = 2; i < plugboard.Length; i += 2)
        {
            plugboardDisplay[0] = plugboardDisplay[0] + "" + plugboard.Substring(i, 1);
            plugboardDisplay[1] = plugboardDisplay[1] + "" + plugboard.Substring(i + 1, 1);
        }
        logMessages.Add(string.Format("Plugboard: {0} {1}", plugboardDisplay[0], plugboardDisplay[1]));
        logMessages.Add(string.Format("Top Rotor: {0}, {1}", rotorNums[0], rotorLets[2]));
        logMessages.Add(string.Format("Middle Rotor: {0}, {1}", rotorNums[1], rotorLets[1]));
        logMessages.Add(string.Format("Bottom Rotor: {0}, {1}", rotorNums[2], rotorLets[0]));
        logMessages.Add(string.Format("Reflector: {0}", reflector));
        for (int i = 0; i < word.Length; i++)
        {
            encrypt = encrypt + "" + encryptLetter(word[i], enigma, logMessages);
            enigma = turnRotors(enigma);
        }
        //Setting up Screens

        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { rotorLets, rotorNums[0], plugboardDisplay[0], rotorNums[1], plugboardDisplay[1], rotorNums[2], null, reflector + "" }) }
        };
    }
    //Generates the Plugboard
    private string generatePlugboard(int length)
    {
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", plugboard = "";
        for (int i = 0; i < (12 - length); i++)
        {
            plugboard = plugboard + "" + alpha[Random.Range(0, alpha.Length)];
            alpha = alpha.Replace(plugboard[plugboard.Length - 1] + "", "");
            plugboard = plugboard + "" + alpha[Random.Range(0, alpha.Length)];
            alpha = alpha.Replace(plugboard[plugboard.Length - 1] + "", "");
        }
        return plugboard;
    }
    //Generates the Rotors
    private string[] generateRotors()
    {
        List<string> rotorList = new List<string>() { "I", "II", "III", "IV", "V", "VI", "VII", "VIII" };
        string[] rotorNums = new string[3];
        for (int i = 0; i < 3; i++)
        {
            rotorNums[i] = rotorList[Random.Range(0, rotorList.Count)].ToUpperInvariant();
            rotorList.Remove(rotorNums[i]);
        }
        return rotorNums;
    }
    //Creating the Engima Machine
    private string[][] createEnigmaMachine(string plugboard, string[] rotorNums, string rotorLets, char reflector)
    {
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        List<string> rotorList = new List<string>() { "I", "II", "III", "IV", "V", "VI", "VII", "VIII" };
        string[][] rotors =
        {
            new string[]{ "EKMFLGDQVZNTOWYHXUSPAIBRCJ", alpha.ToUpperInvariant(), "DQ" },
            new string[]{ "AJDKSIRUXBLHWTMCQGZNPYFVOE", alpha.ToUpperInvariant(), "ER" },
            new string[]{ "BDFHJLCPRTXVZNYEIWGAKMUSQO", alpha.ToUpperInvariant(), "IV" },
            new string[]{ "ESOVPZJAYQUIRHXLNFTGKDCMWB", alpha.ToUpperInvariant(), "JW" },
            new string[]{ "VZBRGITYUPSDNHLXAWMJQOFECK", alpha.ToUpperInvariant(), "MZ" },
            new string[]{ "JPGVOUMFYQBENHZRDKASXLICTW", alpha.ToUpperInvariant(), "LY" },
            new string[]{ "NZJHGRCXMYSWBOUFAIVLPEKQDT", alpha.ToUpperInvariant(), "HU" },
            new string[]{ "FKQHTLXOCBJSPDZRAMEWNIUYGV", alpha.ToUpperInvariant(), "CP" }
        };
        string[] reflectors =
        {
            "LUSNPQOMJIYAHDGEFXCVBTZRKW",
            "XQUMFEPOWLTJDZHGBVYKCRIASN",
            "ESKOAQMJYHCPGTDLFUBNRXZVIW"
        };
        string keyboard = alpha.ToUpperInvariant();
        for (int i = 0; i < plugboard.Length; i += 2)
        {
            keyboard = keyboard.Replace(plugboard[i], '*');
            keyboard = keyboard.Replace(plugboard[i + 1], plugboard[i]);
            keyboard = keyboard.Replace('*', plugboard[i + 1]);
        }
        string[][] enigma =
        {
            new string[]{ keyboard.ToUpperInvariant() },
            rotors[rotorList.IndexOf(rotorNums[0])],
            rotors[rotorList.IndexOf(rotorNums[1])],
            rotors[rotorList.IndexOf(rotorNums[2])],
            new string[]{ alpha.ToUpperInvariant(), reflectors[alpha.IndexOf(reflector)] }
        };
        for (int i = 0; i < rotorLets.Length; i++)
        {
            enigma[3 - i][0] = enigma[3 - i][0].Substring(alpha.IndexOf(rotorLets[i])) + enigma[3 - i][0].Substring(0, alpha.IndexOf(rotorLets[i]));
            enigma[3 - i][1] = enigma[3 - i][1].Substring(alpha.IndexOf(rotorLets[i])) + enigma[3 - i][1].Substring(0, alpha.IndexOf(rotorLets[i]));
        }
        return enigma;
    }
    //Encrypts the Letter
    private char encryptLetter(char let, string[][] enigma, List<string> logMessages)
    {
        string changes = let + "";
        int index = enigma[0][0].IndexOf(let);
        for (int i = 1; i < 4; i++)
        {
            changes = changes + "->" + enigma[i][0][index];
            index = enigma[i][1].IndexOf(enigma[i][0][index]);
        }
        changes = changes + "->" + enigma[4][0][index] + "->" + enigma[4][1][index];
        index = enigma[4][0].IndexOf(enigma[4][1][index]);
        for (int i = 3; i > 0; i--)
        {
            changes = changes + "->" + enigma[i][1][index];
            index = enigma[i][0].IndexOf(enigma[i][1][index]);
        }
        changes = changes + "->" + enigma[0][0][index];
        logMessages.Add(changes);
        return enigma[0][0][index];
    }
    //Turns the Rotors
    private string[][] turnRotors(string[][] enigma)
    {
        if (enigma[2][2].Contains(enigma[2][1][0] + ""))
        {
            enigma[2][0] = enigma[2][0].Substring(1) + "" + enigma[2][0][0];
            enigma[2][1] = enigma[2][1].Substring(1) + "" + enigma[2][1][0];
            enigma[3][0] = enigma[3][0].Substring(1) + "" + enigma[3][0][0];
            enigma[3][1] = enigma[3][1].Substring(1) + "" + enigma[3][1][0];
        }
        else if (enigma[1][2].Contains(enigma[1][1][0] + ""))
        {
            enigma[2][0] = enigma[2][0].Substring(1) + "" + enigma[2][0][0];
            enigma[2][1] = enigma[2][1].Substring(1) + "" + enigma[2][1][0];
        }
        enigma[1][0] = enigma[1][0].Substring(1) + "" + enigma[1][0][0];
        enigma[1][1] = enigma[1][1].Substring(1) + "" + enigma[1][1][0];
        return enigma;
    }
}
