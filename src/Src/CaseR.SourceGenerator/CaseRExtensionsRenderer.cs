﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaseR.SourceGenerator
{
    internal class CaseRExtensionsRenderer
    {
        public static string RenderExtensionsFile(List<UseCaseImplDefinitions> useCaseDefinitions)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// <auto-generated>");
            sb.AppendLine("// Auto-generated list of classes implementing CaseR.IUseCaseHandler<TRequest, TResponse>");
            sb.AppendLine("// </auto-generated>");
            sb.AppendLine();


            foreach (IGrouping<string, UseCaseImplDefinitions>? nsGroup in useCaseDefinitions.GroupBy(t => t.ClassDefinition.Namespace))
            {
                sb.AppendLine($"namespace {nsGroup.Key}");
                sb.AppendLine("{");
                sb.AppendLine();

                RenderClasses(nsGroup, sb);

                sb.AppendLine("}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static void RenderClasses(IEnumerable<UseCaseImplDefinitions> classesDefinitions, StringBuilder sb)
        {
            foreach (IGrouping<string, UseCaseImplDefinitions>? classGroup in classesDefinitions.GroupBy(t => t.ClassDefinition.Name))
            {
                sb.AppendLine("    /// <summary>");
                sb.AppendLine($"    /// Use case extensions for {classGroup.Key.Replace("<", "&lt;").Replace(">", "&gt;")} interactor.");
                sb.AppendLine("    /// </summary>");
                sb.AppendLine($"    {GetClassModifier(classGroup.First().ClassDefinition.Symbol)} static partial class {classGroup.Key}CaseRExtensions");
                sb.AppendLine("    {");

                foreach (IGrouping<string, UseCaseImplDefinitions>? methodGroup in classGroup.GroupBy(t => t.TRequestType.ToString()))
                {
                    int methodIndex = 0;
                    foreach (UseCaseImplDefinitions? extensionMethodDef in methodGroup)
                    {
                        RenderMethod(extensionMethodDef, methodIndex, sb);
                        methodIndex++;
                    }
                }

                sb.AppendLine("    }");
                sb.AppendLine();
            }
        }

        private static void RenderMethod(UseCaseImplDefinitions extensionMethodDef, int methodIndex, StringBuilder sb)
        {
            string methodName = methodIndex switch
            {
                0 => "Execute",
                _ => $"Execute{methodIndex + 1}"
            };

            sb.Append($$$"""
                     /// <summary>
                     /// Execute the use case interactor <see cref="{{{GetFullTypeName(extensionMethodDef.ClassDefinition.Symbol)}}}"/>.
                     /// </summary>
                     /// <param name="useCase">The use case.</param>
                     /// <param name="request">The use case interactor request.</param>
                     /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
                     /// <returns>Returns value task with result.</returns>
                     [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                     public static System.Threading.Tasks.Task<{{{GetFullTypeName(extensionMethodDef.TResultType)}}}> {{{methodName}}}(this global::CaseR.IUseCase<{{{GetFullTypeName(extensionMethodDef.ClassDefinition.Symbol)}}}> useCase, {{{GetFullTypeName(extensionMethodDef.TRequestType)}}} request, System.Threading.CancellationToken cancellationToken = default)
                     {
                        return global::CaseR.UseCaseExtensions.Execute<{{{GetFullTypeName(extensionMethodDef.ClassDefinition.Symbol)}}}, {{{GetFullTypeName(extensionMethodDef.TRequestType)}}}, {{{GetFullTypeName(extensionMethodDef.TResultType)}}}>(useCase, request, cancellationToken);
                     }

             """);
        }

        private static string GetFullTypeName(ITypeSymbol typeSymbol)
        {
            return typeSymbol.ToString();
        }

        private static string GetFullTypeName(INamedTypeSymbol symbol)
        {
            return symbol.ToString();
        }

        private static string GetClassModifier(INamedTypeSymbol symbol)
        {
            return symbol.DeclaredAccessibility switch
            {
                Accessibility.Public => "public",
                Accessibility.Internal => "internal",
                Accessibility.Protected => "protected",
                Accessibility.Private => "private",
                Accessibility.ProtectedAndInternal => "internal",
                Accessibility.ProtectedOrInternal => "internal",
                _ => ""
            };
        }
    }
}
