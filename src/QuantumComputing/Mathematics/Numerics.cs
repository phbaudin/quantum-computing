/*
 * Quantum computing
 * A library to manipulate qubits and simulate quantum circuits
 * Author: Pierre-Henry Baudin
 */

using System;
using System.Numerics;

namespace Lachesis.QuantumComputing.Mathematics
{
	public static class Numerics
	{
		/*
		 * Complex exponential
		 */
		public static Complex ComplexExp(Complex value)
		{
			if (value.Equals(Complex.ImaginaryOne * Math.PI / 2))
			{
				return Complex.ImaginaryOne;
			}
			else if (value.Equals(Complex.ImaginaryOne * Math.PI))
			{
				return -Complex.One;
			}
			else if (value.Equals(Complex.ImaginaryOne * 3 * Math.PI / 2))
			{
				return -Complex.ImaginaryOne;
			}
			else {
				return Complex.Exp(value);
			}
		}

		/*
		 * Next strictly larger power of two
		 */
		public static int NextStrictlyLargerPowerOfTwo(int value)
		{
			value |= (value >> 1);
			value |= (value >> 2);
			value |= (value >> 4);
			value |= (value >> 8);
			value |= (value >> 16);

			return value + 1;
		}
	}
}
