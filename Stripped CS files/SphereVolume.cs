using UnityEngine;

[ExecuteInEditMode]
public class SphereVolume : VolumetricObjectBase
{
	public float radius = 3f;

	public float previousRadius = 1f;

	public float Contrast = 1.9f;

	public float previousContrast = -1f;

	public float Brightness = 1f;

	public float previousBrightness = -1f;

	public float radiusPower = 1f;

	public float previousRadiusPower = 1f;

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
		volumeShader = "Advanced SS/Volumetric/Sphere Volume";
	}

	public override bool HasChanged()
	{
		if (radius == previousRadius && Contrast == previousContrast && Brightness == previousBrightness && radiusPower == previousRadiusPower && !base.HasChanged())
		{
			return false;
		}
		return true;
	}

	public override void SetChangedValues()
	{
		previousRadius = radius;
		previousContrast = Contrast;
		previousBrightness = Brightness;
		previousRadiusPower = radiusPower;
		base.SetChangedValues();
	}

	public override void UpdateVolume()
	{
		Vector3 vector = Vector3.one * radius * 2f;
		if ((bool)meshInstance)
		{
			ScaleMesh(meshInstance, vector);
			Bounds bounds = default(Bounds);
			bounds.SetMinMax(-vector, vector);
			meshInstance.bounds = bounds;
		}
		if ((bool)materialInstance)
		{
			materialInstance.SetVector("_TextureData", new Vector4(0f - textureMovement.x, 0f - textureMovement.y, 0f - textureMovement.z, 1f / textureScale));
			materialInstance.SetFloat("_RadiusSqr", radius * radius);
			materialInstance.SetFloat("_Visibility", visibility);
			materialInstance.SetColor("_Color", volumeColor);
			materialInstance.SetTexture("_Tex", texture);
			materialInstance.SetFloat("_Contrast", Contrast);
			materialInstance.SetFloat("_Brightness", Brightness);
			materialInstance.SetFloat("_RadiusPower", radiusPower);
		}
	}
}
