﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators
{
    public partial class QrackSimulator
    {
        public class QrackSimMeasure : Intrinsic.Measure
        {
            [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Measure")]
            private static extern uint Measure(uint id, uint n, Pauli[] b, uint[] ids);

            private QrackSimulator Simulator { get; }


            public QrackSimMeasure(QrackSimulator m) : base(m)
            {
                this.Simulator = m;
            }

            public override Func<(IQArray<Pauli>, IQArray<Qubit>), Result> Body => (_args) =>
            {
                var (paulis, qubits) = _args;

                Simulator.CheckQubits(qubits);

                if (paulis.Length != qubits.Length)
                {
                    throw new InvalidOperationException($"Both input arrays for {this.GetType().Name} (paulis,qubits), must be of same size");
                }

                return Measure(Simulator.Id, (uint)paulis.Length, paulis.ToArray(), qubits.GetIds()).ToResult();
            };
        }
    }
}
