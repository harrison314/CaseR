using Microsoft.CodeAnalysis;

namespace CaseR.SourceGenerator;

internal class DomainHandlerImplDefinitions
{
    public ProcessableClassDefinition ClassDefinition
    {
        get;
    }

    public ITypeSymbol? TDomainEvent
    {
        get;
    }

    public string? CathegoryName
    {
        get;
    }

    public DomainHandlerImplDefinitions(ProcessableClassDefinition classDefinition,
       ITypeSymbol? tDomainEvent,
       string? cathegoryName)
    {
        this.ClassDefinition = classDefinition;
        this.TDomainEvent = tDomainEvent;
        this.CathegoryName = cathegoryName;
    }
}
