using UnityEngine;

namespace CipherMachine
{
    public struct ScreenInfo
    {
        public string Text { get; private set; }
        public Font TextFont { get; set; }
        public Material FontMaterial { get; set; }
        public ScreenInfo(string text)
        {
            Text = text;
            TextFont = null;
            FontMaterial = null;
        }
        public override string ToString()
        {
            return Text;
        }
        public static implicit operator ScreenInfo(string s) { return new ScreenInfo(s); }
    }
}