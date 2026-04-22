using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using PassGen.Commander;
using PassGen.Configuration;
using PassGen.UI;
using PassGen.UI.Palettes;
using PassGen.Password;
using PassGen.Services;
using PassGen.UI.Widgets;

using PupaLib.FileIO;

namespace PassGen;

internal static class Program {
   public static readonly VirtualFile ConfigurationFile = VirtualIo.RootFolder.GetFileIn("user.json") ??
                                                          throw new Exception(
                                                             "file with name \'user.json\' not found.");

   private static async Task Main() {
      var cts = new CancellationTokenSource();
      var token = cts.Token;

      var services = new ServiceCollection();
      var config = new UserConfiguration(ConfigurationFile);
      await config.LoadAsync(token).ConfigureAwait(false);
      services.AddSingleton<IUserConfiguration>(_ => config);
      services.AddSingleton<IColorPalette>(static serviceProvider => {
         var userConfig = serviceProvider.GetRequiredService<IUserConfiguration>();
         var asm = Assembly.GetEntryAssembly()!;
         return IColorPalette.GetPalette(userConfig.Palette, asm)
            .Out(out var palette)
            ? palette
            : IColorPalette.GetPalette("default", asm).Content;
      });
      services.AddSingleton<Graphics, ConsoleGraphics>();
      services.AddSingleton<ConsoleInputService>();
      services.AddSingleton<ICommandHandlerStorage, PassGenCommandHandlerStorage>();
      services.AddSingleton<ICommandProcessor, PassGenCommandProcessor>();
      services.AddSingleton<PasswordRouter>();

      var provider = services.BuildServiceProvider();
      var graphics = provider.GetRequiredService<Graphics>();
      var commander = provider.GetRequiredService<ICommandProcessor>();
      var consoleInputService = provider.GetRequiredService<ConsoleInputService>();

      Console.CancelKeyPress += (_, consoleCancelEventArgs) => {
         cts.Cancel();
         consoleCancelEventArgs.Cancel = true;
      };

      var inputField = InputField.ConsoleInputField("Command", consoleInputService);
      while (!token.IsCancellationRequested) {
         await graphics.RenderElement(inputField, token);
         if (inputField.Result == "<-") break;
         var command = new BasicCommand();
         if (!command.Load(inputField.Result)) continue;
         await commander.Execute(command, token);
         await graphics.RenderText("\n");
      }

      await graphics.RenderTextLine(
         $"\n\n Token -> {token.GetHashCode()} -> Canceled <{token.IsCancellationRequested}>, Graceful goodbye",
         graphics.Wrong);
   }
}