using System.Runtime.CompilerServices;
using UnityEngine;

public static class CometVesselExtension
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T GetCopyOf<T>(this Component comp, T other) where T : Component
	{
		throw null;
	}
}
