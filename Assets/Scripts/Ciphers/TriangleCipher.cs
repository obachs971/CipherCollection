using System.Collections.Generic;
using CipherMachine;
using Words;

public class TriangleCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Triangle Cipher" : "Triangle Cipher"; } }
    public override string Code { get { return "TC"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public TriangleCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        string[] pos = {
            "11",
            "21","22",
            "31","32","33",
            "41","42","43","44",
            "51","52","54","55",
            "61","62","63","64","65","66",
            "71","72","73","74","75","76","77"
        };
        var logMessages = new List<string>();
        string encrypt = "";
        string kw = new Data().PickWord(4, 8);
        var kwfront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront.Value);
        logMessages.Add(string.Format("Keyword: {0}", kw));
        logMessages.Add(string.Format("Screen A: {0} -> {1}", kwfront.Expression, kwfront.Value));
        logMessages.Add(string.Format("Key: {0}", key));
        int num = UnityEngine.Random.Range(0, key.Length);
        key = key.Substring(0, num) + "#" + key.Substring(num);
        string screenB = pos[key.IndexOf("#")];
        logMessages.Add(string.Format("Screen B: {0}", screenB));
        string[] matrix = {
            key[0] + "" + key[26] + "" + key[20],
            key[2] + "" + key[25] + "" + key[14],
            key[5] + "" + key[24] + "" + key[10],
            key[9] + "" + key[23] + "" + key[6],
            key[13] + "" + key[22] + "" + key[3],
            key[19] + "" + key[21] + "" + key[1],
            key[4] + "" + key[18] + "" + key[15],
            key[8] + "" + key[17] + "" + key[11],
            key[12] + "" + key[16] + "" + key[7]
        };
        key = matrix[0] + matrix[1] + matrix[2] + matrix[3] + matrix[4] + matrix[5] + matrix[6] + matrix[7] + matrix[8];
        string order = invert ? "↻↺" : "↺↻", directions = "";
        foreach (char letter in word)
        {
            num = UnityEngine.Random.Range(1, 3);
            int index = key.IndexOf(letter);
            index = ((index / 3) * 3) + ((index + num) % 3);
            if (key[index] == '#')
                index = ((index / 3) * 3) + ((index + num) % 3);
            encrypt = encrypt + "" + key[index];
            directions = directions + "" + order[num - 1];
        }
        logMessages.Add(string.Format("Directions: {0}", directions));

        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { kw, kwfront.Expression, directions.Substring(0, directions.Length / 2), screenB, directions.Substring(directions.Length / 2) }, invert) },
            Score = 7
        };
    }
}
