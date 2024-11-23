namespace Open.Numeric;

public static class Extensions
{
	#region Numeric shortcuts.
	public static bool IsNaN(this double value)
		=> double.IsNaN(value);

	public static bool IsNaN(this float value)
		=> float.IsNaN(value);

	public static bool IsDefault(this double value)
		=> value.Equals(default);
	#endregion
}
