using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR;

public readonly struct Unit : IEquatable<Unit>
{
    public static readonly Unit Value = new Unit();

    public static ValueTask<Unit> ValueTask
    {
        get => new ValueTask<Unit>(Value);
    }

    public Unit()
    {
    }

    public bool Equals(Unit other)
    {
        return true;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Unit;
    }

    public override int GetHashCode()
    {
        return 458;
    }

    public override string ToString()
    {
        return "()";
    }
}
