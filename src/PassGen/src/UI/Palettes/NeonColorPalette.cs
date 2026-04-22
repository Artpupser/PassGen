namespace PassGen.UI.Palettes;

[ColorPaletteInfo("neon")]
public record NeonColorPalette : IColorPalette {
   public Color Primary { get; } = Color.FromRgb(57, 255, 20);
   public Color Secondary { get; } = Color.FromRgb(255, 20, 147);
   public Color Success { get; } = Color.FromRgb(0, 255, 255);
   public Color Bad { get; } = Color.FromRgb(255, 0, 0);
   public Color Wrong { get; } = Color.FromRgb(255, 255, 0);
   public Color Default { get; } = Color.FromRgb(180, 180, 180);
}