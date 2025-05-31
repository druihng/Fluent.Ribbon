using System.Windows;

namespace Goat.OpenControls.Base.HandyControl.Data
{
    public class CancelRoutedEventArgs : RoutedEventArgs
    {
        public CancelRoutedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
        }

        public bool Cancel { get; set; }
    }
}
