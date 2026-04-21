using PassGen.Graphics;
using PassGen.Password.Configs;

using PupaLib.Core;

namespace PassGen.Password.Builders;

public sealed class PasswordGeneratorBetaConfigBuilder : PasswordGeneratorConfigBuilder {

   public PasswordGeneratorBetaConfigBuilder WithKey(string value) {
      Instance.Set("key", value);
      return this;
   }
   
   public PasswordGeneratorBetaConfigBuilder WithLength(int value) {
      Instance.Set("length", value);
      return this;
   }
   
   public PasswordGeneratorBetaConfigBuilder WithTime(DateTime value) {
      Instance.Set("time", value);
      return this;
   }
   
   public override PasswordGeneratorConfig Build() => Instance;

   public override async Task<Option> Render(IGraphics graphics, CancellationToken cancellationToken) {
      var lengthOption = await graphics.RenderSwitchDialogue("Password length", [2<<2,2<<3,2<<4,2<<5,2<<6,2<<7,2<<8],cancellationToken);
      if (lengthOption.Out(out var lengthOptionTuple)) {
         WithLength(lengthOptionTuple.Item2);
      } else {
         return Option.Fail();
      }
      var keyOption = await graphics.RenderInputDialogue("Secret key", cancellationToken);
      if (keyOption.Out(out var secretKey)) {
         WithKey(secretKey);
      } else {
         return Option.Fail();
      }
      var timeSwitchOption = await graphics.RenderSwitchDialogue("Select time", [$"now \'{DateTime.UtcNow}\'", "input"], cancellationToken);
      if (timeSwitchOption.Out(out var timeSwitchTuple)) {
         if (timeSwitchTuple.Item2 == "now") {
            WithTime(DateTime.UtcNow);
         } else {
            while (!cancellationToken.IsCancellationRequested) {
               var timeStrOption = await graphics.RenderInputDialogue("Time", cancellationToken);
               if (!timeStrOption.Out(out var timeStr))
                  return Option.Fail();
               if (!DateTime.TryParse(timeStr, out var time)) 
                  continue;
               WithTime(time);
               break;
            }
         }
      } else {
         return Option.Fail();
      }
      return Option.Ok();
   }
}