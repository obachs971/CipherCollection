using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripCipher
{
    public ResultInfo encrypt(string word, string id, string log, KMBombInfo Bomb, bool invert)
    {
        Debug.LogFormat("{0} Begin Strip Cipher", log);
        CMTools cm = new CMTools();
        int[] val = cm.generateValue(Bomb);
        int col = (invert) ? ((val[1] % 25) + 1) : (25 - (val[1] % 25));
        string[] temp = getStrips(word.Length), strips = new string[word.Length], nums = { "", "" };
        string encrypt = "";
        for(int i = 0; i < temp.Length; i++)
        {
            nums[0] = nums[0] + temp[i].Substring(0, 1);
            nums[1] = nums[1] + temp[i].Substring(1, 1);
            strips[i] = temp[i].Substring(2);
            int index = strips[i].IndexOf(word[i]);
            strips[i] = strips[i].Substring(index) + strips[i].Substring(0, index);
            encrypt = encrypt + "" + strips[i][col];
        }
        Debug.LogFormat("{0} [Strip Cipher] {1}", log, nums[0]);
        Debug.LogFormat("{0} [Strip Cipher] {1}", log, nums[1]);
        for (int i = 0; i < strips.Length; i++)
            Debug.LogFormat("{0} [Strip Cipher] {1}", log, strips[i]);
        Debug.LogFormat("{0} [Strip Cipher] Using {1} Instructions", log, (invert) ? "Encrypt" : "Decrypt");
        Debug.LogFormat("{0} [Strip Cipher] Column: {1} -> {2} -> {3}", log, ((char)val[0]), val[1], (col + 1));
        Debug.LogFormat("{0} [Strip Cipher] {1} -> {2}", log, word, encrypt);
        ScreenInfo[] screens = new ScreenInfo[9];
        screens[0] = new ScreenInfo(nums[0], new int[] { 35, 35, 35, 32, 28 }[nums[0].Length - 4]);
        screens[1] = new ScreenInfo(((char)val[0]) + "", 25);
        screens[2] = new ScreenInfo(nums[1], new int[] { 35, 35, 35, 32, 28 }[nums[1].Length - 4]);
        for (int i = 3; i < 8; i++)
            screens[i] = new ScreenInfo();
        screens[8] = new ScreenInfo(id, 35);
        return new ResultInfo
        {
            Encrypted = encrypt,
            Score = 5,
            Pages = new PageInfo[] { new PageInfo(screens, invert) }
        };
    }
    private string[] getStrips(int length)
    {
        string[] key = new string[length];
        List<string> strips = new List<string>()
        {
            "00DTYLUKCNOIVSEPFQJWRGMBZAXH",
            "01LOGKWPRMDUVSYFJNTQIHEAZCBX",
            "02YGSKCJETRMUWNHQXIZFAOVBLDP",
            "03BERHJYKWCSLNPDZIGFUAOXVQTM",
            "04JLSGAOPZEMBVQCUIYDTHXRWFKN",
            "05IPWSURJTOQGEVDBMYKLNHAXZFC",
            "06CGLXSIBTJHOKNDMQPVRZAYEFUW",
            "07YGHKDVLQEXUOASZWPJFBCNRITM",
            "08ACZLSOGEDPYWFXHBVIMUKNTJQR",
            "09XTHNIBAFEUQSGLJDWOZKMPVCYR",
            "10MFKGNURPJZTBQWLCASIHVYOXDE",
            "11NHTEPCFDXRYZBAIMSGVJKUOQWL",
            "12AHFPGVUKLMNCTSRDEIWZXYQBOJ",
            "13SAHIKWDQJNPVUTZCBYLOGFMREX",
            "14GPNWMTOSQHJVYKFEALXCIRDBZU",
            "15GPCWQOSVZKINJHERUABDTMXLFY",
            "16NKAELXYVRDOGZIMTFUBSPJQHWC",
            "17EFTXLBCAWHUJGVOMYRSNKDQIZP",
            "18ZLNRQBAVYMJUDOTSHXCWGPKIEF",
            "19YAGDUTFIXMBWLOJQVNERHKSZPC",
            "20THRAWJEMNFKYZCIGOBXVSULDQP",
            "21INESDOMTPBQGHYFUZCRVWLKJXA",
            "22YIRAELHQOSXGCWTPJVZUNFMDKB",
            "23ITLFAXYCMOPVGZHURDBWNSJQKE",
            "24QRJGEFWVLKSUHPCXYBMOITNZAD",
            "25VYGNODPUJEMFCZIXASKBWHRTLQ",
            "26ANZIFPCLOKMHSJDEWVBYQXTURG",
            "27NEBZCRMKDPATGLSOWHFIJQVUYX",
            "28EQJTZFYSINAUBWCVDXRKHPLGMO",
            "29KDOTRBZWIAUYPLNHEGFQSJMCXV",
            "30XVSRDIUZFCQTMPLHYEGJAKNWOB",
            "31MVELIBQAFGJNPCKWXUSTZRDYHO",
            "32BMOITNEWDAUVPHZJXYSLRQCFGK",
            "33UYWVFPNHDCRSMZQGXOBTEKLAIJ",
            "34LBTNMCYODQIKHSFUJEVARPXWGZ",
            "35NFGUQPSTMCKOXJVWELIABRHYZD",
            "36CWMFKHPLOVIQXDARBUTEGZJSNY",
            "37ITOPAJWHEDZKMCVUXBYQSNLRGF",
            "38EODXLUVTSYHMFGAJNWPZQBCIKR",
            "39HZACTFNPIXMQRDUBYKVJOGWLES",
            "40KYFAZHCLPNQGXWDTVBORUJIMSE",
            "41NSWZTGXHKVBPCORLYQEAIJDFUM",
            "42XSYLMPKQIZWEANTOVHRDBFUCGJ",
            "43XYISKJVQMTRCAUNGZEOHFWLBDP",
            "44KDMEPYHGQZTSUVJFXOILRANBWC",
            "45GEVKJRNFBXWQPHDOAMTLIZYUCS",
            "46MZEKBDFIGQTLJPOWUXSHCRNYVA",
            "47AZLETRUFIPJHBXKOSYQMNGDCVW",
            "48KNEOJVXFQWCHTDGUMZLYSRAIPB",
            "49UIROVSWAGEQXTHZCFYLBDJPKMN",
            "50XBDQIFRUVENLHOAZPWGMKJTCSY",
            "51BPJZGEVCNTMAOIKHDWSRFUXQLY",
            "52CXEDARNFZGLSPWKQHTVIUBMOJY",
            "53IZMLCRNWAKTBUHJSPFOEGYQXDV",
            "54JOBCRSIAHGZKNYQLDFEPVXWMUT",
            "55WPCKJMQTZIELARUBSOXFVYHDNG",
            "56KHVUDGMOJWPYRFSQBLZACITNXE",
            "57VFIZLTQPMKRACDSOGJXEUNBWYH",
            "58GFYVTDQLHWJPKMBAZNIUOSCXER",
            "59QKMSFAZBVPHGWIODEXUCNYJLTR",
            "60IAPDHNYVFCMOERLUJTQBWSZXKG",
            "61ARZOWSMPBKJLVDGUIYNXFHCETQ",
            "62DHOXKZWVTCPBRMGIQALYJFUNES",
            "63TJFPRHKUWQOMXNIBLYVDZEAGSC",
            "64ACBZGTPNSJYDVLXRHOWKUEFMQI",
            "65FPHEOKUXNQMZWIVRTCSGBDLJAY",
            "66DJABIUXEYQOKRZNSLMPGCTVHFW",
            "67AIHDGCNLPQOVTKMJFSRZEBUYXW",
            "68JKULTOCZYWNDBIXHQMPSFGEARV",
            "69BZJTGQCFKWRPODNLYMSEVHIUAX",
            "70CQDBVGIZRNJKFLUXAWYTESPMOH",
            "71TADSIQMURKNHYVXCELWOPZGFJB",
            "72VSYUFWHJKOBNTIEDRXMLACZPGQ",
            "73ILCBVHDKSURWXJNFAEYOPQMGZT",
            "74WHVQOLDZPURMGEXTSFYBAICJKN",
            "75EZKTAMWIYJQXPLOVBCHNGUDFRS",
            "76VCLSBQWEDKGTYIFXHARMZUNPJO",
            "77FYAJDGSOVPRCHQWUNITEBKZLMX",
            "78BXQWVTCEURIZKAGPNLODFJHMYS",
            "79MJPKITCUYZSXBOGLADEWVFQNHR",
            "80IZEXRFDHAGSQNPTVBMLWKOUJCY",
            "81CTPONKGRMUJWQEYXZVALHIDBFS",
            "82HVFBTXSJLNAYPZUQOMRGWICKED",
            "83UCSBJDZOTEIQHARVYNWLPMGKFX",
            "84LUJHAXCWIRPMVDQNTGBZEFYOKS",
            "85AHUKMVEPFNBXYCTORQDSIWZGJL",
            "86XWLNYZGIAKJSURDHMQCETFVPBO",
            "87VSIJNAXHZLPOQYGRDKMUWFBETC",
            "88EMWOFAKYTNQZGXJPLVBRCIUDSH",
            "89RSLIHTMPNJXGOCKDUQFAZYWVBE",
            "90XCJIGNOKFEHMTADBYWPLSZRUQV",
            "91AJCKXTMLDWHEZBNYORUIVGPSQF",
            "92RZEWMCBXITNQLYADSOVGFUPHKJ",
            "93JMSOVGIPCLYUNDRTFEWQBHXKZA",
            "94FYCRPOJNHLSKVUBIDMXAEQZTGW",
            "95UXZRBFIQNYLWDKCHSJTPAVEGMO",
            "96SRJTMXUZBWGFYDKEVOPAHILQCN",
            "97QOHEWVDSTAKJIBNXPGCLRYMZFU",
            "98OKSGRZYCDEWVJPAHXFLIMUNBTQ",
            "99ULDOMNSRCYGVBPXQWAZJFKEITH"
        };
        for(int i = 0; i < key.Length; i++)
        {
            key[i] = strips[UnityEngine.Random.Range(0, strips.Count)].ToUpperInvariant();
            strips.Remove(key[i]);
        }
        return key;
    }
}
