using System.Windows.Media;

namespace Goat.OpenControls.Base.HandyControl.Expression.Media
{
    public interface IGeometrySourceParameters
    {
        Stretch Stretch { get; }

        Brush Stroke { get; }

        double StrokeThickness { get; }
    }
}
