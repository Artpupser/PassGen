namespace PassGen.Password.Results;

public sealed class PasswordArgonResult : PasswordResult {
   public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   public string KeyCode { get; set; } = string.Empty;

   public override Task Accept(IPasswordResultVisitor visitor, CancellationToken cancellationToken) {
      return visitor.VisitArgon(this, cancellationToken);
   }
}