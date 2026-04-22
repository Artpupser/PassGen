namespace PassGen.UI.Palettes;

[ColorPaletteInfo("pastel")]
public record PastelColorPalette : IColorPalette {
   public Color Primary { get; } = Color.FromRgb(174, 198, 207);
   public Color Secondary { get; } = Color.FromRgb(255, 209, 220);
   public Color Success { get; } = Color.FromRgb(119, 221, 119);
   public Color Bad { get; } = Color.FromRgb(255, 105, 97);
   public Color Wrong { get; } = Color.FromRgb(253, 253, 150);
   public Color Default { get; } = Color.FromRgb(210, 210, 210);
}