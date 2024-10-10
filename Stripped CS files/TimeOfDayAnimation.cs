using System;
using System.Collections.Generic;
using UnityEngine;
using Upgradeables;

public class TimeOfDayAnimation : MonoBehaviour
{
	[Serializable]
	public class MaterialProperty
	{
		public Material material;

		public string propertyName;

		public List<Renderer> instances = new List<Renderer>();

		public int propertyID;

		public MaterialPropertyBlock mpb;

		public bool isDirty { get; set; }

		public void UpdateInstances(Renderer[] renderers)
		{
			if (mpb == null)
			{
				mpb = new MaterialPropertyBlock();
				propertyID = Shader.PropertyToID(propertyName);
			}
			instances.Clear();
			string name = material.name;
			string text = name + " (Instance)";
			int num = renderers.Length;
			while (num-- > 0)
			{
				Material[] sharedMaterials = renderers[num].sharedMaterials;
				int num2 = sharedMaterials.Length;
				while (num2-- > 0)
				{
					if (!(sharedMaterials[num2] == null))
					{
						string name2 = sharedMaterials[num2].name;
						if (name2 == name || name2 == text)
						{
							instances.Add(renderers[num]);
							break;
						}
					}
				}
			}
			isDirty = false;
		}

		public void SetColor(Color c)
		{
			mpb.SetColor(propertyID, c);
			int count = instances.Count;
			while (count-- > 0)
			{
				if (instances[count] != null)
				{
					instances[count].SetPropertyBlock(mpb);
				}
				else
				{
					isDirty = true;
				}
			}
		}
	}

	public Transform target;

	public float dot;

	public Color emissiveColor;

	public Color emissiveTgtColor;

	public string emissiveColorProperty;

	public AnimationCurve emissivesCurve;

	public List<MaterialProperty> emissives;

	public Renderer[] renderers;

	public bool facilityDirty = true;

	public void Start()
	{
		if (target == null && Sun.Instance != null)
		{
			target = Sun.Instance.transform;
		}
		GameEvents.OnUpgradeableObjLevelChange.Add(onFacilityEvent);
		GameEvents.OnKSCFacilityUpgraded.Add(onFacilityEvent);
		GameEvents.OnKSCStructureCollapsing.Add(onDestructibleEvent);
		GameEvents.OnKSCStructureRepaired.Add(onDestructibleEvent);
		facilityDirty = true;
	}

	public void OnDestroy()
	{
		GameEvents.OnUpgradeableObjLevelChange.Remove(onFacilityEvent);
		GameEvents.OnKSCFacilityUpgraded.Remove(onFacilityEvent);
		GameEvents.OnKSCStructureCollapsing.Remove(onDestructibleEvent);
		GameEvents.OnKSCStructureRepaired.Remove(onDestructibleEvent);
	}

	public void onFacilityEvent(UpgradeableObject upObj, int lvl)
	{
		facilityDirty = true;
	}

	public void onDestructibleEvent(DestructibleBuilding dB)
	{
		facilityDirty = true;
	}

	public void LateUpdate()
	{
		if (target != null)
		{
			dot = Vector3.Dot(target.forward, base.transform.up);
			dot = 1f - (dot + 1f) * 0.5f;
		}
		if (facilityDirty || MaterialPropertiesDirty())
		{
			UpdateRenderers();
			facilityDirty = false;
		}
		emissiveColor = emissiveTgtColor.smethod_0(emissivesCurve.Evaluate(dot));
		int count = emissives.Count;
		while (count-- > 0)
		{
			emissives[count].SetColor(emissiveColor);
		}
	}

	public void UpdateRenderers()
	{
		renderers = GetComponentsInChildren<Renderer>(includeInactive: true);
		int count = emissives.Count;
		while (count-- > 0)
		{
			emissives[count].UpdateInstances(renderers);
		}
	}

	public bool MaterialPropertiesDirty()
	{
		int count = emissives.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!emissives[count].isDirty);
		return true;
	}
}
