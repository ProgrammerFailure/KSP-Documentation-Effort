using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class KSPPart : Attribute
{
	public DefaultIcons stageIcon;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPPart()
	{
		throw null;
	}
}
