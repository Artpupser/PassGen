using System.Text;
using PassGen.Password.Results;
using PassGen.Utils;
using PassGen.Valid;
using PassGen.Valid.ValidTests;

namespace PassGen.Password.Cores;

[PassCoreId("beta")]
public sealed class PassCoreBeta : PassCore<PassResultBeta> {
	public override PassResultBeta Generate() {
		var codeKey = ProjectContext.Terminal.Input("Key word");
		repeatLength:
		var lengthValidator = new Validator(ProjectContext.Terminal.Input("Length"), [
			new ValidTestStrToIntParse(),
			new ValidTestIntRangeNotStrict(10, (int)Math.Pow(2, 13))
		]);
		if (!lengthValidator.IsValid()) {
			ProjectContext.Terminal.OutL(lengthValidator.Message);
			goto repeatLength;
		}

		var creationDate = DateTime.Now;
		return new PassResultBeta(Process(codeKey, lengthValidator.IntResult, creationDate),
			$"\n\tKey word: {codeKey}\n\tPassword length: {lengthValidator.IntResult}\n\tCreation date: {creationDate}",
			creationDate);
	}

	private static string Process(string codeKey, int length, DateTime creationDate) {
		var seed = Encoding.UTF8.GetBytes(codeKey).Sum(x => x);
		var hash = Tools.GenerateHash(length,
			$"{creationDate.Year}{creationDate.Month}{creationDate.Day}{creationDate.Hour}{creationDate.Minute}{creationDate.Second}{Tools.GetBase64FromStr(codeKey)}");
		var rnd = new Random(seed);
		var rnd2 = new Random(seed + length);
		var password = new StringBuilder();
		password.Append(hash);
		for (var i = 0; i < password.Length; i++) {
			var skipSymbol = rnd2.Next(int.MinValue, int.MaxValue) % 2 == rnd.Next(int.MinValue, int.MaxValue);
			if (skipSymbol) continue;
			var rndIndex = rnd.Next(0, password.Length);
			password[rndIndex] = PasswordCharacters[rnd2.Next(0, PasswordCharacters.Length)];
		}

		return password.ToString();
	}

	public override PassResultBeta Regenerate() {
		var codeKey = ProjectContext.Terminal.Input("Key word");
		repeatLength:
		var lengthValidator = new Validator(ProjectContext.Terminal.Input("Length"), [
			new ValidTestStrToIntParse(),
			new ValidTestIntRangeNotStrict(10, (int)Math.Pow(2, 13))
		]);
		if (!lengthValidator.IsValid()) {
			ProjectContext.Terminal.OutL(lengthValidator.Message);
			goto repeatLength;
		}

		repeatTime:
		var creationDateValidator =
			new Validator(ProjectContext.Terminal.Input("Creation date"), [new ValidTestIsDateTime()]);
		if (!creationDateValidator.IsValid()) {
			ProjectContext.Terminal.OutL(creationDateValidator.Message);
			goto repeatTime;
		}

		var creationDate = DateTime.Parse((string)creationDateValidator.Result);
		return new PassResultBeta(Process(codeKey, lengthValidator.IntResult, creationDate),
			$"\n\tСекретный ключ: {codeKey}\n\tДлинна пароля: {lengthValidator.IntResult}\n\tДата создания: {creationDate}",
			creationDate);
	}
}
