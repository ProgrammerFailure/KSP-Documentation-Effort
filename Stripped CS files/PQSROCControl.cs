using System;
using System.Collections.Generic;
using Expansions;
using UnityEngine;

public class PQSROCControl : PQSMod
{
	[Serializable]
	public class RocPositionInfo
	{
		public string rocType;

		public Vector3 position;
	}

	public CelestialBody celestialBody;

	public string currentCBName;

	public List<LandClassROC> rocs;

	public float quadArea;

	public DictionaryValueList<int, List<RocPositionInfo>> rocPositionsUsed;

	public int itr;

	public bool rocsActive;

	public int rocMinSubdev;

	public int rocCount;

	public bool allowROCScatter;

	public DictionaryValueList<int, List<ROCDefinition>> quadAppliedROCTypes;

	public List<ROCDefinition> quadROCTypesCache;

	public void Awake()
	{
		modExpansionDisabled = !ExpansionsLoader.IsExpansionInstalled("Serenity");
		rocPositionsUsed = new DictionaryValueList<int, List<RocPositionInfo>>();
		quadAppliedROCTypes = new DictionaryValueList<int, List<ROCDefinition>>();
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.VertexMapCoords | GClass4.ModiferRequirements.MeshColorChannel;
		celestialBody = sphere.transform.GetComponentInParent<CelestialBody>();
		if (!(ROCManager.Instance != null))
		{
			return;
		}
		rocCount = rocs.Count;
		rocMinSubdev = int.MaxValue;
		int i = 0;
		for (int count = rocs.Count; i < count; i++)
		{
			LandClassROC landClassROC = rocs[i];
			landClassROC.Setup(sphere);
			if (landClassROC.minLevel < rocMinSubdev)
			{
				rocMinSubdev = landClassROC.minLevel;
			}
		}
	}

	public override void OnQuadPreBuild(GClass3 quad)
	{
		if (quad.subdivision >= rocMinSubdev && ROCManager.Instance != null && celestialBody != null)
		{
			rocsActive = true;
		}
		else
		{
			rocsActive = false;
		}
	}

	public override void OnVertexBuild(GClass4.VertexBuildData data)
	{
		allowROCScatter = data.allowScatter;
	}

	public override void OnQuadBuilt(GClass3 quad)
	{
		int num = 0;
		if (!rocsActive || rocCount <= 0 || !allowROCScatter)
		{
			return;
		}
		num++;
		Vector2d latitudeAndLongitude = celestialBody.GetLatitudeAndLongitude(quad.quadTransform.position);
		string experimentBiome = ScienceUtil.GetExperimentBiome(celestialBody, latitudeAndLongitude.x, latitudeAndLongitude.y);
		quadROCTypesCache = null;
		Mesh sharedMesh = quad.gameObject.GetComponent<MeshFilter>().sharedMesh;
		if (sharedMesh != null)
		{
			Bounds bounds = sharedMesh.bounds;
			quadArea = bounds.size.x / 1000f * bounds.size.z / 1000f;
		}
		else
		{
			quadArea = (float)(quad.quadArea / 10000000000.0);
			Debug.LogErrorFormat("[PQSROCControl]: Failed to calculate Quad Area. Quad:{0}", quad.name);
		}
		for (itr = 0; itr < rocCount; itr++)
		{
			if (rocs[itr].rocType.ContainsCBBiome(celestialBody.name, experimentBiome))
			{
				rocs[itr].AddScatterMeshController(quad, num);
				if (quadROCTypesCache == null)
				{
					quadROCTypesCache = new List<ROCDefinition>();
				}
				if ((bool)ROCManager.Instance && ROCManager.Instance.debugROCStats)
				{
					quadROCTypesCache.Add(rocs[itr].rocType);
					ROCManager.Instance.AddROCStats_Quad(this, rocs[itr].rocType, quad);
				}
			}
		}
		if (quadROCTypesCache != null && (bool)ROCManager.Instance && ROCManager.Instance.debugROCStats)
		{
			quadAppliedROCTypes.Add(quad.gameObject.name.GetHashCode_Net35(), quadROCTypesCache);
		}
		rocsActive = false;
	}

	public override void OnSphereStarted()
	{
		int i = 0;
		for (int count = rocs.Count; i < count; i++)
		{
			rocs[i].SphereActive();
		}
	}

	public override void OnSphereReset()
	{
		int i = 0;
		for (int count = rocs.Count; i < count; i++)
		{
			rocs[i].SphereInactive();
		}
	}

	public override void OnQuadDestroy(GClass3 quad)
	{
		if ((bool)ROCManager.Instance && ROCManager.Instance.debugROCStats && quadAppliedROCTypes.ContainsKey(quad.gameObject.name.GetHashCode_Net35()))
		{
			for (int i = 0; i < quadAppliedROCTypes[quad.gameObject.name.GetHashCode_Net35()].Count; i++)
			{
				ROCManager.Instance.SubtractROCStats_Quad(this, quadAppliedROCTypes[quad.gameObject.name.GetHashCode_Net35()][i], quad);
			}
			quadAppliedROCTypes.Remove(quad.gameObject.name.GetHashCode_Net35());
		}
	}
}
