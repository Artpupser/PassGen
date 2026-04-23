namespace PassGen.Password.Results;

public interface IPasswordResultVisitor {
   public Task VisitAlpha(PasswordAlphaResult result, CancellationToken cancellationToken);
   public Task VisitBeta(PasswordBetaResult result, CancellationToken cancellationToken);
   public Task VisitArgon(PasswordArgonResult result, CancellationToken cancellationToken);
}