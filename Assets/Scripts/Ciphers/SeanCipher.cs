using System.Collections.Generic;
using CipherMachine;
using UnityEngine;
using Words;

public class SeanCipher : CipherBase
{
	public override string Name { get { return "Sean Cipher"; } }
	public override int Score(int wordLength) { return 7; }
	public override string Code { get { return "SE"; } }
	public override ResultInfo Encrypt(string word, KMBombInfo bomb)
	{
		var logMessages = new List<string>();
		var cw = CMTools.generateBoolExp(bomb);
		var kwfront = CMTools.generateBoolExp(bomb);
		string kw = new Data().PickWord(4, 8);
		string encrypt = "", key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", kwfront.Value);
		logMessages.Add(string.Format("Keyword: {0}", kw));
		logMessages.Add(string.Format("Key Front Rule: {0} -> {1}", kwfront.Expression, kwfront.Value));
		logMessages.Add(string.Format("Clockwise Rule: {0} -> {1}", cw.Expression, cw.Value));
		
		if (cw.Value)
		{
			foreach (char c in word)
			{
				encrypt = encrypt + "" + key[(key.IndexOf(c) + 13) % 26];
				logMessages.Add(string.Format("{0}", key.Substring(0, 13)));
				logMessages.Add(string.Format("{0}", key.Substring(13)));
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
		return new ResultInfo
		{
			LogMessages = logMessages,
			Encrypted = encrypt,
			Pages = new[] { new PageInfo(new ScreenInfo[] { kw, kwfront.Expression, cw.Expression }) }
		};
	}
}
