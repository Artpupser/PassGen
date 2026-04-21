namespace PassGen.Graphics;

public interface IColorPalette {
   public Color Primary { get; }
   public Color Secondary { get; }
   public Color Success { get; }
   public Color Bad { get; }
   public Color Wrong { get; }
   public Color Default { get; }
}