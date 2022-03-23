public abstract class CipherBase
{
    public abstract string Name { get; }
    public abstract int Score { get; }
    public abstract string Code { get; }
    public abstract ResultInfo Encrypt(string word, KMBombInfo bomb);
}
