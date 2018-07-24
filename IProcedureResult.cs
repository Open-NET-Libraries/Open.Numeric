using System.Diagnostics.CodeAnalysis;

namespace Open.Numeric
{
	// ReSharper disable once ArrangeTypeModifiers
	[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
	interface IProcedureResult<out T>
	{
		int Count { get; }
		T Sum { get; }
		T Average { get; }
	}
}
