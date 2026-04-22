namespace PassGen.UI.Widgets;

public class TextLabel(string text, Color color, IGraphicsElement? parent = null) : Leaf {
   public override IGraphicsElement? Parent => parent;
   private string _text = text;
   private Color _color = color;

   public TextLabel() : this(string.Empty, Color.White) { }

   public TextLabel WithColor(Color color) {
      _color = color;
      return this;
   }

   public TextLabel WithText(string text) {
      _text = text;
      return this;
   }


   public override async Task Render(Graphics graphics, CancellationToken cancellationToken = default) {
      await graphics.RenderText(_text, _color);
   }
}