using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace Lachesis.QuantumComputing.Tests
{
	[TestClass]
	public class QubitTests
	{
		[TestMethod]
		public void Qubit_Zero_IsZero()
		{
			Assert.AreEqual(Qubit.Zero.Vector, Vector<Complex>.Build.SparseOfArray(new Complex[] { Complex.One, Complex.Zero }));
		}

		[TestMethod]
		public void Qubit_One_IsOne()
		{
			Assert.AreEqual(Qubit.One.Vector, Vector<Complex>.Build.SparseOfArray(new Complex[] { Complex.Zero, Complex.One }));
		}

		[TestMethod]
		public void Qubit_FromComplex_IsValid()
		{
			Assert.AreEqual((new Qubit(new Complex(1, 0), new Complex(0, 0))), Qubit.Zero);
		}

		[TestMethod]
		public void Qubit_FromDouble_IsValid()
		{
			Assert.AreEqual((new Qubit(0, 0, 1, 0)), Qubit.One);
		}

		[TestMethod]
		public void Qubit_FromBlochCoordinates_IsValid()
		{
			Assert.IsTrue((new Qubit(Math.PI, 0)).AlmostEquals(Qubit.One));
		}
	}
}