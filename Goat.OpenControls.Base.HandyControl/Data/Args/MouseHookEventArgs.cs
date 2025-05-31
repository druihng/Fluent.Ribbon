using System;
using Goat.OpenControls.Base.HandyControl.Tools.Interop;

namespace Goat.OpenControls.Base.HandyControl.Data
{
    internal class MouseHookEventArgs : EventArgs
    {
        public MouseHookMessageType MessageType { get; set; }

        public InteropValues.POINT Point { get; set; }
    }
}
