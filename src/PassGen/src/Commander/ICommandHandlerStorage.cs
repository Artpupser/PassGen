using System.Collections.Frozen;

using PupaLib.Core;

namespace PassGen.Commander;

public interface ICommandHandlerStorage {
   public Option<ICommandProcessor.CommanderHandlerDelegate> Get(string name);
}