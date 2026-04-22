using System.Reflection;
using System.Text.Json.Serialization;

using PassGen.Graphics.Palettes;

using PupaLib.Core;

namespace PassGen.Configuration;

public interface IUserConfiguration {
   public string Palette { get; set; }
   public bool Hidden { get; set; }
   public bool QrCodeBuffer { get; set; }
   public bool QrCodeHidden { get; set; }

   public Option<IColorPalette> GetPaletteObject(Assembly asm) {
      return IColorPalette.GetPalette(Palette, asm);
   }

   public Task Save(CancellationToken cancellationToken);
}

public static class UserConfigurationExtensions {
   public static IEnumerable<PropertyInfo> GetProps(this IUserConfiguration configuration) {
      return configuration.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
         .Where(x => x.IsDefined(typeof(JsonPropertyNameAttribute), false));
   }
}