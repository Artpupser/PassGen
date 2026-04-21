using PassGen.Commander;

namespace PassGen.Static;

public static class CommanderCommandStatic {
   public static string GetInfo(this ICommand command) {
      return $"<{command.FirstSymbol}> [{command.Name}] ({string.Join(", ", command.Tags)}) \"{command.Value}\"";
   }
}