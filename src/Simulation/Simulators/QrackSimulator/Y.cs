// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators
{
    public partial class QrackSimulator
    {
        public class QrackSimY : Intrinsic.Y
        {
            [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Y")]
            private static extern void Y(uint id, uint qubit);

            [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MCY")]
            private static extern void MCY(uint id, uint count, uint[] ctrls, uint qubit);

            private QrackSimulator Simulator { get; }


            public QrackSimY(QrackSimulator m) : base(m)
            {
                this.Simulator = m;
            }

            public override Func<Qubit, QVoid> Body => (q1) =>
            {
                Simulator.CheckQubit(q1);

                Y(Simulator.Id, (uint)q1.Id);

                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledBody => (_args) =>
            {
                (IQArray<Qubit> ctrls, Qubit q1) = _args;

                Simulator.CheckQubits(ctrls, q1);

                SafeControlled(ctrls,
                    () => this.Apply(q1),
                    (count, ids) => MCY(Simulator.Id, count, ids, (uint)q1.Id));

                return QVoid.Instance;
            };            
        }
    }
}
