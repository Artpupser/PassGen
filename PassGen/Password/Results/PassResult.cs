namespace PassGen.Password.Results;

public abstract class PassResult
{
    public abstract string GetPass();
    public abstract string GetInfo();
    public abstract DateTime GetDate();

    public string GetPassCheckForQrCode()
    {
        return $"<{GetPass()}>\n{GetInfo()}";
    }
    public string GetPassCheck()
    {
        return $"<< START PASS CHECK >>\nCORE: {GetType().Name[10..]}\nCREATION DATE: {GetDate()}\nPASSWORD: {GetPass()}\nINFORMATION: {GetInfo()}\n<< END PASS CHECK >>";
    }
}