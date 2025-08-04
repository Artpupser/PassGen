using System.Security.Cryptography;
using System.Text;
using PassGen.Password.Results;

namespace PassGen.Password.Cores;

public abstract class PassCore<T> where T : PassResult
{
    public abstract T Generate();
    public abstract T Regenerate();
    protected static string Hash256(object content)
    {
        return string.Join(string.Empty, SHA256.HashData(Encoding.UTF8.GetBytes(content.ToString() ?? string.Empty)).Select(b => b.ToString("x2")));
    }
    
    protected static string HashMD5(object content)
    {
        return string.Join(string.Empty, MD5.HashData(Encoding.UTF8.GetBytes(content.ToString() ?? string.Empty)).Select(b => b.ToString("x2")));
    }
}