namespace PassGen.Commander;

public sealed class CommanderHandlerIdAttribute(string id) : Attribute {
   public string Id { get; private set; } = id;
}