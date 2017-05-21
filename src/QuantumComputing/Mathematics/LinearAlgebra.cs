/*
 * Quantum.NET
 * A library to manipulate qubits and simulate quantum circuits
 * Author: Pierre-Henry Baudin
 */

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
		public static Vector<Complex> CartesianProduct(Vector<Complex> vector1, Vector<Complex> vector2)
		{
			Complex[] cartesianProductArray = new Complex[vector1.Count * vector2.Count];

			for (int i = 0; i < vector1.Count; i++)
			{
				for (int j = 0; j < vector2.Count; j++)
				{
					cartesianProductArray[i * vector2.Count + j] = vector1.At(i) * vector2.At(j);
				}
			}
			
			return Vector<Complex>.Build.SparseOfArray(cartesianProductArray);
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
