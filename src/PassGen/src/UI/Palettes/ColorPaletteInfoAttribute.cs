namespace PassGen.UI.Palettes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ColorPaletteInfoAttribute(string name) : Attribute {
   public string Name { get; private set; } = name;
}