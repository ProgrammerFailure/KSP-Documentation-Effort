using UnityEngine;

[ExecuteInEditMode]
public class BoxVolume : VolumetricObjectBase
{
	public Vector3 boxSize = Vector3.one * 5f;

	public Vector3 previousBoxSize = Vector3.one;

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
		volumeShader = "Advanced SS/Volumetric/Box Volume";
	}

	public override bool HasChanged()
	{
		if (!(boxSize != previousBoxSize) && !base.HasChanged())
		{
			return false;
		}
		return true;
	}

	public override void SetChangedValues()
	{
		previousBoxSize = boxSize;
		base.SetChangedValues();
	}

	public override void UpdateVolume()
	{
		Vector3 vector = boxSize * 0.5f;
		if ((bool)meshInstance)
		{
			ScaleMesh(meshInstance, boxSize);
			Bounds bounds = default(Bounds);
			bounds.SetMinMax(-vector, vector);
			meshInstance.bounds = bounds;
		}
		if ((bool)materialInstance)
		{
			materialInstance.SetVector("_BoxMin", new Vector4(0f - vector.x, 0f - vector.y, 0f - vector.z, 0f));
			materialInstance.SetVector("_BoxMax", new Vector4(vector.x, vector.y, vector.z, 0f));
			materialInstance.SetVector("_TextureData", new Vector4(0f - textureMovement.x, 0f - textureMovement.y, 0f - textureMovement.z, 1f / textureScale));
			materialInstance.SetFloat("_Visibility", visibility);
			materialInstance.SetColor("_Color", volumeColor);
			materialInstance.SetTexture("_MainTex", texture);
		}
	}
}
