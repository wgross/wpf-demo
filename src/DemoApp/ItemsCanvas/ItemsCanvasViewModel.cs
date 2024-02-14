using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace DemoApp.ItemsCanvas;

public class ItemsCanvasViewModel : ObservableObject
{
    private readonly DispatcherTimer timer;

    public ItemsCanvasViewModel()
    {
        this.Segments.Add(new PedestalWithGeometry() { Left = 265, Top = 505 });
        this.Segments.Add(new SegmentWithGeometry { Left = 300, Top = 500, Angle = 0 });
        this.Segments.Add(new SegmentWithGeometry { Left = 400, Top = 500, Angle = 0 });
        this.Segments.Add(new SegmentWithGeometry { Left = 500, Top = 500, Angle = 0 });
        this.Segments.Add(new SegmentWithGeometry { Left = 600, Top = 500, Angle = 0 });
        this.Segments.Add(new SegmentWithGeometry { Left = 700, Top = 500, Angle = 0 });
        this.Segments.Add(new SegmentWithGeometry { Left = 800, Top = 500, Angle = 0 });

        this.timer = new DispatcherTimer();
        this.timer.Tick += this.Tick;
        this.timer.Interval = TimeSpan.FromMilliseconds(100);
        this.timer.Start();
    }

    public ObservableCollection<ObservableObject> Segments { get; } = new ObservableCollection<ObservableObject>();

    private void Tick(object sender, EventArgs e)
    {
        // in first pass update the angle of all segments according to the input
        var segments = this.Segments.OfType<SegmentWithGeometry>().ToArray();
        var formula = new ProjectPointWithAngleAndDistanceFormula(segments[0].ActualWidth);

        ProjectPointWithAngleAndDistanceFormula.Point? projectedPoint = null;

        for (int i = 0; i < segments.Length; i++)
        {
            segments[i].Angle += i + 1;

            // attach to the predecessor of there is a predecessor
            if (projectedPoint is not null)
            {
                segments[i].Left = projectedPoint.Value.Left;
                segments[i].Top = projectedPoint.Value.Top;
            }

            // calculate the rotated box for the current segment
            projectedPoint = formula.Project(segments[i].AsPoint(), segments[i].Angle);
        }
    }
}