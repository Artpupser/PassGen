using System.Diagnostics.CodeAnalysis;
using PassGen.Password;
using PassGen.Password.Results;
using PassGen.Utils;
using QRCoder;
using TextCopy;

namespace PassGen;

public static class ProjectContext
{
    public static Terminal Terminal { get; private set; }

    private static async Task Main()
    {
        Terminal = new Terminal("PassGen", ConsoleColor.White, ConsoleColor.Black);
        IReadOnlyList<(object, PassCoreIdAttribute)> allCores = [.. AttributeUtils.GetInstancesAndAttrs<PassCoreIdAttribute>()];
        var ids = string.Join(", ", allCores.Select(x => x.Item2.Id));
        while (true)
        {
            var coreName = Terminal.Input($"Select ID: {ids} | exit", ConsoleColor.Cyan);
            if (coreName == "exit") break;
            var core = allCores.FirstOrDefault(x => x.Item2.Id == coreName).Item1;
            if (core != null)
            {
                var actionName = Terminal.Input($"\n\t1. generate \n\t2. recover\n", ConsoleColor.Cyan);
                if (actionName != "1" && actionName != "2") continue;
#pragma warning disable CS8509 
                var result = actionName switch
#pragma warning restore CS8509
                {
                    "1" => (PassResult)((dynamic)core).Generate(),
                    "2" => (PassResult)((dynamic)core).Regenerate(),
                };
                var passCheck = result.GetPassCheck();
                Terminal.OutL(passCheck, ConsoleColor.Yellow);
                await ClipboardService.SetTextAsync(passCheck);
                Terminal.OutL("Pass check copied to clipboard.", ConsoleColor.Green);
                using var generator = new QRCodeGenerator();
                using var qrCode = new AsciiQRCode(generator.CreateQrCode(result.GetPassCheckForQrCode(), QRCodeGenerator.ECCLevel.L));
                foreach (var s in qrCode.GetGraphic(repeatPerModule: 1, darkColorString: "██", whiteSpaceString:"  ", drawQuietZones: true))
                {
                    if (s == '\n')
                    {
                        Terminal.Out(s);
                        continue;
                    }
                    Terminal.Out(s, ConsoleColor.Black, ConsoleColor.White);
                }
                Terminal.OutL("\nQR-code pass check generated.", ConsoleColor.Yellow);
                Terminal.OutL("Password success created!", ConsoleColor.Green);
                continue;
            }
            Terminal.OutL($"Core with name [{coreName}] not found");
        }
        await Task.CompletedTask;
    }
}