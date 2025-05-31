using System;

namespace Goat.OpenControls.Base.HandyControl.Controls
{
    public interface ISingleOpen : IDisposable
    {
        bool CanDispose { get; }
    }
}
