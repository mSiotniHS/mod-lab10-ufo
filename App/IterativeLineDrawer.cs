using static App.TaylorSeriesFunctions;

namespace App;

public abstract record DrawingStatus
{
	private DrawingStatus() {}

	public record Continue(PointD Next) : DrawingStatus;

	public record OutOfReact : DrawingStatus;

	public record InNeighbourhood : DrawingStatus;
}

public sealed class IterativeLineDrawer
{
	public SystemParameters Parameters;
	public uint Precision;
	public double Epsilon;

	private double _angle;
	private PointD _currentPoint;
	private readonly double _outOfReachDistance;

	public PointD CurrentPoint => _currentPoint;

	public IterativeLineDrawer(
		SystemParameters parameters,
		uint precision,
		double epsilon,
		double outOfReachDistance)
	{
		Parameters = parameters;
		Precision = precision;
		Epsilon = epsilon;
		_outOfReachDistance = outOfReachDistance;

		InitializeState();
	}

	public DrawingStatus Next()
	{
		if (CurrentPointInNeighbourhood())
			return new DrawingStatus.InNeighbourhood();

		if (CurrentPointOutOfReach())
			return new DrawingStatus.OutOfReact();

		_currentPoint.X += Parameters.Step * Cos(_angle, Precision);
		_currentPoint.Y -= Parameters.Step * Sin(_angle, Precision);

		return new DrawingStatus.Continue(_currentPoint);
	}

	private double CalculateAngle() =>
		Arctan((Parameters.StartPoint.Y - Parameters.EndPoint.Y) / (Parameters.EndPoint.X - Parameters.StartPoint.X), Precision);

	public void InitializeState()
	{
		_currentPoint = Parameters.StartPoint;
		_angle = CalculateAngle();
	}

	private static double Distance(PointD first, PointD second) =>
		Math.Sqrt(Pow(first.X - second.X, 2) + Pow(first.Y - second.Y, 2));

	private bool CurrentPointOutOfReach() =>
		Distance(_currentPoint, Parameters.EndPoint) >= _outOfReachDistance;

	private bool CurrentPointInNeighbourhood() =>
		Distance(_currentPoint, Parameters.EndPoint) <= Epsilon;
}
