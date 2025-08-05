namespace PassGen.Valid;

public sealed class Validator(object result, IEnumerable<IValidTest> tests) {
	private object _result = result;
	public object Result => _result;
	public int IntResult => (int)_result;
	public string Message { get; private set; }

	public bool IsValid() {
		foreach (var test in tests) {
			if (test.Test(ref _result)) continue;
			Message = test.Message();
			return false;
		}

		return true;
	}
}
