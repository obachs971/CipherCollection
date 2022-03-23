using System.Collections.Generic;
using CipherMachine;
using UnityEngine;
using Words;

public class SeanCipher : CipherBase
{
	public override string Name { get { return "Sean Cipher"; } }
	public override int Score { get { return 5; } }
	public override string Code { get { return "SE"; } }
	public override ResultInfo Encrypt(string word, KMBombInfo bomb)
	{
		var logMessages = new List<string>();
		string[] cw = CMTools.generateBoolExp(bomb);
		string[] kwfront = CMTools.generateBoolExp(bomb);
		string kw = new Data().PickWord(4, 8);
		string encrypt = "", key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront[1][0] == 'T');
		logMessages.Add(string.Format("Keyword: {0}", kw));
		logMessages.Add(string.Format("Key Front Rule: {0} -> {1}", kwfront[0], kwfront[1]));
		logMessages.Add(string.Format("Clockwise Rule: {0} -> {1}", cw[0], cw[1]));
		
		if (cw[1][0] == 'T')
		{
			foreach (char c in word)
			{
				encrypt = encrypt + "" + key[(key.IndexOf(c) + 13) % 26];
				logMessages.Add(string.Format("\n{0}\n{1}", key.Substring(0, 13), key.Substring(13)));
				logMessages.Add(string.Format("{0} -> {1}", c, encrypt[encrypt.Length - 1]));
				key = key[13] + key.Substring(0, 12) + key.Substring(14, 12) + key[12];
			}
		}
		else
		{
			foreach (char c in word)
			{
				encrypt = encrypt + "" + key[(key.IndexOf(c) + 13) % 26];
				logMessages.Add(string.Format("\n{0}\n{1}", key.Substring(0, 13), key.Substring(13)));
				logMessages.Add(string.Format("{0} -> {1}", c, encrypt[encrypt.Length - 1]));
				key = key.Substring(1, 12) + key[25] + "" + key[0] + key.Substring(13, 12);
			}
		}
		logMessages.Add(string.Format("{0} -> {1}", word, encrypt));
		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
		screens[1] = new ScreenInfo(kwfront[0], 25);
		screens[2] = new ScreenInfo(cw[0], 35);
		return new ResultInfo
		{
			LogMessages = logMessages,
			Encrypted = encrypt,
			Pages = new PageInfo[] { new PageInfo(screens) }
		};
	}
}