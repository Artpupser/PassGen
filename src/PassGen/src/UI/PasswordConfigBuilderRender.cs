using PassGen.Password.Builders;
using PassGen.Services;
using PassGen.UI;
using PassGen.UI.Widgets;

using PupaLib.Core;

namespace PassGen.UI;

public sealed class PasswordConfigBuilderRender(Graphics graphics, ConsoleInputService inputService)
   : IPasswordConfigBuilderVisitor {
   public async Task<Option> VisitAlpha(PasswordGeneratorAlphaConfigBuilder builder,
      CancellationToken cancellationToken = default) {
      var checkoutWidgetLength = CheckoutWidget<int>.ConsoleInputCheckoutWidget("Password length",
         [2 << 2, 2 << 3, 2 << 4, 2 << 5, 2 << 6, 2 << 7, 2 << 8], inputService);
      await checkoutWidgetLength.Render(graphics, cancellationToken);
      if (!checkoutWidgetLength.Result.Out(out var checkoutWidgetLengthResult)) return Option.Fail();
      builder.WithLength(checkoutWidgetLengthResult.Value);
      var inputField = InputField.ConsoleInputField("Secret key", inputService);
      await inputField.Render(graphics, cancellationToken);
      if (inputField.Result == string.Empty) return Option.Fail();
      builder.WithKey(inputField.Result);
      return Option.Ok();
   }

   public async Task<Option> VisitBeta(PasswordGeneratorBetaConfigBuilder builder,
      CancellationToken cancellationToken) {
      var checkoutWidgetLength = CheckoutWidget<int>.ConsoleInputCheckoutWidget("Password length",
         [2 << 2, 2 << 3, 2 << 4, 2 << 5, 2 << 6, 2 << 7, 2 << 8], inputService);
      await checkoutWidgetLength.Render(graphics, cancellationToken);
      if (!checkoutWidgetLength.Result.Out(out var checkoutWidgetLengthResult)) return Option.Fail();
      builder.WithLength(checkoutWidgetLengthResult.Value);

      var inputField = InputField.ConsoleInputField("Secret key", inputService);
      await inputField.Render(graphics, cancellationToken);
      if (inputField.Result == string.Empty) return Option.Fail();
      builder.WithKey(inputField.Result);


      var checkoutWidgetTime =
         CheckoutWidget<string>.ConsoleInputCheckoutWidget("Select time", [$"now \'{DateTime.UtcNow}\'", "input"],
            inputService);
      await checkoutWidgetTime.Render(graphics, cancellationToken);
      if (!checkoutWidgetTime.Result.Out(out var checkoutWidgetTimeResult)) return Option.Fail();
      if (checkoutWidgetTimeResult.Index == 0) {
         builder.WithTime(DateTime.UtcNow);
         return Option.Ok();
      }

      while (!cancellationToken.IsCancellationRequested) {
         inputField = InputField.ConsoleInputField("Time", inputService);
         if (inputField.Result == string.Empty)
            return Option.Fail();
         if (!DateTime.TryParse(inputField.Result, out var time))
            continue;
         builder.WithTime(time);
      }

      return Option.Ok();
   }

   public async Task<Option> VisitArgon(PasswordGeneratorArgonConfigBuilder builder,
      CancellationToken cancellationToken = default) {
      var checkoutWidgetLength = CheckoutWidget<int>.ConsoleInputCheckoutWidget("Password length",
         [2 << 2, 2 << 3, 2 << 4, 2 << 5, 2 << 6, 2 << 7, 2 << 8], inputService);
      await checkoutWidgetLength.Render(graphics, cancellationToken);
      if (!checkoutWidgetLength.Result.Out(out var checkoutWidgetLengthResult)) return Option.Fail();
      builder.WithLength(checkoutWidgetLengthResult.Value);

      var inputField = InputField.ConsoleInputField("Secret key", inputService);
      await inputField.Render(graphics, cancellationToken);
      if (inputField.Result == string.Empty) return Option.Fail();
      builder.WithKey(inputField.Result);


      var checkoutWidgetTime =
         CheckoutWidget<string>.ConsoleInputCheckoutWidget("Select time", [$"now \'{DateTime.UtcNow}\'", "input"],
            inputService);
      await checkoutWidgetTime.Render(graphics, cancellationToken);
      if (!checkoutWidgetTime.Result.Out(out var checkoutWidgetTimeResult)) return Option.Fail();
      if (checkoutWidgetTimeResult.Index == 0) {
         builder.WithTime(DateTime.UtcNow);
         return Option.Ok();
      }

      while (!cancellationToken.IsCancellationRequested) {
         inputField = InputField.ConsoleInputField("Time", inputService);
         if (inputField.Result == string.Empty)
            return Option.Fail();
         if (!DateTime.TryParse(inputField.Result, out var time))
            continue;
         builder.WithTime(time);
      }

      return Option.Ok();
   }
}