using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaseR.SourceGenerator;

internal class ProcessableClassDefinition
{
    public INamedTypeSymbol Symbol
    {
        get;
    }

    public string Namespace
    {
        get => this.Symbol.ContainingNamespace.ToString();
    }

    public string Name
    {
        get => this.Symbol.Name;
    }

    public ProcessableClassDefinition(INamedTypeSymbol symbol)
    {
        this.Symbol = symbol;
    }
}
