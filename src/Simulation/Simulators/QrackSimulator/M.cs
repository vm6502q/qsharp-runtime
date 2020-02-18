// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Quantum.Simulation.Core;

namespace Microsoft.Quantum.Simulation.Simulators
{
    public partial class QrackSimulator
    {
        public class QrackSimM : Quantum.Intrinsic.M
        {
            [DllImport(QRACKSIM_DLL_NAME, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "M")]
            private static extern uint M(uint id, uint q);

            private QrackSimulator Simulator { get; }


            public QrackSimM(QrackSimulator m) : base(m)
            {
                this.Simulator = m;
            }

            public override Func<Qubit, Result> Body => (q) =>
            {
                Simulator.CheckQubit(q);

                return M(Simulator.Id, (uint)q.Id).ToResult();
            };
        }
    }
}
