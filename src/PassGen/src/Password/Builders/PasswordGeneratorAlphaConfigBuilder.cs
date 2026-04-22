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

   public override Task<Option> Accept(IPasswordConfigBuilderVisitor visitor,
      CancellationToken cancellationToken = default) {
      return visitor.VisitAlpha(this, cancellationToken);
   }
}