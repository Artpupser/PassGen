namespace PassGen.UI.Palettes;

[ColorPaletteInfo("default")]
public record DefaultColorPalette : IColorPalette {
   public Color Primary { get; } = Color.FromRgb(13, 110, 253);
   public Color Secondary { get; } = Color.FromRgb(173, 181, 189);
   public Color Success { get; } = Color.FromRgb(25, 135, 84);
   public Color Bad { get; } = Color.FromRgb(220, 53, 69);
   public Color Wrong { get; } = Color.FromRgb(255, 193, 7);
   public Color Default { get; } = Color.FromRgb(150, 150, 150);
}