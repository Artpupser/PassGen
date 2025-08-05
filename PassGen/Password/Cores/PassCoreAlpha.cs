using System.Text;
using PassGen.Password.Results;
using PassGen.Utils;
using PassGen.Valid;
using PassGen.Valid.ValidTests;

namespace PassGen.Password.Cores;

[PassCoreId("alpha")]
public sealed class PassCoreAlpha : PassCore<PassResultAlpha> {
	public override PassResultAlpha Generate() {
		var codeKey = ProjectContext.Terminal.Input("Ключ. слово");
		repeat:
		var lengthValidator = new Validator(ProjectContext.Terminal.Input("Длинна"), [
			new ValidTestStrToIntParse(),
			new ValidTestIntRangeNotStrict(10, (int)Math.Pow(2, 13))
		]);
		if (!lengthValidator.IsValid()) {
			ProjectContext.Terminal.OutL(lengthValidator.Message);
			goto repeat;
		}

		return new PassResultAlpha(Process(codeKey, lengthValidator.IntResult),
			$"\n\tСекретный ключ: {codeKey}\n\tДлинна пароля: {lengthValidator.IntResult}");
	}

	private static string Process(string codeKey, int length) {
		var hash = new StringBuilder();
		for (var i = 0; i < length + 1 % Tools.Hash256('*').Length; i++)
			hash.Append(Tools.Hash256(codeKey));
		var ex = hash.ToString()[..length];
		hash.Clear();
		var rnd = new Random(codeKey.Select(x => (byte)x).Sum(x => x % 2 == 0 ? x + length : x - length));
		while (ex.Length != hash.Length)
			hash.Append(ex[rnd.Next(0, ex.Length)]);
		return hash.ToString();
	}

	public override PassResultAlpha Regenerate() {
		return Generate();
	}
}
