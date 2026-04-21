using System.Globalization;
using System.Text;

using PassGen.Password.Results;

using QRCoder;

using TextCopy;

namespace PassGen.Graphics;

public sealed class PasswordResultRender(IGraphics graphics) : IPasswordResultVisitor {
   private IGraphics Graphics { get; } = graphics;
   private const int LineCheckWidth = 30;
   private const string StartCheckText = "PASSWORD CHECK";
   

   private async Task DrawQrCode(string value) {
      using var qrCodeData = QRCodeGenerator.GenerateQrCode(value, QRCodeGenerator.ECCLevel.M, forceUtf8: true, utf8BOM: false, eciMode: QRCodeGenerator.EciMode.Utf8);
      for (var i = 4; i < qrCodeData.ModuleMatrix.Count - 4; i++) {
         var line = qrCodeData.ModuleMatrix[i];
         for (var j = 4; j < line.Length - 4; j++) {
            await Graphics.RenderText(line[j] ? "██" : "  ", Color.White);
         }
         await Graphics.RenderTextLine(string.Empty);
      }
   }

   public async Task VisitBeta(PasswordBetaResult result, CancellationToken cancellationToken) {
      var sb = new StringBuilder();
      Graphics.Clear();
      await Graphics.RenderTextLine("<< Beta generation result ⚡⚡ >>", Graphics.CurrentPalette.Primary);
      await Graphics.RenderTextLine(string.Empty.PadRight(20,'-'), Graphics.CurrentPalette.Wrong);
      await Graphics.RenderText($"Password: ", Graphics.CurrentPalette.Primary);
      await Graphics.RenderTextLine($"{result.Password}", Graphics.CurrentPalette.Success);
      await Graphics.RenderText($"Length: ", Graphics.CurrentPalette.Primary);
      await Graphics.RenderTextLine($"{result.Password.Length}", Graphics.CurrentPalette.Secondary);
      await Graphics.RenderText($"Created At: ", Graphics.CurrentPalette.Primary);
      await Graphics.RenderTextLine($"{result.CreatedAt}", Graphics.CurrentPalette.Secondary);
      await Graphics.RenderTextLine(string.Empty.PadRight(20,'-'), Graphics.CurrentPalette.Wrong);
      
      var endCheckText = result.CreatedAt.ToString(CultureInfo.InvariantCulture);
      sb.AppendLine(StartCheckText.PadRight(LineCheckWidth / 2 + StartCheckText.Length / 2, '_').PadLeft(LineCheckWidth, '_'));
      sb.AppendLine($"PASSWORD: {result.Password}");
      sb.AppendLine($"RESTORE: ");
      sb.AppendLine($"\tSECRET: {result.KeyCode}");
      sb.AppendLine($"\tLENGTH: {result.Length}");
      sb.AppendLine($"\tCREATED_AT: {result.CreatedAt}");
      sb.AppendLine($"INFO: ");
      sb.AppendLine($"\tEMAIL: ");
      sb.AppendLine(endCheckText.PadRight(LineCheckWidth / 2 + endCheckText.Length / 2, '_').PadLeft(LineCheckWidth, '_'));

      await ClipboardService.SetTextAsync(sb.ToString(), cancellationToken);
      
      await Graphics.RenderTextLine("Data copied to clipboard!", Graphics.CurrentPalette.Success);
      await Graphics.RenderTextLine("QR Code: ", Graphics.CurrentPalette.Primary);
      await DrawQrCode(result.Password);
      await Graphics.RenderTextLine("Saved content: ", Graphics.CurrentPalette.Primary);
      await Graphics.RenderText(sb.ToString(), Graphics.CurrentPalette.Default);
   }

   public async Task VisitAlpha(PasswordAlphaResult result, CancellationToken cancellationToken) {
      var sb = new StringBuilder();
      Graphics.Clear();
      await Graphics.RenderTextLine("<< Alpha generation result ⚡ >>", Graphics.CurrentPalette.Primary);
      await Graphics.RenderTextLine(string.Empty.PadRight(20,'-'), Graphics.CurrentPalette.Wrong);
      await Graphics.RenderText($"Password: ", Graphics.CurrentPalette.Primary);
      await Graphics.RenderTextLine($"{result.Password}", Graphics.CurrentPalette.Success);
      await Graphics.RenderText($"Length: ", Graphics.CurrentPalette.Primary);
      await Graphics.RenderTextLine($"{result.Password.Length}", Graphics.CurrentPalette.Secondary);
      await Graphics.RenderTextLine(string.Empty.PadRight(20,'-'), Graphics.CurrentPalette.Wrong);
      
      var endCheckText = result.CreatedAt.ToString(CultureInfo.InvariantCulture);
      
      sb.AppendLine(StartCheckText.PadRight(LineCheckWidth / 2 + StartCheckText.Length / 2, '_').PadLeft(LineCheckWidth, '_'));
      sb.AppendLine($"PASSWORD: {result.Password}");
      sb.AppendLine($"RESTORE: ");
      sb.AppendLine($"\tSECRET: {result.KeyCode}");
      sb.AppendLine($"\tLENGTH: {result.Length}");
      sb.AppendLine($"INFO: ");
      sb.AppendLine($"\tEMAIL: ");
      sb.AppendLine(endCheckText.PadRight(LineCheckWidth / 2 + endCheckText.Length / 2, '_').PadLeft(LineCheckWidth, '_'));

      await ClipboardService.SetTextAsync(sb.ToString(), cancellationToken);
      
      await Graphics.RenderTextLine("Data copied to clipboard!", Graphics.CurrentPalette.Success);
      await Graphics.RenderTextLine("QR Code: ", Graphics.CurrentPalette.Primary);
      await DrawQrCode(result.Password);
      await Graphics.RenderTextLine("Saved content: ", Graphics.CurrentPalette.Primary);
      await Graphics.RenderText(sb.ToString(), Graphics.CurrentPalette.Default);
   }
}




