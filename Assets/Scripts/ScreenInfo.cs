namespace CipherMachine
{
    public struct ScreenInfo
    {
        public string Text { get; private set; }
        public CMFont Font { get; private set; }
        public ScreenInfo(string text)
        {
            Text = text;
            Font = CMFont.Default;
        }
        public ScreenInfo(string text, CMFont font)
        {
            Text = text;
            Font = font;
        }
        public static implicit operator ScreenInfo(string s) { return new ScreenInfo(s); }
    }
}