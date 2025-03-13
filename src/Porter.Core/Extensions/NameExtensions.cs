using org.eclipse.jdt.core.dom;

namespace Porter.Extensions;

public static class NameExtensions
{
    public static IVariableBinding? ResolveVariableBinding(this Name name)
    {
        if (name.resolveBinding() is IVariableBinding variableBinding)
        {
            return variableBinding;
        }
        return null;
    }
}
