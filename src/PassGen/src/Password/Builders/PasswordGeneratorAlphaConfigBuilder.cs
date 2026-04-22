using PassGen.Graphics;
using PassGen.Password.Configs;

using PupaLib.Core;

namespace PassGen.Password.Builders;

public sealed class PasswordGeneratorAlphaConfigBuilder : PasswordGeneratorConfigBuilder {
   public PasswordGeneratorAlphaConfigBuilder WithKey(string value) {
      Instance.Set("key", value);
      return this;
   }

   public PasswordGeneratorAlphaConfigBuilder WithLength(int value) {
      Instance.Set("length", value);
      return this;
   }

   public override PasswordGeneratorConfig Build() {
      return Instance;
   }

   public override async Task<Option> Render(IGraphics graphics, CancellationToken cancellationToken) {
      var lengthOption = await graphics.RenderSwitchDialogue("Password length",
         [2 << 2, 2 << 3, 2 << 4, 2 << 5, 2 << 6, 2 << 7, 2 << 8], cancellationToken);
      if (lengthOption.Out(out var lengthOptionTuple))
         WithLength(lengthOptionTuple.Item2);
      else
         return Option.Fail();
      var keyOption = await graphics.RenderInputDialogue("Secret key", cancellationToken);
      if (keyOption.Out(out var secretKey))
         WithKey(secretKey);
      else
         return Option.Fail();
      return Option.Ok();
   }
}