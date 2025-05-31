using System.Windows;
using Goat.OpenControls.Base.HandyControl.Data;

namespace Goat.OpenControls.Base.HandyControl.Controls
{
    public class TransferItem : SelectableItem
    {
        public static readonly DependencyProperty IsTransferredProperty = DependencyProperty.Register(
            "IsTransferred", typeof(bool), typeof(TransferItem), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsTransferred
        {
            get => (bool) GetValue(IsTransferredProperty);
            set => SetValue(IsTransferredProperty, ValueBoxes.BooleanBox(value));
        }

        public static readonly DependencyProperty IsOriginProperty = DependencyProperty.Register(
            "IsOrigin", typeof(bool), typeof(TransferItem), new PropertyMetadata(ValueBoxes.TrueBox));

        public bool IsOrigin
        {
            get => (bool) GetValue(IsOriginProperty);
            internal set => SetValue(IsOriginProperty, ValueBoxes.BooleanBox(value));
        }
    }
}
