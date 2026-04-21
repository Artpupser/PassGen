using System.Collections.Frozen;

using PassGen.Graphics;
using PassGen.Password;
using PassGen.Static;

using PupaLib.Core;

namespace PassGen.Commander;

public sealed class PassGenCommandHandlerStorage : ICommandHandlerStorage {
   private readonly FrozenDictionary<string, ICommandProcessor.CommanderHandlerDelegate> _handlers;

   private readonly CommandHelpData[] _helpContent = [
      new("[input] > ", "you can back/cancel operation printed \'<-\'"),
      new("/help", "see help."),
      new("/set", "set generation model."),
      new("/gen", "generate password."),
      new("/restore", "restore password.")
   ];

   private IGraphics Graphics { get; }
   private PasswordRouter PasswordRouter { get; }
   private PasswordResultRender PasswordResultRender { get; }

   private record CommandHelpData(string Name, string Description);


   [CommanderHandlerId("/help")]
   public async Task HelpHandler(ICommand command, CancellationToken cancellationToken) {
      await Graphics.RenderTable(
         ["Command", "Description"], 
         _helpContent.Select(x => new[] {x.Name, x.Description}).ToArray(), 
         [20, 120]);
   }

   [CommanderHandlerId("/set")]
   public async Task SetHandler(ICommand command, CancellationToken cancellationToken) {
      if (command.Tags.Contains("list")) {
         if (!PasswordRouter.GetAttributes().Out(out var list)) {
            return;
         }
         await Graphics.RenderTable("Name|Description".Split('|'), list.Select(x=>new string[] {x.Name, x.Description}).ToArray(),[20, 120]);
         return;
      }

      if (command.Tags.Contains("unset")) {
         PasswordRouter.ClearModel();
         await Graphics.RenderTextLine("Generator model unsetted.");
         return;
      }

      if (PasswordRouter.ChangeModel(command.Value)) {
         await Graphics.RenderText($"Sets generator model, -> {command.Value}");
         return;
      }
      await Graphics.RenderText($"Model <{command.Value}> not found");
   }

   [CommanderHandlerId("/gen")]
   public async Task GenHandler(ICommand command, CancellationToken cancellationToken) {
      var route = PasswordRouter.Current;
      if (route == null) {
         await Graphics.RenderTextLine("Route not sets, use: \'/set -list\'", Graphics.CurrentPalette.Bad);
         return;
      }
      var builder = route.GetBuilder();
      var renderOption = await builder.Render(Graphics, cancellationToken);
      if (!renderOption) {
         await Graphics.RenderTextLine("Canceled or data not correct", Graphics.CurrentPalette.Bad);
         return;
      }
      var generateOption = await route.Generate(builder.Build());
      if (generateOption.Out(out var result)) {
         await result.Accept(PasswordResultRender, cancellationToken);
      }
   }
   
   [CommanderHandlerId("/receive")]
   public async Task ReceiveHandler(ICommand command, CancellationToken cancellationToken) {
      
   }

   public PassGenCommandHandlerStorage(IGraphics graphics, PasswordRouter passwordRouter) {
      Graphics = graphics;
      PasswordRouter = passwordRouter;
      PasswordResultRender = new PasswordResultRender(graphics);
      _handlers = ICommandHandlerStorage.LoadHandlers(this);
   }

   public Option<ICommandProcessor.CommanderHandlerDelegate> Get(string id) {
      return _handlers.TryGetValue(id, out var value)
         ? Option<ICommandProcessor.CommanderHandlerDelegate>.Ok(value)
         : Option<ICommandProcessor.CommanderHandlerDelegate>.Fail();
   }
}