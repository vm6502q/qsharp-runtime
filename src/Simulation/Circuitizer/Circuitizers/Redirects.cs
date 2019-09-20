using Microsoft.Quantum.Simulation.Core;
using Microsoft.Quantum.Simulation.QCTraceSimulatorRuntime;
using Microsoft.Quantum.Simulation.Simulators.QCTraceSimulators.Implementation;

namespace Microsoft.Quantum.Simulation.Circuitizer
{
    internal static class CommonUtils
    {
        public static void PruneObservable(IQArray<Pauli> observable, IQArray<Qubit> qubits, out QArray<Pauli> prunnedObservable, out QArray<Qubit> prunnedQubits)
        {
            Utils.PruneObservable(observable, qubits, out var prunnedObs, out var prunnedQs);
            prunnedObservable = new QArray<Pauli>(prunnedObs);
            prunnedQubits = new QArray<Qubit>(prunnedQs);
        }

        static internal (long, long) Reduce(long numerator, long denominatorPower) =>
            QCTraceSimulatorImpl.Reduce(numerator, denominatorPower);
    }
}