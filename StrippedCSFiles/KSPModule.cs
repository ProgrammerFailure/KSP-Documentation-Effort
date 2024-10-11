using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class KSPModule : Attribute
{
	public string guiName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPModule(string guiName)
	{
		throw null;
	}
}
