using System.Globalization;
using System.Text;

using PassGen.Configuration;
using PassGen.Password.Results;

using QRCoder;

using TextCopy;

namespace PassGen.UI;

public sealed class PasswordResultRender(Graphics graphics, IUserConfiguration configuration)
   : IPasswordResultVisitor {
   private Graphics Graphics { get; } = graphics;
   private const int LineCheckWidth = 30;
   private const string StartCheckText = "PASSWORD CHECK";


   private async Task<string> DrawQrCode(string value) {
      var sb = new StringBuilder();
      using var qrCodeData = QRCodeGenerator.GenerateQrCode(value, QRCodeGenerator.ECCLevel.M, true, false,
         QRCodeGenerator.EciMode.Utf8);
      for (var i = 4; i < qrCodeData.ModuleMatrix.Count - 4; i++) {
         var line = qrCodeData.ModuleMatrix[i];
         for (var j = 4; j < line.Length - 4; j++) {
            var chunk = line[j] ? "██" : "  ";
            sb.Append(chunk);
            if (configuration.QrCodeHidden)
               continue;
            await Graphics.RenderText(chunk, Color.White);
         }

         sb.AppendLine();
         if (configuration.QrCodeHidden)
            continue;
         await Graphics.RenderTextLine(string.Empty);
      }

      return sb.ToString();
   }

   public async Task VisitBeta(PasswordBetaResult result, CancellationToken cancellationToken) {
      var sb = new StringBuilder();
      Graphics.ClearScreen();
      await BeforeCheckGeneration("<< Beta generation result ⚡⚡ >>", result, cancellationToken);
      var endCheckText = result.CreatedAt.ToString(CultureInfo.InvariantCulture);
      sb.AppendLine(StartCheckText.PadRight(LineCheckWidth / 2 + StartCheckText.Length / 2, '_')
         .PadLeft(LineCheckWidth, '_'));
      sb.AppendLine($"PASSWORD: {result.Password}");
      sb.AppendLine($"RESTORE: ");
      sb.AppendLine($"\tSECRET: {result.KeyCode}");
      sb.AppendLine($"\tLENGTH: {result.Length}");
      sb.AppendLine($"\tCREATED_AT: {result.CreatedAt}");
      sb.AppendLine($"INFO: ");
      sb.AppendLine($"\tEMAIL: ");
      sb.AppendLine(endCheckText.PadRight(LineCheckWidth / 2 + endCheckText.Length / 2, '_')
         .PadLeft(LineCheckWidth, '_'));
      await AfterCheckGeneration(sb, result.Password, cancellationToken);
   }

   public async Task VisitAlpha(PasswordAlphaResult result, CancellationToken cancellationToken) {
      var sb = new StringBuilder();
      await BeforeCheckGeneration("<< Alpha generation result ⚡ >>", result, cancellationToken);

      var endCheckText = result.CreatedAt.ToString(CultureInfo.InvariantCulture);
      sb.AppendLine(StartCheckText.PadRight(LineCheckWidth / 2 + StartCheckText.Length / 2, '_')
         .PadLeft(LineCheckWidth, '_'));
      sb.AppendLine($"PASSWORD: {result.Password}");
      sb.AppendLine($"RESTORE: ");
      sb.AppendLine($"\tSECRET: {result.KeyCode}");
      sb.AppendLine($"\tLENGTH: {result.Length}");
      sb.AppendLine($"INFO: ");
      sb.AppendLine($"\tEMAIL: ");
      sb.AppendLine(endCheckText.PadRight(LineCheckWidth / 2 + endCheckText.Length / 2, '_')
         .PadLeft(LineCheckWidth, '_'));

      await AfterCheckGeneration(sb, result.Password, cancellationToken);
   }

   private async Task BeforeCheckGeneration(string title, PasswordResult result, CancellationToken cancellationToken) {
      var props = result.GetProps();
      Graphics.ClearScreen();
      await Graphics.RenderTextLine($"{title}", Graphics.Primary);
      await Graphics.RenderTextLine(string.Empty.PadRight(20, '-'), Graphics.Wrong);
      foreach (var prop in props) {
         await Graphics.RenderText($"{prop.Name}: ", Graphics.Primary);
         var valueContent = prop.GetValue(result)!.ToString()!;
         if (prop.Name == "Password" && configuration.Hidden) valueContent = new string('*', valueContent.Length);
         await Graphics.RenderTextLine(valueContent, Graphics.Success);
      }

      await Graphics.RenderTextLine(string.Empty.PadRight(20, '-'), Graphics.Wrong);
   }

   private async Task AfterCheckGeneration(StringBuilder sb, string password, CancellationToken cancellationToken) {
      await Graphics.RenderTextLine("QR Code: ", Graphics.Primary);
      var qrCode = await DrawQrCode(password);
      await ClipboardService.SetTextAsync($"{(configuration.QrCodeBuffer ? $"{qrCode}\n\n" : string.Empty)}{sb}",
         cancellationToken);
      await Graphics.RenderTextLine("Data copied to clipboard!", Graphics.Success);
      await Graphics.RenderTextLine("Saved content: ", Graphics.Primary);
      if (!configuration.Hidden) await Graphics.RenderText(sb.ToString(), Graphics.Default);
   }
}