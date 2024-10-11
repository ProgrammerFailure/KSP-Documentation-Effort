using System.Runtime.CompilerServices;
using UnityEngine;

public class AxisFieldLimit
{
	public BaseAxisField limitedField;

	public Vector2 hardLimits;

	public Vector2 softLimits;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisFieldLimit()
	{
		throw null;
	}
}
