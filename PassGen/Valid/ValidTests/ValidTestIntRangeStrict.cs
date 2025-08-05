namespace PassGen.Valid.ValidTests;

public sealed class ValidTestIntRangeStrict(int min, int max) : IValidTest {
	public bool Test(ref object content) {
		var value = (int)content;
		return value > min && value < max;
	}

	public string Message() {
		return $"value is not in range [{min} < your value < {max}]";
	}
}
