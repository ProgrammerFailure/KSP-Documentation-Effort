using System.Runtime.CompilerServices;

namespace KSP.IO;

public class PluginConfiguration
{
	private string file;

	private PluginConfigNode root;

	public object this[string key]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected PluginConfiguration(string pathToFile)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T GetValue<T>(string key)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T GetValue<T>(string key, T _default)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetValue(string key, object value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PluginConfiguration CreateForType<T>(Vessel flight = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void save()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void load()
	{
		throw null;
	}
}
