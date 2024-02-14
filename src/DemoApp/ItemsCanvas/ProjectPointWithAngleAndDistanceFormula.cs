namespace DemoApp.ItemsCanvas;

public record struct ProjectPointWithAngleAndDistanceFormula(double Distance)
{
    public record struct Point(double Left, double Top);

    public Point Project(Point point, double angle)
    {
        var (newLeft, newTop) = this.TranslatePointWithLengthAndAngle(left: point.Left, top: point.Top, angle);

        // the bottom right is actually the top corner
        return new(Left: newLeft, Top: newTop);
    }

    private (double Right, double Bottom) TranslatePointWithLengthAndAngle(double left, double top, double angle)
        //=> (left + Math.Round(Length * Math.Cos(Deg2Rad(angle)), 4), top + Math.Round(Length * Math.Sin(Deg2Rad(angle)), 4));
        => (left + this.Distance * Math.Cos(Deg2Rad(angle)), top + this.Distance * Math.Sin(Deg2Rad(angle)));

    private double Deg2Rad(double deg) => deg * Math.PI / 180.0;
}