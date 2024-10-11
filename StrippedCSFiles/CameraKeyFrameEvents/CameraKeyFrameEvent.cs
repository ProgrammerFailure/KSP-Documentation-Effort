using System.Runtime.CompilerServices;
using UnityEngine;

namespace CameraKeyFrameEvents;

public class CameraKeyFrameEvent : MonoBehaviour
{
	public float timeIntoFrame;

	public bool done;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CameraKeyFrameEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void RunEvent()
	{
		throw null;
	}
}
