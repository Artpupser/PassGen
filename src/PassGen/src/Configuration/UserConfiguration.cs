using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

using PupaLib.FileIO;
using PupaLib.FileIO.Serializers;

namespace PassGen.Configuration;

internal sealed class UserConfiguration(VirtualFile file) : IUserConfiguration {
   [JsonPropertyName("palette")] public string Palette { get; set; } = string.Empty;
   [JsonPropertyName("hidden")] public bool Hidden { get; set; }
   [JsonPropertyName("qrcode_buffer")] public bool QrCodeBuffer { get; set; }
   [JsonPropertyName("qrcode_hidden")] public bool QrCodeHidden { get; set; }

   public async Task Load(CancellationToken cancellationToken) {
      Dictionary<string, object> objects;
      var loaded = false;
      try {
         objects = await file.ReadTContentAsync<Dictionary<string, object>>(new JsonSystemSerializer(),
            cancellationToken);
         loaded = true;
      } catch (Exception) {
         objects = Default();
      }

      foreach (var prop in this.GetProps()) {
         var jsonName = prop.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name;
         prop.SetValue(this, loaded ? Convert.ChangeType(((JsonElement)objects[jsonName]).ToString(), prop.PropertyType) : objects[jsonName]);
      }
      
   }

   private static Dictionary<string, object> Default() {
      var dict = new Dictionary<string, object> {
         ["palette"] = "default",
         ["hidden"] = false,
         ["qrcode_buffer"] = false,
         ["qrcode_hidden"] = false,
      };
      return dict;
   }

   public Task Save(CancellationToken cancellationToken) {
      var dict = this.GetProps().Select(x => (x.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name, x.GetValue(this))).ToDictionary();
      return file.WriteTContentAsync(dict, new JsonSystemSerializer(), cancellationToken);
   }
}