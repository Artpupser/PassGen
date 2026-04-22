using PassGen.UI;
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

   public override PasswordGeneratorConfig Build() {
      return Instance;
   }

   public override Task<Option> Accept(IPasswordConfigBuilderVisitor visitor,
      CancellationToken cancellationToken = default) {
      return visitor.VisitBeta(this, cancellationToken);
   }
}