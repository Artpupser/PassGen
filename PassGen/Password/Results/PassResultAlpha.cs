

namespace PassGen.Password.Results;

public sealed class PassResultAlpha(string pass, string info) : PassResult
{
    private readonly DateTime _creationTime = DateTime.Now;
    public override string GetPass()
    {
        return pass;
    }

    public override string GetInfo()
    {
        return info;
    }

    public override DateTime GetDate()
    {
        return _creationTime;
    }
}