namespace PassGen.Commander;

[AttributeUsage(AttributeTargets.Method)]
public sealed class CommanderHandlerInfoAttribute(string name, string description) : Attribute {
   public string Name { get; private set; } = name;
   public string Description { get; private set; } = description;
}