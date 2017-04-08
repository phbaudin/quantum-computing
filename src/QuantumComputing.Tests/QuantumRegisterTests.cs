using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace Lachesis.QuantumComputing.Tests
{
	[TestClass]
	public class QuantumRegisterTests
	{
		private static Random random;

		[ClassInitialize]
		public static void ClassInit(TestContext context)
		{
			QuantumRegisterTests.random = new Random();
		}

		[TestMethod]
		public void QuantumRegister_FromInteger_IsValid()
		{
			Assert.AreEqual(new QuantumRegister(15), new QuantumRegister(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1));
		}

		[TestMethod]
		public void QuantumRegister_FromIntegerWithBitCount_IsValid()
		{
			Assert.AreEqual(new QuantumRegister(7, 4), new QuantumRegister(0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void QuantumRegister_FromIntegerWithLowBitCount_ThrowsArgumentException()
		{
			QuantumRegister quantumRegister = new QuantumRegister(7, 2);
		}

		[TestMethod]
		public void QuantumRegister_FromQuantumRegisters_IsValid()
		{
			Assert.AreEqual(new QuantumRegister(Qubit.One, QuantumRegister.EPRPair), new QuantumRegister(0, 0, 0, 0, 1 / Math.Sqrt(2), 0, 0, 1 / Math.Sqrt(2)));
		}

		[TestMethod]
		public void QuantumRegister_FromVector_IsValid()
		{
			Assert.AreEqual(new QuantumRegister(QuantumRegister.EPRPair.Vector), QuantumRegister.EPRPair);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void QuantumRegister_FromVectorNotInAPowerOfTwoDimension_ThrowsArgumentException()
		{
			QuantumRegister quantumRegister = new QuantumRegister(Complex.One, Complex.Zero, Complex.One);
		}

		[TestMethod]
		public void QuantumRegister_CollapsePureState_StaysTheSame()
		{
			QuantumRegister zeroOne = new QuantumRegister(Qubit.Zero, Qubit.One);
			zeroOne.Collapse(QuantumRegisterTests.random);
			Assert.AreEqual(zeroOne, new QuantumRegister(Qubit.Zero, Qubit.One));
		}

		[TestMethod]
		public void QuantumRegister_CollapseEPRPair_Is00Or11()
		{
			QuantumRegister quantumRegister = QuantumRegister.EPRPair;
			quantumRegister.Collapse(QuantumRegisterTests.random);
			Assert.IsTrue(quantumRegister.Equals(new QuantumRegister(Qubit.Zero, Qubit.Zero)) || quantumRegister.Equals(new QuantumRegister(Qubit.One, Qubit.One)));
		}
	}
}
