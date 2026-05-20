using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CaseR.SourceGenerator;

[Generator]
public class CaseRGenerator : IIncrementalGenerator
{
    public CaseRGenerator()
    {

    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (s, _) => s is ClassDeclarationSyntax { },
                transform: (ctx, _) => (ClassDeclarationSyntax)ctx.Node)
            .Where(cls => cls != null);

        IncrementalValueProvider<(Compilation Left, ImmutableArray<ClassDeclarationSyntax> Right)> compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses, (spc, source) =>
        {
            (Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes) = source;

            List<UseCaseImplDefinitions> useCaseDefinitions = new List<UseCaseImplDefinitions>();
            List<DomainHandlerImplDefinitions> domainEvenets = new List<DomainHandlerImplDefinitions>();

            foreach (ClassDeclarationSyntax? classSyntax in classes)
            {
                SemanticModel model = compilation.GetSemanticModel(classSyntax.SyntaxTree);
                INamedTypeSymbol? symbol = model.GetDeclaredSymbol(classSyntax);
                if (symbol == null) continue;

                //TODO: report as diagnostic error
                if (symbol is INamedTypeSymbol namedType && namedType.IsAbstract)
                {
                    continue;
                }

                foreach (INamedTypeSymbol usedInterface in symbol.AllInterfaces)
                {
                    UseCaseInteractorType interactorType = this.GetInteractorInterface(usedInterface);
                    if (interactorType != UseCaseInteractorType.None)
                    {
                        if (symbol.IsGenericType)
                        {
                            spc.ReportDiagnostic(
                                Diagnostic.Create(
                                    new DiagnosticDescriptor("CaseR001", "Invalid Use Case Interactor",
                                        "Use Case Interactor must not be generic, but found: {0}",
                                        "CaseR", DiagnosticSeverity.Error, true),
                                    Location.Create(classSyntax.SyntaxTree, classSyntax.Span),
                                    symbol.ToDisplayString()));
                        }
                        else
                        {
                            if (this.TypeIsExcludeFromRegistration(symbol))
                            {
                                continue;
                            }

                            foreach (string? cathegoryName in this.GetInteractorCathegoryNames(symbol))
                            {
                                useCaseDefinitions.Add(new UseCaseImplDefinitions(new ProcessableClassDefinition(symbol),
                                    interactorType,
                                    usedInterface.TypeArguments[0],
                                    usedInterface.TypeArguments[1],
                                    cathegoryName));
                            }
                        }
                    }

                    if (this.TypeIsDomainEventHandlerInterface(usedInterface))
                    {
                        if (this.TypeIsExcludeFromRegistration(symbol))
                        {
                            continue;
                        }

                        if (symbol.IsGenericType)
                        {
                            foreach (string? cathegoryName in this.GetInteractorCathegoryNames(symbol))
                            {
                                domainEvenets.Add(new DomainHandlerImplDefinitions(new ProcessableClassDefinition(symbol),
                                null,
                                cathegoryName));
                            }
                        }
                        else
                        {
                            foreach (string? cathegoryName in this.GetInteractorCathegoryNames(symbol))
                            {
                                domainEvenets.Add(new DomainHandlerImplDefinitions(new ProcessableClassDefinition(symbol),
                                usedInterface.TypeArguments[0],
                                cathegoryName));
                            }
                        }
                    }
                }
            }

            string extensionFile = CaseRExtensionsRenderer.RenderExtensionsFile(useCaseDefinitions);
            spc.AddSource("CaseR.Extensions.g.cs", SourceText.From(extensionFile, Encoding.UTF8));

            string registrationFile = CaseRRegistrationRenderer.RenderRegistrationFile(useCaseDefinitions, domainEvenets);
            spc.AddSource("CaseR.Registsrations.g.cs", SourceText.From(registrationFile, Encoding.UTF8));
        });
    }

    private UseCaseInteractorType GetInteractorInterface(INamedTypeSymbol typeSymbol)
    {
        UseCaseInteractorType type = UseCaseInteractorType.None;
        if (typeSymbol.OriginalDefinition.ToDisplayString() == "CaseR.IUseCaseInteractor<TRequest, TResponse>")
        {
            type |= UseCaseInteractorType.Standard;
        }

        if (typeSymbol.OriginalDefinition.ToDisplayString() == "CaseR.IUseCaseStreamInteractor<TRequest, TResponse>")
        {
            type |= UseCaseInteractorType.Streaming;
        }

        return type;
    }

    private bool TypeIsDomainEventHandlerInterface(INamedTypeSymbol typeSymbol)
    {
        return typeSymbol.OriginalDefinition.ToDisplayString() == "CaseR.IDomainEventHandler<TEvent>";
    }

    private bool TypeIsExcludeFromRegistration(INamedTypeSymbol typeSymbol)
    {
        ImmutableArray<AttributeData> attributeList = typeSymbol.GetAttributes();
        return attributeList.Any(t => t.AttributeClass?.ToDisplayString() == "CaseR.ExcludeFromRegistrationAttribute");
    }

    private IEnumerable<string?> GetInteractorCathegoryNames(INamedTypeSymbol typeSymbol)
    {
        ImmutableArray<AttributeData> attributeList = typeSymbol.GetAttributes();
        bool containsAny = false;
        foreach (AttributeData cathegoryAttribute in attributeList.Where(t => t.AttributeClass?.ToDisplayString() == "CaseR.RegistrationCathegoryAttribute"))
        {
            if (cathegoryAttribute != null && cathegoryAttribute.ConstructorArguments.Length > 0)
            {
                TypedConstant cathegoryNameConstant = cathegoryAttribute.ConstructorArguments[0];
                if (cathegoryNameConstant.Value is string cathegoryName)
                {
                    containsAny = true;
                    yield return cathegoryName;
                }
            }
        }

        if (!containsAny)
        {
            yield return null;
        }
    }
}
