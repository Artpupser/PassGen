using System.Reflection;

using PassGen.Password.Models;

using PupaLib.Core;

namespace PassGen.Password;

public sealed class PasswordRouter {
   private readonly Type[] _types;
   public PasswordGenerator? Current { get; private set; }

   public PasswordRouter() {
      _types = Assembly.GetEntryAssembly()!.GetTypes()
         .Where(x => x.IsDefined(typeof(PasswordGeneratorInfoAttribute), false)).ToArray();
   }

   public Option<IEnumerable<PasswordGeneratorInfoAttribute>> GetAttributes() {
      return _types.Length == 0
         ? Option<IEnumerable<PasswordGeneratorInfoAttribute>>.Fail()
         : Option<IEnumerable<PasswordGeneratorInfoAttribute>>.Ok(_types.Select(x =>
            x.GetCustomAttribute<PasswordGeneratorInfoAttribute>())!);
   }

   public Option ChangeModel(string name) {
      return ChangeModel(() =>
         _types.FirstOrDefault(x => x.GetCustomAttribute<PasswordGeneratorInfoAttribute>(false)!.Name == name));
   }

   public Option ClearModel() {
      Current = null;
      return Option.Ok();
   }

   public Option ChangeModel(Type type) {
      return ChangeModel(() => _types.FirstOrDefault(x => x == type));
   }

   private Option ChangeModel(Func<Type?> typeCallback) {
      var model = typeCallback.Invoke();
      if (model == null) return Option.Fail();
      Current = (PasswordGenerator)Activator.CreateInstance(model)!;
      return Option.Ok();
   }
}