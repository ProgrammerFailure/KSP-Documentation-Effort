using System;
using UnityEngine;

[ExecuteInEditMode]
public class ConeVolumeOcclusion : VolumetricObjectBase
{
	public float coneHeight = 2f;

	public float coneAngle = 30f;

	public float startOffset;

	public int occlusionTextureSize = 128;

	public float previousConeHeight;

	public float previousConeAngle;

	public float previousStartOffset;

	public int previousOcclusionTextureSize = 128;

	public Transform coneCameraTransform;

	public Camera coneCamera;

	public RenderTexture coneCameraRT;

	public override void OnEnable()
	{
		if (volumeShader == "")
		{
			PopulateShaderName();
		}
		base.OnEnable();
		SetupCamera();
	}

	public override void CleanUp()
	{
		if ((bool)coneCameraRT)
		{
			UnityEngine.Object.DestroyImmediate(coneCameraRT);
		}
		base.CleanUp();
	}

	public override void PopulateShaderName()
	{
		volumeShader = "Advanced SS/Volumetric/Cone Volume Occlusion";
	}

	public override bool HasChanged()
	{
		if (coneHeight == previousConeHeight && coneAngle == previousConeAngle && startOffset == previousStartOffset && occlusionTextureSize == previousOcclusionTextureSize && !base.HasChanged())
		{
			return false;
		}
		return true;
	}

	public override void SetChangedValues()
	{
		if (coneHeight < 0f)
		{
			coneHeight = 0f;
		}
		if (coneAngle >= 89f)
		{
			coneAngle = 89f;
		}
		if (occlusionTextureSize < 16)
		{
			occlusionTextureSize = 16;
		}
		previousConeHeight = coneHeight;
		previousConeAngle = coneAngle;
		previousStartOffset = startOffset;
		previousOcclusionTextureSize = occlusionTextureSize;
		base.SetChangedValues();
		SetupCamera();
	}

	public override void UpdateVolume()
	{
		float f = coneAngle * 0.5f * ((float)Math.PI / 180f);
		float num = Mathf.Tan(f) * (coneHeight * 2f);
		float x = num * 0.5f;
		Vector3 vector = new Vector3(num, coneHeight, num);
		if ((bool)meshInstance)
		{
			ScaleMesh(meshInstance, vector, -Vector3.up * coneHeight * 0.5f);
			Bounds bounds = default(Bounds);
			bounds.SetMinMax(-vector, vector);
			meshInstance.bounds = bounds;
		}
		if ((bool)materialInstance)
		{
			materialInstance.SetVector("_ConeData", new Vector4(x, coneHeight, startOffset, Mathf.Cos(f)));
			materialInstance.SetVector("_TextureData", new Vector4(0f - textureMovement.x, 0f - textureMovement.y, 0f - textureMovement.z, 1f / textureScale));
			materialInstance.SetFloat("_Visibility", visibility);
			materialInstance.SetColor("_Color", volumeColor);
			materialInstance.SetTexture("_MainTex", texture);
		}
	}

	public void CreateDepthCamera()
	{
		if (!coneCameraTransform)
		{
			coneCameraTransform = base.transform.Find("ConeCamera");
		}
		if (!coneCameraTransform)
		{
			GameObject gameObject = new GameObject("ConeCamera");
			coneCameraTransform = gameObject.transform;
			coneCameraTransform.parent = base.transform;
		}
		if (!coneCamera)
		{
			coneCamera = coneCameraTransform.GetComponent<Camera>();
		}
		if (!coneCamera)
		{
			coneCamera = coneCameraTransform.gameObject.AddComponent<Camera>();
			coneCamera.gameObject.AddComponent<DrawDepth>();
			coneCamera.enabled = false;
		}
	}

	public RenderTexture ConeRenderTexture()
	{
		if ((bool)coneCameraRT)
		{
			if (coneCameraRT.width == occlusionTextureSize && coneCameraRT.height == occlusionTextureSize)
			{
				return coneCameraRT;
			}
			UnityEngine.Object.Destroy(coneCameraRT);
		}
		coneCameraRT = new RenderTexture(occlusionTextureSize, occlusionTextureSize, 0);
		materialInstance.SetTexture("_OcclusionTex", coneCameraRT);
		return coneCameraRT;
	}

	public void SetupCamera()
	{
		CreateDepthCamera();
		coneCameraTransform.position = base.transform.position;
		coneCameraTransform.rotation = Quaternion.LookRotation(-base.transform.up, base.transform.forward);
		coneCamera.farClipPlane = coneHeight;
		coneCamera.nearClipPlane = 0.01f;
		coneCamera.fieldOfView = coneAngle;
		coneCamera.clearFlags = CameraClearFlags.Color;
		coneCamera.backgroundColor = Color.black;
		coneCamera.targetTexture = ConeRenderTexture();
	}
}
