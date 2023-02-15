using System.Collections.Generic;
using CipherMachine;
using UnityEngine;
using Words;

public class TridigitalCipher : CipherBase
{
	public override string Name { get { return "Tridigital Cipher"; } }
	public override int Score(int wordLength) { return 6; }
	public override string Code { get { return "TR"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
	{
		var logMessages = new List<string>();
        string kw = new Data().PickWord(4, 8);
		string encrypt = "";
		string nums = "";
		var keyFront = CMTools.generateBoolExp(bomb);
		string key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", keyFront.Value);
        logMessages.Add(string.Format("Keyword: {0}", kw));
		logMessages.Add(string.Format("Key Front Rule: {0} -> {1}", keyFront.Expression, keyFront.Value));
		logMessages.Add(string.Format("Key: {0}", key));
		string[] alpha = { "AJS", "BKT", "CLU", "DMV", "ENW", "FOX", "GPY", "HQZ", "IR" };
		foreach(char c in word)
		{
			int index = key.IndexOf(c);
			nums = nums + "" + ((index / 9) + 1);
			encrypt = encrypt + "" + alpha[index % 9][Random.Range(0, alpha[index % 9].Length)];
			logMessages.Add(string.Format("{0} -> {1}{2} -> {3}{4}", c, nums[nums.Length - 1], ((index % 9) + 1), nums[nums.Length - 1], encrypt[encrypt.Length - 1]));
		}
		return new ResultInfo
		{
			LogMessages = logMessages,
			Encrypted = encrypt,
			Pages = new[] { new PageInfo(new ScreenInfo[] { kw, keyFront.Expression, nums }) }
		};
	}
}
