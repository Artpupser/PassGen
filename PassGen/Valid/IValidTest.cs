namespace PassGen.Valid;

public interface IValidTest
{
    public bool Test(ref object content);
    public string Message();
}