using System.Collections.Generic;
using CipherMachine;

public class CaesareanRoleSwitchingCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Caesarean Role-Switching Cipher" : "Caesarean Role-Switching Cipher"; } }
    public override string Code { get { return "CW"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public CaesareanRoleSwitchingCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string encrypt = "";
        char keyLetter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[UnityEngine.Random.Range(0, 26)];
        logMessages.Add(string.Format("Key Letter: {0}", keyLetter));
        if (invert)
        {
            encrypt = encrypt + "" + keyLetter;
            foreach (char letter in word)
            {
                int offset = (encrypt[encrypt.Length - 1] - 'A') - (letter - 'A');
                encrypt = encrypt + "" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[CMTools.mod(encrypt[encrypt.Length - 1] - 'A' + offset, 26)];
                //logMessages.Add(string.Format("{0}", offset));
                logMessages.Add(string.Format("{0} - {1} -> {0} + {2} -> {3}", encrypt[encrypt.Length - 2], letter, offset, encrypt[encrypt.Length - 1]));
            }
            encrypt = encrypt.Substring(1);
        }
        else
        {
            word = keyLetter + "" + word;
            for (int i = 1; i < word.Length; i++)
            {
                int offset = (word[i - 1] - 'A') - (word[i] - 'A');
                encrypt = encrypt + "" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[CMTools.mod(word[i - 1] - 'A' + offset, 26)];
                //logMessages.Add(string.Format("{0}", offset));
                logMessages.Add(string.Format("{0} - {1} -> {0} + {2} -> {3}", word[i - 1], word[i], offset, encrypt[i - 1]));
            }
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { keyLetter + "" }, invert) },
            Score = 1 + word.Length
        };
    }
}
