using UnityEngine;
using CipherMachine;

namespace CipherMachine
{
    public sealed class PageInfo
    {
        public ScreenInfo[] Screens { get; private set; }
        
        public PageInfo(params ScreenInfo[] screens)
        {
            Screens = screens;
        }
    }
}
