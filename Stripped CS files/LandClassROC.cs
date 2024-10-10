using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LandClassROC
{
	public ROCDefinition rocType;

	public string rocName;

	public int gameSeed;

	public int quadSeed;

	public int maxCache;

	public float cacheMaxSubdivMultiplier;

	public float rocFrequency;

	public bool castShadows;

	public bool recieveShadows;

	public int maxROCs;

	public int maxROCsMultiplier;

	public GameObject rocParent;

	public GameObject rocObject;

	[HideInInspector]
	public Stack<PQSMod_ROCScatterQuad> cacheUnassigned;

	[HideInInspector]
	public List<PQSMod_ROCScatterQuad> cacheAssigned;

	public int cacheUnassignedCount;

	public int cacheAssignedCount;

	public GClass4 sphere;

	public Vector3 rocPOS;

	public Vector3 quadNormal;

	public int rndCount;

	public PQSMod_ROCScatterQuad qc;

	public Vector3 rocUp;

	public Quaternion rocRot;

	public float rocAngle;

	public int rocLoop;

	public int rocN;

	public bool cacheCreated;

	public bool cacheRecreated;

	public PQSROCControl rocControl;

	public CelestialBody celestialBody;

	public int minLevel { get; set; }

	public LandClassROC(ROCDefinition rocDefinition, PQSROCControl rocControl)
	{
		rocType = rocDefinition;
		rocName = rocType.type;
		this.rocControl = rocControl;
		for (int i = 0; i < PSystemManager.Instance.localBodies.Count; i++)
		{
			if (PSystemManager.Instance.localBodies[i].name == rocControl.currentCBName)
			{
				celestialBody = PSystemManager.Instance.localBodies[i];
			}
		}
		cacheMaxSubdivMultiplier = 20f;
		maxROCsMultiplier = 5;
		rocFrequency = rocDefinition.frequency;
		castShadows = rocDefinition.castShadows;
		recieveShadows = rocDefinition.receiveShadows;
		if ((bool)ROCManager.Instance)
		{
			rocObject = null;
			ROCManager.Instance.RocTypeObjects.TryGetValue(rocType.type, out rocObject);
		}
	}

	public void Setup(GClass4 sphere)
	{
		this.sphere = sphere;
		minLevel = sphere.maxLevel;
		maxCache = (int)(((rocFrequency < 1f) ? 1f : rocFrequency) * (float)sphere.maxLevel * cacheMaxSubdivMultiplier);
		maxROCs = maxROCsMultiplier;
		CreateCache();
	}

	public void CreateCache()
	{
		if (!cacheCreated)
		{
			cacheAssigned = new List<PQSMod_ROCScatterQuad>(maxCache);
			cacheAssignedCount = 0;
			cacheUnassigned = new Stack<PQSMod_ROCScatterQuad>(maxCache);
			cacheUnassignedCount = 0;
			cacheCreated = true;
		}
	}

	public void SphereActive()
	{
		if (GClass4.Global_AllowScatter && cacheUnassignedCount == 0)
		{
			BuildCache(maxCache);
		}
	}

	public void SphereInactive()
	{
		ClearCache();
	}

	public void AddScatterMeshController(GClass3 quad, int rocIDCounter)
	{
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && ResourceScenario.Instance != null)
		{
			HighLogic.CurrentGame.ROCSeed = ResourceScenario.Instance.gameSettings.ROCMissionSeed;
			gameSeed = ResourceScenario.Instance.gameSettings.ROCMissionSeed;
		}
		else if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.ROCSeed > -1)
		{
			gameSeed = HighLogic.CurrentGame.ROCSeed;
		}
		else
		{
			gameSeed = 0;
		}
		quadSeed = gameSeed + quad.gameObject.name.GetHashCode_Net35() + rocName.GetHashCode_Net35();
		UnityEngine.Random.InitState(quadSeed);
		float num = rocFrequency * rocControl.quadArea;
		float num2 = Mathf.Round(1f / num);
		rocN = (int)num;
		int num3 = UnityEngine.Random.Range(1, (int)num2);
		if (num < 1f && num > 0f && num3 == 1)
		{
			rocN = 1;
		}
		maxROCs = rocN * maxROCsMultiplier;
		if (rocN >= 1)
		{
			if (!cacheRecreated)
			{
				cacheCreated = false;
				CreateCache();
				cacheRecreated = true;
			}
			if (cacheUnassignedCount == 0)
			{
				BuildCache(maxCache);
			}
			qc = cacheUnassigned.Pop();
			cacheAssigned.Add(qc);
			cacheUnassignedCount--;
			cacheAssignedCount++;
			if (qc != null)
			{
				qc.Setup(quad, quadSeed, this, rocN, rocIDCounter);
			}
		}
	}

	public void BuildCache(int countToAdd)
	{
		if (rocParent == null)
		{
			rocParent = new GameObject();
			rocParent.name = "ROC " + rocName;
			rocParent.transform.parent = sphere.transform;
			rocParent.transform.localPosition = Vector3.zero;
			rocParent.transform.localRotation = Quaternion.identity;
			rocParent.transform.localScale = Vector3.one;
		}
		for (int i = 0; i < countToAdd; i++)
		{
			GameObject gameObject = new GameObject();
			PQSMod_ROCScatterQuad pQSMod_ROCScatterQuad = gameObject.AddComponent<PQSMod_ROCScatterQuad>();
			pQSMod_ROCScatterQuad.rocObj = gameObject;
			gameObject.layer = 15;
			gameObject.name = "Unassigned";
			gameObject.transform.parent = rocParent.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			pQSMod_ROCScatterQuad.rocObj.SetActive(value: false);
			cacheUnassigned.Push(pQSMod_ROCScatterQuad);
		}
		cacheUnassignedCount += countToAdd;
	}

	public void DestroyQuad(PQSMod_ROCScatterQuad q)
	{
		if (rocControl != null && rocControl.rocPositionsUsed.ContainsKey(q.quad.gameObject.name.GetHashCode_Net35()))
		{
			List<PQSROCControl.RocPositionInfo> list = rocControl.rocPositionsUsed[q.quad.gameObject.name.GetHashCode_Net35()];
			int count = list.Count;
			while (count-- > 0)
			{
				if (list[count].rocType == rocName)
				{
					list.RemoveAt(count);
				}
			}
		}
		q.Destroy();
		if (cacheAssigned.Contains(q))
		{
			cacheAssigned.Remove(q);
			cacheUnassigned.Push(q);
			cacheAssignedCount--;
			cacheUnassignedCount++;
		}
		q.gameObject.name = "Unassigned";
		q.gameObject.SetActive(value: false);
	}

	public void CreateScatterMesh(PQSMod_ROCScatterQuad q)
	{
		int num = 0;
		UnityEngine.Random.InitState(q.quadSeed);
		List<PQSROCControl.RocPositionInfo> list = null;
		bool flag = false;
		if (rocControl != null)
		{
			if (rocControl.rocPositionsUsed.ContainsKey(q.quad.gameObject.name.GetHashCode_Net35()))
			{
				list = rocControl.rocPositionsUsed[q.quad.gameObject.name.GetHashCode_Net35()];
			}
			else
			{
				list = new List<PQSROCControl.RocPositionInfo>();
				rocControl.rocPositionsUsed.Add(q.quad.gameObject.name.GetHashCode_Net35(), list);
			}
			for (rocLoop = 0; rocLoop < q.count; rocLoop++)
			{
				int num2 = -1;
				int num3 = -1;
				flag = false;
				Renderer componentInChildren = rocObject.GetComponentInChildren<Renderer>();
				float num4 = q.roc.rocType.depth * componentInChildren.bounds.size.y;
				float num5 = UnityEngine.Random.value * 360f;
				Quaternion rotation = UnityEngine.Random.rotation;
				float num6 = UnityEngine.Random.Range(0.1f, num4);
				while (num3 == num2)
				{
					int num7 = UnityEngine.Random.Range(1, GClass4.cacheRes + 1);
					int num8 = UnityEngine.Random.Range(1, GClass4.cacheRes + 1);
					int x = num7 + UnityEngine.Random.Range(-1, 1);
					int z = num8 + UnityEngine.Random.Range(-1, 1);
					num3 = GClass4.vi(num7, num8);
					num2 = GClass4.vi(x, z);
				}
				if (UnityEngine.Random.Range(0, 1) == 0)
				{
					rocPOS = q.quad.verts[num3];
					quadNormal = q.quad.vertNormals[num3];
				}
				else
				{
					rocPOS = q.quad.verts[num2];
					quadNormal = q.quad.vertNormals[num2];
				}
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].position.x.Equals(Mathf.Round(rocPOS.x)) && list[i].position.y.Equals(Mathf.Round(rocPOS.y)) && list[i].position.z.Equals(Mathf.Round(rocPOS.z)))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					PQSROCControl.RocPositionInfo rocPositionInfo = new PQSROCControl.RocPositionInfo();
					rocPositionInfo.rocType = rocName;
					rocPositionInfo.position = new Vector3(Mathf.Round(rocPOS.x), Mathf.Round(rocPOS.y), Mathf.Round(rocPOS.z));
					list.AddUnique(rocPositionInfo);
					if (sphere.surfaceRelativeQuads)
					{
						rocUp = (rocPOS + q.quad.positionPlanet).normalized;
					}
					else
					{
						rocUp = rocPOS.normalized;
					}
					Quaternion quaternion = q.quad.quadRotation;
					if (!q.roc.rocType.orientateUp)
					{
						quaternion = Quaternion.FromToRotation(Vector3.up, quadNormal);
					}
					if (q.roc.rocType.randomRotation)
					{
						rocRot = rotation * quaternion;
					}
					else if (q.roc.rocType.randomOrientation)
					{
						rocAngle = num5;
						rocRot = quaternion * Quaternion.AngleAxis(rocAngle, Vector3.up);
					}
					else
					{
						rocRot = Quaternion.AngleAxis(0f, rocUp) * quaternion;
					}
					if (componentInChildren != null)
					{
						rocPOS += rocUp * componentInChildren.bounds.extents.y;
						if (!q.roc.rocType.randomDepth)
						{
							rocPOS += -rocUp * num4;
						}
						else
						{
							rocPOS += -rocUp * num6;
						}
					}
					int num9 = gameSeed + rocObject.name.GetHashCode_Net35() + rocLoop + (int)rocPOS.magnitude;
					if (rocObject != null && (bool)ROCManager.Instance && !ROCManager.Instance.ROCRemoved(num9))
					{
						GameObject gameObject = UnityEngine.Object.Instantiate(rocObject);
						gameObject.transform.SetParent(q.gameObject.transform);
						gameObject.transform.localScale = new Vector3(q.roc.rocType.scale, q.roc.rocType.scale, q.roc.rocType.scale);
						gameObject.transform.localRotation = rocRot;
						gameObject.transform.localPosition = rocPOS;
						gameObject.SetActive(value: true);
						GClass0 component = gameObject.GetComponent<GClass0>();
						if (component != null)
						{
							component.rocID = num9;
							component.upDirection = q.transform.TransformDirection(quadNormal);
						}
					}
					if (rocObject != null && (bool)ROCManager.Instance && ROCManager.Instance.ROCRemoved(num9))
					{
						Debug.LogFormat("[LandClassROC]: Skipped ROC {0} as it has been removed from this game.", num9);
					}
					else
					{
						num++;
					}
				}
			}
			rocControl.rocPositionsUsed[q.quad.gameObject.name.GetHashCode_Net35()] = list;
			q.rocObj.SetActive(value: true);
			q.isBuilt = true;
		}
		else
		{
			Debug.LogErrorFormat("[LandClassROC]: Unable to CreateScatterMesh for Quad {0} Roc Type {1} as Roc Control reference is not set!", q.quad.name, rocName);
		}
	}

	public void ClearCache()
	{
		if (cacheAssigned != null)
		{
			List<PQSMod_ROCScatterQuad> list = cacheAssigned;
			for (int num = list.Count - 1; num >= 0; num--)
			{
				PQSMod_ROCScatterQuad q = list[num];
				DestroyQuad(q);
			}
		}
		cacheAssignedCount = 0;
		for (int i = 0; i < cacheUnassignedCount; i++)
		{
			PQSMod_ROCScatterQuad pQSMod_ROCScatterQuad = cacheUnassigned.Pop();
			if (!(pQSMod_ROCScatterQuad != null))
			{
				continue;
			}
			for (int j = 0; j < pQSMod_ROCScatterQuad.transform.childCount; j++)
			{
				Transform child = pQSMod_ROCScatterQuad.transform.GetChild(j);
				if (child != null)
				{
					child.gameObject.DestroyGameObjectImmediate();
				}
			}
			pQSMod_ROCScatterQuad.gameObject.DestroyGameObjectImmediate();
		}
		cacheUnassignedCount = 0;
		cacheCreated = false;
		if (ROCManager.Instance != null)
		{
			ROCManager.Instance.ClearStats();
		}
	}
}
