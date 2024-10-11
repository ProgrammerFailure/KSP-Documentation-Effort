using System.Runtime.CompilerServices;

public class ValueBox<T>
{
	public T value;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ValueBox(T value)
	{
		throw null;
	}
}
