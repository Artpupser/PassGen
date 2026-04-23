using System.Reflection;

using PassGen.Commander;

namespace PassGen.Static;

public static class CommandHandlerStorageStatic {
   public static IEnumerable<(string, ICommandProcessor.CommanderHandlerDelegate)> GetHandlersWithName(
      this ICommandHandlerStorage obj) {
      var methods = obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
         .Where(x => x.IsDefined(typeof(CommanderHandlerInfoAttribute), false)).ToArray();
      var result = new (string, ICommandProcessor.CommanderHandlerDelegate)[methods.Length];

      for (var i = 0; i < methods.Length; i++) {
         var attr = methods[i].GetCustomAttribute<CommanderHandlerInfoAttribute>()!.Name ??
                    throw new Exception($"{nameof(CommanderHandlerInfoAttribute)} not found");
         var method =
            methods[i].CreateDelegate<ICommandProcessor.CommanderHandlerDelegate>(obj);
         result[i] = (attr, method);
      }

      return result;
   }

   public static IEnumerable<CommanderHandlerInfoAttribute> GetHandlersAttributes(this ICommandHandlerStorage obj) {
      var methods = obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
         .Where(x => x.IsDefined(typeof(CommanderHandlerInfoAttribute), false)).ToArray();
      var result = new CommanderHandlerInfoAttribute[methods.Length];

      for (var i = 0; i < methods.Length; i++) {
         var attr = methods[i].GetCustomAttribute<CommanderHandlerInfoAttribute>() ??
                    throw new Exception($"{nameof(CommanderHandlerInfoAttribute)} not found");
         result[i] = attr;
      }

      return result;
   }
}