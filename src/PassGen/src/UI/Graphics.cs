using PassGen.UI.Palettes;

namespace PassGen.UI;

public abstract class Graphics(IColorPalette currentPalette) : IColorPalette {
   private IColorPalette CurrentPalette { get; set; } = currentPalette;

   #region IColorPalette

   public Color Default => CurrentPalette.Default;
   public Color Primary => CurrentPalette.Primary;
   public Color Secondary => CurrentPalette.Secondary;
   public Color Success => CurrentPalette.Success;
   public Color Wrong => CurrentPalette.Wrong;
   public Color Bad => CurrentPalette.Bad;

   #endregion

   public void ChangePalette(IColorPalette colorPalette) {
      CurrentPalette = colorPalette;
   }

   public abstract ValueTask RenderText(string content, Color? color = null);
   public abstract ValueTask RenderTextLine(string content, Color? color = null);
   public abstract Task RenderElement(IGraphicsElement element, CancellationToken cancellationToken = default);

   public abstract void ClearScreen();
}