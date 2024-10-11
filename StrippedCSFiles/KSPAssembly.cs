using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Assembly)]
public class KSPAssembly : Attribute
{
	public string name;

	public int versionMajor;

	public int versionMinor;

	public int versionRevision;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPAssembly(string name, int versionMajor, int versionMinor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPAssembly(string name, int versionMajor, int versionMinor, int versionRevision)
	{
		throw null;
	}
}
