namespace PassGen.Password;

[AttributeUsage(AttributeTargets.Class)]
public sealed class PassCoreIdAttribute(string id) : Attribute
{
    public string Id { get; } = id;
}