using System.Reflection;
using System.Text.Json.Serialization;

using PassGen.Components;
using PassGen.Configuration;
using PassGen.UI;
using PassGen.UI.Palettes;
using PassGen.Password;
using PassGen.Services;
using PassGen.Static;
using PassGen.UI.Widgets;

using PupaLib.Core;

namespace PassGen.Commander;

public sealed class PassGenCommandHandlerStorage(
   IUserConfiguration configuration,
   Graphics graphics,
   PasswordRouter passwordRouter,
   ConsoleInputService inputService)
   : ICommandHandlerStorage {
   private readonly Cache<ICommandProcessor.CommanderHandlerDelegate> _handlersCache = new();
   private PasswordResultRender PasswordResultRender { get; } = new(graphics, configuration);
   private PasswordConfigBuilderRender PasswordConfigBuilderRender { get; } = new(graphics, inputService);
   private static Assembly Asm => Assembly.GetEntryAssembly()!;

   [CommanderHandlerInfo("help", "View all commands")]
   public async Task HelpHandler(ICommand command, CancellationToken cancellationToken) {
      var table = new TableWidget(["Command", "Description"],
         this.GetHandlersAttributes().Select(x => new[] { x.Name, x.Description }).ToArray(), [20, 120]);
      await table.Render(graphics, cancellationToken);
   }

   [CommanderHandlerInfo("config", "Manipulate configuration system.")]
   public async Task ConfigHandler(ICommand command, CancellationToken cancellationToken) {
      if (command.Tags.Contains("set")) {
         while (!cancellationToken.IsCancellationRequested) {
            var settingCheckoutWidget = CheckoutWidget<string>.ConsoleInputCheckoutWidget("Select setting",
               configuration.GetProps().Select(x => x.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name).ToArray(),
               inputService);
            await settingCheckoutWidget.Render(graphics, cancellationToken);
            var dict = new Dictionary<string, Func<CancellationToken, Task>> {
               ["palette"] = ChangePalette,
               ["hidden"] = ChangeHidden,
               ["qrcode_buffer"] = ChangeQrCodeBuffer,
               ["qrcode_hidden"] = ChangeQrCodeHidden
            };

            if (!settingCheckoutWidget.Result.Out(out var settingCheckoutWidgetResult)) break;
            await dict[settingCheckoutWidgetResult.Value](cancellationToken);
            await configuration.Save(cancellationToken);
         }

         await graphics.RenderTextLine("Exited from configuration panel", graphics.Wrong);
         return;

         async Task ChangeBoolean(string prompt, string cancelMessage, PropertyInfo prop, object instance,
            CancellationToken cancellationToken2) {
            var checkoutWidget = CheckoutWidget<string>.ConsoleInputCheckoutWidget(prompt, ["yes", "no"], inputService);
            await checkoutWidget.Render(graphics, cancellationToken2);
            if (checkoutWidget.Result.Out(out var tuple)) {
               prop.SetValue(tuple.Index == 0, instance);
               return;
            }

            await graphics.RenderTextLine(cancelMessage, graphics.Bad);
         }

         async Task ChangeQrCodeBuffer(CancellationToken cancellationToken2) {
            await ChangeBoolean("Save qrcode in buffer?", "qrcode_buffer not sets.",
               configuration.GetType().GetProperty(nameof(configuration.QrCodeBuffer))!, configuration,
               cancellationToken2);
         }

         async Task ChangeQrCodeHidden(CancellationToken cancellationToken2) {
            await ChangeBoolean("Hide qrcode to render?", "qrcode_hidden not sets.",
               configuration.GetType().GetProperty(nameof(configuration.QrCodeHidden))!, configuration,
               cancellationToken2);
         }

         async Task ChangeHidden(CancellationToken cancellationToken2) {
            await ChangeBoolean("Hide character password?", "hidden not sets.",
               configuration.GetType().GetProperty(nameof(configuration.Hidden))!, configuration, cancellationToken2);
         }

         async Task ChangePalette(CancellationToken cancellationToken2) {
            var paletteCheckoutWidget = CheckoutWidget<string>.ConsoleInputCheckoutWidget("Select palette",
               IColorPalette.GetPaletteNames(Asm), inputService);
            await paletteCheckoutWidget.Render(graphics, cancellationToken2);
            if (!paletteCheckoutWidget.Result.Out(out var paletteCheckoutWidgetResult))
               return;
            configuration.Palette = paletteCheckoutWidgetResult.Value;
            if (IColorPalette.GetPalette(configuration.Palette, Asm).Out(out var palette))
               graphics.ChangePalette(palette);
         }
      }

      var table = new TableWidget(["Name", "JSON Name", "Value"], configuration.GetProps().Select(x => new[] {
            x.Name, x.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name, x.GetValue(configuration)!.ToString()
         })
         .ToArray()!, [20, 20, 30]);
      await table.Render(graphics, cancellationToken);
   }

   [CommanderHandlerInfo("color", "Manipulate color scheme")]
   public async Task ColorHandler(ICommand command, CancellationToken cancellationToken) {
      if (command.Tags.Contains("list")) {
         var colorTableWidget = new TableWidget(["Name"],
            IColorPalette.GetPaletteNames(Asm).Select(x => new[] { x }).ToArray(), [20]);
         await colorTableWidget.Render(graphics, cancellationToken);
         return;
      }

      if (command.Tags.Contains("all")) {
         var width = 20;
         foreach (var paletteName in IColorPalette.GetPaletteNames(Asm)) {
            graphics.ChangePalette(IColorPalette.GetPalette(paletteName, Asm).Content);
            await graphics.RenderTextLine(paletteName.PadLeft(width/2 + paletteName.Length / 2, ' ').PadRight(width, ' '));
            await RenderPalette();
            await graphics.RenderTextLine(string.Empty);
         }
         graphics.ChangePalette(IColorPalette.GetPalette(configuration.Palette, Asm).Content);
         return;
      }

      if (!string.IsNullOrWhiteSpace(command.Value)) {
         if (IColorPalette.GetPalette(command.Value, Asm).Out(out var palette)) {
            graphics.ChangePalette(palette);
            configuration.Palette = command.Value;
            await configuration.Save(cancellationToken);
            await graphics.RenderTextLine("Palette success changed", graphics.Success);
            return;
         }
         await graphics.RenderTextLine("Palette not found", graphics.Bad);
         return;
      }

      await RenderPalette();
      return;

      async ValueTask RenderPalette() {
         await graphics.RenderTextLine("Primary", graphics.Primary);
         await graphics.RenderTextLine("Default", graphics.Default);
         await graphics.RenderTextLine("Secondary", graphics.Secondary);
         await graphics.RenderTextLine("Success", graphics.Success);
         await graphics.RenderTextLine("Bad", graphics.Bad);
         await graphics.RenderTextLine("Wrong", graphics.Wrong);
      }
   }

   [CommanderHandlerInfo("model", "Manipulate generation mode.")]
   public async Task SetHandler(ICommand command, CancellationToken cancellationToken) {
      if (command.Tags.Contains("list")) {
         if (!passwordRouter.GetAttributes().Out(out var list)) return;
         var modelTableWidget = new TableWidget("Name|Description".Split('|'),
            list.Select(x => new string[] { x.Name, x.Description }).ToArray(), [20, 120]);
         await modelTableWidget.Render(graphics, cancellationToken);
         return;
      }

      if (command.Tags.Contains("unset")) {
         passwordRouter.ClearModel();
         await graphics.RenderTextLine("Generator model unsetted.");
         return;
      }

      if (!string.IsNullOrWhiteSpace(command.Value)) {
         if (passwordRouter.ChangeModel(command.Value)) {
            await graphics.RenderTextLine($"Sets generator model, -> {command.Value}");
            return;
         }

         await graphics.RenderTextLine($"Model <{command.Value}> not found");

         return;
      }

      await graphics.RenderTextLine(
         $"Current model is {(passwordRouter.Current == null ? "null" : $"\'{passwordRouter.Current.GetType().GetCustomAttribute<PasswordGeneratorInfoAttribute>()!.Name}\'")}");
   }

   [CommanderHandlerInfo("gen", "Generate password.")]
   public async Task GenHandler(ICommand command, CancellationToken cancellationToken) {
      var route = passwordRouter.Current;
      if (route == null) {
         await graphics.RenderTextLine("Route not sets, use: \'model --list\'", graphics.Bad);
         return;
      }

      var builder = route.GetBuilder();
      var renderOption = await builder.Accept(PasswordConfigBuilderRender, cancellationToken);
      if (!renderOption) {
         await graphics.RenderTextLine("Canceled or data not correct", graphics.Bad);
         return;
      }

      var generateOption = await route.Generate(builder.Build());
      if (generateOption.Out(out var result)) await result.Accept(PasswordResultRender, cancellationToken);
   }

   public Option<ICommandProcessor.CommanderHandlerDelegate> Get(string name) {
      if (!_handlersCache.Preloaded) {
         _handlersCache.Preload(() => Task.FromResult(this.GetHandlersWithName())).Wait();
      }
      return _handlersCache.TryGet(name);
   }
}