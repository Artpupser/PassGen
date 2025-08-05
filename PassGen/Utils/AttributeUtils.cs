using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using PassGen.Password.Cores;
using PassGen.Password.Results;

namespace PassGen.Utils;

public static class Tools
{
    public static async Task TryCatch(Func<Task> task)
    {
        try
        {
            await task();
        }
        catch (Exception e)
        {
            ProjectContext.Terminal.OutL(e);
            throw;
        }
    }

    public static string GetBase64FromStr(string str)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
    }

    public static string GenerateHash(int length, string key)
    {
        var hash = new StringBuilder();
        for (var i = 0; i < length + 1 % Hash256('*').Length; i++)
            hash.Append(Hash256(key));
        return hash.ToString()[..length];
    }
    public static string Hash256(object content)
    {
        return string.Join(string.Empty, SHA256.HashData(Encoding.UTF8.GetBytes(content.ToString() ?? string.Empty)).Select(b => b.ToString("x2")));
    }
    
    public static string HashMD5(object content)
    {
        return string.Join(string.Empty, MD5.HashData(Encoding.UTF8.GetBytes(content.ToString() ?? string.Empty)).Select(b => b.ToString("x2")));
    }
}
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