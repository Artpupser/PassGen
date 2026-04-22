using System.Reflection;

namespace PassGen.Password.Results;

public abstract class PasswordResult {
   public string Password { get; set; } = string.Empty;
   public int Length => Password.Length;
   public abstract Task Accept(IPasswordResultVisitor visitor, CancellationToken cancellationToken);
}

public static class PasswordResultStatic {
   public static IEnumerable<PropertyInfo> GetProps(this PasswordResult instance) {
      return instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
   }
}