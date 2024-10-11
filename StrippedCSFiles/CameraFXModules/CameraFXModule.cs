using System.Runtime.CompilerServices;
using UnityEngine;

namespace CameraFXModules;

public abstract class CameraFXModule
{
	public string id;

	public Views views;

	public float linFactor;

	public float rotFactor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CameraFXModule(string id, Views views, float rotFactor = 1f, float linFactor = 1f)
	{
		throw null;
	}

	public abstract void OnFXAdded(CameraFXCollection host);

	public abstract void OnFXRemoved(CameraFXCollection host);

	public abstract Vector3 UpdateLocalPosition(Vector3 defaultPos, Vector3 currPos, float m, Views viewMask);

	public abstract Quaternion UpdateLocalRotation(Quaternion defaultRot, Quaternion currRot, float m, Views viewMask);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string GetModuleID()
	{
		throw null;
	}
}
