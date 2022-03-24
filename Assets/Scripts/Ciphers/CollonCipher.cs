using CipherMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Words;

public class CollonCipher : CipherBase
{
	public override string Name { get { return "Collon Cipher"; } }
	public override int Score { get { return 5; } }
	public override string Code { get { return "CO"; } }

	public override ResultInfo Encrypt(string word, KMBombInfo bomb)
	{
		var logMessages = new List<string>();
        string kw = new Data().PickWord(4, 8);
		string replaceJ = "";
		string alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
		logMessages.Add(string.Format("Before Replacing Js: {0}", word));
		for (int i = 0; i < word.Length; i++)
		{
			if (word[i] == 'J')
			{
				word = word.Substring(0, i) + "" + alpha[Random.Range(0, alpha.Length)] + "" + word.Substring(i + 1);
				replaceJ = replaceJ + "" + word[i];
			}
			else
				replaceJ = replaceJ + "" + alpha.Replace(word[i].ToString(), "")[Random.Range(0, 24)];
		}
		logMessages.Add(string.Format("After Replacing Js: {0}", word));
		logMessages.Add(string.Format("Screen 3: {0}", replaceJ));
		var keyFront = CMTools.generateBoolExp(bomb);
		string key = CMTools.getKey(kw.Replace("J", "I"), alpha.ToString(), keyFront.Value);
		logMessages.Add(string.Format("Keyword: {0}", kw));
		logMessages.Add(string.Format("Keyword Front Rule: {0} -> {1}", keyFront.Expression, keyFront.Value));
		logMessages.Add(string.Format("Key: {0}", key));
		string[] rc = { "", "" };
		for(int i = 0; i < word.Length; i++)
		{
			int row = key.IndexOf(word[i]) / 5;
			int col = key.IndexOf(word[i]) % 5;
			rc[0] = rc[0] + "" + key[(row * 5) + ((col + Random.Range(0, 4) + 1) % 5)];
			rc[1] = rc[1] + "" + key[(((row + Random.Range(0, 4) + 1) % 5) * 5) + col];
			logMessages.Add(string.Format("{0} -> {1}{2}", word[i], rc[0][i], rc[1][i]));
		}
		logMessages.Add(string.Format("{0} -> {1}", word, rc[0]));
		return new ResultInfo
		{
			LogMessages = logMessages,
			Encrypted = rc[0],
			Pages = new[] { new PageInfo(new ScreenInfo[] { kw, keyFront.Expression, rc[1], null, replaceJ }) }
		};
	}
}
