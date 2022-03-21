using CipherMachine;
using UnityEngine;
using Words;

public class VigenereCipher
{
    public ResultInfo encrypt(string word, string id, string log, bool invert)
    {
        Debug.LogFormat("{0} Begin Vigenere Cipher", log);
        string keyword = new Data().PickWord(3, word.Length);
        string encrypt = "";
        string alpha = "ZABCDEFGHIJKLMNOPQRSTUVWXY";
        Debug.LogFormat("{0} [Vigenere Cipher] Keyword: {1}", log, keyword);
        Debug.LogFormat("{0} [Vigenere Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
        if (invert)
        {
            for (int i = 0; i < word.Length; i++)
                encrypt = encrypt + "" + alpha[CMTools.mod(alpha.IndexOf(word[i]) - alpha.IndexOf(keyword[i % keyword.Length]), 26)];
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
                encrypt = encrypt + "" + alpha[CMTools.mod(alpha.IndexOf(word[i]) + alpha.IndexOf(keyword[i % keyword.Length]), 26)];
        }
        Debug.LogFormat("{0} [Vigenere Cipher] {1} + {2} -> {3}", log, word, keyword, encrypt);
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(keyword, new int[] { 35, 35, 35, 35, 32, 28 }[keyword.Length - 3]);
        screens[8] = new ScreenInfo(id, 35);
        return new ResultInfo
        {
            Encrypted = encrypt,
            Score = 5,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
}
