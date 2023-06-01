namespace App;

public static class Analyzer
{
	public static IEnumerable<double> AnalyzeEpsilonAndPrecisionConnection(
		SystemParameters parameters,
		IEnumerable<uint> precisionSamples
	) => precisionSamples.Select(precision => FindBestFittingEpsilon(parameters, precision));

	private static double FindBestFittingEpsilon(SystemParameters parameters, uint precision)
	{
		var upperBound = 75.0;
		var lowerBound = 0.001;

		for (var i = 0;; i++)
		{
			var epsilon = (upperBound + lowerBound) / 2;
			var drawer = new IterativeLineDrawer(parameters, precision, epsilon, 1000);

			Console.WriteLine(epsilon);

			switch (IsReachingEndPoint(drawer), i >= 15)
			{
				case (true, true):
					return epsilon;
				case (true, false):
					upperBound = epsilon;
					break;
				default:
					lowerBound = epsilon;
					break;
			}
		}
	}

	private static bool IsReachingEndPoint(IterativeLineDrawer drawer)
	{
		drawer.InitializeState();

		while (true)
		{
			var status = drawer.Next();

			if (status is DrawingStatus.InNeighbourhood)
				return true;

			if (status is DrawingStatus.OutOfReact)
				return false;
		}
	}
}
