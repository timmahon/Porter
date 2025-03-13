using org.eclipse.jdt.core.dom;

namespace Porter.Extensions;

public static class ITypeBindingExtensions
{
    public static bool IsNonStaticNestedType(this ITypeBinding typeBinding)
    {
        ArgumentNullException.ThrowIfNull(typeBinding, nameof(typeBinding));
        if (typeBinding.isInterface())
        {
            return false;
        }
        if (!typeBinding.isNested())
        {
            return false;
        }
        return !typeBinding.IsStatic();
    }

    public static bool IsStatic(this ITypeBinding typeBinding)
    {
        ArgumentNullException.ThrowIfNull(typeBinding, nameof(typeBinding));
        return Modifier.isStatic(typeBinding.getModifiers());
    }
}
