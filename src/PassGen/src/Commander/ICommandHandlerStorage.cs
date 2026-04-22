using System.Collections.Frozen;
using System.Reflection;

using PupaLib.Core;

namespace PassGen.Commander;

public interface ICommandHandlerStorage {
   public Option<ICommandProcessor.CommanderHandlerDelegate> Get(string id);

   protected static FrozenDictionary<string, ICommandProcessor.CommanderHandlerDelegate> LoadHandlers(object obj) {
      var methods = obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
         .Where(x => x.IsDefined(typeof(CommanderHandlerIdAttribute), false));
      var dict = new Dictionary<string, ICommandProcessor.CommanderHandlerDelegate>();

      foreach (var info in methods) {
         var id = info.GetCustomAttribute<CommanderHandlerIdAttribute>()?.Id ??
                  throw new Exception($"{nameof(CommanderHandlerIdAttribute)} not found");
         var method =
            (ICommandProcessor.CommanderHandlerDelegate)info.CreateDelegate(
               typeof(ICommandProcessor.CommanderHandlerDelegate), obj);
         dict.Add(id, method);
      }

      return dict.ToFrozenDictionary();
   }
}