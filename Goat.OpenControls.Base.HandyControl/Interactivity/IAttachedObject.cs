using System.Windows;

namespace Goat.OpenControls.Base.HandyControl.Interactivity
{
    public interface IAttachedObject
    {
        void Attach(DependencyObject dependencyObject);
        void Detach();

        DependencyObject AssociatedObject { get; }
    }
}
