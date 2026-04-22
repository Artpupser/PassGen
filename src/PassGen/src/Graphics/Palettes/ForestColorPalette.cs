namespace PassGen.Graphics.Palettes;

[ColorPaletteInfo("forest")]
public record ForestColorPalette : IColorPalette {
   public Color Primary { get; } = Color.FromRgb(34, 139, 34);
   public Color Secondary { get; } = Color.FromRgb(107, 142, 35);
   public Color Success { get; } = Color.FromRgb(50, 205, 50);
   public Color Bad { get; } = Color.FromRgb(139, 0, 0);
   public Color Wrong { get; } = Color.FromRgb(255, 215, 0);
   public Color Default { get; } = Color.FromRgb(160, 160, 160);
}