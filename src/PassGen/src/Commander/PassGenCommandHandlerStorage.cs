using System.Collections.Frozen;
using System.Reflection;
using System.Text.Json.Serialization;

using PassGen.Configuration;
using PassGen.Graphics;
using PassGen.Graphics.Palettes;
using PassGen.Password;

using PupaLib.Core;

namespace PassGen.Commander;

public sealed class PassGenCommandHandlerStorage : ICommandHandlerStorage {
   private readonly FrozenDictionary<string, ICommandProcessor.CommanderHandlerDelegate> _handlers;

   private readonly CommandHelpData[] _helpContent = [
      new("[input] > ", "you can back/cancel operation printed \'<-\'"),
      new("/help", "see help."),
      new("/color", "sets color scheme."),
      new("/set", "set generation model."),
      new("/gen", "generate password."),
      new("/restore", "restore password.")
   ];

   private IGraphics Graphics { get; }
   private PasswordRouter PasswordRouter { get; }
   private IUserConfiguration Configuration { get; }
   private PasswordResultRender PasswordResultRender { get; }
   private Assembly Asm => Assembly.GetEntryAssembly()!;

   private record CommandHelpData(string Name, string Description);


   public PassGenCommandHandlerStorage(IUserConfiguration userConfiguration, IGraphics graphics,
      PasswordRouter passwordRouter) {
      Graphics = graphics;
      Configuration = userConfiguration;
      PasswordRouter = passwordRouter;
      PasswordResultRender = new PasswordResultRender(graphics, userConfiguration);
      _handlers = ICommandHandlerStorage.LoadHandlers(this);
   }

   [CommanderHandlerId("/help")]
   public async Task HelpHandler(ICommand command, CancellationToken cancellationToken) {
      await Graphics.RenderTable(
         ["Command", "Description"],
         _helpContent.Select(x => new[] { x.Name, x.Description }).ToArray(),
         [20, 120]);
   }

   [CommanderHandlerId("/config")]
   public async Task ConfigHandler(ICommand command, CancellationToken cancellationToken) {
      if (command.Tags.Contains("set")) {
         while (!cancellationToken.IsCancellationRequested) {
            var optionSwitchSetting = await Graphics.RenderSwitchDialogue("Select setting",
               Configuration.GetProps().Select(x => x.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name).ToArray(),
               cancellationToken);
            var dict = new Dictionary<string, Func<CancellationToken, Task>> {
               ["palette"] = ChangePalette,
               ["hidden"] = ChangeHidden,
               ["qrcode_buffer"] = ChangeQrCodeBuffer,
               ["qrcode_hidden"] = ChangeQrCodeHidden,
            };

            if (!optionSwitchSetting.Out(out var settingSwitchTuple)) break;
            await dict[settingSwitchTuple.Item2](cancellationToken);
            await Configuration.Save(cancellationToken);
         }

         await Graphics.RenderTextLine("Exited from configuration panel", Graphics.CurrentPalette.Wrong);
         return;

         async Task ChangeBoolean(string prompt, string cancelMessage, PropertyInfo prop, object instance, CancellationToken cancellationToken2) {
            var option =
               await Graphics.RenderSwitchDialogue(prompt, ["yes", "no"], cancellationToken2);
            if (option.Out(out var tuple)) {
               prop.SetValue(tuple.Item1 == 0, instance);
               return;
            };
            await Graphics.RenderTextLine(cancelMessage, Graphics.CurrentPalette.Bad);
         }
         
         async Task ChangeQrCodeBuffer(CancellationToken cancellationToken2) {
            var option =
               await Graphics.RenderSwitchDialogue("Save qrcode in buffer?", [true, false], cancellationToken2);
            if (option.Out(out var tuple)) {
               Configuration.QrCodeBuffer = tuple.Item2;
               return;
            };
            await Graphics.RenderTextLine("qrcode_buffer not sets", Graphics.CurrentPalette.Bad);
         }
         
         async Task ChangeQrCodeHidden(CancellationToken cancellationToken2) {
            var option =
               await Graphics.RenderSwitchDialogue("Hide qrcode to render?", [true, false], cancellationToken2);
            if (option.Out(out var tuple)) {
               Configuration.QrCodeHidden = tuple.Item2;
               return;
            };
            await Graphics.RenderTextLine("qrcode_hidden not sets.", Graphics.CurrentPalette.Bad);
         }

         async Task ChangeHidden(CancellationToken cancellationToken2) {
            var option =
               await Graphics.RenderSwitchDialogue("Hide character password?", [true, false], cancellationToken2);
            if (option.Out(out var tuple)) {
               Configuration.Hidden = tuple.Item2;
               return;
            };
            await Graphics.RenderTextLine("hidden not sets.", Graphics.CurrentPalette.Bad);
         }

         async Task ChangePalette(CancellationToken cancellationToken2) {
            var optionSwitchPalette =
               await Graphics.RenderSwitchDialogue("Select palette", IColorPalette.GetPaletteNames(Asm),
                  cancellationToken2);
            if (optionSwitchPalette.Out(out var paletteSwitchTuple)) {
               Configuration.Palette = paletteSwitchTuple.Item2;
               if (IColorPalette.GetPalette(Configuration.Palette, Asm).Out(out var palette))
                  Graphics.ChangePalette(palette);
            }
         }
      }

      await Graphics.RenderTable(["Name", "JSON Name", "Value"]
         ,
         Configuration.GetProps().Select(x => new[] {
               x.Name, x.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name, x.GetValue(Configuration)!.ToString()
            })
            .ToArray()!,
         [20, 20, 30]);
   }

   [CommanderHandlerId("/color")]
   public async Task ColorHandler(ICommand command, CancellationToken cancellationToken) {
      if (command.Tags.Contains("list")) {
         await Graphics.RenderTable("Name".Split('|'),
            IColorPalette.GetPaletteNames(Asm).Select(x => new[] { x }).ToArray(), [20]);
         return;
      }

      if (command.Tags.Contains("current")) {
         await Graphics.RenderTextLine("Primary", Graphics.CurrentPalette.Primary);
         await Graphics.RenderTextLine("Default", Graphics.CurrentPalette.Default);
         await Graphics.RenderTextLine("Secondary", Graphics.CurrentPalette.Secondary);
         await Graphics.RenderTextLine("Success", Graphics.CurrentPalette.Success);
         await Graphics.RenderTextLine("Bad", Graphics.CurrentPalette.Bad);
         await Graphics.RenderTextLine("Wrong", Graphics.CurrentPalette.Wrong);
         return;
      }

      if (IColorPalette.GetPalette(command.Value, Asm).Out(out var palette)) {
         Graphics.ChangePalette(palette);
         Configuration.Palette = command.Value;
         await Configuration.Save(cancellationToken);
         await Graphics.RenderTextLine("Palette success changed", Graphics.CurrentPalette.Success);
         return;
      }

      await Graphics.RenderTextLine("Palette not found", Graphics.CurrentPalette.Bad);
   }

   [CommanderHandlerId("/model")]
   public async Task SetHandler(ICommand command, CancellationToken cancellationToken) {
      if (command.Tags.Contains("list")) {
         if (!PasswordRouter.GetAttributes().Out(out var list)) return;
         await Graphics.RenderTable("Name|Description".Split('|'),
            list.Select(x => new string[] { x.Name, x.Description }).ToArray(), [20, 120]);
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