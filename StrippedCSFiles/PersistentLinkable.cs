using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public class PersistentLinkable : Attribute
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public PersistentLinkable()
	{
		throw null;
	}
}
