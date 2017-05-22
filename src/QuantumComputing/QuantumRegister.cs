/*
 * Quantum.NET
 * A library to manipulate qubits and simulate quantum circuits
 * Author: Pierre-Henry Baudin
 */

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Lachesis.QuantumComputing
{
	public class QuantumRegister
	{
		/*
		 * Vector representation of a quantum register
		 */
		public Vector<Complex> Vector { get; protected set; }

		/*
		 * Constructor from integer
		 */
		public QuantumRegister(int value, int bitCount = 0) : this(Mathematics.LinearAlgebra.VectorFromInteger(value, bitCount)) { }

		/*
		 * Constructor from other quantum registers
		 */
		public QuantumRegister(params QuantumRegister[] quantumRegisters) : this((IEnumerable<QuantumRegister>)quantumRegisters) { }

		/*
		 * Constructor from enumerable of other quantum registers
		 */
		public QuantumRegister(IEnumerable<QuantumRegister> quantumRegisters)
		{
			this.Vector = quantumRegisters.Aggregate(Vector<Complex>.Build.Sparse(1, Complex.One), (vector, quantumRegister) => Mathematics.LinearAlgebra.CartesianProduct(vector, quantumRegister.Vector));
		}

		/*
		 * Constructor from probability amplitudes
		 */
		public QuantumRegister(params Complex[] array) : this((IEnumerable<Complex>)array) { }

		/*
		 * Constructor from enumerable of probability amplitudes
		 */
		public QuantumRegister(IEnumerable<Complex> enumerable) : this(Vector<Complex>.Build.SparseOfEnumerable(enumerable)) { }

		/*
		 * Constructor from vector representation
		 */
		public QuantumRegister(Vector<Complex> vector)
		{
			if ((vector.Count & (vector.Count - 1)) != 0)
			{
				throw new ArgumentException("A quantum register can only be initialized from a vector whose dimension is a power of 2.");
			}
			
			this.Vector = vector;

			this.Normalize();
		}

		/*
		 * Normalizes a quantum register
		 */
		protected virtual void Normalize()
		{
			// Normalize magnitude
			double magnitudeFactor = Math.Sqrt(this.Vector.Aggregate(0.0, (factor, amplitude) => factor + amplitude.MagnitudeSquared()));
			if (magnitudeFactor != 1)
			{
				this.Vector = this.Vector / magnitudeFactor;
			}
		}

		/*
		 * Collapses a quantum register into a pure state
		 */
		public void Collapse(Random random)
		{
			Vector<Complex> collapsedVector = Vector<Complex>.Build.Sparse(this.Vector.Count);
			double probabilitySum = 0d;
			double probabilityThreshold = random.NextDouble();

			for (int i = 0; i < this.Vector.Count; i++)
			{
				probabilitySum += this.Vector.At(i).MagnitudeSquared();

				if (probabilitySum > probabilityThreshold)
				{
					collapsedVector.At(i, Complex.One);
					break;
				}
			}

			this.Vector = collapsedVector;
		}

		/*
		 * Returns the value contained in a quantum register, with optional portion start and length
		 */
		public int GetValue(int portionStart = 0, int portionLength = 0)
		{
			int registerLength = Mathematics.Numerics.Log2(this.Vector.Count - 1);
			
			if (portionLength == 0)
			{
				portionLength = registerLength - portionStart;
			}

			int trailingBitCount = registerLength - portionStart - portionLength;

			if (trailingBitCount < 0)
			{
				throw new ArgumentException("The supplied portion overflows the given quantum register.");
			}

			int index = -1;

			for (int i = 0; i < this.Vector.Count; i++)
			{
				if (this.Vector.At(i) == 1)
				{
					index = i;
					break;
				}
			}

			if (index == -1)
			{
				throw new SystemException("A value can only be extracted from a pure state quantum register.");
			}

			// If trailing bits need to be removed
			if (trailingBitCount > 0)
			{
				index >>= trailingBitCount;
			}

			// If leading bits need to be removed
			if (portionStart > 0)
			{
				index &= (1 << portionLength) - 1;
			}

			return index;
		}

		/*
		 * Einstein–Podolsky–Rosen pair
		 */
		public static QuantumRegister EPRPair
		{
			get
			{
				return new QuantumRegister(Vector<Complex>.Build.SparseOfArray(new Complex[] { Complex.One, Complex.Zero, Complex.Zero, Complex.One }) / Math.Sqrt(2));
			}
		}

		/*
		 * W state
		 */
		public static QuantumRegister WState
		{
			get
			{
				return QuantumRegister.WStateOfLength(3);
			}
		}

		/*
		 * Generalized W state
		 */
		public static QuantumRegister WStateOfLength(int length)
		{
			Vector<Complex> vector = Vector<Complex>.Build.Sparse(1 << length);

			for (int i = 0; i < length; i++)
			{
				vector.At(1 << i, Complex.One);
			}
			
			return new QuantumRegister(vector / Math.Sqrt(3));
		}

		/*
		 * Simplest Greenberger–Horne–Zeilinger state
		 */
		public static QuantumRegister GHZState
		{
			get
			{
				return QuantumRegister.GHZStateOfLength(3);
			}
		}

		/*
		 * Greenberger–Horne–Zeilinger state
		 */
		public static QuantumRegister GHZStateOfLength(int length)
		{
			Vector<Complex> vector = Vector<Complex>.Build.Sparse(1 << length);

			vector.At(0, Complex.One);
			vector.At((1 << length) - 1, Complex.One);

			return new QuantumRegister(vector / Math.Sqrt(2));
		}

		/*
		 * String representation of a quantum register
		 */
		public override string ToString()
		{
			string representation = "";

			for (int i = 0; i < this.Vector.Count; i++)
			{
				Complex amplitude = this.Vector.At(i);

				if (amplitude != 0)
				{
					string complexString = "";

					if (amplitude.Real < 0 || amplitude.Real == 0 && amplitude.Imaginary < 0)
					{
						complexString += " - ";
						amplitude = -amplitude;
					}
					else if (representation.Length > 0)
					{
						complexString += " + ";
					}

					if (amplitude != 1)
					{
						if (amplitude.Real != 0 && amplitude.Imaginary != 0)
						{
							complexString += "(";
						}

						if (amplitude.Real != 0)
						{
							complexString += amplitude.Real;
						}

						if (amplitude.Real != 0 && amplitude.Imaginary > 0)
						{
							complexString += " + ";
						}

						if (amplitude.Imaginary != 0)
						{
							complexString += amplitude.Imaginary + " i";
						}

						if (amplitude.Real != 0 && amplitude.Imaginary != 0)
						{
							complexString += ")";
						}

						complexString += " ";
					}

					representation += complexString + "|" + Convert.ToString(i, 2) + ">";
				}
			}

			return representation;
		}

		/*
		 * Determines whether the specified quantum register is equal to the current quantum register
		 */
		public override bool Equals(object obj)
		{
			QuantumRegister quantumRegister = obj as QuantumRegister;

			if (quantumRegister == null || this.Vector.Count != quantumRegister.Vector.Count)
			{
				return false;
			}

			return this.Vector.Equals(quantumRegister.Vector);
		}

		/*
		 * Determines whether the specified quantum register is equal to the current quantum register, ignoring floating-point precision issues
		 */
		public bool AlmostEquals(object obj)
		{
			QuantumRegister quantumRegister = obj as QuantumRegister;

			if (quantumRegister == null || this.Vector.Count != quantumRegister.Vector.Count)
			{
				return false;
			}

			return Precision.AlmostEqual<Complex>(this.Vector, quantumRegister.Vector, 15);
		}

		/*
		 * Serves as a hash function for a quantum register
		 */
		public override int GetHashCode()
		{
			return this.Vector.GetHashCode();
		}
	}
}
