using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace PassGen.Utils;

public static class AttributeUtils
{
    public static IEnumerable<T> GetAttrs<T>() where T : Attribute
    {
        return Assembly.GetEntryAssembly()!.GetTypes().Where(x => x.GetCustomAttribute<T>() != null)
            .Select(x => x.GetCustomAttribute<T>());
    }
    public static IEnumerable<(object, TAttr)> GetInstancesAndAttrs<TAttr>() 
        where TAttr : Attribute
    {
        return Assembly.GetEntryAssembly()!.GetTypes().Where(x => x.GetCustomAttribute<TAttr>() != null)
            .Select(x => (Activator.CreateInstance(x), x.GetCustomAttribute<TAttr>()));
    }
}