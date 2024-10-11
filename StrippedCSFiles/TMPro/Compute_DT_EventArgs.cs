using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

public class Compute_DT_EventArgs
{
	public Compute_DistanceTransform_EventTypes EventType;

	public float ProgressPercentage;

	public Color[] Colors;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Compute_DT_EventArgs(Compute_DistanceTransform_EventTypes type, float progress)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Compute_DT_EventArgs(Compute_DistanceTransform_EventTypes type, Color[] colors)
	{
		throw null;
	}
}
