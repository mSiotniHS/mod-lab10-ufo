using System.Numerics;

namespace App;

public static class TaylorSeriesFunctions
{
	private static double SinTerm(double x, uint i) =>
		(i % 2 == 0 ? -1 : 1) * Pow(x, 2 * i - 1) / Factorial(2 * i - 1);

	public static double Sin(double x, uint n) =>
		TaylorSeries(SinTerm, x, n);

	private static double CosTerm(double x, uint i) =>
		(i % 2 == 0 ? -1 : 1) * Pow(x, 2 * i - 2) / Factorial(2 * i - 2);

	public static double Cos(double x, uint n) =>
		TaylorSeries(CosTerm, x, n);

	private static double ArctanTerm(double x, uint i) =>
		(i % 2 == 0 ? -1 : 1) * Pow(x, 2*i - 1) / (2 * i - 1);

	public static double Arctan(double x, uint n) =>
		TaylorSeries(ArctanTerm, x, n);

	private static T TaylorSeries<T>(Func<T, uint, T> term, T x, uint n)
		where T : INumber<T>
	{
		var sum = T.Zero;
		for (var i = 1u; i <= n; i++)
		{
			sum += term(x, i);
		}

		return sum;
	}

	private static uint Factorial(uint n)
	{
		var value = 1u;

		for (var i = 2u; i <= n; i++)
		{
			value *= i;
		}

		return value;
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
