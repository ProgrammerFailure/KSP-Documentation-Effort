using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DatabaseLoaderAttrib : Attribute
{
	public string[] extensions;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DatabaseLoaderAttrib(string[] extensions)
	{
		throw null;
	}
}
