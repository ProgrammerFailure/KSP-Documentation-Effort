using System;
using System.Runtime.CompilerServices;

public class AssemblyInfo
{
	public string name;

	public string path;

	public Version assemblyVersion;

	public bool isDuplicate;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AssemblyInfo(string n, string p, Version aV)
	{
		throw null;
	}
}
