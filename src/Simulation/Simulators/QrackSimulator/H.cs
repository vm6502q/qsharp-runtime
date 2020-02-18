// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators
{
    public partial class QrackSimulator
    {
        public class QrackSimH : Intrinsic.H
        {
            [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "H")]
            private static extern void H(uint id, uint qubit);

            [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MCH")]
            private static extern void MCH(uint id, uint count, uint[] ctrls, uint qubit);

            private QrackSimulator Simulator { get; }


            public QrackSimH(QrackSimulator m) : base(m)
            {
                this.Simulator = m;
            }

            public override Func<Qubit, QVoid> Body => (q1) =>
            {
                Simulator.CheckQubit(q1);

                H(Simulator.Id, (uint)q1.Id);

                return QVoid.Instance;
            };


            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledBody => (args) =>
            {
                var (ctrls, q1) = args;

                Simulator.CheckQubits(ctrls, q1);

                SafeControlled(ctrls,
                    () => this.Apply(q1),
                    (count, ids) => MCH(Simulator.Id, count, ids, (uint)q1.Id));

                return QVoid.Instance;
            };
        }
    }
}
