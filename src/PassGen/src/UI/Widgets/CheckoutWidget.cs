using PassGen.Services;

using PupaLib.Core;

namespace PassGen.UI.Widgets;

public class CheckoutWidget<T>(
   string prompt,
   T[] items,
   CheckoutWidget<T>.KeyPressedDelegate keyPressedCallback,
   IGraphicsElement? parent = null) : Leaf {
   public delegate Task<ConsoleKeyInfo> KeyPressedDelegate(CancellationToken cancellationToken = default);

   public Option<CheckoutWidgetResult<T>> Result { get; protected set; } = Option<CheckoutWidgetResult<T>>.Fail();

   public static CheckoutWidget<T> ConsoleInputCheckoutWidget(string prompt, T[] items,
      ConsoleInputService inputService) {
      return new CheckoutWidget<T>(prompt, items, async token => await inputService.InputKeyAsync(token));
   }

   public override IGraphicsElement? Parent => parent;

   public override async Task Render(Graphics graphics, CancellationToken cancellationToken = default) {
      var cursor = 0;
      while (!cancellationToken.IsCancellationRequested) {
         await graphics.RenderTextLine($"<< [{prompt}] >>", graphics.Secondary);
         for (var i = 0; i < items.Length; i++) {
            var color = cursor == i ? graphics.Primary : graphics.Default;
            await graphics.RenderTextLine($"{items[i]}", color);
         }

         var key = await keyPressedCallback(cancellationToken);

         switch (key.Key) {
            case ConsoleKey.W:
               cursor = Math.Clamp(cursor - 1, 0, items.Length - 1);
               break;
            case ConsoleKey.S:
               cursor = Math.Clamp(cursor + 1, 0, items.Length - 1);
               break;
            case ConsoleKey.Enter:
               Result = Option<CheckoutWidgetResult<T>>.Ok(new CheckoutWidgetResult<T>(cursor, items[cursor]));
               return;
            case ConsoleKey.Backspace:
               return;
         }

         graphics.ClearScreen();
      }

      await graphics.RenderText("Reset");
   }
}