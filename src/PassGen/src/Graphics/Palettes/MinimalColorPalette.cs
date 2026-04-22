namespace PassGen.Graphics.Palettes;

[ColorPaletteInfo("minimal")]
public record MinimalColorPalette : IColorPalette {
   public Color Primary { get; } = Color.FromRgb(0, 0, 0);
   public Color Secondary { get; } = Color.FromRgb(100, 100, 100);
   public Color Success { get; } = Color.FromRgb(50, 50, 50);
   public Color Bad { get; } = Color.FromRgb(150, 150, 150);
   public Color Wrong { get; } = Color.FromRgb(200, 200, 200);
   public Color Default { get; } = Color.FromRgb(230, 230, 230);
}