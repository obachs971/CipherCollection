using System;
using System.Collections.Generic;
using CipherMachine;

public class AESCipher : CipherBase
{
    public override string Name { get { return "AES Cipher"; } }
    public override string Code { get { return "AE"; } }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        string screens12 = "", screen3 = "", encrypt = "";
        string[] RK = { "", "", "", "", "" };
        for (int i = 0; i < 10; i++)
        {
            screens12 = screens12 + "" + "0123456789ABCDEF"[UnityEngine.Random.Range(0, 16)];
            RK[i % 5] = RK[i % 5] + "" + screens12[i];
        }
        for (int i = 0; i < word.Length; i++)
        {
            string letBin = LetToBin(word[i]), encBin = "";
            logMessages.Add(string.Format("{0} -> {1}", word[i], letBin));
            for (int j = 0; j < RK.Length; j++)
            {
                string bin = HexToBin(RK[j]) + "" + letBin[j];
                encBin = encBin + "" + (bin.Replace("0", "").Length % 2);
                logMessages.Add(string.Format("{0} + {1} -> {2} -> {3}", RK[j], letBin[j], bin, encBin[j]));
            }
            char let = BinToLet(encBin);
            if (let == '-')
            {
                screen3 += "1";
                encrypt = encrypt + "" + BinToLet(encBin.Replace("1", "*").Replace("0", "1").Replace("*", "0"));
            }
            else
            {
                char temp = BinToLet(encBin.Replace("1", "*").Replace("0", "1").Replace("*", "0"));
                if (temp != '-' && UnityEngine.Random.Range(0, 3) == 0)
                {
                    encrypt = encrypt + "" + temp;
                    screen3 += "1";
                }
                else
                {
                    encrypt = encrypt + "" + let;
                    screen3 += "0";
                }
            }
            logMessages.Add(string.Format("{0} + {1} -> {2}", encBin, screen3[i], encrypt[i]));
            if (i < word.Length - 1)
            {
                RK = getNewRoundKey(RK);
                logMessages.Add(string.Format("New Round Key: {0} {1} {2} {3} {4}", RK[0], RK[1], RK[2], RK[3], RK[4]));
            }
        }
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { screens12.Substring(0, 5), null, screens12.Substring(5), null, screen3 }) },
            Score = 8 + 2 * word.Length
        };
    }
    private string HexToBin(string hex)
    {
        string[] bins = { "0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111", "1000", "1001", "1010", "1011", "1100", "1101", "1110", "1111" };
        string bin = "", alpha = "0123456789ABCDEF";
        for (int i = 0; i < hex.Length; i++)
            bin += bins[alpha.IndexOf(hex[i])];
        return bin;
    }
    private string BinToHex(string bin)
    {
        string[] bins = { "0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111", "1000", "1001", "1010", "1011", "1100", "1101", "1110", "1111" };
        string hex = "", alpha = "0123456789ABCDEF";
        for (int i = 0; i < bin.Length; i += 4)
            hex = hex + "" + alpha[Array.IndexOf(bins, bin.Substring(i, 4))];
        return hex;
    }
    private string LetToBin(char c)
    {
        string[] bins = { "00000", "00001", "00010", "00011", "00100", "00101", "00110", "00111", "01000", "01001", "01010", "01011", "01100", "01101", "01110", "01111",
        "10000", "10001", "10010", "10011", "10100", "10101", "10110", "10111", "11000", "11001" };
        return bins["ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c)];
    }
    private char BinToLet(string bin)
    {
        string[] bins = {
            "00000", "00001", "00010", "00011", "00100", "00101", "00110", "00111", "01000", "01001", "01010", "01011", "01100", "01101", "01110", "01111",
            "10000", "10001", "10010", "10011", "10100", "10101", "10110", "10111", "11000", "11001", "11010", "11011", "11100", "11101", "11110", "11111"
        };
        return "ABCDEFGHIJKLMNOPQRSTUVWXYZ------"[Array.IndexOf(bins, bin)];
    }
    private string[] getNewRoundKey(string[] RK)
    {
        string alpha = "0123456789ABCDEF";
        string[] SBOX =
        {
            "63","7C","77","7B","F2","6B","6F","C5","30","01","67","2B","FE","D7","AB","76",
            "CA","82","C9","7D","FA","59","47","F0","AD","D4","A2","AF","9C","A4","72","C0",
            "B7","FD","93","26","36","3F","F7","CC","34","A5","E5","F1","71","D8","31","15",
            "04","C7","23","C3","18","96","05","9A","07","12","80","E2","EB","27","B2","75",
            "09","83","2C","1A","1B","6E","5A","A0","52","3B","D6","B3","29","E3","2F","84",
            "53","D1","00","ED","20","FC","B1","5B","6A","CB","BE","39","4A","4C","58","CF",
            "D0","EF","AA","FB","43","4D","33","85","45","F9","02","7F","50","3C","9F","A8",
            "51","A3","40","8F","92","9D","38","F5","BC","B6","DA","21","10","FF","F3","D2",
            "CD","0C","13","EC","5F","97","44","17","C4","A7","7E","3D","64","5D","19","73",
            "60","81","4F","DC","22","2A","90","88","46","EE","B8","14","DE","5E","0B","DB",
            "E0","32","3A","0A","49","06","24","5C","C2","D3","AC","62","91","95","E4","79",
            "E7","C8","37","6D","8D","D5","4E","A9","6C","56","F4","EA","65","7A","AE","08",
            "BA","78","25","2E","1C","A6","B4","C6","E8","DD","74","1F","4B","BD","8B","8A",
            "70","3E","B5","66","48","03","F6","0E","61","35","57","B9","86","C1","1D","9E",
            "E1","F8","98","11","69","D9","8E","94","9B","1E","87","E9","CE","55","28","DF",
            "8C","A1","89","0D","BF","E6","42","68","41","99","2D","0F","B0","54","BB","16"
        };
        RK = new string[] { RK[1], RK[2], RK[3], RK[4], RK[0] };
        RK[0] = SBOX[alpha.IndexOf(RK[0][0]) * 16 + alpha.IndexOf(RK[0][1])];
        for (int i = 1; i < RK.Length; i++)
        {
            string b1 = HexToBin(RK[i - 1]);
            string b2 = HexToBin(RK[i]);
            string b3 = "";
            for (int j = 0; j < b1.Length; j++)
                b3 = b3 + (b1[j] == b2[j] ? "0" : "1");
            RK[i] = BinToHex(b3);
        }
        return RK;
    }
}
