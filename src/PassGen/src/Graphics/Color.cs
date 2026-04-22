namespace PassGen.Graphics;

public readonly record struct Color {
   public static Color White { get; } = FromRgb(255, 255, 255);
   public byte R { get; }
   public byte G { get; }
   public byte B { get; }

   public static Color FromRgb(byte r, byte g, byte b) {
      return new Color(r, g, b);
   }

   public static Color FromV(byte v) {
      return new Color(v, v, v);
   }

   private Color(byte r, byte g, byte b) {
      R = r;
      G = g;
      B = b;
   }
}