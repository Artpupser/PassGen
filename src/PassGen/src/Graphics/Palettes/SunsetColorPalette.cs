namespace PassGen.Graphics.Palettes;

[ColorPaletteInfo("sunset")]
public record SunsetColorPalette : IColorPalette {
   public Color Primary { get; } = Color.FromRgb(255, 94, 77);
   public Color Secondary { get; } = Color.FromRgb(255, 149, 0);
   public Color Success { get; } = Color.FromRgb(76, 217, 100);
   public Color Bad { get; } = Color.FromRgb(255, 59, 48);
   public Color Wrong { get; } = Color.FromRgb(255, 204, 0);
   public Color Default { get; } = Color.FromRgb(180, 180, 180);
}