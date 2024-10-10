using System;
using UnityEngine;

public class DisplacementExplosion : MonoBehaviour
{
	public float explosionDuration = 1f;

	public float animationLoops = 1f;

	public AnimationCurve scaleCurve;

	public float scaleMax = 1f;

	public AnimationCurve clipCurve;

	public bool destroyOnFinish = true;

	public float startTime;

	public void Awake()
	{
		startTime = Time.time;
		UpdateExplosion(0f);
	}

	public void Update()
	{
		float num = Mathf.Clamp01((Time.time - startTime) / explosionDuration);
		UpdateExplosion(num);
		if (num >= 1f && destroyOnFinish)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void UpdateExplosion(float time)
	{
		UpdateTransform(time);
		UpdateShaderAnimation(time);
		UpdateShaderClip(time);
	}

	public void UpdateTransform(float time)
	{
		float num = scaleMax * scaleCurve.Evaluate(time);
		base.transform.localScale = new Vector3(num, num, num);
	}

	public void UpdateShaderAnimation(float time)
	{
		float num = time * animationLoops;
		float num2 = Mathf.Sin(num * ((float)Math.PI * 2f)) * 0.5f + 0.25f;
		float num3 = Mathf.Sin((num + 1f / 3f) * 2f * (float)Math.PI) * 0.5f + 0.25f;
		float num4 = Mathf.Sin((num + 2f / 3f) * 2f * (float)Math.PI) * 0.5f + 0.25f;
		float num5 = 1f / (num2 + num3 + num4);
		num2 *= num5;
		num3 *= num5;
		num4 *= num5;
		GetComponent<Renderer>().material.SetVector("_ChannelFactor", new Vector4(num2, num3, num4, 0f));
	}

	public void UpdateShaderClip(float time)
	{
		float value = clipCurve.Evaluate(time);
		GetComponent<Renderer>().material.SetFloat("_ClipRange", value);
	}
}
