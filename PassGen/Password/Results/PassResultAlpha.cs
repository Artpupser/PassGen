

namespace PassGen.Password.Results;

public sealed class PassResultAlpha(string pass, string info) : PassResult
{
    private readonly DateTime _creationTime = DateTime.Now;
    public override string GetPass() => pass;
    public override string GetInfo() => info;
    public override DateTime GetDate() => _creationTime;
}