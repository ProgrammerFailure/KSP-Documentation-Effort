using System;
using System.Runtime.CompilerServices;

namespace A;

internal class AssemblyInfoAttribute : Attribute
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public AssemblyInfoAttribute(string str)
	{
		throw null;
	}
}
