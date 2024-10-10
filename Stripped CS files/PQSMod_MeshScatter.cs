using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[AddComponentMenu("PQuadSphere/Mods/Terrain/Scatter")]
public class PQSMod_MeshScatter : PQSMod
{
	public string scatterName;

	public int seed;

	public int maxCache;

	public int maxScatter;

	public Material material;

	public Texture2D scatterMap;

	public Color baseColor;

	public Mesh baseMesh;

	public int minSubdivision;

	public float countPerSqM;

	public float verticalOffset;

	public Vector3 minScale;

	public Vector3 maxScale;

	public bool castShadows;

	public bool recieveShadows;

	[HideInInspector]
	public int vertStride;

	[HideInInspector]
	public int triStride;

	[HideInInspector]
	public int maxCacheCount;

	[HideInInspector]
	public GameObject scatterParent;

	[HideInInspector]
	public Vector3[] vertsUntrans;

	[HideInInspector]
	public Vector3[] normUntrans;

	[HideInInspector]
	public Vector2[] uvUntrans;

	[HideInInspector]
	public int[] trisUntrans;

	[HideInInspector]
	public Vector3[] verts;

	[HideInInspector]
	public Vector3[] normals;

	[HideInInspector]
	public List<MeshFilter> cacheUnassigned;

	[HideInInspector]
	public int cacheUnassignedCount;

	[HideInInspector]
	public List<MeshFilter> cacheAssigned;

	[HideInInspector]
	public int cacheAssignedCount;

	public Vector3 scatterPos;

	public int rndCount;

	public PQSMod_MeshScatter_QuadControl qc;

	public double countFactor;

	public Vector3 scatterUp;

	public Vector3 scatterScale;

	public Quaternion scatterRot;

	public float scatterAngle;

	public int scatterLoop;

	public void Reset()
	{
		seed = UnityEngine.Random.Range(0, 1073741823);
		base.name = "Unnamed";
		baseColor = new Color(0.5f, 0.5f, 0.5f);
		countPerSqM = 0.01f;
		minSubdivision = 11;
		minScale = Vector3.one;
		maxScale = Vector3.one;
		maxCache = 256;
	}

	public override void OnSetup()
	{
		if (baseMesh == null)
		{
			BuildCacheQuad();
		}
		else
		{
			BuildCacheMesh(baseMesh.vertices, baseMesh.uv, baseMesh.normals, baseMesh.triangles);
		}
		BuildCache();
	}

	public override void OnQuadBuilt(GClass3 quad)
	{
		if (quad.subdivision >= minSubdivision)
		{
			AddScatterMeshController(quad);
		}
	}

	public void AddScatterMeshController(GClass3 quad)
	{
		int num = seed + ((int)(quad.positionPlanetRelative.x * 1023.0) << 20) + ((int)(quad.positionPlanetRelative.y * 1023.0) << 10) + (int)(quad.positionPlanetRelative.z * 1023.0);
		if (scatterMap != null)
		{
			countFactor = sphere.BilinearInterpFloatMap(quad.positionPlanetRelative, scatterMap);
			if (countFactor < 0.1)
			{
				return;
			}
			rndCount = (int)Math.Floor(countFactor * quad.quadArea * (double)UnityEngine.Random.Range(0f, countPerSqM));
		}
		else
		{
			rndCount = (int)Math.Floor(quad.quadArea * (double)UnityEngine.Random.Range(0f, countPerSqM));
		}
		if (rndCount != 0)
		{
			qc = quad.gameObject.AddComponent<PQSMod_MeshScatter_QuadControl>();
			qc.Setup(quad, num, this, rndCount);
		}
	}

	public void BuildCacheMesh(Vector3[] vertBase, Vector2[] uvBase, Vector3[] normBase, int[] triBase)
	{
		vertStride = vertBase.Length;
		triStride = triBase.Length;
		maxCacheCount = 16000 / vertStride;
		vertsUntrans = new Vector3[vertStride];
		vertBase.CopyTo(vertsUntrans, 0);
		normUntrans = new Vector3[vertStride];
		normBase.CopyTo(normUntrans, 0);
		uvUntrans = new Vector2[vertStride];
		uvBase.CopyTo(uvUntrans, 0);
		trisUntrans = new int[triStride];
		triBase.CopyTo(trisUntrans, 0);
	}

	public void BuildCacheQuad()
	{
		Vector3[] vertBase = new Vector3[4]
		{
			Vector3.zero,
			Vector3.right,
			Vector3.up,
			new Vector3(1f, 1f, 0f)
		};
		Vector2[] uvBase = new Vector2[4]
		{
			Vector2.zero,
			Vector2.right,
			Vector2.up,
			Vector2.one
		};
		Vector3[] array = new Vector3[4];
		array[0] = Vector3.back;
		array[1] = Vector3.back;
		array[2] = Vector3.back;
		array[3] = new Vector3(1f, 1f, 0f);
		BuildCacheMesh(vertBase, uvBase, array, new int[6] { 0, 1, 2, 2, 1, 3 });
	}

	public void BuildCache()
	{
		int num = 0;
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		List<Vector2> list3 = new List<Vector2>();
		List<int> list4 = new List<int>();
		for (int i = 0; i < maxScatter; i++)
		{
			int num2 = -1;
			int num3 = -1;
			while (num3 == num2)
			{
				int num4 = UnityEngine.Random.Range(1, GClass4.cacheRes + 1);
				int num5 = UnityEngine.Random.Range(1, GClass4.cacheRes + 1);
				int x = num4 + UnityEngine.Random.Range(-1, 1);
				int z = num5 + UnityEngine.Random.Range(-1, 1);
				num3 = GClass4.vi(num4, num5);
				num2 = GClass4.vi(x, z);
			}
			scatterPos = Vector3.Lerp(GClass4.cacheVerts[num3], GClass4.cacheVerts[num2], UnityEngine.Random.value);
			scatterUp = scatterPos.normalized;
			scatterPos.y += verticalOffset;
			scatterAngle = UnityEngine.Random.value * 360f;
			scatterRot = Quaternion.AngleAxis(scatterAngle, Vector3.up);
			scatterScale = RandomRange(minScale, maxScale);
			for (int j = 0; j < vertStride; j++)
			{
				Vector3 vector = vertsUntrans[j];
				vector.Scale(scatterScale);
				list.Add(scatterPos + scatterRot * vector);
				list2.Add(scatterRot * normUntrans[j]);
			}
			list3.AddRange(uvUntrans);
			for (int j = 0; j < triStride; j++)
			{
				list4.Add(trisUntrans[j] + num);
			}
			num += vertStride;
		}
		verts = list.ToArray();
		normals = list2.ToArray();
		Mesh mesh = new Mesh();
		mesh.vertices = verts;
		mesh.normals = normals;
		mesh.uv = list3.ToArray();
		mesh.triangles = list4.ToArray();
		mesh.RecalculateBounds();
		cacheUnassigned = new List<MeshFilter>(maxCache);
		cacheAssigned = new List<MeshFilter>(maxCache);
		if (scatterParent == null)
		{
			scatterParent = new GameObject();
			scatterParent.name = "Scatter " + scatterName;
			scatterParent.transform.parent = base.transform;
			scatterParent.transform.localPosition = Vector3.zero;
			scatterParent.transform.localRotation = Quaternion.identity;
			scatterParent.transform.localScale = Vector3.one;
		}
		for (int k = 0; k < maxCache; k++)
		{
			GameObject obj = new GameObject();
			obj.name = "Unass";
			obj.transform.parent = scatterParent.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
			obj.transform.localScale = Vector3.one;
			MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
			meshFilter.mesh = mesh;
			MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = material;
			if (castShadows)
			{
				meshRenderer.shadowCastingMode = ShadowCastingMode.On;
			}
			else
			{
				meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			}
			meshRenderer.receiveShadows = recieveShadows;
			meshFilter.gameObject.SetActive(value: false);
			cacheUnassigned.Add(meshFilter);
			cacheUnassignedCount++;
		}
	}

	public MeshFilter AssignScatterMesh(GClass3 quad, int seed, int count)
	{
		if (cacheUnassignedCount == 0)
		{
			return null;
		}
		int num = 0;
		UnityEngine.Random.InitState(seed);
		for (scatterLoop = 0; scatterLoop < maxScatter; scatterLoop++)
		{
			int num2 = -1;
			int num3 = -1;
			while (num3 == num2)
			{
				int num4 = UnityEngine.Random.Range(1, GClass4.cacheRes + 1);
				int num5 = UnityEngine.Random.Range(1, GClass4.cacheRes + 1);
				int x = num4 + UnityEngine.Random.Range(-1, 1);
				int z = num5 + UnityEngine.Random.Range(-1, 1);
				num3 = GClass4.vi(num4, num5);
				num2 = GClass4.vi(x, z);
			}
			scatterPos = Vector3.Lerp(quad.verts[num3], quad.verts[num2], UnityEngine.Random.value);
			scatterUp = scatterPos.normalized;
			scatterPos += scatterUp * verticalOffset;
			scatterAngle = UnityEngine.Random.value * 360f;
			scatterRot = Quaternion.AngleAxis(scatterAngle, scatterUp) * (Quaternion)quad.quadRotation;
			scatterScale = RandomRange(minScale, maxScale);
			int num6 = 0;
			while (num6 < vertStride)
			{
				verts[num] = scatterPos + scatterRot * Vector3.Scale(vertsUntrans[num6], scatterScale);
				normals[num] = scatterRot * normUntrans[num6];
				num6++;
				num++;
			}
		}
		MeshFilter meshFilter = cacheUnassigned[0];
		meshFilter.gameObject.name = "Ass" + cacheAssignedCount;
		meshFilter.mesh.vertices = verts;
		meshFilter.mesh.normals = normals;
		meshFilter.mesh.RecalculateBounds();
		meshFilter.gameObject.SetActive(value: true);
		cacheUnassigned.RemoveAt(0);
		cacheUnassignedCount--;
		cacheAssigned.Add(meshFilter);
		cacheAssignedCount++;
		return meshFilter;
	}

	public void UnassignScatterMesh(MeshFilter mf)
	{
		mf.gameObject.name = "Unass";
		mf.gameObject.SetActive(value: false);
		cacheUnassigned.Add(mf);
		cacheUnassignedCount++;
		cacheAssigned.Remove(mf);
		cacheAssignedCount--;
	}

	public static Vector3 RandomRange(Vector3 min, Vector3 max)
	{
		return new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
	}
}
