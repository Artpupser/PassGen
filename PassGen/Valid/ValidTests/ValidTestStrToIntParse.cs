namespace PassGen.Valid.ValidTests;

public sealed class ValidTestStrToIntParse : IValidTest
{
    public bool Test(ref object content)
    {
        var str = content as string;
        if (int.TryParse(str, out var res))
        {
            content = res;
            return true;
        }
        return false;
    }
    public string Message() => "Value is not int32";
}