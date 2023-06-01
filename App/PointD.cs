namespace App;

public record struct PointD(double X, double Y)
{
	public PointF ToPointF() => new((float) X, (float) Y);
}
