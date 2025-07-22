using CaseR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions for registering CaseR interactors and domain event handlers dynamically.
/// </summary>
public static class ServiceCollectionDynamicExtensions
{
    /// <summary>
    /// Dynamically registers all use case interactors and domain event handlers from the specified assembly.
    /// </summary>
    /// <param name="services">IoC services.</param>
    /// <param name="searchAssembly">Assembly when CaseR search interactors and domain event handlers.</param>
    [RequiresDynamicCode("This code require dynamic assembly scanning.")]
    [RequiresUnreferencedCode("This code require dynamic assembly scanning.")]
    public static void AddCaseRInteractors(this IServiceCollection services, Assembly searchAssembly)
    {
        ArgumentNullException.ThrowIfNull(searchAssembly);

        Type useCaseInteactorType = typeof(IUseCaseInteractor<,>);
        Type eventHandlerType = typeof(IDomainEventHandler<>);

        var typesToRegister = searchAssembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
            .Where(x => x.Interface.IsGenericType
                        && x.Interface.GetGenericTypeDefinition() == useCaseInteactorType
                        && !Attribute.IsDefined(x.Type, typeof(ExcludeFromRegistrationAttribute)))
            .ToList();

        foreach (var item in typesToRegister)
        {
            //TODO: Process generic types not only continue
            if (item.Type.IsGenericTypeDefinition) continue;

            services.AddScoped(item.Type);
        }

        var typesToRegisterEventeHandlers = searchAssembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
            .Where(x => x.Interface.IsGenericType
                        && x.Interface.GetGenericTypeDefinition() == eventHandlerType
                        && !Attribute.IsDefined(x.Type, typeof(ExcludeFromRegistrationAttribute)))
            .ToList();

        foreach (var item in typesToRegisterEventeHandlers)
        {
            if (item.Type.IsGenericTypeDefinition)
            {
                if (item.Type.GetGenericArguments().Length != 1)
                {
                    continue;
                }

                services.AddScoped(typeof(IDomainEventHandler<>), item.Type);
            }
            else
            {
                services.AddScoped(item.Interface, item.Type);
            }
        }
    }

    /// <summary>
    /// Dynamically registers all use case interactors and domain event handlers from the specified assembly when contains <paramref name="assemblyType"/>.
    /// </summary>
    /// <param name="services">IoC services.</param>
    /// <param name="assemblyType">Type from assembly when CaseR search interactors and domain event handlers.</param>
    [RequiresDynamicCode("This code require dynamic assembly scanning.")]
    [RequiresUnreferencedCode("This code require dynamic assembly scanning.")]
    public static void AddCaseRInteractors(this IServiceCollection services, Type assemblyType)
    {
        ArgumentNullException.ThrowIfNull(assemblyType);

        services.AddCaseRInteractors(assemblyType.Assembly);
    }
}
