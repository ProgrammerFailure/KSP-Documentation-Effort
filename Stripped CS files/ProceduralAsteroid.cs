using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ProceduralAsteroid : ProceduralSpaceObject
{
	public PAsteroid paGenerated;

	public void Start()
	{
		Generate(seed, radius, null, base.RangefinderGeneric, delegate
		{
		});
	}

	public PAsteroid Generate(int seed, float radius, Transform parent, Func<Transform, float> rangefinder, Callback onComplete, bool isSecondary = false)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		UnityEngine.Random.InitState(seed);
		List<PQSMod> list = new List<PQSMod>();
		int i = 0;
		for (int count = mods.Count; i < count; i++)
		{
			mods[i].mod = GetComponent(mods[i].name) as PQSMod;
			if (mods[i].mod != null)
			{
				list.Add(mods[i].mod);
			}
		}
		int j = 0;
		for (int count2 = mods.Count; j < count2; j++)
		{
			if (mods[j].mod == null)
			{
				continue;
			}
			ModWrapper modWrapper = mods[j];
			int k = 0;
			for (int num = modWrapper.values.Length; k < num; k++)
			{
				ModValue modValue = modWrapper.values[k];
				FieldInfo field = modWrapper.mod.GetType().GetField(modValue.name);
				if (field != null)
				{
					float num2 = UnityEngine.Random.Range(modValue.minValue, modValue.maxValue);
					if (modValue.radiusFactor != 0f)
					{
						num2 *= radius * modValue.radiusFactor;
					}
					if (field.FieldType == typeof(int))
					{
						field.SetValue(modWrapper.mod, (int)num2);
					}
					else if (field.FieldType == typeof(float))
					{
						field.SetValue(modWrapper.mod, num2);
					}
					else if (field.FieldType == typeof(double))
					{
						field.SetValue(modWrapper.mod, (double)num2);
					}
					else if (field.FieldType == typeof(bool))
					{
						field.SetValue(modWrapper.mod, num2 > 0.5f);
					}
				}
			}
		}
		Color secondaryColor;
		PAsteroid pAsteroid = CreatePAsteroid(radius, list, rangefinder, onComplete, isSecondary, out secondaryColor);
		pAsteroid.transform.parent = parent;
		pAsteroid.transform.localPosition = Vector3.zero;
		pAsteroid.transform.localRotation = Quaternion.identity;
		Part componentInParent = pAsteroid.gameObject.GetComponentInParent<Part>();
		if (isSecondary && componentInParent != null && componentInParent.mpb != null)
		{
			componentInParent.mpb.SetColor("_emissiveColor", secondaryColor);
		}
		if (debugGenTime)
		{
			Debug.Log("AsteroidCreate: " + (Time.realtimeSinceStartup - realtimeSinceStartup).ToString("F3"));
		}
		return pAsteroid;
	}

	public PAsteroid CreatePAsteroid(float radius, List<PQSMod> modArray, Func<Transform, float> rangefinder, Callback onComplete, bool isSecondary, out Color secondaryColor)
	{
		Mesh mesh = null;
		Mesh mesh2 = null;
		Mesh convexMesh = null;
		mesh = CreateMeshVisual(visualSphere, radius, modArray, out var volume, out var highestPoint);
		if (colliderSphere != null)
		{
			mesh2 = ((!(colliderSphere == visualSphere)) ? CreateMeshCollider(colliderSphere, radius, modArray) : mesh);
		}
		if (convexSphere != null)
		{
			convexMesh = ((convexSphere == visualSphere) ? mesh : ((!(convexSphere == colliderSphere)) ? CreateMeshCollider(convexSphere, radius, modArray) : mesh2));
		}
		PAsteroid pAsteroid = new GameObject("Asteroid").AddComponent<PAsteroid>();
		pAsteroid.Setup(mesh, (!isSecondary || !(secondaryMaterial != null)) ? primaryMaterial : secondaryMaterial, visualLayer, visualTag, mesh2, colliderMaterial, colliderLayer, colliderTag, convexMesh, convexMaterial, convexLayer, convexTag, rangefinder, onComplete);
		if (isSecondary)
		{
			ColorHSV colorHSV = new ColorHSV(UnityEngine.Random.value, 0.684f, 0.992f, UnityEngine.Random.value);
			secondaryColor = colorHSV.ToColor();
			pAsteroid.SetMaterialColor("_emissiveColor", secondaryColor);
		}
		else
		{
			secondaryColor = default(Color);
		}
		pAsteroid.volume = volume;
		pAsteroid.highestPoint = highestPoint;
		if (Application.isPlaying)
		{
			paGenerated = pAsteroid;
		}
		return pAsteroid;
	}

	[ContextMenu("Rebuild")]
	public void Rebuild()
	{
		if (!(paGenerated == null) && Application.isPlaying)
		{
			UnityEngine.Object.Destroy(paGenerated.gameObject);
			Generate(seed, radius, base.transform.parent, base.RangefinderGeneric, delegate
			{
			});
		}
	}
}
