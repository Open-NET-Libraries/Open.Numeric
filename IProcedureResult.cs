using System.Diagnostics.CodeAnalysis;

namespace Open.Numeric;

interface IProcedureResult<out T>
{
	int Count { get; }
	T Sum { get; }
	T Average { get; }
}
