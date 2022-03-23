using System.Collections.Generic;
using CipherMachine;

public class AtbashCipher : CipherBase
{
    public override string Name { get { return "Atbash Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "AT"; } }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string encrypt = "";
        foreach (char c in word)
            encrypt = encrypt + "" + (char) (155 - c);
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        ScreenInfo[] screens = new ScreenInfo[9];
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens) }
        };
    }
}
