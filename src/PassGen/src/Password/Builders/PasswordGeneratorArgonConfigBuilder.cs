using PassGen.UI;
using PassGen.Password.Configs;

using PupaLib.Core;

namespace PassGen.Password.Builders;

public sealed class PasswordGeneratorArgonConfigBuilder : PasswordGeneratorConfigBuilder {
   public PasswordGeneratorArgonConfigBuilder WithKey(string value) {
      Instance.Set("key", value);
      return this;
   }

   public PasswordGeneratorArgonConfigBuilder WithLength(int value) {
      Instance.Set("length", value);
      return this;
   }

   public PasswordGeneratorArgonConfigBuilder WithTime(DateTime value) {
      Instance.Set("time", value);
      return this;
   }

   public override PasswordGeneratorConfig Build() {
      return Instance;
   }

   public override Task<Option> Accept(IPasswordConfigBuilderVisitor visitor,
      CancellationToken cancellationToken = default) {
      return visitor.VisitArgon(this, cancellationToken);
   }
}