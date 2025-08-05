namespace PassGen.Valid.ValidTests;

public sealed class ValidTestIsDateTime : IValidTest
{
    public bool Test(ref object content)
    {
        var str = content as string;
        return DateTime.TryParse(str, out _);
    }

    public string Message() => "Value is not int32";
}