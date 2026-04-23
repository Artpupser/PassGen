using System.Collections.Frozen;
using System.Reflection;
using System.Text.Json.Serialization;

using PassGen.Configuration;
using PassGen.UI;
using PassGen.UI.Palettes;
using PassGen.Password;
using PassGen.Services;
using PassGen.UI.Widgets;

using PupaLib.Core;

namespace PassGen.Commander;

public sealed class PassGenCommandHandlerStorage : ICommandHandlerStorage {
   private readonly FrozenDictionary<string, ICommandProcessor.CommanderHandlerDelegate> _handlers;

   private readonly CommandHelpData[] _helpContent = [
      new("[input] > ", "you can back/cancel operation printed \'<-\'"),
      new("/help", "see help."),
      new("/color", "sets color scheme."),
      new("/model", "set generation model."),
      new("/gen", "generate password."),
      new("/restore", "restore password.")
   ];

   private Graphics Graphics { get; }
   private PasswordRouter PasswordRouter { get; }
   private IUserConfiguration Configuration { get; }
   private PasswordResultRender PasswordResultRender { get; }
   private ConsoleInputService InputService { get; }
   private PasswordConfigBuilderRender PasswordConfigBuilderRender { get; }
   private static Assembly Asm => Assembly.GetEntryAssembly()!;

   private record CommandHelpData(string Name, string Description);


   public PassGenCommandHandlerStorage(IUserConfiguration userConfiguration, Graphics graphics,
      PasswordRouter passwordRouter, ConsoleInputService inputService) {
      Graphics = graphics;
      Configuration = userConfiguration;
      PasswordRouter = passwordRouter;
      PasswordResultRender = new PasswordResultRender(graphics, userConfiguration);
      PasswordConfigBuilderRender = new PasswordConfigBuilderRender(graphics, inputService);
      InputService = inputService;
      _handlers = ICommandHandlerStorage.LoadHandlers(this);
   }

   [CommanderHandlerId("/help")]
   public async Task HelpHandler(ICommand command, CancellationToken cancellationToken) {
      var table = new TableWidget(["Command", "Description"],
         _helpContent.Select(x => new[] { x.Name, x.Description }).ToArray(), [20, 120]);
      await table.Render(Graphics, cancellationToken);
   }

   [CommanderHandlerId("/config")]
   public async Task ConfigHandler(ICommand command, CancellationToken cancellationToken) {
      if (command.Tags.Contains("set")) {
         while (!cancellationToken.IsCancellationRequested) {
            var settingCheckoutWidget = CheckoutWidget<string>.ConsoleInputCheckoutWidget("Select setting",
               Configuration.GetProps().Select(x => x.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name).ToArray(),
               InputService);
            await settingCheckoutWidget.Render(Graphics, cancellationToken);
            var dict = new Dictionary<string, Func<CancellationToken, Task>> {
               ["palette"] = ChangePalette,
               ["hidden"] = ChangeHidden,
               ["qrcode_buffer"] = ChangeQrCodeBuffer,
               ["qrcode_hidden"] = ChangeQrCodeHidden
            };

            if (!settingCheckoutWidget.Result.Out(out var settingCheckoutWidgetResult)) break;
            await dict[settingCheckoutWidgetResult.Value](cancellationToken);
            await Configuration.Save(cancellationToken);
         }

         await Graphics.RenderTextLine("Exited from configuration panel", Graphics.Wrong);
         return;

         async Task ChangeBoolean(string prompt, string cancelMessage, PropertyInfo prop, object instance,
            CancellationToken cancellationToken2) {
            var checkoutWidget = CheckoutWidget<string>.ConsoleInputCheckoutWidget(prompt, ["yes", "no"], InputService);
            await checkoutWidget.Render(Graphics, cancellationToken2);
            if (checkoutWidget.Result.Out(out var tuple)) {
               prop.SetValue(tuple.Index == 0, instance);
               return;
            }
            await Graphics.RenderTextLine(cancelMessage, Graphics.Bad);
         }

         async Task ChangeQrCodeBuffer(CancellationToken cancellationToken2) {
            await ChangeBoolean("Save qrcode in buffer?", "qrcode_buffer not sets.", Configuration.GetType().GetProperty(nameof(Configuration.QrCodeBuffer))!, Configuration, cancellationToken2);
         }

         async Task ChangeQrCodeHidden(CancellationToken cancellationToken2) {
            await ChangeBoolean("Hide qrcode to render?", "qrcode_hidden not sets.", Configuration.GetType().GetProperty(nameof(Configuration.QrCodeHidden))!, Configuration, cancellationToken2);
         }

         async Task ChangeHidden(CancellationToken cancellationToken2) {
            await ChangeBoolean("Hide character password?", "hidden not sets.", Configuration.GetType().GetProperty(nameof(Configuration.Hidden))!, Configuration, cancellationToken2);
         }

         Task ChangePalette(CancellationToken cancellationToken2) {
            var paletteCheckoutWidget = CheckoutWidget<string>.ConsoleInputCheckoutWidget("Select palette", IColorPalette.GetPaletteNames(Asm), InputService);
            if (!paletteCheckoutWidget.Result.Out(out var paletteCheckoutWidgetResult))
               return Task.CompletedTask;
            Configuration.Palette = paletteCheckoutWidgetResult.Value;
            if (IColorPalette.GetPalette(Configuration.Palette, Asm).Out(out var palette))
               Graphics.ChangePalette(palette);
            return Task.CompletedTask;
         }
      }

      var table = new TableWidget(["Name", "JSON Name", "Value"], Configuration.GetProps().Select(x => new[] {
            x.Name, x.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name, x.GetValue(Configuration)!.ToString()
         })
         .ToArray()!, [20, 20, 30]);
      await table.Render(Graphics, cancellationToken);
   }

   [CommanderHandlerId("/color")]
   public async Task ColorHandler(ICommand command, CancellationToken cancellationToken) {
      if (command.Tags.Contains("list")) {
         var colorTableWidget = new TableWidget(["Name"], IColorPalette.GetPaletteNames(Asm).Select(x => new[] { x }).ToArray(), [20]);
         await colorTableWidget.Render(Graphics, cancellationToken);
         return;
      }

      if (command.Tags.Contains("current")) {
         await Graphics.RenderTextLine("Primary", Graphics.Primary);
         await Graphics.RenderTextLine("Default", Graphics.Default);
         await Graphics.RenderTextLine("Secondary", Graphics.Secondary);
         await Graphics.RenderTextLine("Success", Graphics.Success);
         await Graphics.RenderTextLine("Bad", Graphics.Bad);
         await Graphics.RenderTextLine("Wrong", Graphics.Wrong);
         return;
      }

      if (IColorPalette.GetPalette(command.Value, Asm).Out(out var palette)) {
         Graphics.ChangePalette(palette);
         Configuration.Palette = command.Value;
         await Configuration.Save(cancellationToken);
         await Graphics.RenderTextLine("Palette success changed", Graphics.Success);
         return;
      }

      await Graphics.RenderTextLine("Palette not found", Graphics.Bad);
   }

   [CommanderHandlerId("/model")]
   public async Task SetHandler(ICommand command, CancellationToken cancellationToken) {
      if (command.Tags.Contains("list")) {
         if (!PasswordRouter.GetAttributes().Out(out var list)) return;
         var modelTableWidget = new TableWidget("Name|Description".Split('|'), list.Select(x => new string[] { x.Name, x.Description }).ToArray(), [20,120]);
         await modelTableWidget.Render(Graphics,cancellationToken);
         return;
      }

      if (command.Tags.Contains("unset")) {
         PasswordRouter.ClearModel();
         await Graphics.RenderTextLine("Generator model unsetted.");
         return;
      }

      if (!string.IsNullOrWhiteSpace(command.Value)) {
         if (PasswordRouter.ChangeModel(command.Value)) {
            await Graphics.RenderTextLine($"Sets generator model, -> {command.Value}");
            return;
         }
         await Graphics.RenderTextLine($"Model <{command.Value}> not found");
         
         return;
      }
      
      await Graphics.RenderTextLine($"Current model is {(PasswordRouter.Current == null ? "null" : $"\'{PasswordRouter.Current.GetType().GetCustomAttribute<PasswordGeneratorInfoAttribute>()!.Name}\'")}");
   }

   [CommanderHandlerId("/gen")]
   public async Task GenHandler(ICommand command, CancellationToken cancellationToken) {
      var route = PasswordRouter.Current;
      if (route == null) {
         await Graphics.RenderTextLine("Route not sets, use: \'/model --list\'", Graphics.Bad);
         return;
      }

      var builder = route.GetBuilder();
      var renderOption = await builder.Accept(PasswordConfigBuilderRender, cancellationToken);
      if (!renderOption) {
         await Graphics.RenderTextLine("Canceled or data not correct", Graphics.Bad);
         return;
      }

      var generateOption = await route.Generate(builder.Build());
      if (generateOption.Out(out var result)) await result.Accept(PasswordResultRender, cancellationToken);
   }

   [CommanderHandlerId("/receive")]
   public async Task ReceiveHandler(ICommand command, CancellationToken cancellationToken) { }

   public Option<ICommandProcessor.CommanderHandlerDelegate> Get(string id) {
      return _handlers.TryGetValue(id, out var value)
         ? Option<ICommandProcessor.CommanderHandlerDelegate>.Ok(value)
         : Option<ICommandProcessor.CommanderHandlerDelegate>.Fail();
   }
}