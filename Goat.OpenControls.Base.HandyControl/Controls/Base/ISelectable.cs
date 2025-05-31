using System.Windows;

namespace Goat.OpenControls.Base.HandyControl.Controls
{
    public interface ISelectable
    {
        event RoutedEventHandler Selected;

        bool IsSelected { get; set; }
    }
}
