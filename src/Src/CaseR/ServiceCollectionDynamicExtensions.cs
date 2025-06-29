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

public static class ServiceCollectionDynamicExtensions
{
    [RequiresDynamicCode("This code require dynamic assembly scanning.")]
    public static void AddCaseRInteractors(this IServiceCollection services, Assembly searchAssembly)
    {
        ArgumentNullException.ThrowIfNull(searchAssembly);

        Type useCaseInteactorType = typeof(IUseCaseInteractor<,>);
        Type eventHandlerType = typeof(IDomainEventHandler<>);

        var typesToRegister = searchAssembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
            .Where(x => x.Interface.IsGenericType &&
                        x.Interface.GetGenericTypeDefinition() == useCaseInteactorType)
            .ToList();

        foreach (var item in typesToRegister)
        {
            services.AddScoped(item.Type);
        }


        var typesToRegisterEventeHandlers = searchAssembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
            .Where(x => x.Interface.IsGenericType &&
                        x.Interface.GetGenericTypeDefinition() == eventHandlerType)
            .ToList();

        foreach (var item in typesToRegisterEventeHandlers)
        {
            services.AddScoped(item.Interface, item.Type);
        }
    }

    [RequiresDynamicCode("This code require dynamic assembly scanning.")]
    public static void AddCaseRInteractors(this IServiceCollection services, Type assemblyType)
    {
        ArgumentNullException.ThrowIfNull(assemblyType);

        services.AddCaseRInteractors(assemblyType.Assembly);
    }
}
