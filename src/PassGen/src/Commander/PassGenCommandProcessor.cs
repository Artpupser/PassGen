namespace PassGen.Commander;

public sealed class PassGenCommandProcessor(ICommandHandlerStorage storage) : ICommandProcessor {
   public async Task Execute(ICommand command, CancellationToken cancellationToken) {
      if (storage.Get($"{command.FirstSymbol}{command.Name}").Out(out var handler))
         await handler(command, cancellationToken);
   }
}