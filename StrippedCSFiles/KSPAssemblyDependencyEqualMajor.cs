using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class KSPAssemblyDependencyEqualMajor : Attribute
{
	public string name;

	public int versionMajor;

	public int versionMinor;

	public int versionRevision;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPAssemblyDependencyEqualMajor(string name, int versionMajor, int versionMinor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPAssemblyDependencyEqualMajor(string name, int versionMajor, int versionMinor, int versionRevision)
	{
		throw null;
	}
}
