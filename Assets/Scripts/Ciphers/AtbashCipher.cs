using System.Collections.Generic;
using CipherMachine;

public class AtbashCipher : CipherBase
{
    public override string Name { get { return "Atbash Cipher"; } }
    public override int Score(int wordLength) { return 3; }
    public override string Code { get { return "AT"; } }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string encrypt = "";
        foreach (char c in word)
            encrypt = encrypt + "" + (char) (155 - c);
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[0]) }
        };
    }
}
