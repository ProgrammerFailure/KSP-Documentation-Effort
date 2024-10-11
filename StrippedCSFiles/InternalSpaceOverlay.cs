using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalSpaceOverlay : MonoBehaviour
{
	private Callback onDismiss;

	private Camera ivaCam;

	private Transform ivaCamTrf;

	private Camera extCam;

	private Transform extCamTrf;

	private Vessel vessel;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalSpaceOverlay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static InternalSpaceOverlay Create(Vessel v, Callback onDismiss)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Setup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnCameraChange(CameraManager.CameraMode data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onCrewTransferred(GameEvents.HostedFromToAction<ProtoCrewMember, Part> data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}
