using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class EffectDefinition : Attribute
{
	public string nodeName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EffectDefinition(string nodeName)
	{
		throw null;
	}
}
