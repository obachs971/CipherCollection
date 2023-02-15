using System.Collections.Generic;
using CipherMachine;
using Words;

public class RagbabyCipher : CipherBase
{
	public override string Name { get { return invert ? "Inverted Ragbaby Cipher" : "Ragbaby Cipher"; } }
	public override int Score(int wordLength) { return 5; }
	public override string Code { get { return "RA"; } }
    
    private readonly bool invert;
    public override bool IsInvert { get { return invert; } }
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

		return new ResultInfo
		{
			LogMessages = logMessages,
			Encrypted = encrypt,
			Pages = new[] { new PageInfo(new ScreenInfo[] { kw, keyFront.Expression }, invert) }
		};
	}
}
