using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class GrandpreCipher : CipherBase
{
    public override string Name { get { return "Grandpré Cipher"; } }
    public override int Score { get { return 5; } }
    public override string Code { get { return "GP"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        var words = generateWords(UnityEngine.Random.Range(6, 9));
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", encrypt = "", screenRows = "", key = "";
        string[] possLets = new string[words.Length];
        for (int i = 0; i < words.Length; i++)
        {
            key += words[i];
            possLets[i] = "";
            logMessages.Add(string.Format("Keyword #{0}: {1}", (i + 1), words[i]));
        }
        for (int i = 0; i < alpha.Length; i++)
            possLets[i % possLets.Length] = possLets[i % possLets.Length] + "" + alpha[i];
        for (int i = 0; i < word.Length; i++)
        {
            List<int> poss = new List<int>();
            for (int j = 0; j < key.Length; j++)
            {
                if (word[i] == key[j])
                    poss.Add(j);
            }
            int index = poss[UnityEngine.Random.Range(0, poss.Count)];
            int row = index / words.Length, col = (index % words.Length) + 1;
            logMessages.Add(string.Format("{0} -> {1}, {2}", word[i], (row + 1), col));
            encrypt = encrypt + "" + possLets[row].ToCharArray().Shuffle()[0];
            screenRows = screenRows + "" + col;
        }
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        var screens1 = new ScreenInfo[7];
        for (int i = 0; i < 4; i++)
            screens1[i * 2] = words[i];
        screens1[1] = screenRows.Substring(0, screenRows.Length / 2);
        screens1[3] = screenRows.Substring(screenRows.Length / 2);
        var screens2 = new ScreenInfo[(words.Length - 4) * 2 - 1];
        for (int i = 0; i < words.Length - 4; i++)
            screens2[i * 2] = words[i + 4];
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens1), new PageInfo(screens2) }
        };
    }
    private string[] generateWords(int len)
    {
        tryAgain:
        var wordList = new Data();
        // If len == 8, generate 8 words, etc., so they can form a square
        string[] words = new string[len];
        var alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
        for (int i = 0; i < words.Length; i++)
        {
            words[i] = wordList.PickBestWord(len, w => alpha.Count(ch => w.Contains(ch)));
            alpha.RemoveAll(ch => words[i].Contains(ch));
        }
        if (alpha.Count > 0)
            goto tryAgain;
        return words.Shuffle();
    }
}