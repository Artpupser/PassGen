namespace PassGen.Password;

[AttributeUsage(AttributeTargets.Class)]
public sealed class PasswordGeneratorInfoAttribute(string name, string? description = null) : Attribute {
   public string Name { get; private set; } = name;
   public string Description { get; private set; } = description ?? string.Empty;
}