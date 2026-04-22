using System.Reflection;

using PupaLib.Core;

namespace PassGen.Graphics.Palettes;

public interface IColorPalette {
   public Color Primary { get; }
   public Color Secondary { get; }
   public Color Success { get; }
   public Color Bad { get; }
   public Color Wrong { get; }
   public Color Default { get; }

   public static Option<IColorPalette> GetPalette(string name, Assembly assembly) {
      var types = assembly.GetTypes().Where(x => x.IsDefined(typeof(ColorPaletteInfoAttribute), false));
      var type = types.FirstOrDefault(x => x.GetCustomAttribute<ColorPaletteInfoAttribute>()!.Name == name);
      return type != null
         ? Option<IColorPalette>.Ok((IColorPalette)Activator.CreateInstance(type)!)
         : Option<IColorPalette>.Fail();
   }

   public static string[] GetPaletteNames(Assembly assembly) {
      var types = assembly.GetTypes().Where(x => x.IsDefined(typeof(ColorPaletteInfoAttribute), false));
      return types.Select(x => x.GetCustomAttribute<ColorPaletteInfoAttribute>()?.Name).ToArray()!;
   }
}