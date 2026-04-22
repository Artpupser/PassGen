namespace PassGen.Graphics.Palettes;

[ColorPaletteInfo("ocean")]
public record OceanColorPalette : IColorPalette {
   public Color Primary { get; } = Color.FromRgb(0, 123, 167);
   public Color Secondary { get; } = Color.FromRgb(108, 117, 125);
   public Color Success { get; } = Color.FromRgb(40, 167, 69);
   public Color Bad { get; } = Color.FromRgb(220, 53, 69);
   public Color Wrong { get; } = Color.FromRgb(255, 193, 7);
   public Color Default { get; } = Color.FromRgb(200, 200, 200);
}