using Microsoft.CodeAnalysis;

namespace CaseR.SourceGenerator;

internal class UseCaseImplDefinitions
{
    public ProcessableClassDefinition ClassDefinition
    {
        get;
    }

    public ITypeSymbol TRequestType
    {
        get;
    }

    public ITypeSymbol TResultType
    {
        get;
    }

    public UseCaseImplDefinitions(ProcessableClassDefinition classDefinition,
       ITypeSymbol tRequestType,
       ITypeSymbol tResultType)
    {
        this.ClassDefinition = classDefinition;
        this.TRequestType = tRequestType;
        this.TResultType = tResultType;
    }
}
