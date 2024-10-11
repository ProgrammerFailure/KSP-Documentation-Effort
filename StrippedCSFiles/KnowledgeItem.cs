using System.Runtime.CompilerServices;

public class KnowledgeItem<T>
{
	public delegate T UpdateDataCallback();

	private string caption;

	private string valueFallback;

	private T value;

	private decimal decimalValue;

	private string suffix;

	private UpdateDataCallback updateValue;

	private DiscoveryLevels ItemLevel;

	private DiscoveryInfo host;

	private string formatString;

	public string Caption
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string Value
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string OneLiner
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KnowledgeItem(DiscoveryInfo host, DiscoveryLevels itemLevel, string caption, string valueFallback, UpdateDataCallback updateValueCallback, string suffix = "", string format = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetValue(string noValueFallback)
	{
		throw null;
	}
}
