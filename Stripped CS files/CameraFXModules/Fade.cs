using System.Collections;
using UnityEngine;

namespace CameraFXModules;

public class Fade : CameraFXModule
{
	public CameraFXModule fxMod;

	public CameraFXCollection host;

	public float duration;

	public float falloff;

	public float fxScale;

	public float T0;

	public float float_0;

	public bool willRemove;

	public Fade(CameraFXModule fxModule, float duration, float falloff = 1f)
		: base(fxModule.id, fxModule.views)
	{
		fxMod = fxModule;
		fxScale = 1f;
		this.falloff = falloff;
		this.duration = Mathf.Max(duration, 0.01f);
		T0 = Time.realtimeSinceStartup;
		float_0 = T0;
		willRemove = false;
	}

	public override void OnFXAdded(CameraFXCollection host)
	{
		T0 = Time.realtimeSinceStartup;
		float_0 = T0;
		this.host = host;
	}

	public override void OnFXRemoved(CameraFXCollection host)
	{
		this.host = null;
	}

	public override Vector3 UpdateLocalPosition(Vector3 defaultPos, Vector3 currPos, float m, Views viewMask)
	{
		UpdateFade(Time.realtimeSinceStartup);
		return fxMod.UpdateLocalPosition(defaultPos, currPos, m * fxScale, viewMask);
	}

	public override Quaternion UpdateLocalRotation(Quaternion defaultRot, Quaternion currRot, float m, Views viewMask)
	{
		UpdateFade(Time.realtimeSinceStartup);
		return fxMod.UpdateLocalRotation(defaultRot, currRot, m * fxScale, viewMask);
	}

	public void UpdateFade(float t)
	{
		float_0 = t;
		fxScale = Mathf.Pow(1f - Mathf.Clamp01((float_0 - T0) / duration), falloff);
		if (fxScale == 0f && !willRemove)
		{
			CameraFX.Instance.StartCoroutine(RemoveSelf());
		}
	}

	public IEnumerator RemoveSelf()
	{
		willRemove = true;
		yield return new WaitForEndOfFrame();
		if (host != null)
		{
			host.RemoveFX(this);
		}
	}
}
