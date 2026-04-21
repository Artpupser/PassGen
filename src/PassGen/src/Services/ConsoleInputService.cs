using System.Text;

using PupaLib.Core;

namespace PassGen.Services;

internal sealed class ConsoleInputService {
   private readonly Eventer<ConsoleKeyInfo> _keyPressed;

   public ConsoleInputService() {
      _keyPressed = new Eventer<ConsoleKeyInfo>();
      Task.Run(() => {
         while (true) _keyPressed?.Invoke(this, Console.ReadKey(true));
      });
   }

   public async Task<ConsoleKeyInfo> InputKeyAsync(CancellationToken cancellationToken) {
      var tcs = new TaskCompletionSource();
      var key = new ConsoleKeyInfo();
      var handler = _keyPressed.Subscribe((_,  args) => {
         key = args;
         tcs.SetResult();;
      });
      await Task.WhenAny(
         tcs.Task,
         Task.Delay(Timeout.Infinite, cancellationToken)
      ).ConfigureAwait(false);
      _keyPressed.Unsubscribe(handler);
      return key;
   }

   public async Task<string> InputAsync(CancellationToken cancellationToken) {
      var sb = new StringBuilder();
      var tcs = new TaskCompletionSource();
      _keyPressed.Subscribe(InputMethod);
      await Task.WhenAny(
         tcs.Task,
         Task.Delay(Timeout.Infinite, cancellationToken)
      ).ConfigureAwait(false);
      _keyPressed.Unsubscribe(InputMethod);
      return sb.ToString();

      void InputMethod(object? _, ConsoleKeyInfo args) {
         if (args.Key == ConsoleKey.Backspace) {
            if (sb.Length == 0) return;
            var cursor = (left: Console.GetCursorPosition().Left - 1, top: Console.GetCursorPosition().Top);
            Console.SetCursorPosition(cursor.left, cursor.top);
            Console.Write(' ');
            sb.Remove(sb.Length - 1, 1);
            Console.SetCursorPosition(cursor.left, cursor.top);
            return;
         }

         if (args.Key == ConsoleKey.Enter) {
            tcs.SetResult();
            return;
         }

         var symbol = args.KeyChar;
         sb.Append(symbol);
         Console.Write(symbol);
      }
   }
}