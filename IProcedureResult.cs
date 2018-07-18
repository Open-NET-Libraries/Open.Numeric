namespace Open.Numeric
{
	interface IProcedureResult<T>
	{
		int Count { get; }
		T Sum { get; }
		T Average { get; }
	}
}
