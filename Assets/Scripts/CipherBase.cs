public abstract class CipherBase
{
    public abstract string Name { get; }
    public abstract string Code { get; }
    public virtual bool IsInvert { get { return false; } }
    public virtual bool IsTransposition { get { return false; } }
    public abstract ResultInfo Encrypt(string word, KMBombInfo bomb);
}
