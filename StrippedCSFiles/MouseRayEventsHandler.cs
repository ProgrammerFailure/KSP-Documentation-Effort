using System.Runtime.CompilerServices;
using UnityEngine;

public class MouseRayEventsHandler : MouseEventsHandlerBase
{
	private const int trackDelayFrames = 5;

	private int lastTrackFrame;

	private bool isDragging;

	private bool canDrag;

	public bool forwardMouseEvents;

	protected bool isRayOver
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MouseRayEventsHandler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnRayHit(RaycastHit hit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void LateUpdate()
	{
		throw null;
	}
}
