using System;
using UnityEngine;

public class AtmosphereFromGround : MonoBehaviour
{
	public Transform mainCamera;

	public GameObject sunLight;

	public Vector3 sunLightDirection;

	public Vector3 cameraPos;

	public Color waveLength;

	public Color invWaveLength;

	public float cameraHeight;

	public float cameraHeight2;

	public float outerRadius;

	public float outerRadius2;

	public float innerRadius;

	public float innerRadius2;

	public float ESun;

	public float Kr;

	public float Km;

	public float KrESun;

	public float KmESun;

	public float Kr4PI;

	public float Km4PI;

	public float scale;

	public float scaleDepth;

	public float scaleOverScaleDepth;

	public float samples;

	public float g;

	public float g2;

	public float exposure = 2.5f;

	public float camHeightUnderwater;

	public float underwaterOpacityAltBase;

	public float underwaterOpacityAltMult;

	public Color underwaterColorStart;

	public Color underwaterColorEnd;

	public float lightDot;

	public float dawnFactor = 10f;

	public float underwaterDepth;

	public float initialKrESun;

	public float lerpKrESun = 1f;

	public bool useKrESunLerp;

	public float oceanDepthRecip = 0.001f;

	public float pRadius;

	public CelestialBody planet;

	public bool doScale = true;

	public float scaleToApply;

	public bool DEBUG_alwaysUpdateAll;

	public Renderer r;

	public Transform t;

	public static int shaderPropertyCameraPos;

	public static int shaderPropertyOffsetTransform;

	public static int shaderPropertyLightDir;

	public static int shaderPropertyInvWaveLength;

	public static int shaderPropertyV4CameraHeight;

	public static int shaderPropertyV4CameraHeight2;

	public static int shaderPropertyKrESun;

	public static int shaderPropertyExposure;

	public static int shaderPropertyCamHeightUnderwater;

	public static int shaderPropertyLightDot;

	public MaterialPropertyBlock mpb;

	public Color invisible;

	public void Start()
	{
		mpb = new MaterialPropertyBlock();
		invisible = new Color(0f, 0f, 0f, 0f);
		shaderPropertyCameraPos = Shader.PropertyToID("_v4CameraPos");
		shaderPropertyOffsetTransform = Shader.PropertyToID("_OffsetTransform");
		shaderPropertyLightDir = Shader.PropertyToID("_v4LightDir");
		shaderPropertyInvWaveLength = Shader.PropertyToID("_cInvWaveLength");
		shaderPropertyV4CameraHeight = Shader.PropertyToID("_fCameraHeight");
		shaderPropertyV4CameraHeight2 = Shader.PropertyToID("_fCameraHeight2");
		shaderPropertyKrESun = Shader.PropertyToID("_fKrESun");
		shaderPropertyExposure = Shader.PropertyToID("_fExposure");
		shaderPropertyCamHeightUnderwater = Shader.PropertyToID("_fCamHeightUnderwater");
		shaderPropertyLightDot = Shader.PropertyToID("_lightDot");
		r = GetComponent<Renderer>();
		t = base.transform;
		if (doScale)
		{
			if (scaleToApply <= 0f)
			{
				scaleToApply = 1.025f;
			}
			base.transform.localScale = Vector3.one * scaleToApply;
		}
		ESun = 30f;
		Kr = 0.00125f;
		Km = 0.00015f;
		KrESun = Kr * ESun;
		KmESun = Km * ESun;
		Kr4PI = Kr * 4f * (float)Math.PI;
		Km4PI = Km * 4f * (float)Math.PI;
		samples = 4f;
		g = -0.85f;
		g2 = g * g;
		exposure = 2.5f;
		lerpKrESun = 1f;
		UpdateAtmosphere(updateAll: true);
	}

	public void LateUpdate()
	{
		UpdateAtmosphere(DEBUG_alwaysUpdateAll);
	}

	public void SetKrESunLerp(float newValue)
	{
		if (useKrESunLerp)
		{
			lerpKrESun = newValue;
			KrESun = Mathf.Lerp(0f, initialKrESun, newValue);
		}
	}

	public void UpdateAtmosphere(bool updateAll)
	{
		t.rotation = Quaternion.identity;
		invWaveLength = new Color(pow(waveLength[0], 4), pow(waveLength[1], 4), pow(waveLength[2], 4), 0.5f);
		sunLightDirection = sunLight.transform.TransformDirection(-Vector3.forward);
		cameraPos = mainCamera.position;
		Vector3 lhs = planet.scaledBody.transform.position - mainCamera.transform.position;
		cameraHeight = lhs.magnitude;
		cameraHeight2 = cameraHeight * cameraHeight;
		underwaterDepth = (pRadius - cameraHeight) * ScaledSpace.ScaleFactor;
		if (underwaterDepth >= 0f)
		{
			camHeightUnderwater = underwaterDepth * oceanDepthRecip;
			if (camHeightUnderwater > 1f)
			{
				camHeightUnderwater = 1f;
			}
		}
		else
		{
			camHeightUnderwater = 0f;
		}
		lightDot = Vector3.Dot(lhs, mainCamera.transform.position - Planetarium.fetch.Sun.scaledBody.transform.position);
		lightDot = Mathf.Clamp01(lightDot * dawnFactor);
		if (r != null && MapView.MapIsEnabled && TimeWarp.CurrentRate > 99999f && cameraHeight > outerRadius * 2f)
		{
			r.enabled = false;
			return;
		}
		if (r != null && !r.enabled)
		{
			r.enabled = true;
		}
		if (updateAll)
		{
			if (waveLength == invisible)
			{
				waveLength = new Color(0.65f, 0.57f, 0.475f, 0.5f);
			}
			outerRadius = (float)planet.Radius * scaleToApply * ScaledSpace.InverseScaleFactor;
			outerRadius2 = outerRadius * outerRadius;
			innerRadius = outerRadius * 0.975f;
			innerRadius2 = innerRadius * innerRadius;
			pRadius = (float)planet.Radius * ScaledSpace.InverseScaleFactor;
			scale = 1f / (outerRadius - innerRadius);
			scaleDepth = -0.25f;
			scaleOverScaleDepth = scale / scaleDepth;
			initialKrESun = KrESun;
			SetMaterial(initialSet: true);
		}
		else
		{
			SetMaterial(initialSet: false);
		}
	}

	public void SetMaterial(bool initialSet)
	{
		mpb.SetVector(shaderPropertyCameraPos, new Vector4(cameraPos.x, cameraPos.y, cameraPos.z, 0f));
		mpb.SetVector(shaderPropertyOffsetTransform, t.position);
		mpb.SetVector(shaderPropertyLightDir, new Vector4(sunLightDirection.x, sunLightDirection.y, sunLightDirection.z, 0f));
		mpb.SetColor(shaderPropertyInvWaveLength, invWaveLength);
		mpb.SetFloat(shaderPropertyV4CameraHeight, cameraHeight);
		mpb.SetFloat(shaderPropertyV4CameraHeight2, cameraHeight2);
		mpb.SetFloat(shaderPropertyCamHeightUnderwater, camHeightUnderwater);
		mpb.SetFloat(shaderPropertyLightDot, lightDot);
		mpb.SetFloat(shaderPropertyKrESun, KrESun);
		if (initialSet)
		{
			mpb.SetFloat("_fOuterRadius", outerRadius);
			mpb.SetFloat("_fOuterRadius2", outerRadius2);
			mpb.SetFloat("_fInnerRadius", innerRadius);
			mpb.SetFloat("_fInnerRadius2", innerRadius2);
			mpb.SetFloat("_fKmESun", KmESun);
			mpb.SetFloat("_fKr4PI", Kr4PI);
			mpb.SetFloat("_fKm4PI", Km4PI);
			mpb.SetFloat("_fScale", scale);
			mpb.SetFloat("_fScaleDepth", scaleDepth);
			mpb.SetFloat("_fScaleOverScaleDepth", scaleOverScaleDepth);
			mpb.SetFloat("_Samples", samples);
			mpb.SetFloat("_G", g);
			mpb.SetFloat("_G2", g2);
			mpb.SetFloat(shaderPropertyExposure, exposure);
			mpb.SetFloat("_underwaterOpacityAltBase", underwaterOpacityAltBase);
			mpb.SetFloat("_underwaterOpacityAltMult", underwaterOpacityAltMult);
			mpb.SetColor("_underwaterColorStart", underwaterColorStart);
			mpb.SetColor("_underwaterColorEnd", underwaterColorEnd);
		}
		r.SetPropertyBlock(mpb);
	}

	public float pow(float f, int p)
	{
		return 1f / Mathf.Pow(f, p);
	}
}
