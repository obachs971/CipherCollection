namespace CipherMachine
{
    public sealed class PageInfo
    {
        // Info provided by the ciphers
        public ScreenInfo[] Screens { get; private set; }
        public bool Invert { get; private set; }

        // Info added by the module
        public string Code { get; set; }
        public int? Checksum { get; set; }

        public PageInfo(ScreenInfo[] screens, bool invert = false)
        {
            Screens = screens;
            Invert = invert;
        }
    }
}
