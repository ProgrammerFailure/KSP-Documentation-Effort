using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ProceduralComet : ProceduralSpaceObject
{
	public PComet pcGenerated;

	[SerializeField]
	public SphereBaseSO optimizedCollider;

	public void Start()
	{
		if (visualSphere != null)
		{
			Generate(seed, radius, null, base.RangefinderGeneric, delegate
			{
			}, optimizeCollider: false);
		}
	}

	public PComet Generate(int seed, float radius, Transform parent, Func<Transform, float> rangefinder, Callback onComplete, bool optimizeCollider, bool isSecondary = false)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		UnityEngine.Random.InitState(seed);
		List<PQSMod> list = new List<PQSMod>();
		int i = 0;
		for (int count = mods.Count; i < count; i++)
		{
			mods[i].mod = (PQSMod)GetComponent(mods[i].name);
			if (mods[i].mod != null)
			{
				list.Add(mods[i].mod);
				continue;
			}
			Debug.LogWarningFormat("[ProceduralComet]: Mod {0} not found.", mods[i].name);
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
				else
				{
					Debug.LogWarningFormat("[ProceduralComet]: Mod {0} Field {1} not found.", modWrapper.name, modValue.name);
				}
			}
		}
		mods.Sort((ModWrapper a, ModWrapper b) => (a.mod == null || b.mod == null) ? 1 : a.mod.order.CompareTo(b.mod.order));
		Color secondaryColor;
		PComet pComet = CreatePComet(radius, list, rangefinder, onComplete, isSecondary, optimizeCollider && optimizedCollider != null, out secondaryColor);
		pComet.transform.parent = parent;
		pComet.transform.localPosition = Vector3.zero;
		pComet.transform.localRotation = Quaternion.identity;
		Part componentInParent = pComet.gameObject.GetComponentInParent<Part>();
		if (isSecondary && componentInParent != null && componentInParent.mpb != null)
		{
			componentInParent.mpb.SetColor("_emissiveColor", secondaryColor);
		}
		if (debugGenTime)
		{
			Debug.Log("CometCreate: " + (Time.realtimeSinceStartup - realtimeSinceStartup).ToString("F3"));
		}
		return pComet;
	}

	public PComet CreatePComet(float radius, List<PQSMod> modArray, Func<Transform, float> rangefinder, Callback onComplete, bool isSecondary, bool optimizeCollider, out Color secondaryColor)
	{
		Mesh mesh = null;
		Mesh mesh2 = null;
		Mesh convexMesh = null;
		mesh = CreateMeshVisual(visualSphere, radius, modArray, out var volume, out var highestPoint);
		if (optimizeCollider)
		{
			mesh2 = CreateMeshCollider(optimizedCollider, radius, modArray);
			convexMesh = mesh2;
		}
		else
		{
			if (colliderSphere != null)
			{
				mesh2 = ((!(colliderSphere == visualSphere)) ? CreateMeshCollider(colliderSphere, radius, modArray) : mesh);
			}
			if (convexSphere != null)
			{
				convexMesh = ((convexSphere == visualSphere) ? mesh : ((!(convexSphere == colliderSphere)) ? CreateMeshCollider(convexSphere, radius, modArray) : mesh2));
			}
		}
		PComet pComet = new GameObject("Comet").AddComponent<PComet>();
		pComet.Setup(mesh, (!isSecondary || !(secondaryMaterial != null)) ? primaryMaterial : secondaryMaterial, visualLayer, visualTag, mesh2, colliderMaterial, colliderLayer, colliderTag, convexMesh, convexMaterial, convexLayer, convexTag, rangefinder, onComplete);
		if (isSecondary)
		{
			ColorHSV colorHSV = new ColorHSV(UnityEngine.Random.value, 0.684f, 0.992f, UnityEngine.Random.value);
			secondaryColor = colorHSV.ToColor();
			pComet.SetMaterialColor("_emissiveColor", secondaryColor);
		}
		else
		{
			secondaryColor = default(Color);
		}
		pComet.volume = volume;
		pComet.highestPoint = highestPoint;
		if (Application.isPlaying)
		{
			pcGenerated = pComet;
		}
		return pComet;
	}

	[ContextMenu("Rebuild")]
	public void Rebuild()
	{
		if (!(pcGenerated == null) && Application.isPlaying)
		{
			UnityEngine.Object.Destroy(pcGenerated.gameObject);
			Generate(seed, radius, base.transform.parent, base.RangefinderGeneric, delegate
			{
			}, optimizeCollider: false);
		}
	}
}
