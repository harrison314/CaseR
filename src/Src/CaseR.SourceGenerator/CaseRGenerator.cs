using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
                    if (this.TypeIsInteractorInterface(usedInterface))
                    {
                        useCaseDefinitions.Add(new UseCaseImplDefinitions(new ProcessableClassDefinition(symbol),
                            usedInterface.TypeArguments[0],
                            usedInterface.TypeArguments[1]));
                    }

                    if (this.TypeIsDomainEventHandlerInterface(usedInterface))
                    {
                        domainEvenets.Add(new DomainHandlerImplDefinitions(new ProcessableClassDefinition(symbol),
                            usedInterface.TypeArguments[0]));
                    }
                }
            }

            string extensionFile = CaseRExtensionsRenderer.RenderExtensionsFile(useCaseDefinitions);
            spc.AddSource("CaseR.Extensions.g.cs", SourceText.From(extensionFile, Encoding.UTF8));

            string registrationFile = CaseRRegistrationRenderer.RenderRegistrationFile(useCaseDefinitions, domainEvenets);
            spc.AddSource("CaseR.Registsrations.g.cs", SourceText.From(registrationFile, Encoding.UTF8));
        });
    }

    private bool TypeIsInteractorInterface(INamedTypeSymbol typeSymbol)
    {
        return typeSymbol.OriginalDefinition.ToDisplayString() == "CaseR.IUseCaseInteractor<TRequest, TResponse>";
    }

    private bool TypeIsDomainEventHandlerInterface(INamedTypeSymbol typeSymbol)
    {
        return typeSymbol.OriginalDefinition.ToDisplayString() == "CaseR.IDomainEventHandler<TEvent>";
    }
}
