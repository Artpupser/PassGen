using PassGen.Graphics.Palettes;

using PupaLib.Core;

namespace PassGen.Graphics;

public interface IGraphics {
   public IColorPalette CurrentPalette { get; protected set; }

   public virtual void ChangePalette(IColorPalette colorPalette) {
      CurrentPalette = colorPalette;
   }

   public Task RenderText(string content, Color? color = null);
   public Task RenderTextLine(string content, Color? color = null);
   public void Clear();
   public void ColorReset();

   public Task RednerInstruction(IGraphicsInstruction instruction, CancellationToken cancellationToken) {
      return instruction.Render(this, cancellationToken);
   }

   public Task<Option<string>> RenderInputDialogue(string prompt, CancellationToken cancellationToken);

   public Task<Option<(int, T)>> RenderSwitchDialogue<T>(string prompt, T[] items,
      CancellationToken cancellationToken);

   public Task RenderTable(string[] titles, string[][] items, int[] width);
}