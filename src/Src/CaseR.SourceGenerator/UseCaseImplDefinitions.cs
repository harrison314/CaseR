using Microsoft.CodeAnalysis;

namespace CaseR.SourceGenerator;

internal class UseCaseImplDefinitions
{
    public ProcessableClassDefinition ClassDefinition
    {
        get;
    }

    public UseCaseInteractorType UseCaseInteractorType 
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

    public string? CathegoryName
    {
        get;
    }

    public UseCaseImplDefinitions(ProcessableClassDefinition classDefinition,
        UseCaseInteractorType useCaseInteractorType,
       ITypeSymbol tRequestType,
       ITypeSymbol tResultType,
       string? cathegoryName)
    {
        this.ClassDefinition = classDefinition;
        this.UseCaseInteractorType = useCaseInteractorType;
        this.TRequestType = tRequestType;
        this.TResultType = tResultType;
        this.CathegoryName = cathegoryName;
    }
}
