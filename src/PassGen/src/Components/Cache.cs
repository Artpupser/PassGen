using PupaLib.Core;

namespace PassGen.Components;

public sealed class Cache<T> {
   private readonly Dictionary<string, T> _dict = new();
   public bool Preloaded { get; private set; }

   public int Count => _dict.Count;

   public T this[string index] => _dict[index];

   public Option<T> TryGet(string name) {
      return _dict.TryGetValue(name, out var value) ? Option<T>.Ok(value) : Option<T>.Fail();
   }

   public async Task<T> AddOrLoad(string name, Func<Task<T>> loadCallback) {
      if (_dict.TryGetValue(name, out var load)) {
         return load;
      }
      _dict.Add(name, await loadCallback());
      return _dict[name];
   }

   public async Task Preload(Func<Task<IEnumerable<(string, T)>>> loadCallback) {
      var list = await loadCallback();
      foreach (var item in list) {
         _dict.Add(item.Item1, item.Item2);
      }

      Preloaded = true;
   }
}