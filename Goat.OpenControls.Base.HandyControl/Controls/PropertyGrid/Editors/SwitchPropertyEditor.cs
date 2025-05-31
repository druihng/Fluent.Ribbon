using System.Windows;
using System.Windows.Controls.Primitives;
using Goat.OpenControls.Base.HandyControl.Tools;

namespace Goat.OpenControls.Base.HandyControl.Controls
{
    public class SwitchPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new ToggleButton
        {
            Style = ResourceHelper.GetResourceInternal<Style>("ToggleButtonSwitch"),
            HorizontalAlignment = HorizontalAlignment.Left,
            IsEnabled = !propertyItem.IsReadOnly
        };

        public override DependencyProperty GetDependencyProperty() => ToggleButton.IsCheckedProperty;
    }
}
