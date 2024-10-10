using System;
using UnityEngine;

[AddComponentMenu("KSP/Scaled Space/Planet Fader")]
[RequireComponent(typeof(Renderer))]
public class ScaledSpaceFader : MonoBehaviour
{
	public CelestialBody celestialBody;

	public string floatName;

	public float fadeStart;

	public float fadeEnd;

	public float highQualityShaderFadeStart = -1f;

	public float highQualityShaderFadeEnd = -1f;

	public static double faderMult = 180.0 / Math.PI;

	[HideInInspector]
	public Renderer r;

	public double a;

	public float t;

	public Texture2D resourceMap;

	public int floatID;

	public Renderer rend;

	public int shaderResourceID;

	public int shaderFadeID;

	public void Reset()
	{
		floatName = "_FadeAltitude";
		fadeStart = 0f;
		fadeEnd = 1f;
	}

	public void Start()
	{
		r = GetComponent<Renderer>();
		if (celestialBody == null)
		{
			Debug.LogError("ScaledSpaceFader(" + base.gameObject.name + "): No body assigned");
		}
		floatID = Shader.PropertyToID(floatName);
		rend = GetComponent<Renderer>();
		shaderResourceID = Shader.PropertyToID("_ResourceMap");
	}

	public void Update()
	{
		if (FlightGlobals.currentMainBody == celestialBody && !MapView.MapIsEnabled && FlightGlobals.currentMainBody.pqsController != null)
		{
			a = FlightGlobals.currentMainBody.pqsController.visibleAltitude;
			float num = fadeStart;
			if (highQualityShaderFadeStart > 0f && GameSettings.TERRAIN_SHADER_QUALITY >= 2)
			{
				num = highQualityShaderFadeStart;
			}
			float num2 = fadeEnd;
			if (highQualityShaderFadeEnd > 0f && GameSettings.TERRAIN_SHADER_QUALITY >= 2)
			{
				num2 = highQualityShaderFadeEnd;
			}
			if (a <= (double)num)
			{
				t = 0f;
				r.enabled = false;
			}
			else
			{
				if (a >= (double)num2)
				{
					t = 1f;
				}
				else
				{
					t = (float)((a - (double)num) / (double)(num2 - num));
				}
				r.enabled = true;
				r.material.SetFloat(floatID, t);
			}
		}
		else if (GetAngularSize(base.transform, rend, ScaledCamera.Instance.cam) > 1f)
		{
			r.material.SetFloat(floatID, 1f);
			r.enabled = true;
		}
		else
		{
			r.enabled = false;
		}
		if (celestialBody.ResourceMap != resourceMap)
		{
			resourceMap = celestialBody.ResourceMap;
			r.material.SetTexture(shaderResourceID, resourceMap);
		}
	}

	public void OnDestroy()
	{
		if (r != null && r.material != null)
		{
			r.material.SetFloat(floatID, 1f);
		}
	}

	public static float GetAngularSize(Transform t, Renderer r, Camera c)
	{
		double num = Vector3d.Distance(t.position, c.transform.position);
		return (float)((double)r.bounds.extents.magnitude * faderMult / num) * (float)Screen.height / c.fieldOfView;
	}
}
