using System.Text.RegularExpressions;

using PupaLib.Core;

namespace PassGen.Commander;

public sealed partial class BasicCommand : ICommand {
   public string FirstSymbol { get; private set; } = string.Empty;
   public string Name { get; private set; } = string.Empty;
   public string Value { get; private set; } = string.Empty;
   public HashSet<string> Tags { get; private set; } = [];

   private static readonly Regex CommandRegex = MyRegex();
   
   public Option Load(string input) {
      if (string.IsNullOrWhiteSpace(input) || input.Length < 2)
         return Option.Fail();

      var match = CommandRegex.Match(input);
      if (!match.Success)
         return Option.Fail();

      FirstSymbol = match.Groups["first"].Value;
      Name = match.Groups["name"].Value;
      Value = match.Groups["value"].Value;      
      Tags = match.Groups["tags"].Value
         .Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x[2..]).ToHashSet();

      return Option.Ok();
   }

    [GeneratedRegex(@"^(?<first>.)(?<name>\S+)(?:\s+(?<value>(?!--)\S+))?(?<tags>(?:\s+--\S+)*)?\s*$", RegexOptions.Compiled
    )]
    private static partial Regex MyRegex();
}