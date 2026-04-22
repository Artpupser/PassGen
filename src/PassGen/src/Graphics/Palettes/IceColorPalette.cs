namespace PassGen.Graphics.Palettes;

[ColorPaletteInfo("ice")]
public record IceColorPalette : IColorPalette {
   public Color Primary { get; } = Color.FromRgb(173, 216, 230);
   public Color Secondary { get; } = Color.FromRgb(224, 255, 255);
   public Color Success { get; } = Color.FromRgb(0, 191, 255);
   public Color Bad { get; } = Color.FromRgb(70, 130, 180);
   public Color Wrong { get; } = Color.FromRgb(135, 206, 250);
   public Color Default { get; } = Color.FromRgb(220, 220, 220);
}