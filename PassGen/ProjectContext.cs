using PassGen.Password;
using PassGen.Password.Results;
using PassGen.Utils;
using QRCoder;
using TextCopy;

namespace PassGen;

public static class ProjectContext {
	public static Terminal Terminal { get; private set; }

	private static async Task Main() {
		Terminal = new Terminal("PassGen", "1.2", ConsoleColor.White, ConsoleColor.Black);
		Terminal.SetDefault();
		IReadOnlyList<(object, PassCoreIdAttribute)> allCores =
			[.. AttributeUtils.GetInstancesAndAttrs<PassCoreIdAttribute>()];
		var ids = string.Join(", ", allCores.Select(x => x.Item2.Id));
		while (true) {
			var coreName = Terminal.Input($"Select ID: {ids} | exit", ConsoleColor.Cyan);
			if (coreName == "exit") break;
			dynamic core = allCores.FirstOrDefault(x => x.Item2.Id == coreName).Item1;
			if (core != null) {
				var actionName = Terminal.Input("\n\t1. generate \n\t2. recover\n", ConsoleColor.Cyan);
				if (actionName is not "1" and "2") continue;
				var result = actionName == "1" ? (PassResult)core.Generate() : (PassResult)core.Regenerate();
				var passCheck = result.GetPassCheck();
				Terminal.OutL(passCheck, ConsoleColor.Yellow);
				await ClipboardService.SetTextAsync(passCheck);
				Terminal.OutL("Pass check copied to clipboard.", ConsoleColor.Green);
				using var generator = new QRCodeGenerator();
				using var qrCode =
					new AsciiQRCode(generator.CreateQrCode(result.GetPassCheckForQrCode(), QRCodeGenerator.ECCLevel.L));
				Terminal.OutQrCode(qrCode);
				Terminal.OutL("\nQR-code pass check generated.", ConsoleColor.Yellow);
				Terminal.OutL("Password success created!", ConsoleColor.Green);
				continue;
			}

			Terminal.OutL($"Core with name [{coreName}] not found");
		}

		await Task.CompletedTask;
	}
}
