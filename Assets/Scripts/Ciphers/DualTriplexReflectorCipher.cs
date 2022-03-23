using System.Collections.Generic;
using CipherMachine;
using Words;

public class DualTriplexReflectorCipher : CipherBase
{
	public override string Name { get { return invert ? "Inverted Dual Triplex Reflector Cipher" : "Dual Triplex Reflector Cipher"; } }
	public override int Score { get { return 5; } }
	public override string Code { get { return "DT"; } }
    
    private readonly bool invert;
    public DualTriplexReflectorCipher(bool invert) { this.invert = invert; }
    
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string alpha = "-ABCDEFGHIJKLMNOPQRSTUVWXYZ", encrypt = "";
        var wordList = new Data();
        string kw1 = wordList.PickWord(4, 8);
        string kw2 = wordList.PickWord(4, 8);
        string kw3 = wordList.PickWord(word.Length);
        string[] kw1front = CMTools.generateBoolExp(bomb), kw2front = CMTools.generateBoolExp(bomb);
        string ref1 = CMTools.getKey(kw1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kw1front[1][0] == 'T');
        ref1 = ref1.Substring(0, 13) + " " + ref1.Substring(13);
        string ref2 = CMTools.getKey(kw2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kw2front[1][0] == 'T');
        ref2 = ref2.Substring(0, 13) + " " + ref2.Substring(13);
        logMessages.Add(string.Format("Keyword 1: {0}", kw1));
        logMessages.Add(string.Format("Keyword 2: {0}", kw2));
        logMessages.Add(string.Format("Keyword 3: {0}", kw3));
        logMessages.Add(string.Format("Screen A: {0} -> {1} -> {2}", kw1front[0], kw1front[1], ref1));
        logMessages.Add(string.Format("Screen B: {0} -> {1} -> {2}", kw2front[0], kw2front[1], ref2));
        logMessages.Add(ref1.Substring(0, 9));
        logMessages.Add(ref1.Substring(9, 9));
        logMessages.Add(ref1.Substring(18));
        logMessages.Add(string.Format("--------------"));
        logMessages.Add(ref2.Substring(0, 9));
        logMessages.Add(ref2.Substring(9, 9));
        logMessages.Add(ref2.Substring(18));
        for (int i = 0; i < word.Length; i++)
        {
            string temp = word[i] + "";
            if (invert)
            {
                for (int j = 0; j < 3; j++)
                    temp = temp + "" + ref2[ref1.IndexOf(temp[j])];
            }
            else
            {
                for (int j = 0; j < 3; j++)
                    temp = temp + "" + ref1[ref2.IndexOf(temp[j])];
            }
            encrypt += temp[3];
            logMessages.Add(string.Format("{0}->{1}->{2}->{3}", temp[0], temp[1], temp[2], temp[3]));
            int indexA, indexB;
            if (invert)
            {
                indexA = ref2.IndexOf(temp[1]);
                indexB = ref1.IndexOf(temp[2]);
            }
            else
            {
                indexA = ref2.IndexOf(temp[2]);
                indexB = ref1.IndexOf(temp[1]);
            }
            if (i < (word.Length - 1))
            {
                int[] tri = { alpha.IndexOf(kw3[i % kw3.Length]) / 9, (alpha.IndexOf(kw3[i % kw3.Length]) % 9) / 3, alpha.IndexOf(kw3[i % kw3.Length]) % 3 };
                ref2 = putRowBack(ref2, shiftLets(ref2.Substring((indexA / 9) * 9, 9), (tri[0] * 3) + tri[1]), indexA / 9);
                temp = shiftLets(ref2[(indexA % 9) + 18] + "" + ref2[(indexA % 9) + 9] + "" + ref2[indexA % 9], tri[2]);
                for (int j = 0; j < 3; j++)
                    ref2 = ref2.Substring(0, (indexA % 9) + (j * 9)) + temp[2 - j] + ref2.Substring((indexA % 9) + (j * 9) + 1);
                temp = shiftLets(ref1[(indexB % 9) + 18] + "" + ref1[(indexB % 9) + 9] + "" + ref1[indexB % 9], tri[0]);
                for (int j = 0; j < 3; j++)
                    ref1 = ref1.Substring(0, (indexB % 9) + (j * 9)) + temp[2 - j] + ref1.Substring((indexB % 9) + (j * 9) + 1);
                ref1 = putRowBack(ref1, shiftLets(ref1.Substring((indexB / 9) * 9, 9), (tri[1] * 3) + tri[2]), indexB / 9);
                logMessages.Add(string.Format("{0} -> {1}{2}{3}", kw3[i % kw3.Length], tri[0], tri[1], tri[2]));
                logMessages.Add(ref1.Substring(0, 9));
                logMessages.Add(ref1.Substring(9, 9));
                logMessages.Add(ref1.Substring(18));
                logMessages.Add(string.Format("--------------"));
                logMessages.Add(ref2.Substring(0, 9));
                logMessages.Add(ref2.Substring(9, 9));
                logMessages.Add(ref2.Substring(18));
            }
        }
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(kw1, new int[] { 35, 35, 35, 32, 28 }[kw1.Length - 4]);
        screens[1] = new ScreenInfo(kw1front[0], 25);
        screens[2] = new ScreenInfo(kw2, new int[] { 35, 35, 35, 32, 28 }[kw2.Length - 4]);
        screens[3] = new ScreenInfo(kw2front[0], 25);
        screens[4] = new ScreenInfo(kw3, new int[] { 35, 35, 35, 35, 32, 28 }[kw3.Length - 3]);
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
    private string shiftLets(string lets, int shift)
    {
        lets = lets.Replace(" ", "");
        shift = shift % lets.Length;
        lets = lets.Substring(shift) + lets.Substring(0, shift);
        if (lets.Length % 3 == 2)
            lets = lets.Substring(0, lets.Length / 2) + " " + lets.Substring(lets.Length / 2);
        return lets;
    }
    private string putRowBack(string refl, string temp, int index)
    {
        switch (index)
        {
            case 0: return temp + refl.Substring(9);
            case 1: return refl.Substring(0, 9) + temp + refl.Substring(18);
            default: return refl.Substring(0, 18) + temp;
        }
    }
}