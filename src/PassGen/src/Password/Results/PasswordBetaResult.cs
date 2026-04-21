namespace PassGen.Password.Results;

public sealed class PasswordBetaResult : PasswordResult {
   public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   public string KeyCode { get; set; } = string.Empty;
   public override Task Accept(IPasswordResultVisitor visitor, CancellationToken cancellationToken) => visitor.VisitBeta(this, cancellationToken);
}