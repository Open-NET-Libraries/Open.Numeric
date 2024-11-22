namespace Open.Numeric;

public interface IProcedureResult<out T>
{
	int Count { get; }
	T Sum { get; }
	T Average { get; }
}
