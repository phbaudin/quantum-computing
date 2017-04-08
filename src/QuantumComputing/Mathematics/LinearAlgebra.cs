using MathNet.Numerics.LinearAlgebra;
using System;
using System.Numerics;

namespace Lachesis.QuantumComputing.Mathematics
{
	public static class LinearAlgebra
	{
		/*
		 * Cartesian product of two vectors
		 */
		public static Vector<Complex> CartesianProduct(Vector<Complex> Vector1, Vector<Complex> Vector2)
		{
			Complex[] CartesianProductArray = new Complex[Vector1.Count * Vector2.Count];

			for (int i = 0; i < Vector1.Count; i++)
			{
				for (int j = 0; j < Vector2.Count; j++)
				{
					CartesianProductArray[i * Vector2.Count + j] = Vector1.At(i) * Vector2.At(j);
				}
			}
			
			return Vector<Complex>.Build.SparseOfArray(CartesianProductArray);
		}

		/*
		 * Vector representation of an integer
		 */
		public static Vector<Complex> VectorFromInteger(int value, int bitCount = 0)
		{
			int order;

			if (bitCount == 0)
			{
				order = Mathematics.Numerics.NextStrictlyLargerPowerOfTwo(value);
			}
			else
			{
				order = 1 << bitCount;
			}

			if (order <= value)
			{
				throw new ArgumentException("The submitted value cannot be stored on the given bit count.");
			}

			Vector<Complex> vector = Vector<Complex>.Build.Sparse(order);
			vector.At(value, Complex.One);

			return vector;
		}
	}
}
