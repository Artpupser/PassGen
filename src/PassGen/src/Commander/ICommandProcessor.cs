namespace PassGen.Commander;

public interface ICommandProcessor {
   public delegate Task CommanderHandlerDelegate(ICommand command, CancellationToken cancellationToken);
   Task Execute(ICommand command, CancellationToken cancellationToken);
}