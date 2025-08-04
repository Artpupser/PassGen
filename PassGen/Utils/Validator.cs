namespace PassGen.Utils;


public sealed class Validator(object result, IEnumerable<IValidTest> tests)
{
    private object _result = result;
    public object Result => _result;
    public int IntResult => (int)_result;
    public string Message { get; private set; }
    public bool IsValid()
    {
        foreach (var test in tests)
        {
            if (test.Test(ref _result))
            {
                continue;
            }
            Message = test.Message();
            return false;
        }
        return true;
    }
}

public sealed class ValidTestIntRangeStrict(int min, int max) : IValidTest
{
    public bool Test(ref object content)
    {
        var value = (int)content;
        return value > min && value < max;
    }
    public string Message() => $"value is not in range [{min} < your value < {max}]";
}
public sealed class ValidTestIntRangeNotStrict(int min, int max) : IValidTest
{
    public bool Test(ref object content)
    {
        var value = (int)content;
        return value >= min && value <= max;
    }
    public string Message() => $"value is not in range [{min} <= your value <= {max}]";
}
public sealed class ValidTestIsDateTime : IValidTest
{
    public bool Test(ref object content)
    {
        var str = content as string;
        return DateTime.TryParse(str, out _);
    }

    public string Message() => "Value is not int32";
}
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
public interface IValidTest
{
    public bool Test(ref object content);
    public string Message();
}
