using System;
using UnityEngine;

namespace CameraFXModules;

[Serializable]
public class Wobble : CameraFXModule
{
	public float amplitude;

	public float frequency;

	public WobbleModes modes;

	public float seed;

	public float t;

	public Wobble(string id, float amplitude, float frequency, WobbleModes modes, Views views, float seed, float rotFactor = 1f, float linFactor = 1f)
		: base(id, views, rotFactor, linFactor)
	{
		this.amplitude = amplitude;
		this.frequency = frequency;
		this.modes = modes;
		this.seed = seed;
		t = 0f;
	}

	public override void OnFXAdded(CameraFXCollection host)
	{
	}

	public override void OnFXRemoved(CameraFXCollection host)
	{
	}

	public override Vector3 UpdateLocalPosition(Vector3 defaultPos, Vector3 currPos, float m, Views viewMask)
	{
		if ((modes & WobbleModes.Linear) != 0 && (views & viewMask) != 0 && amplitude != 0f && frequency != 0f && m != 0f)
		{
			t += Time.deltaTime * frequency;
			return currPos + new Vector3(((modes & WobbleModes.flag_1) != 0) ? ((Mathf.PerlinNoise(t, seed * 1f % float.MaxValue) * 2f - 1f) * amplitude * m * linFactor) : 0f, ((modes & WobbleModes.flag_2) != 0) ? ((Mathf.PerlinNoise(t, seed * 2f % float.MaxValue) * 2f - 1f) * amplitude * m * linFactor) : 0f, ((modes & WobbleModes.flag_3) != 0) ? ((Mathf.PerlinNoise(t, seed * 3f % float.MaxValue) * 2f - 1f) * amplitude * m * linFactor) : 0f);
		}
		return currPos;
	}

	public override Quaternion UpdateLocalRotation(Quaternion defaultRot, Quaternion currRot, float m, Views viewMask)
	{
		if ((modes & WobbleModes.Rot) != 0 && (views & viewMask) != 0 && amplitude != 0f && frequency != 0f && m != 0f)
		{
			t += Time.deltaTime * frequency;
			return Quaternion.Euler(((modes & WobbleModes.Pitch) != 0) ? ((Mathf.PerlinNoise(t, seed * 1f % float.MaxValue) * 2f - 1f) * amplitude * 10f * m * rotFactor) : 0f, ((modes & WobbleModes.Yaw) != 0) ? ((Mathf.PerlinNoise(t, seed * 2f % float.MaxValue) * 2f - 1f) * amplitude * 10f * m * rotFactor) : 0f, ((modes & WobbleModes.Roll) != 0) ? ((Mathf.PerlinNoise(t, seed * 3f % float.MaxValue) * 2f - 1f) * amplitude * 10f * m * rotFactor) : 0f) * currRot;
		}
		return currRot;
	}
}
