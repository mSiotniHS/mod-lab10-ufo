using System.Numerics;

namespace App;

public static class TaylorSeriesFunctions
{
	public static double Sin(double x, uint n)
	{
		var sum = 0.0;
		var term = x;
		var xSquared = x * x;

		for (var i = 1u; i <= n; i++)
		{
			if (i != 1u)
			{
				term *= -xSquared;
				term /= (2 * i - 2) * (2 * i - 1);
			}

			sum += term;
		}

		return sum;
	}

	public static double Cos(double x, uint n)
	{
		var sum = 0.0;
		var term = 1.0;
		var xSquared = x * x;

		for (var i = 1u; i <= n; i++)
		{
			if (i != 1u)
			{
				term *= -xSquared;
				term /= (2 * i - 3) * (2 * i - 2);
			}

			sum += term;
		}

		return sum;
	}

	public static double Arctan(double x, uint n)
	{
		var sum = 0.0;
		var term = x;
		var xSquared = x * x;

		for (var i = 1u; i <= n; i++)
		{
			if (i != 1u)
			{
				term *= -xSquared * (2 * i - 3);
				term /= 2 * i - 1;
			}

			sum += term;
		}

		return sum;
	}

	public static T Pow<T>(T x, uint n) where T : INumber<T>
	{
		var value = T.One;

		for (var i = 1u; i <= n; i++)
		{
			value *= x;
		}

		return value;
	}
}
