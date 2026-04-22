namespace PassGen.UI.Palettes;

[ColorPaletteInfo("dark")]
public record DarkColorPalette : IColorPalette {
   public Color Primary { get; } = Color.FromRgb(33, 37, 41);
   public Color Secondary { get; } = Color.FromRgb(73, 80, 87);
   public Color Success { get; } = Color.FromRgb(40, 167, 69);
   public Color Bad { get; } = Color.FromRgb(220, 53, 69);
   public Color Wrong { get; } = Color.FromRgb(255, 193, 7);
   public Color Default { get; } = Color.FromRgb(120, 120, 120);
}