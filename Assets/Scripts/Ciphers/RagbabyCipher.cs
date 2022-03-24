using System.Collections.Generic;
using CipherMachine;
using Words;

public class RagbabyCipher : CipherBase
{
	public override string Name { get { return invert ? "Inverted Ragbaby Cipher" : "Ragbaby Cipher"; } }
	public override int Score { get { return 5; } }
	public override string Code { get { return "RA"; } }
    
    private readonly bool invert;
    public RagbabyCipher(bool invert) { this.invert = invert; }
    
    public override ResultInfo Encrypt(string word, KMBombInfo bomb)
    {
		var logMessages = new List<string>();
		var keyFront = CMTools.generateBoolExp(bomb);
		string kw = new Data().PickWord(4, 8);
		string key = CMTools.getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", keyFront.Value);
		string encrypt = "";
		if(invert)
		{
			for(int i = 0; i < word.Length; i++)
				encrypt = encrypt + "" + key[CMTools.mod(key.IndexOf(word[i]) - (i + 1), 26)];
		}
		else
		{
			for (int i = 0; i < word.Length; i++)
				encrypt = encrypt + "" + key[CMTools.mod(key.IndexOf(word[i]) + (i + 1), 26)];
		}
		logMessages.Add(string.Format("Keyword: {0}", kw));
		logMessages.Add(string.Format("Key: {0} -> {1} -> {2}", keyFront.Expression, keyFront.Value, key));
		logMessages.Add(string.Format("{0} -> {1}", word, encrypt));

		ScreenInfo[] screens = new ScreenInfo[9];
		screens[0] = new ScreenInfo(kw, new int[] { 35, 35, 35, 32, 28 }[kw.Length - 4]);
		screens[1] = new ScreenInfo(keyFront.Expression, 25);
		return new ResultInfo
		{
			LogMessages = logMessages,
			Encrypted = encrypt,
			Pages = new PageInfo[] { new PageInfo(screens, invert) }
		};
	}
}
