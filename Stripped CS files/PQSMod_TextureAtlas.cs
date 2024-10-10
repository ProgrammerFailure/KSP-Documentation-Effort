using System.Collections.Generic;
using Expansions.Missions.Editor;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Terrain/Texture Atlas")]
public class PQSMod_TextureAtlas : PQSMod
{
	public CBTextureAtlasSO textureAtlasMap;

	public readonly int NUM_PACKED = 4;

	public Material material1Blend;

	public Material material2Blend;

	public Material material3Blend;

	public Material material4Blend;

	public CelestialBody CurrentCelestialBody
	{
		get
		{
			if (!HighLogic.LoadedSceneIsFlight && HighLogic.LoadedScene != GameScenes.SPACECENTER)
			{
				if (HighLogic.LoadedSceneIsMissionBuilder && MissionEditorLogic.Instance.actionPane != null && MissionEditorLogic.Instance.actionPane.CurrentGapDisplay != null)
				{
					GAPCelestialBody gAPCelestialBody = MissionEditorLogic.Instance.actionPane.CurrentGapDisplay as GAPCelestialBody;
					if (gAPCelestialBody != null)
					{
						return gAPCelestialBody.CelestialBody;
					}
				}
				return null;
			}
			return FlightGlobals.currentMainBody;
		}
	}

	public void Start()
	{
		material1Blend = new Material(material1Blend);
		material2Blend = new Material(material2Blend);
		material3Blend = new Material(material3Blend);
		material4Blend = new Material(material4Blend);
	}

	public bool CanTextureAtlasModBeUsed()
	{
		if (GameSettings.TERRAIN_SHADER_QUALITY >= 3 && !(CurrentCelestialBody == null) && !(textureAtlasMap == null))
		{
			return true;
		}
		return false;
	}

	public override void OnSetup()
	{
		base.OnSetup();
		if (CanTextureAtlasModBeUsed())
		{
			if (!sphere.materialsForUpdates.Contains(material1Blend))
			{
				sphere.materialsForUpdates.Add(material1Blend);
			}
			if (!sphere.materialsForUpdates.Contains(material2Blend))
			{
				sphere.materialsForUpdates.Add(material2Blend);
			}
			if (!sphere.materialsForUpdates.Contains(material3Blend))
			{
				sphere.materialsForUpdates.Add(material3Blend);
			}
			if (!sphere.materialsForUpdates.Contains(material4Blend))
			{
				sphere.materialsForUpdates.Add(material4Blend);
			}
			requirements = GClass4.ModiferRequirements.MeshUV3;
		}
	}

	public override void OnQuadBuilt(GClass3 quad)
	{
		if (!CanTextureAtlasModBeUsed())
		{
			return;
		}
		CelestialBody currentCelestialBody = CurrentCelestialBody;
		Vector3d vector3d_ = Vector3d.zero;
		Vector3d vector3d_2 = Vector3d.zero;
		Vector3d vector3d_3 = Vector3d.zero;
		if (HighLogic.LoadedSceneIsMissionBuilder)
		{
			vector3d_ = currentCelestialBody.BodyFrame.vector3d_0;
			vector3d_2 = currentCelestialBody.BodyFrame.vector3d_1;
			vector3d_3 = currentCelestialBody.BodyFrame.vector3d_2;
			Planetarium.CelestialFrame.PlanetaryFrame(0.0, 90.0, 0.0, ref currentCelestialBody.BodyFrame);
		}
		CBTextureAtlasSO.CBTextureAtlasPoint[] array = new CBTextureAtlasSO.CBTextureAtlasPoint[quad.mesh.vertices.Length];
		Vector3[] vertices = quad.mesh.vertices;
		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 vector = quad.quadTransform.TransformPoint(vertices[i]);
			array[i] = textureAtlasMap.GetCBTextureAtlasPoint(currentCelestialBody.GetLatitude(vector) * 0.01745329238474369, currentCelestialBody.GetLongitude(vector) * 0.01745329238474369);
		}
		List<int> list = new List<int>();
		for (int j = 0; j < array.Length; j++)
		{
			for (int k = 0; k < array[j].TextureAtlasIndices.Count; k++)
			{
				list.AddUnique(array[j].TextureAtlasIndices[k]);
			}
		}
		float[] array2 = new float[list.Count];
		for (int l = 0; l < array.Length; l++)
		{
			for (int m = 0; m < list.Count; m++)
			{
				for (int n = 0; n < array[l].TextureAtlasIndices.Count; n++)
				{
					if (list[m] == array[l].TextureAtlasIndices[n])
					{
						array2[m] += array[l].TextureAtlasStrengths[n];
					}
				}
			}
		}
		int[] array3 = new int[NUM_PACKED];
		float[] array4 = new float[NUM_PACKED];
		for (int num = 0; num < NUM_PACKED; num++)
		{
			array3[num] = 255;
			array4[num] = 0f;
		}
		for (int num2 = 0; num2 < list.Count; num2++)
		{
			for (int num3 = 0; num3 < array3.Length; num3++)
			{
				if (!(array2[num2] <= array4[num3]))
				{
					for (int num4 = array3.Length - 1; num4 > num3; num4--)
					{
						array3[num4] = array3[num4 - 1];
						array4[num4] = array4[num4 - 1];
					}
					array3[num3] = list[num2];
					array4[num3] = array2[num2];
					break;
				}
			}
		}
		Material material = material4Blend;
		if (array4[1] == 0f)
		{
			material = material1Blend;
		}
		else if (array4[2] == 0f)
		{
			material = material2Blend;
		}
		else if (array4[3] == 0f)
		{
			material = material3Blend;
		}
		if (quad.sphereRoot.useSharedMaterial)
		{
			quad.meshRenderer.sharedMaterial = material;
		}
		else
		{
			quad.meshRenderer.material = material;
		}
		float x = (float)array3[0] / 10f * 32768f + (float)array3[1] / 10f * 1024f + (float)array3[2] / 10f * 32f + (float)array3[3] / 10f;
		for (int num5 = 0; num5 < vertices.Length; num5++)
		{
			GClass4.cacheUV3s[num5].x = x;
			CBTextureAtlasSO.CBTextureAtlasPoint cBTextureAtlasPoint = array[num5];
			float[] array5 = new float[array3.Length];
			for (int num6 = 0; num6 < cBTextureAtlasPoint.TextureAtlasIndices.Count; num6++)
			{
				for (int num7 = 0; num7 < array3.Length; num7++)
				{
					if (array3[num7] == cBTextureAtlasPoint.TextureAtlasIndices[num6])
					{
						array5[num7] = cBTextureAtlasPoint.TextureAtlasStrengths[num6];
					}
				}
			}
			float num8 = 0f;
			for (int num9 = 0; num9 < array5.Length; num9++)
			{
				num8 += array5[num9];
			}
			if (num8 != 0f)
			{
				for (int num10 = 0; num10 < array5.Length; num10++)
				{
					array5[num10] /= num8;
				}
			}
			else
			{
				array5[0] = 1f;
			}
			GClass4.cacheUV3s[num5].y = Mathf.Floor(array5[0] * 200f) * 200f * 200f + Mathf.Floor(array5[1] * 100f) * 200f + array5[2] * 100f;
		}
		quad.mesh.uv3 = GClass4.cacheUV3s;
		if (HighLogic.LoadedSceneIsMissionBuilder)
		{
			currentCelestialBody.BodyFrame.vector3d_0 = vector3d_;
			currentCelestialBody.BodyFrame.vector3d_1 = vector3d_2;
			currentCelestialBody.BodyFrame.vector3d_2 = vector3d_3;
		}
	}
}
