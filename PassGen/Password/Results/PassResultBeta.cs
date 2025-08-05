namespace PassGen.Password.Results;

public sealed class PassResultBeta(string pass, string info, DateTime creationTime) : PassResult
{
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
        return creationTime;
    }
}