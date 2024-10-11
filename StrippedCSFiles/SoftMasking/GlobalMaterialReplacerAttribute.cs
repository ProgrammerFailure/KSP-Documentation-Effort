using System;
using System.Runtime.CompilerServices;

namespace SoftMasking;

[AttributeUsage(AttributeTargets.Class)]
public class GlobalMaterialReplacerAttribute : Attribute
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public GlobalMaterialReplacerAttribute()
	{
		throw null;
	}
}
