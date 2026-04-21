using PupaLib.Core;

namespace PassGen.Commander;

public interface ICommand {
   public string FirstSymbol { get; }
   public string Name { get; }
   public string Value { get; }
   public HashSet<string> Tags { get; }

   public Option Load(string input);
   
}
