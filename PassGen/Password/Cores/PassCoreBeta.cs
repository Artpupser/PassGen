using PassGen.Password.Results;
using PassGen.Utils;

namespace PassGen.Password.Cores;

[PassCoreId("beta (not working)")]
public sealed class PassCoreBeta : PassCore<PassResultBeta>
{
    public override PassResultBeta Generate()
    {
        var codeKey = ProjectContext.Terminal.Input("Key word");
        repeatLength:
        var lengthValidator = new Validator(ProjectContext.Terminal.Input("Length"), [
            new ValidTestStrToIntParse(),
            new ValidTestIntRangeNotStrict(10, (int)Math.Pow(2, 13))]);
        if (!lengthValidator.IsValid())
        {
            ProjectContext.Terminal.OutL(lengthValidator.Message);
            goto repeatLength;
        }
        var creationDate = DateTime.Now;
        return new PassResultBeta(Process(codeKey, lengthValidator.IntResult, creationDate), $"\n\tKey word: {codeKey}\n\tPassword length: {lengthValidator.IntResult}\n\tCreation date: {creationDate}", creationDate);
    }

    private static string Process(string codeKey, int length, DateTime creationDate)
    {
        throw new NotImplementedException();
    }
    public override PassResultBeta Regenerate()
    {
        var codeKey = ProjectContext.Terminal.Input("Key word");
        repeatLength:
        var lengthValidator = new Validator(ProjectContext.Terminal.Input("Length"), [
            new ValidTestStrToIntParse(),
            new ValidTestIntRangeNotStrict(10, (int)Math.Pow(2, 13))]);
        if (!lengthValidator.IsValid())
        {
            ProjectContext.Terminal.OutL(lengthValidator.Message);
            goto repeatLength;
        }
        repeatTime:
        var creationDateValidator = new Validator(ProjectContext.Terminal.Input("Creation date"), [new ValidTestIsDateTime()]);
        if (!creationDateValidator.IsValid())
        {
            ProjectContext.Terminal.OutL(creationDateValidator.Message);
            goto repeatTime;
        }
        var creationDate = DateTime.Parse((string)creationDateValidator.Result);
        return new PassResultBeta(Process(codeKey, lengthValidator.IntResult, creationDate), $"\n\tСекретный ключ: {codeKey}\n\tДлинна пароля: {lengthValidator.IntResult}\n\tДата создания: {creationDate}", creationDate);
    }
}