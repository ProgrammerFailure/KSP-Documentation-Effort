using System;
using System.Collections.Generic;

[Serializable]
public abstract class InspectorKeyValuePair<T, U>
{
	public T key;

	public U value;

	public InspectorKeyValuePair()
	{
	}

	public InspectorKeyValuePair(T key, U value)
	{
		this.key = key;
		this.value = value;
	}

	public KeyValuePair<T, U> ToKeyValuePair()
	{
		return new KeyValuePair<T, U>(key, value);
	}
}
