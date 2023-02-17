using System;
using System.Collections.Generic;
using System.Linq;
using CipherMachine;
using Words;

public class BinaryCipher : CipherBase
{
    public override string Name { get { return "Binary Cipher"; } }
    public override string Code { get { return "BN"; } }
    private readonly static string[] grids =
    {
        "X-X-X----X----X-", "XX---X---X-X----", "XX--X-X-------X-", "XX---------XX-X-",
        "XXX-----X------X", "XXX--X-X--------", "XX--XX---X------", "XX--X----XX-----",
        "XXX-------XX----", "XX-----X--X-X---", "XX---X-X------X-", "XX---X-X-X------",
        "XX---X--X-----X-", "XX----X---XX----", "XX-----X-X----X-", "XX-----XX--X----",
        "XX----XX-------X", "X-X-X------XX---", "XX------X-X---X-", "XX--X--X------X-",
        "XXX--X------X---", "XX----X-X---X---", "XXXXX-----------", "XXX---------X-X-",
        "X-X-X----X--X---", "X-X--X--X---X---"
    };

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string grid5 = getBinaryGrid(word);
        string keyword = new Data().PickWord(4, 8);
        var kwfront = CMTools.generateBoolExp(bomb);
        string key = CMTools.getKey(keyword, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront.Value);
        logMessages.Add(string.Format("Screen 3: {0}", keyword));
        logMessages.Add(string.Format("Screen C: {0} -> {1}", kwfront.Expression, kwfront.Value));
        logMessages.Add(string.Format("Binary Key: {0}", key));
        logMessages.Add(string.Format("Grid #5: {0}", grid5));
        string encrypt = "", screen4 = "";
        foreach (char letter in word)
        {
            List<string> poss = getPossible(numToBin(letter - 'A' + 1, 5), grid5);
            string[] chosen = poss.Shuffle()[0].Split(' ');
            encrypt = encrypt + "" + chosen[0];
            screen4 = screen4 + "" + key[Convert.ToInt32(chosen[1])];

            logMessages.Add(string.Format("{0} -> {1} + {2} -> {3}", letter, encrypt[encrypt.Length - 1], screen4[screen4.Length - 1], chosen[2]));
        }
        string grid3 = "", grid4 = "";
        foreach (char bit in grid5)
        {
            string bits = bit == '1' ? UnityEngine.Random.Range(0, 2) + "" : new string("01".ToCharArray().Shuffle());
            grid3 = grid3 + "" + bits[0];
            grid4 = grid4 + "" + bits[1 % bits.Length];
        }
        int[] grid1 = new int[16];
        string grid2 = "";
        var v1 = CMTools.generateValue(bomb);
        var v2 = CMTools.generateValue(bomb);
        int[] values = { v1.Value % 8, v2.Value % 8, };
        for (int i = 0; i < 8; i++)
        {
            grid1[i] = ((values[0] + i) % 8) + 1;
            grid1[i + 8] = ((values[1] + i) % 8) + 1;
        }
        for (int i = 0; i < 16; i++)
        {
            if (grid3[i] == '0')
                grid2 = grid2 + "" + ((grid1[i] + 1) % 2);
            else
                grid2 = grid2 + "" + (grid1[i] % 2);
        }
        string hex1 = "", hex2 = "", hex = "0123456789ABCDEF";
        for (int i = 0; i < 4; i++)
        {
            char[] temp = grid2.Substring(i * 4, 4).ToCharArray();
            Array.Reverse(temp);
            grid2 = grid2.Substring(0, i * 4) + new string(temp) + grid2.Substring(i * 4 + 4);

            temp = grid4.Substring(i * 4, 4).ToCharArray();
            Array.Reverse(temp);
            grid4 = grid4.Substring(0, i * 4) + new string(temp) + grid4.Substring(i * 4 + 4);

            hex1 = hex1 + "" + hex[binToNum(grid2.Substring(i * 4, 4))];
            hex2 = hex2 + "" + hex[binToNum(grid4.Substring(i * 4, 4))];
        }
        logMessages.Add(string.Format("Grid #4: {0}", grid4));
        logMessages.Add(string.Format("Grid #3: {0}", grid3));
        logMessages.Add(string.Format("Grid #2: {0}", grid2));
        logMessages.Add(string.Format("Grid #1: {0}", String.Join("", new List<int>(grid1).ConvertAll(i => i.ToString()).ToArray())));

        logMessages.Add(string.Format("Screen A: {0}", v1.Expression));
        logMessages.Add(string.Format("Screen B: {0}", v2.Expression));

        logMessages.Add(string.Format("Screen 1: {0}", hex1));
        logMessages.Add(string.Format("Screen 2: {0}", hex2));

        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { hex1, v1.Expression, hex2, v2.Expression, keyword, kwfront.Expression, screen4 }) },
            Score = 8
        };
    }
    //Returns a valid binary grid that will encrypt the word.
    private string getBinaryGrid(string word)
    {
        string letters = new string(word.Distinct().ToArray());
        List<string> bins = new List<string>();
        foreach (char c in letters)
        {
            string bin = numToBin(c - 'A' + 1, 5);
            bins.Add(bin.ToUpperInvariant());
        }
        List<int> nums = new List<int>();
        for (int i = 1; i <= 65535; i++)
            nums.Add(i);
        nums.Shuffle();
        for (int i = 0; i < nums.Count(); i++)
        {
            bool flag = true;
            string grid = numToBin(nums[i], 16);
            foreach (string bin in bins)
            {
                List<string> temp = getPossible(bin, grid);
                flag = flag && (temp.Count > 0);
                if (!(flag))
                    break;
            }
            if (flag)
                return grid;
        }
        return null;
    }
    private List<string> getPossible(string bin, string grid)
    {
        List<string> poss = new List<string>();
        for (int i = 0; i < 26; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                string temp = grids[i].ToUpperInvariant();
                for (int a = 0; a <= j % 4; a++)
                    temp = right(temp.ToCharArray());
                for (int b = 0; b <= j / 4; b++)
                    temp = down(temp);
                string tempBin = "";
                for (int k = 0; k < 16; k++)
                {
                    if (temp[k] == 'X')
                        tempBin = tempBin + "" + grid[k];
                }
                if (bin.Equals(tempBin))
                    poss.Add("ABCDEFGHIJKLMNOPQRSTUVWXYZ"[i] + " " + j + " " + temp);
            }
        }
        return poss;
    }
    private string right(char[] s)
    {
        return s[3] + "" + s[0] + "" + s[1] + "" + s[2] + "" + s[7] + "" + s[4] + "" + s[5] + "" + s[6] + "" + s[11] + "" + s[8] + "" + s[9] + "" + s[10] + "" + s[15] + "" + s[12] + "" + s[13] + "" + s[14];
    }
    private string down(string s)
    {
        return s.Substring(12) + s.Substring(0, 12);
    }
    private string numToBin(int n, int min)
    {
        string bin = "";
        while (n > 0)
        {
            bin = (n % 2) + "" + bin;
            n /= 2;
        }
        while (bin.Length < min)
            bin = "0" + bin;
        return bin;
    }
    private int binToNum(string b)
    {
        int mult = 1, num = 0;
        for (int i = b.Length - 1; i >= 0; i--)
        {
            if (b[i] == '1')
                num += mult;
            mult *= 2;
        }
        return num;
    }
}
