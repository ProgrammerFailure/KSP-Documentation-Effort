using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalSeat : InternalModule
{
	[KSPField]
	public string seatTransformName;

	[KSPField]
	public string displayseatName;

	[KSPField]
	public string displayseatIndex;

	public Transform seatTransform;

	[KSPField]
	public bool allowCrewHelmet;

	[KSPField]
	public Vector3 kerbalEyeOffset;

	[KSPField]
	public Vector3 kerbalScale;

	[KSPField]
	public Vector3 kerbalOffset;

	[KSPField]
	public string portraitCameraName;

	public Camera portraitCamera;

	public Vector3 portraitCameraInitialPosition;

	public Quaternion portraitCameraInitialRotation;

	private bool capturedInitial;

	[KSPField]
	public Vector3 portraitOffset;

	[NonSerialized]
	public ProtoCrewMember crew;

	public bool taken;

	public Kerbal kerbalRef;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalSeat()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DespawnCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdatePortraitOffset()
	{
		throw null;
	}
}
