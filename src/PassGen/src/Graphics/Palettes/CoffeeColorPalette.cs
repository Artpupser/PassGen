namespace PassGen.Graphics.Palettes;

[ColorPaletteInfo("coffee")]
public record CoffeeColorPalette : IColorPalette {
   public Color Primary { get; } = Color.FromRgb(111, 78, 55);
   public Color Secondary { get; } = Color.FromRgb(181, 101, 29);
   public Color Success { get; } = Color.FromRgb(139, 69, 19);
   public Color Bad { get; } = Color.FromRgb(92, 51, 23);
   public Color Wrong { get; } = Color.FromRgb(210, 180, 140);
   public Color Default { get; } = Color.FromRgb(170, 170, 170);
}