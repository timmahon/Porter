using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using org.eclipse.jdt.core.dom;

namespace Porter.Visitors;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddNodeVisitor<TKey, TImplementation>(this IServiceCollection services) where TKey : ASTNode
    {
        services.TryAddKeyedScoped(typeof(NodeVisitor), typeof(TKey), typeof(TImplementation));
        return services;
    }
}
