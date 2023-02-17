using System.Collections.Generic;
using System.Linq;
using CipherMachine;

public class HomophonicCipher : CipherBase
{
    public override string Name { get { return "Homophonic Cipher"; } }
    public override string Code { get { return "HO"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string key = string.Concat(alpha[UnityEngine.Random.Range(0, 26)], alpha[UnityEngine.Random.Range(0, 26)], alpha[UnityEngine.Random.Range(0, 26)]);
        string encrypt = "";
        int[][] nums = new int[3][];
        logMessages.Add(string.Format("Key: {0}", key));
        for (int aa = 0; aa < nums.Length; aa++)
        {
            int index = alpha.IndexOf(key[aa]);
            nums[aa] = new int[26];
            for (int bb = 0; bb < 26; bb++)
                nums[aa][(bb + index) % 26] = bb + 1 + 26 * aa;
            logMessages.Add(string.Format("Row {0}: {1}", aa + 1, nums[aa][0]));
        }
        List<int> choices = new List<int>();
        List<int> list = new List<int>() { 0, 1, 2 };
        for (int i = 0; i < word.Length; i++)
        {
            choices.Add(list[UnityEngine.Random.Range(0, list.Count())]);
            list.Remove(choices[i]);
            if (list.Count() == 0)
                list = new List<int>() { 0, 1, 2 };
        }
        string tens = "";
        string[] alphas = { "JT", "AKU", "BLV", "CMW", "DNX", "EOY", "FPZ", "GQ", "HR", "IS" };
        for (int i = 0; i < word.Length; i++)
        {
            int index = UnityEngine.Random.Range(0, choices.Count());
            int numEnc = nums[choices[index]][alpha.IndexOf(word[i])];
            tens = tens + "" + (numEnc / 10);
            encrypt = encrypt + "" + alphas[numEnc % 10][UnityEngine.Random.Range(0, alphas[numEnc % 10].Length)];
            choices.RemoveAt(index);
        }
        logMessages.Add(string.Format("{0} -> {1}, {2}", word, tens, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { tens, key }) },
            Score = 4
        };
    }
}
