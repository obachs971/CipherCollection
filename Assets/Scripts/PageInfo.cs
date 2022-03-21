namespace CipherMachine
{
    public sealed class PageInfo
    {
        public ScreenInfo[] Screens { get; private set; }
        public bool Invert { get; private set; }
        public PageInfo(ScreenInfo[] screens, bool invert = false)
        {
            Screens = screens;
            Invert = invert;
        }
    }
}
