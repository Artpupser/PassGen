using PupaLib.Core;

namespace PassGen.Password.Builders;

public interface IPasswordConfigBuilderVisitor {
   public Task<Option> VisitAlpha(PasswordGeneratorAlphaConfigBuilder builder,
      CancellationToken cancellationToken = default);

   public Task<Option> VisitBeta(PasswordGeneratorBetaConfigBuilder builder,
      CancellationToken cancellationToken = default);
   
   public Task<Option> VisitArgon(PasswordGeneratorArgonConfigBuilder builder,
      CancellationToken cancellationToken = default);
}