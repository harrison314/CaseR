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

    public DomainHandlerImplDefinitions(ProcessableClassDefinition classDefinition,
       ITypeSymbol? tDomainEvent)
    {
        this.ClassDefinition = classDefinition;
        this.TDomainEvent = tDomainEvent;
    }
}
