using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PartInfo : Attribute
{
	public string name;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartInfo(string name)
	{
		throw null;
	}
}
