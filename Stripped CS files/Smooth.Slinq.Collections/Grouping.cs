namespace Smooth.Slinq.Collections;

public struct Grouping<T, U>
{
	public readonly T key;

	public LinkedHeadTail<U> values;

	public Grouping(T key, LinkedHeadTail<U> values)
	{
		this.key = key;
		this.values = values;
	}
}
public struct Grouping<T, U, V>
{
	public readonly T key;

	public Slinq<U, V> values;

	public Grouping(T key, Slinq<U, V> values)
	{
		this.key = key;
		this.values = values;
	}
}
public static class Grouping
{
	public static Grouping<T, U> Create<T, U>(T key, LinkedHeadTail<U> values)
	{
		return new Grouping<T, U>(key, values);
	}

	public static Grouping<T, U, V> Create<T, U, V>(T key, Slinq<U, V> values)
	{
		return new Grouping<T, U, V>(key, values);
	}
}
