namespace PassGen.UI.Palettes;

[ColorPaletteInfo("cyber")]
public record CyberColorPalette : IColorPalette {
   public Color Primary { get; } = Color.FromRgb(0, 255, 255);
   public Color Secondary { get; } = Color.FromRgb(255, 0, 255);
   public Color Success { get; } = Color.FromRgb(0, 255, 128);
   public Color Bad { get; } = Color.FromRgb(255, 0, 64);
   public Color Wrong { get; } = Color.FromRgb(255, 255, 0);
   public Color Default { get; } = Color.FromRgb(140, 140, 140);
}