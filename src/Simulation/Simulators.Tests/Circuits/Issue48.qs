// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Issue48
{    
    open Microsoft.Quantum.Intrinsic;
    open Microsoft.Quantum.Simulation.Simulators.Tests.Circuits;

    operation IterateThroughCartesianProduct (bounds : Int[], op : (Int[] => Unit)) : Unit
    {
        mutable arr = new Int[Length(bounds)];
        mutable finished = false;
        
        repeat
        {
            if (not finished)
            {
                op(arr);
            }
        }
        until (finished)
        fixup
        {
            //computes the next element in the Cartesian product
            set arr w/= 0 <- arr[0] + 1;
            
            for (i in 0 .. Length(arr) - 2)
            {
                if (arr[i] == bounds[i])
                {
                    set arr w/= i + 1 <- arr[i + 1] + 1;
                    set arr w/= i <- 0;
                }
            }
            
            if (arr[Length(arr) - 1] == bounds[Length(arr) - 1])
            {
                set finished = true;
            }
        }
    }
    
    operation IterateThroughCartesianPower (power : Int, bound : Int, op : (Int[] => Unit)) : Unit {
        IterateThroughCartesianProduct(ConstantArray(power, bound), op);
    }
    

    operation PrintPaulies( paulis : Pauli[] ) : Unit { Message($"{paulis}"); }

    operation ApplyComposed<'U,'V>( op : ('U => Unit), fn : ('V -> 'U), arg : 'V) : Unit {
        op(fn(arg));
    }

    function ArrayFromIndices<'T>( values : 'T[], indices : Int[] ) : 'T[] {
        mutable arr = new 'T[Length(indices)];
        for( i in 0 .. Length(indices) - 1) {
            set arr w/= i <- values[indices[i]];
        }
        return arr;
    }

    operation IterateThroughCartesianPowerT<'T> (power : Int, values : 'T[], op : ('T[] => Unit)) : Unit {
        let opInt = ApplyComposed(op, ArrayFromIndices(values,_),_);
        IterateThroughCartesianPower(power, Length(values), opInt);
    }

    operation PrintAllPaulis() : Unit {
        IterateThroughCartesianPowerT(3, [PauliI,PauliX,PauliY,PauliZ], PrintPaulies);
    }
}

namespace Microsoft.Quantum.Simulation.Simulators.Tests.Circuits
{
    open Issue48;
    open Microsoft.Quantum.Intrinsic;

    operation PrintAllPaulisTest () : Unit {
        IterateThroughCartesianPowerT(3, [PauliI,PauliX,PauliY,PauliZ], PrintPaulies);
    }
}
