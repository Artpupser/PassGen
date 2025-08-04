namespace PassGen.Password.Results;

public sealed class PassResultBeta(string pass, string info, DateTime creationTime) : PassResult
{
    public override string GetPass() => pass;
    public override string GetInfo() => info;
    public override DateTime GetDate() => creationTime;
}