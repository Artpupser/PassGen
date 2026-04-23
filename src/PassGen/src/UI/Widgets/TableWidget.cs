namespace PassGen.UI.Widgets;

public class TableWidget(string[] labels, string[][] items, int[] widths, IGraphicsElement? parent = null) : Leaf {
   public override IGraphicsElement? Parent => parent;

   public override async Task Render(Graphics graphics, CancellationToken cancellationToken = default) {
      var lineWidth = widths.Sum() + 1;
      await graphics.RenderTextLine(string.Empty.PadRight(lineWidth, '-'), graphics.Default);
      if (items.Sum(x => x.Length) % labels.Length == 0 && items.All(x => x.Length == labels.Length)) {
         for (var i = 0; i < labels.Length; i++) {
            await graphics.RenderText(
               $"|{labels[i].PadLeft(widths[i] / 2 + labels[i].Length / 2)}".PadRight(widths[i]),
               graphics.Default);
            if (i == labels.Length - 1) await graphics.RenderText($"|", graphics.Default);
         }

         await graphics.RenderTextLine(string.Empty);
         await graphics.RenderTextLine(string.Empty.PadRight(lineWidth, '-'), graphics.Default);
         for (var i = 0; i < items.Length; i++) {
            for (var j = 0; j < items[i].Length; j++) {
               await graphics.RenderText($"|{items[i][j]}".PadRight(widths[j]), graphics.Default);
               if (j == labels.Length - 1) await graphics.RenderText($"|", graphics.Default);
            }

            await graphics.RenderTextLine(string.Empty);
         }

         await graphics.RenderTextLine(string.Empty.PadRight(lineWidth, '-'), graphics.Default);
      }

      await Task.CompletedTask;
   }
}