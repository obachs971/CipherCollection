using System.Collections.Generic;
using CipherMachine;
using UnityEngine;

public class StripCipher : CipherBase
{
    public override string Name { get { return invert ? "Inverted Strip Cipher" : "Strip Cipher"; } }
    public override string Code { get { return "ST"; } }

    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
    public StripCipher(bool invert) { this.invert = invert; }

    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
        var logMessages = new List<string>();
        var val = CMTools.generateValue(bomb);
        int col = (invert) ? ((val.Value % 25) + 1) : (25 - (val.Value % 25));
        string[] temp = getStrips(word.Length), strips = new string[word.Length], nums = { "", "" };
        string encrypt = "";
        for (int i = 0; i < temp.Length; i++)
        {
            nums[0] = nums[0] + temp[i].Substring(0, 1);
            nums[1] = nums[1] + temp[i].Substring(1, 1);
            strips[i] = temp[i].Substring(2);
            int index = strips[i].IndexOf(word[i]);
            strips[i] = strips[i].Substring(index) + strips[i].Substring(0, index);
            encrypt = encrypt + "" + strips[i][col];
        }
        logMessages.Add(nums[0]);
        logMessages.Add(nums[1]);
        for (int i = 0; i < strips.Length; i++)
            logMessages.Add(strips[i]);
        logMessages.Add(string.Format("Column: {0} -> {1} -> {2}", val.Expression, val.Value, col + 1));
        logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
        return new ResultInfo
        {
            LogMessages = logMessages,
            Encrypted = encrypt,
            Pages = new[] { new PageInfo(new ScreenInfo[] { nums[0], val.Expression, nums[1] }, invert) },
            Score = 7
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
        for (int i = 0; i < key.Length; i++)
        {
            key[i] = strips[Random.Range(0, strips.Count)].ToUpperInvariant();
            strips.Remove(key[i]);
        }
        return key;
    }
}
