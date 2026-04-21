namespace PassGen.Graphics;

public sealed class DefaultColorPalette : IColorPalette {
   private readonly Color _primary = Color.FromRgb(13, 110, 253);
   private readonly Color _secondary = Color.FromRgb(173, 181, 189);
   private readonly Color _success = Color.FromRgb(25, 135, 84);
   private readonly Color _bad = Color.FromRgb(220, 53, 69);
   private readonly Color _wrong = Color.FromRgb(255, 193, 7);
   private readonly Color _default = Color.FromRgb(150, 150, 150);
   
   public Color Primary => _primary;
   public Color Secondary => _secondary;
   public Color Success => _success;
   public Color Bad => _bad;
   public Color Wrong => _wrong;
   public Color Default => _default;
}