using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Goat.OpenControls.Base.HandyControl.Controls
{
    public class EnumPropertyEditor : PropertyEditorBase
    {
        public override FrameworkElement CreateElement(PropertyItem propertyItem) => new System.Windows.Controls.ComboBox
        {
            IsEnabled = !propertyItem.IsReadOnly,
            ItemsSource = Enum.GetValues(propertyItem.PropertyType)
        };

        public override DependencyProperty GetDependencyProperty() => Selector.SelectedValueProperty;
    }
}
