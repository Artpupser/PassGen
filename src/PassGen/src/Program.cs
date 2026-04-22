using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using PassGen.Commander;
using PassGen.Configuration;
using PassGen.Graphics;
using PassGen.Graphics.Palettes;
using PassGen.Password;
using PassGen.Services;

using PupaLib.FileIO;

namespace PassGen;

internal static class Program {
   public static readonly VirtualFile ConfigurationFile = VirtualIo.RootFolder.GetFileIn("user.json") ??
                                                          throw new Exception(
                                                             "file with name \'user.json\' not found.");

   private static async Task Main(string[] args) {
      var cts = new CancellationTokenSource();
      var token = cts.Token;
      var services = new ServiceCollection();
      services.AddSingleton<IUserConfiguration>(_ => {
         var config = new UserConfiguration(ConfigurationFile);
         config.Load(token).Wait(token);
         return config;
      });
      services.AddSingleton<IGraphics, ConsoleGraphics>();
      services.AddSingleton<IColorPalette>((serviceProvider) => {
         var userConfig = serviceProvider.GetService<IUserConfiguration>() ??
                          throw new Exception($"bad load {nameof(IUserConfiguration)}.");
         var asm = Assembly.GetEntryAssembly()!;
         return IColorPalette.GetPalette(userConfig.Palette, asm)
            .Out(out var palette)
            ? palette
            : IColorPalette.GetPalette("default", asm).Content;
      });
      services.AddSingleton<ConsoleInputService>();
      services.AddSingleton<ICommandHandlerStorage, PassGenCommandHandlerStorage>();
      services.AddSingleton<ICommandProcessor, PassGenCommandProcessor>();
      services.AddSingleton<PasswordRouter>();

      var provider = services.BuildServiceProvider();
      var graphics = provider.GetRequiredService<IGraphics>();
      var commander = provider.GetRequiredService<ICommandProcessor>();

      Console.CancelKeyPress += (_, consoleCancelEventArgs) => {
         cts.Cancel();
         consoleCancelEventArgs.Cancel = true;
      };

      while (!token.IsCancellationRequested) {
         var resultOption = await graphics.RenderInputDialogue("Command", token);
         if (!resultOption) break;
         ;
         var command = new BasicCommand();
         if (!command.Load(resultOption.Content)) continue;
         await commander.Execute(command, token);
         await graphics.RenderText("\n");
      }

      await graphics.RenderTextLine(
         $"\n\n Token -> {token.GetHashCode()} -> Canceled <{token.IsCancellationRequested}>, Graceful goodbye");
      graphics.ColorReset();
   }
}