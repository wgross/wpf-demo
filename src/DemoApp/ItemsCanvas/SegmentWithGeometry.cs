using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace DemoApp.ItemsCanvas;

public sealed partial class SegmentWithGeometry : SegmentBase
{
    public SegmentWithGeometry()
    {
        this.Height = 40;
        this.Width = 100;
    }

    [ObservableProperty]
    private EllipseGeometry hingeOuter = new EllipseGeometry(center: new(27, 27), radiusX: 6.25, radiusY: 6.25);

    [ObservableProperty]
    private EllipseGeometry hingeInner = new EllipseGeometry(center: new(27, 27), radiusX: 4.0, radiusY: 4.0);

    [ObservableProperty]
    private LineGeometry armSegment = new LineGeometry(startPoint: new(27, 27), endPoint: new(100, 27));

    [ObservableProperty]
    private double height;

    [ObservableProperty]
    private double width;

    [ObservableProperty]
    private double angle;

    [ObservableProperty]
    private double actualHeight;

    [ObservableProperty]
    private double actualWidth;

    public ProjectPointWithAngleAndDistanceFormula.Point AsPoint() => new(this.Left, this.Top);
}