using PassGen.UI;
using PassGen.Password.Configs;

using PupaLib.Core;

namespace PassGen.Password.Builders;

public abstract class PasswordGeneratorConfigBuilder {
   protected PasswordGeneratorConfig Instance { get; } = new();
   public abstract PasswordGeneratorConfig Build();

   public abstract Task<Option> Accept(IPasswordConfigBuilderVisitor visitor,
      CancellationToken cancellationToken = default);
}