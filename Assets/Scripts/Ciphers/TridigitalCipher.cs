using System.Collections.Generic;
using CipherMachine;
using UnityEngine;
using Words;

public class TridigitalCipher : CipherBase
{
	public override string Name { get { return "Tridigital Cipher"; } }
	public override int Score { get { return 5; } }
	public override string Code { get { return "TR"; } }
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
	{
		var logMessages = new List<string>();
        string kw = new Data().PickWord(4, 8);
		string encrypt = "";
		string nums = "";
		string[] keyFront = CMTools.generateBoolExp(bomb);
		string key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", keyFront[1][0] == 'T');
        logMessages.Add(string.Format("Keyword: {0}", kw));
		logMessages.Add(string.Format("Key Front Rule: {0} -> {1}", keyFront[0], keyFront[1]));
		logMessages.Add(string.Format("Key: {0}", key));
		string[] alpha = { "AJS", "BKT", "CLU", "DMV", "ENW", "FOX", "GPY", "HQZ", "IR" };
		foreach(char c in word)
		{
			int index = key.IndexOf(c);
			nums = nums + "" + ((index / 9) + 1);
			encrypt = encrypt + "" + alpha[index % 9][Random.Range(0, alpha[index % 9].Length)];
			logMessages.Add(string.Format("{0} -> {1}{2} -> {3}{4}", c, nums[nums.Length - 1], ((index % 9) + 1), nums[nums.Length - 1], encrypt[encrypt.Length - 1]));
		}
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
		screens[1] = new ScreenInfo(keyFront[0], 25);
		screens[2] = new ScreenInfo(nums, new int[] { 35, 35, 35, 32, 28 }[nums.Length - 4]);
		return new ResultInfo
		{
			LogMessages = logMessages,
			Encrypted = encrypt,
			Pages = new PageInfo[] { new PageInfo(screens) }
		};
	}
}
