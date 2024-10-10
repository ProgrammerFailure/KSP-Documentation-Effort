using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Tools/Globe Display")]
public class PQSGlobeDisplay : MonoBehaviour
{
	public Color color = Color.yellow;

	public float altitude = 100f;

	public int resolution = 360;

	public int latitudeLines = 36;

	public int longitudeLines = 12;

	public GClass4 pqs;

	public Mesh globeMesh;

	public Material globeMaterial;

	public void Reset()
	{
		color = Color.yellow;
		altitude = 100f;
		resolution = 360;
		latitudeLines = 36;
		longitudeLines = 18;
	}

	public IEnumerator Start()
	{
		pqs = (GClass4)UnityEngine.Object.FindObjectOfType(typeof(GClass4));
		if (pqs == null)
		{
			Debug.LogError("PQSGlobeDisplay: No PQS in scene!");
			yield break;
		}
		while (!pqs.isActive)
		{
			yield return null;
		}
		CreateGlobe();
	}

	public void OnDestroy()
	{
		if (globeMesh != null)
		{
			UnityEngine.Object.Destroy(globeMesh);
		}
		if (globeMaterial != null)
		{
			UnityEngine.Object.Destroy(globeMaterial);
		}
	}

	public void CreateGlobe()
	{
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		CreateLatitudeLines(list, list2);
		CreateLongitudeLines(list, list2);
		globeMesh = new Mesh();
		globeMesh.vertices = list.ToArray();
		globeMesh.SetIndices(list2.ToArray(), MeshTopology.Lines, 0);
		globeMesh.RecalculateBounds();
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		if (meshFilter == null)
		{
			meshFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		meshFilter.mesh = globeMesh;
		if (globeMaterial == null)
		{
			globeMaterial = new Material(Shader.Find("Unlit"));
		}
		globeMaterial.color = color;
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
		if (meshRenderer == null)
		{
			meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
		}
		meshRenderer.material = globeMaterial;
	}

	public void CreateLatitudeLines(List<Vector3> verts, List<int> points)
	{
		float num = 360f / (float)latitudeLines;
		for (float num2 = 0f; num2 < 360f; num2 += num)
		{
			int count = verts.Count;
			verts.AddRange(CreateLatitudeLine(num2));
			int num3 = verts.Count - 1;
			for (int i = count; i < num3; i++)
			{
				points.Add(i);
				points.Add(i + 1);
			}
		}
	}

	public List<Vector3> CreateLatitudeLine(float latitude)
	{
		List<Vector3> list = new List<Vector3>();
		Quaternion quaternion = Quaternion.AngleAxis(latitude, Vector3.up);
		float num = (float)Math.PI / (float)resolution;
		for (float num2 = 0f; num2 <= (float)Math.PI; num2 += num)
		{
			Vector3 vector = quaternion * new Vector3(Mathf.Sin(num2), Mathf.Cos(num2), 0f);
			float num3 = (float)pqs.GetSurfaceHeight(vector) + altitude;
			list.Add(vector * num3);
		}
		return list;
	}

	public void CreateLongitudeLines(List<Vector3> verts, List<int> points)
	{
		float num = 180f / (float)longitudeLines;
		for (float num2 = 0f; num2 < 180f; num2 += num)
		{
			int count = verts.Count;
			verts.AddRange(CreateLongitudeCircle(num2));
			int num3 = verts.Count - 1;
			for (int i = count; i < num3; i++)
			{
				points.Add(i);
				points.Add(i + 1);
			}
		}
	}

	public List<Vector3> CreateLongitudeCircle(float longitude)
	{
		List<Vector3> list = new List<Vector3>();
		float num = (float)Math.PI / (float)resolution;
		Vector3 vector = new Vector3(Mathf.Sin(longitude * ((float)Math.PI / 180f)), Mathf.Cos(longitude * ((float)Math.PI / 180f)), 0f);
		for (float num2 = 0f; num2 <= (float)Math.PI; num2 += num)
		{
			vector = Quaternion.AngleAxis(num2, Vector3.up) * vector;
			float num3 = (float)pqs.GetSurfaceHeight(vector) + altitude;
			list.Add(vector * num3);
		}
		return list;
	}
}
