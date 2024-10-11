using System.Runtime.CompilerServices;
using UnityEngine;

namespace CameraKeyFrameEvents;

public class Fade : CameraKeyFrameEvent
{
	public float fadeDuration;

	public Color T0;

	public Color T1;

	public CameraFade fadeController;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Fade()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void RunEvent()
	{
		throw null;
	}
}
