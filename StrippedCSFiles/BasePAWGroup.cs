using System.Runtime.CompilerServices;

public class BasePAWGroup
{
	public string name;

	public string displayName;

	public bool startCollapsed;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BasePAWGroup(string name, string displayName, bool startCollapsed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BasePAWGroup()
	{
		throw null;
	}
}
