using UnityEngine;

namespace CameraFXModules;

public abstract class CameraFXModule
{
	public string id;

	public Views views;

	public float linFactor;

	public float rotFactor;

	public CameraFXModule(string id, Views views, float rotFactor = 1f, float linFactor = 1f)
	{
		this.id = id;
		this.views = views;
		this.rotFactor = rotFactor;
		this.linFactor = linFactor;
	}

	public abstract void OnFXAdded(CameraFXCollection host);

	public abstract void OnFXRemoved(CameraFXCollection host);

	public abstract Vector3 UpdateLocalPosition(Vector3 defaultPos, Vector3 currPos, float m, Views viewMask);

	public abstract Quaternion UpdateLocalRotation(Quaternion defaultRot, Quaternion currRot, float m, Views viewMask);

	public virtual bool IsActive()
	{
		return true;
	}

	public virtual string GetModuleID()
	{
		return id;
	}
}
