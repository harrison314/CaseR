using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR;

/// <summary>
/// Represents a nothing.
/// </summary>
public readonly struct Unit : IEquatable<Unit>
{
    /// <summary>
    /// <see cref="Unit"/> value.
    /// </summary>
    public static readonly Unit Value = new Unit();

    /// <summary>
    /// <see cref="Unit"/> as <see cref="System.Threading.Tasks.Task" /> value.
    /// </summary>
    public static Task<Unit> Task
    {
        get => System.Threading.Tasks.Task.FromResult(Value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Unit"/> struct.
    /// </summary>
    public Unit()
    {
    }

    /// <inheritdoc />
    public bool Equals(Unit other)
    {
        return true;
    }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Unit;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return 458;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return "()";
    }
}
