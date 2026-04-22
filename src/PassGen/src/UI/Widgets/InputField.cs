using PassGen.Services;

namespace PassGen.UI.Widgets;

public class InputField(string placeholder, InputField.InputDelegate inputCallback, IGraphicsElement? parent = null)
   : Leaf {
   public delegate Task<string> InputDelegate(CancellationToken cancellationToken = default);

   private string _placeholder = placeholder;
   private InputDelegate? _inputCallback = inputCallback;

   public string Result { get; protected set; } = string.Empty;

   public static InputField ConsoleInputField(string placeholder, ConsoleInputService inputService) {
      return new InputField(placeholder, async token => {
         var result = string.Empty;
         while (!token.IsCancellationRequested) {
            if (!string.IsNullOrWhiteSpace(result)) break;
            result = await inputService.InputAsync(token);
         }

         return result;
      });
   }

   public override IGraphicsElement? Parent => parent;

   public InputField WithPlaceholder(string value) {
      _placeholder = value;
      return this;
   }

   public InputField WithGetter(InputDelegate value) {
      _inputCallback = value;
      return this;
   }

   public override async Task Render(Graphics graphics, CancellationToken cancellationToken = default) {
      await graphics.RenderText($"[{_placeholder}] > ", graphics.Primary);
      if (_inputCallback is null)
         return;
      Result = await _inputCallback(cancellationToken);
      await graphics.RenderTextLine(string.Empty, graphics.Default);
   }
}