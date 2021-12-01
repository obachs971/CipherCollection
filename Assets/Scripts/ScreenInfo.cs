using UnityEngine;

namespace CipherMachine
{
    public struct ScreenInfo
    {
        public string Text { get; private set; }
        public int FontSize { get; private set; }
        public Font TextFont { get; set; }
        public Material FontMaterial { get; set; }
        public ScreenInfo(string text, int fontSize)
        {
            Text = text;
            FontSize = fontSize;
            TextFont = null;
            FontMaterial = null;
        }
        public override string ToString()
        {
            return Text;
        }
    }
}
