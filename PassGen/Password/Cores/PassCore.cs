using PassGen.Password.Results;

namespace PassGen.Password.Cores;

public abstract class PassCore<T> where T : PassResult {
	protected const string PasswordCharacters =
		"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_=+{}[]|\\:;\"'<>,.?/ ";

	public abstract T Generate();
	public abstract T Regenerate();
}
