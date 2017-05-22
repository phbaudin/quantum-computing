# Quantum.NET (Lachesis.QuantumComputing)

## Description

A library to manipulate qubits and simulate quantum circuits.

## Usage

A qubit can be created from its probability amplitudes:

```csharp
Qubit qubit = new Qubit(Complex.One / Math.Sqrt(2), Complex.ImaginaryOne / Math.Sqrt(2)); // (|0> + i |1>) / √2
```

Or from its amplitudes' real and imaginary parts:

```csharp
Qubit qubit = new Qubit(1 / Math.Sqrt(2), 0, 0, 1 / Math.Sqrt(2)); // (|0> + i |1>) / √2
```

It can also be created from its colatitude and longitude on the [Bloch sphere](https://en.wikipedia.org/wiki/Bloch_sphere):

```csharp
Qubit qubit = new Qubit(Math.PI / 2, 0); // (|0> + |1>) / √2
```

Shortcuts are available for notable qubits:

```csharp
Qubit zero = Qubit.Zero; // |0>
Qubit one = Qubit.One; // |1>
```

A qubit is a quantum register of length 1 and can be manipulated as such. The `Qubit` class is merely a subclass of `QuantumRegister` designed for ease of use:

```csharp
QuantumRegister quantumRegister = Qubit.Zero;
```

Shortcuts are also available for notable quantum registers:

```csharp
QuantumRegister EPRPair = QuantumRegister.EPRPair; // (|00> + |11>) / √2 (Einstein–Podolsky–Rosen pair)
QuantumRegister WState = QuantumRegister.WState; // (|001> + |010> + |100>) / √3 (W state)
QuantumRegister WState4 = QuantumRegister.WStateOfLength(4); // (|0001> + |0010> + |0100> + |1000>) / 2 (generalized W state for 4 qubits)
QuantumRegister GHZState = QuantumRegister.GHZState; // (|000> + |111>) / √2 (simplest Greenberger–Horne–Zeilinger state)
QuantumRegister GHZState4 = QuantumRegister.GHZStateOfLength(4); // (|0000> + |1111>) / √2 (GHZ state for 4 qubits)
```

A quantum register can be created from other quantum registers (variadic constructor, also works with `QuantumRegister[]` and `IEnumerable<QuantumRegister>`):

```csharp
QuantumRegister quantumRegister = new QuantumRegister(Qubit.Zero, QuantumRegister.EPRPair); // (|000> + |011>) / √2
```

Or from the 2<sup>n</sup> complex probability amplitudes of each of its pure states (variadic constructor, also works with `Complex[]` and `IEnumerable<Complex>`):

```csharp
QuantumRegister quantumRegister = new QuantumRegister(0, 1 / Math.Sqrt(2), 1 / Math.Sqrt(2), 0); // (|01> + |10>) / √2
QuantumRegister error = new QuantumRegister(0, 1, 0); // the number of amplitudes is not a power of 2; throws System.ArgumentException
```

Quantum registers are mostly used to represent numbers and can therefore be created from integers (this will naturally generate pure states):

```csharp
QuantumRegister seven = new QuantumRegister(7); // |111>
QuantumRegister threeOnThreeBits = new QuantumRegister(3, 3); // |011>
```

A quantum register can be observed and collapse into a pure state (note: use your own `Random` instance to avoid issues with pseudorandom number generator determinism):

```csharp
Random random = new Random();
QuantumRegister quantumRegister = QuantumRegister.EPRPair;
quantumRegister.Collapse(random); // |00> or |11>
```

A pure state quantum register can be read to obtain the number it represents, with optional offset and length parameters to read a subsection only:

```csharp
QuantumRegister quantumRegister = new QuantumRegister(27); // |11011>
int a = quantumRegister.GetValue(); // 27 (0b11011)
int b = quantumRegister.GetValue(1); // 11 (0b1011)
int c = quantumRegister.GetValue(1, 3); // 5 (0b101)
int d = Qubit.EPRPair.GetValue(); // cannot be used on a mixed state; throws System.SystemException
```

Quantum gates are required to operate on quantum registers. Shortcuts are also available for notable quantum gates:

* `QuantumGate.IdentityGate`
* `QuantumGate.IdentityGateOfLength(int registerLength)`
* `QuantumGate.HadamardGate`
* `QuantumGate.HadamardGateOfLength(int registerLength)`
* `QuantumGate.NotGate`
* `QuantumGate.PauliYGate`
* `QuantumGate.PauliZGate`
* `QuantumGate.SquareRootNotGate`
* `QuantumGate.PhaseShiftGate(double phase)`
* `QuantumGate.SwapGate`
* `QuantumGate.SquareRootSwapGate`
* `QuantumGate.ControlledNotGate`
* `QuantumGate.ControlledGate(QuantumGate gate)`
* `QuantumGate.ToffoliGate`
* `QuantumGate.FredkinGate`
* `QuantumGate.QuantumFourierTransform(int registerLength)`

A quantum gate can be created from other quantum gates (variadic constructor, also works with `QuantumGate[]` and `IEnumerable<QuantumGate>`):

```csharp
QuantumGate quantumGate = new QuantumGate(QuantumGate.PauliZGate, QuantumGate.IdentityGate); // This gate will apply the Pauli-Z gate to the first qubit and leave the second one unchanged
```

Or from a bidimensional array of complex numbers:

```csharp
QuantumGate quantumGate = new QuantumGate(new Complex[,] {
	{ 1, 1 },
	{ 1, 0 },
});
```

Applying a quantum gate to a quantum register is as simple as using the multiplication operator on them:

```csharp
QuantumRegister quantumRegister = Qubit.Zero; // |0>
quantumRegister = QuantumGate.HadamardGate * quantumRegister; // (|0> + |1>) / √2
quantumRegister = QuantumGate.HadamardGate * quantumRegister; // |0>
quantumRegister = QuantumGate.NotGate * quantumRegister; // |1>
```

Unary gates only operate on one qubit, binary gates on two, etc.:

```csharp
QuantumRegister error = QuantumGate.PauliYGate * QuantumRegister.EPRPair; // a unary gate cannot be applied to two qubits; throws System.ArgumentException
```

Please note that this is not a literal arithmetic library. While measures have been taken to circumvent a range of errors caused by floating-point precision, the use of `QuantumRegister.AlmostEquals` might be required in places:

```csharp
QuantumRegister almostOne = new Qubit(Complex.One, Math.Cos(Math.PI / 2) * Complex.One);
QuantumRegister one = new Qubit(Complex.One, 0);
almostOne.AlmostEquals(one); // true
```	
