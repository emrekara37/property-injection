using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotnetPropertyInjection.Lib;

public static class ServiceExtensions
{
    public static IServiceCollection AddPropertyInjection(this IServiceCollection services)
    {
        var copy = services.ToList();
        foreach (var service in copy)
        {
            if (service.ImplementationType is null) continue;
            var properties = service.ImplementationType.GetProperties();
            var dict = new Dictionary<string, Type>();
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute<InjectAttribute>() is not null)
                {
                    dict.Add(property.Name, property.PropertyType);
                }
            }

            if (dict.Any())
            {
                services.Replace(ServiceDescriptor.Describe(service.ServiceType, (provider) =>
                {
                    var constructor = service.ImplementationType.GetConstructors()[0];

                    var parameters = new HashSet<object?>();
                    foreach (var constructorParameter in constructor.GetParameters())
                    {
                        parameters.Add(provider.GetService(constructorParameter.ParameterType));
                    }

                    var instance = ActivatorUtilities.CreateInstance(provider, service.ImplementationType, parameters.ToArray()!);

                    foreach (var (name, type) in dict)
                    {
                        instance.GetType().GetProperty(name)?.SetValue(instance, provider.GetService(type), null);
                    }
                    return instance;
                }, service.Lifetime));
            }
        }

        return services;
    }
}