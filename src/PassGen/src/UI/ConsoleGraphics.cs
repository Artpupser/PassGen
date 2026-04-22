using PassGen.Services;
using PassGen.UI.Palettes;

using PupaLib.Core;

namespace PassGen.UI;

internal sealed class ConsoleGraphics(IColorPalette palette, ConsoleInputService inputService) : Graphics(palette) {
   public IColorPalette CurrentPalette { get; set; } = palette;

   private void SetColor(Color? color = null) {
      if (color == null) return;
      Console.Out.Write($"\e[38;2;{color.Value.R};{color.Value.G};{color.Value.B}m");
   }

   public override ValueTask RenderText(string content, Color? color = null) {
      SetColor(color);
      Console.Out.Write(content);
      SetColor(Default);
      return ValueTask.CompletedTask;
   }

   public override ValueTask RenderTextLine(string content, Color? color = null) {
      SetColor(color);
      Console.Out.WriteLine(content);
      SetColor(Default);
      return ValueTask.CompletedTask;
   }

   public override async Task RenderElement(IGraphicsElement element, CancellationToken cancellationToken = default) {
      await element.Render(this, cancellationToken);
   }

   public override void ClearScreen() {
      Console.Clear();
   }

   private async Task<string> Input(CancellationToken cancellationToken) {
      var result = string.Empty;
      while (!cancellationToken.IsCancellationRequested) {
         if (!string.IsNullOrWhiteSpace(result)) break;
         result = await inputService.InputAsync(cancellationToken);
      }

      await RenderText("\n");
      return result;
   }

   public async Task<Option<string>> RenderInputDialogue(string prompt, CancellationToken cancellationToken) {
      await RenderText($"[{prompt}] > ", CurrentPalette.Primary);
      var result = await Input(cancellationToken);
      return result == "<-" ? Option<string>.Fail() : Option<string>.Ok(result);
   }

   public async Task<Option<(int, T)>> RenderSwitchDialogue<T>(string prompt, T[] items,
      CancellationToken cancellationToken) {
      var cursor = 0;
      while (!cancellationToken.IsCancellationRequested) {
         await RenderTextLine($"<< [{prompt}] >>", CurrentPalette.Secondary);
         for (var i = 0; i < items.Length; i++) {
            var color = cursor == i ? CurrentPalette.Primary : CurrentPalette.Default;
            await RenderTextLine($"{items[i]}", color);
         }

         var key = await inputService.InputKeyAsync(cancellationToken);

         switch (key.Key) {
            case ConsoleKey.W:
               cursor = Math.Clamp(cursor - 1, 0, items.Length - 1);
               break;
            case ConsoleKey.S:
               cursor = Math.Clamp(cursor + 1, 0, items.Length - 1);
               break;
            case ConsoleKey.Enter:
               return Option<(int, T)>.Ok((cursor, items[cursor]));
            case ConsoleKey.Backspace:
               return Option<(int, T)>.Fail();
         }

         ClearScreen();
      }

      await RenderText("Reset");
      return Option<(int, T)>.Fail();
   }

   public async Task RenderTable(string[] titles, string[][] items, int[] width) {
      var lineWidth = titles.Length - 1 + width.Sum();
      await RenderTextLine(string.Empty.PadRight(lineWidth, '-'), CurrentPalette.Default);
      if (items.Sum(x => x.Length) % titles.Length == 0 && items.All(x => x.Length == titles.Length)) {
         for (var i = 0; i < titles.Length; i++) {
            await RenderText($"|{titles[i].PadLeft(width[i] / 2 + titles[i].Length / 2)}".PadRight(width[i]),
               CurrentPalette.Default);
            if (i == titles.Length - 1) await RenderText($"|", CurrentPalette.Default);
         }

         await RenderTextLine(string.Empty);
         await RenderTextLine(string.Empty.PadRight(lineWidth, '-'), CurrentPalette.Default);
         for (var i = 0; i < items.Length; i++) {
            for (var j = 0; j < items[i].Length; j++) {
               await RenderText($"|{items[i][j]}".PadRight(width[j]), CurrentPalette.Default);
               if (j == titles.Length - 1) await RenderText($"|", CurrentPalette.Default);
            }

            await RenderTextLine(string.Empty);
         }

         await RenderTextLine(string.Empty.PadRight(lineWidth, '-'), CurrentPalette.Default);
      }

      await Task.CompletedTask;
   }
}