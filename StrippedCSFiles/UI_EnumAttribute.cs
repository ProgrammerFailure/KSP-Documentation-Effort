using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UI_EnumAttribute : PropertyAttribute
{
	public readonly Type objectType;

	public readonly int bitoffset;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UI_EnumAttribute(Type objectType, int bitoffset)
	{
		throw null;
	}
}
