using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class ElsieFourCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted ElsieFour Cipher" : "ElsieFour Cipher";  } }
	public override string Code { get { return "EF"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }

    public ElsieFourCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
	{
        var logMessages = new List<string>();
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXY", replaceZ = "", encrypt = "";

        logMessages.Add(string.Format("Before Replacing Zs: {0}", word));
        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == 'Z')
            {
                word = word.Substring(0, i) + "" + alpha[Random.Range(0, alpha.Length)] + "" + word.Substring(i + 1);
                replaceZ = replaceZ + "" + word[i];
            }
            else
                replaceZ = replaceZ + "" + alpha.Replace(word[i].ToString(), "")[Random.Range(0, 24)];
        }
        logMessages.Add(string.Format("After Replacing Zs: {0}", word));
        logMessages.Add(string.Format("Screen 2: {0}", replaceZ));

        string kw = new Data().PickWord(4, 8);
        //kw = "ADVOCACY";
        var keyFront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw.Replace("Z", ""), alpha, keyFront.Value);
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Keyword Front Rule: {0} -> {1}", keyFront.Expression, keyFront.Value));
        logMessages.Add(string.Format("Key: {0}", key));
        string markedLetters = key[Random.Range(0, alpha.Length)] + "";
        //markedLetters = "T";
        logMessages.Add(string.Format("Screen B: {0}", markedLetters));
        if (invert)
        {
            foreach (char letter in word)
            {
                //Encrypt Letter
                int index = key.IndexOf(letter);
                int row = index / 5, col = index % 5;
                int val = markedLetters[markedLetters.Length - 1] - 'A';
                col = CMTools.mod(col - (val % 5), 5);
                row = CMTools.mod(row - (val / 5), 5);
                encrypt += key[(row * 5) + col];
                logMessages.Add(string.Format("{0} + {1} -> {2}", letter, markedLetters[markedLetters.Length - 1], encrypt[encrypt.Length - 1]));
                //Change Key
                string temp = key.Substring((index / 5) * 5, 5);
                temp = temp[4] + temp.Substring(0, 4);
                key = key.Substring(0, (index / 5) * 5) + temp + key.Substring(((index / 5) * 5) + 5);
                col = key.IndexOf(encrypt[encrypt.Length - 1]) % 5;
                temp = "";
                for (int i = 0; i < key.Length; i += 5)
                    temp += key[i + col];
                temp = temp[4] + temp.Substring(0, 4);
                for (int i = 0; i < key.Length; i += 5)
                    key = key.Substring(0, i + col) + temp[i / 5] + key.Substring(i + col + 1);
                logMessages.Add(string.Format("New Key: {0}", key));
                //Change Marked Letter
                index = key.IndexOf(markedLetters[markedLetters.Length - 1]);
                row = index / 5; col = index % 5;
                val = encrypt[encrypt.Length - 1] - 'A';
                col = (col + (val % 5)) % 5;
                row = (row + (val / 5)) % 5;
                markedLetters += key[(row * 5) + col];
                logMessages.Add(string.Format("New Marked Letter: {0} + {1} -> {2}", markedLetters[markedLetters.Length - 2], encrypt[encrypt.Length - 1], markedLetters[markedLetters.Length - 1]));
            }
        }
        else
        {
            foreach (char letter in word)
            {
                //Encrypt Letter
                int index = key.IndexOf(letter);
                int row = index / 5, col = index % 5;
                int val = markedLetters[markedLetters.Length - 1] - 'A';
                col = CMTools.mod(col + (val % 5), 5);
                row = CMTools.mod(row + (val / 5), 5);
                encrypt += key[(row * 5) + col];
                logMessages.Add(string.Format("{0} + {1} -> {2}", letter, markedLetters[markedLetters.Length - 1], encrypt[encrypt.Length - 1]));
                //Change Key
                string temp = key.Substring((index / 5) * 5, 5);
                temp = temp[4] + temp.Substring(0, 4);
                key = key.Substring(0, (index / 5) * 5) + temp + key.Substring(((index / 5) * 5) + 5);
                col = key.IndexOf(encrypt[encrypt.Length - 1]) % 5;
                temp = "";
                for (int i = 0; i < key.Length; i += 5)
                    temp += key[i + col];
                temp = temp[4] + temp.Substring(0, 4);
                for (int i = 0; i < key.Length; i += 5)
                    key = key.Substring(0, i + col) + temp[i / 5] + key.Substring(i + col + 1);
                logMessages.Add(string.Format("New Key: {0}", key));
                //Change Marked Letter
                index = key.IndexOf(markedLetters[markedLetters.Length - 1]);
                row = index / 5; col = index % 5;
                val = encrypt[encrypt.Length - 1] - 'A';
                col = (col + (val % 5)) % 5;
                row = (row + (val / 5)) % 5;
                markedLetters += key[(row * 5) + col];
                logMessages.Add(string.Format("New Marked Letter: {0} + {1} -> {2}", markedLetters[markedLetters.Length - 2], encrypt[encrypt.Length - 1], markedLetters[markedLetters.Length - 1]));
            }
        }
       

        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, keyFront.Expression, replaceZ, markedLetters[0] + "" }, invert) },
            Score = 8
        };
    }
}
