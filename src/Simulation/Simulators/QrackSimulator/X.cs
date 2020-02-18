// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators
{
    public partial class QrackSimulator
    {
        public class QrackSimX : Intrinsic.X
        {
            [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "X")]
            private static extern void X(uint id, uint qubit);

            [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MCX")]
            private static extern void MCX(uint id, uint count, uint[] ctrls, uint qubit);

            private QrackSimulator Simulator { get; }


            public QrackSimX(QrackSimulator m) : base(m)
            {
                this.Simulator = m;
            }

            public override Func<Qubit, QVoid> Body => (q1) =>
            {
                Simulator.CheckQubit(q1);

                X(Simulator.Id, (uint)q1.Id);

                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledBody => (args) =>
            {
                var (ctrls, q1) = args;

                Simulator.CheckQubits(ctrls, q1);

                SafeControlled(ctrls,
                    () => this.Apply(q1),
                    (count, ids) => MCX(Simulator.Id, count, ids, (uint)q1.Id));

                return QVoid.Instance;
            };
        }
    }
}
