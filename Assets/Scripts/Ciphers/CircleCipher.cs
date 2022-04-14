using System.Collections.Generic;
using CipherMachine;
using Words;

public class CircleCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Circle Cipher" : "Circle Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "AK"; } }

    private readonly bool invert;
    public CircleCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        int[] PI =
        {
            3,1,4,1,5,9,2,6,5,3,5,8,9,7,9,3,2,3,8,4,
            6,2,6,4,3,3,8,3,2,7,9,5,0,2,8,8,4,1,9,7,
            1,6,9,3,9,9,3,7,5,1,0,5,8,2,0,9,7,4,9,4,
            4,5,9,2,3,0,7,8,1,6,4,0,6,2,8,6,2,0,8,9,
            9,8,6,2,8,0,3,4,8,2,5,3,4,2,1,1,7,0,6,7,
            9,8,2,1,4,8,0,8,6,5,1,3,2,8,2,3,0,6,6,4
        };
        int index;
        var logMessages = new List<string>();
        string alpha = "-ABCDEFGHIJKLMNOPQRSTUVWXYZ", encrypt = "";
        string kw = new Data().PickWord(4, 8);
        var kwfront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw, alpha.Substring(1), kwfront.Value);
        int SI = index = UnityEngine.Random.Range(0, 120);
        var clockwise = CMTools.generateBoolExp(bomb);
        int mult = clockwise.Value ? 1 : -1;
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", kwfront.Expression, kwfront.Value));
        logMessages.Add(string.Format("Key: {0} ", key));
        logMessages.Add(string.Format("Starting index: {0}", SI));
        logMessages.Add(string.Format("Screen B: {0} -> {1}", clockwise.Expression, clockwise.Value));
        if (invert)
        {
            foreach (char letter in word)
            {
                encrypt = encrypt + "" + key[CMTools.mod(key.IndexOf(letter) - PI[index], 26)];
                logMessages.Add(string.Format("{0} - {1} -> {2}", letter, PI[index], encrypt[encrypt.Length - 1]));
                index = CMTools.mod(index + (alpha.IndexOf(encrypt[encrypt.Length - 1]) * mult), 120);
                logMessages.Add(string.Format("New Index: {0}", index));
            }
        }
        else
        {
            foreach(char letter in word)
            {
                encrypt = encrypt + "" + key[CMTools.mod(key.IndexOf(letter) + PI[index], 26)];
                logMessages.Add(string.Format("{0} + {1} -> {2}", letter, PI[index], encrypt[encrypt.Length - 1]));
                index = CMTools.mod(index + (alpha.IndexOf(letter) * mult), 120);
                logMessages.Add(string.Format("New Index: {0}", index));
            }
        }
        
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, kwfront.Expression, SI + "", clockwise.Expression }, invert) }
        };
    }
}
