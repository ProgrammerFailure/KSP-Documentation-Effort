using System;
using UnityEngine;

[ExecuteInEditMode]
public class ConeVolume : VolumetricObjectBase
{
	public float coneHeight = 2f;

	public float coneAngle = 30f;

	public float startOffset;

	public float previousConeHeight;

	public float previousConeAngle;

	public float previousStartOffset;

	public override void OnEnable()
	{
		if (volumeShader == "")
		{
			PopulateShaderName();
		}
		base.OnEnable();
	}

	public override void PopulateShaderName()
	{
		volumeShader = "Advanced SS/Volumetric/Cone Volume";
	}

	public override bool HasChanged()
	{
		if (coneHeight == previousConeHeight && coneAngle == previousConeAngle && startOffset == previousStartOffset && !base.HasChanged())
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
		previousConeHeight = coneHeight;
		previousConeAngle = coneAngle;
		previousStartOffset = startOffset;
		base.SetChangedValues();
	}

	public override void UpdateVolume()
	{
		float f = coneAngle * ((float)Math.PI / 180f);
		float num = Mathf.Tan(f) * coneHeight;
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
}
