using System.Text;

using dotenv.net;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PassGen.Commander;
using PassGen.Graphics;
using PassGen.Password;
using PassGen.Services;

using PupaLib.FileIO;

namespace PassGen;

internal static class Program {
   private static async Task Main(string[] args) {
      DotEnv.Load();
      var config = new ConfigurationBuilder()
         .SetBasePath(VirtualIo.RootPath)
         .AddEnvironmentVariables()
         .Build();
      var services = new ServiceCollection();
      services.AddSingleton<IConfiguration>(config);
      services.AddSingleton<IGraphics, ConsoleGraphics>();
      services.AddSingleton<IColorPalette, DefaultColorPalette>();
      services.AddSingleton<ConsoleInputService>();
      services.AddSingleton<ICommandHandlerStorage, PassGenCommandHandlerStorage>();
      services.AddSingleton<ICommandProcessor, PassGenCommandProcessor>();
      services.AddSingleton<PasswordRouter>();

      var provider = services.BuildServiceProvider();
      var graphics = provider.GetRequiredService<IGraphics>();
      var commander = provider.GetRequiredService<ICommandProcessor>();
      var cts = new CancellationTokenSource();
      var token = cts.Token;

      Console.CancelKeyPress += (_, consoleCancelEventArgs) => {
         cts.Cancel();
         consoleCancelEventArgs.Cancel = true;
      };

      while (!token.IsCancellationRequested) {
         var resultOption = await graphics.RenderInputDialogue("Command", token);
         if (!resultOption) {
            break;
         };
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