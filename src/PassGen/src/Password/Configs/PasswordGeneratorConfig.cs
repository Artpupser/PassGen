using PupaLib.Core;

namespace PassGen.Password.Configs;

public sealed class PasswordGeneratorConfig {
   private readonly Dictionary<string, object> _dictionary = new();

   public Option Set(string id, object value) {
      return _dictionary.TryAdd(id, value) ? Option.Ok() : Option.Fail();
   }

   public Option<object> Get(string id) {
      _dictionary.TryGetValue(id, out var value);
      return value == null ? Option<object>.Fail() : Option<object>.Ok(value);
   }
}