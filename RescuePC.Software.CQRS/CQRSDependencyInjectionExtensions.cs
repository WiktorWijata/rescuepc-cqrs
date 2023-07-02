using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RescuePC.Software.CQRS.Command;
using RescuePC.Software.CQRS.Event;
using RescuePC.Software.CQRS.Query;

namespace RescuePC.Software.CQRS;
public static class CQRSDependencyInjectionExtensions
{
    public static void AddCQRS(
        this IServiceCollection serviceCollection,
        params Assembly[] handlersAssemblies)
    {
        var handlerTypes = handlersAssemblies.SelectMany(x => x.GetTypes())
            .Where(x => typeof(IHandleQuery).IsAssignableFrom(x) ||
                        typeof(IHandleCommand).IsAssignableFrom(x) ||
                        typeof(IHandleEvent).IsAssignableFrom(x));

        foreach (var handlerType in handlerTypes)
        {
            serviceCollection.RegisterComponent(handlerType, ServiceLifetime.Transient);
        }

        serviceCollection.AddTransient<IQueryBus, QueryBus>();
        serviceCollection.AddTransient<ICommandBus, CommandBus>();
        serviceCollection.AddTransient<IEventBus, EventBus>();

        serviceCollection.AddTransient<Func<Tuple<Type, Type>, IHandleQuery>>(s =>
        {
            return t =>
            {
                var (queryType, resultType) = t;
                var handlerType = typeof(IHandleQuery<,>).MakeGenericType(queryType, resultType);
                return (IHandleQuery)s.GetRequiredService(handlerType);
            };
        });

        serviceCollection.AddTransient<Func<Type, IHandleCommand>>(s =>
        {
            return t =>
            {
                var handlerType = typeof(IHandleCommand<>).MakeGenericType(t);
                return (IHandleCommand)s.GetRequiredService(handlerType);
            };
        });

        serviceCollection.AddTransient<Func<Type, IEnumerable<IHandleEvent>>>(s =>
        {
            return t =>
            {
                var handlerType = typeof(IHandleEvent<>).MakeGenericType(t);
                return (IEnumerable<IHandleEvent>)s.GetRequiredService(handlerType);
            };
        });
    }

    private static IServiceCollection RegisterComponent(this IServiceCollection services, Type type, ServiceLifetime lifetime)
    {
        services.Add(new ServiceDescriptor(type, type, ServiceLifetime.Transient));
        BindAliasesOfComponentToComponent(services, type, lifetime);
        return services;
    }

    private static void BindAliasesOfComponentToComponent(IServiceCollection serviceCollection, Type component, ServiceLifetime lifetime)
    {
        var services = GetAllServices(component).Where(t => t != component);

        foreach (var service in services)
        {
            serviceCollection.Add(new ServiceDescriptor(service, context => context.GetService(component), lifetime));
        }

        var baseType = component.BaseType;

        if (baseType == null || !baseType.IsAbstract)
        {
            return;
        }
    }

    private static IEnumerable<Type> GetAllServices(Type type)
    {
        if (type == null)
        {
            return new List<Type>();
        }

        var result = new List<Type>(type.GetInterfaces()) { type };

        foreach (var interfaceType in type.GetInterfaces())
        {
            result.AddRange(GetAllServices(interfaceType));
        }

        return result.Distinct();
    }
}
