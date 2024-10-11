using System.Runtime.CompilerServices;
using UnityEngine;

namespace SoftMasking;

public static class MaskChannel
{
	public static Color alpha;

	public static Color red;

	public static Color green;

	public static Color blue;

	public static Color gray;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static MaskChannel()
	{
		throw null;
	}
}
