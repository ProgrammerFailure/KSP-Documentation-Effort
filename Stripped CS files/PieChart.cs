using System;
using System.Collections.Generic;
using UnityEngine;

public class PieChart : MonoBehaviour
{
	public delegate void OnInputDelegate(Slice slice);

	[Serializable]
	public class Slice
	{
		public string name;

		public int id;

		public float fraction;

		public Color color;

		public object Data;

		[HideInInspector]
		public PieChartSlice slice;

		[HideInInspector]
		public Mesh mesh;

		[HideInInspector]
		public Material material;

		[HideInInspector]
		public OnInputDelegate onOver;

		[HideInInspector]
		public OnInputDelegate onExit;

		[HideInInspector]
		public OnInputDelegate onTap;

		public Slice(string name, float fraction, Color color)
		{
			this.name = name;
			this.fraction = fraction;
			this.color = color;
		}

		public Slice(string name, int id, float fraction, Color color)
		{
			this.name = name;
			this.id = id;
			this.fraction = fraction;
			this.color = color;
		}

		public Slice(int id, float fraction, Color color)
		{
			this.id = id;
			this.fraction = fraction;
			this.color = color;
		}

		public void AddOnOver(OnInputDelegate onOver)
		{
			if (this.onOver != null)
			{
				this.onOver = (OnInputDelegate)Delegate.Combine(this.onOver, onOver);
			}
			else
			{
				this.onOver = onOver;
			}
		}

		public void RemoveOnOver(OnInputDelegate onOver)
		{
			if (this.onOver == onOver)
			{
				this.onOver = null;
			}
			else
			{
				this.onOver = (OnInputDelegate)Delegate.Remove(this.onOver, onOver);
			}
		}

		public void AddOnExit(OnInputDelegate onExit)
		{
			if (this.onExit != null)
			{
				this.onExit = (OnInputDelegate)Delegate.Combine(this.onExit, onExit);
			}
			else
			{
				this.onExit = onExit;
			}
		}

		public void RemoveOnExit(OnInputDelegate onExit)
		{
			if (this.onExit == onExit)
			{
				this.onExit = null;
			}
			else
			{
				this.onExit = (OnInputDelegate)Delegate.Remove(this.onExit, onExit);
			}
		}

		public void AddOnTap(OnInputDelegate onTap)
		{
			if (this.onTap != null)
			{
				this.onTap = (OnInputDelegate)Delegate.Combine(this.onTap, onTap);
			}
			else
			{
				this.onTap = onTap;
			}
		}

		public void RemoveOnTap(OnInputDelegate onTap)
		{
			if (this.onTap == onTap)
			{
				this.onTap = null;
			}
			else
			{
				this.onTap = (OnInputDelegate)Delegate.Remove(this.onTap, onTap);
			}
		}
	}

	public Material material;

	public int resolution;

	public float radius;

	public float depth;

	public List<Slice> slices;

	public List<PieChartSlice> sliceObjects;

	public void Reset()
	{
		material = new Material(Shader.Find("Diffuse"));
		resolution = 64;
		radius = 1f;
		depth = 1f;
		slices = new List<Slice>();
		slices.Add(new Slice("Unnamed", 1f, Color.white));
	}

	public void Start()
	{
		UpdateChart();
	}

	public void SetSlices(List<Slice> slices)
	{
		this.slices = slices;
		UpdateChart();
	}

	public void UpdateChart()
	{
		if (sliceObjects == null)
		{
			sliceObjects = new List<PieChartSlice>();
		}
		else
		{
			int count = sliceObjects.Count;
			while (count-- > 0)
			{
				PieChartSlice pieChartSlice = sliceObjects[count];
				if (pieChartSlice.slice != null)
				{
					if (pieChartSlice.slice.mesh != null)
					{
						UnityEngine.Object.Destroy(pieChartSlice.slice.mesh);
					}
					if (pieChartSlice.slice.material != null)
					{
						UnityEngine.Object.Destroy(pieChartSlice.slice.material);
					}
				}
				UnityEngine.Object.Destroy(pieChartSlice.gameObject);
			}
			sliceObjects.Clear();
		}
		float num = (float)Math.PI * 2f;
		float deltaAngle = num / (float)resolution;
		float num2 = 0f;
		float num3 = 0f;
		List<int> list = new List<int>(resolution * 6);
		List<Vector3> list2 = new List<Vector3>(resolution * 4);
		int count2 = slices.Count;
		for (int i = 0; i < count2; i++)
		{
			num2 = ((i <= 0) ? 0f : (num2 + slices[i - 1].fraction * num));
			num3 = ((i >= count2 - 1) ? num : (num2 + slices[i].fraction * num));
			CreateTopBottom(num2, num3, deltaAngle, list2, list);
			CreateEdges(num2, num3, deltaAngle, list2, list);
			CreateEnds(num2, num3, deltaAngle, list2, list);
			Mesh mesh = new Mesh();
			mesh.vertices = list2.ToArray();
			mesh.triangles = list.ToArray();
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			GameObject obj = new GameObject("Slice" + i.ToString("D3"));
			obj.transform.parent = base.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
			obj.transform.localScale = Vector3.one;
			obj.layer = base.gameObject.layer;
			obj.AddComponent<MeshFilter>().sharedMesh = mesh;
			Material sharedMaterial = new Material(material)
			{
				color = slices[i].color
			};
			obj.AddComponent<MeshRenderer>().sharedMaterial = sharedMaterial;
			obj.AddComponent<MeshCollider>().sharedMesh = mesh;
			PieChartSlice pieChartSlice2 = obj.AddComponent<PieChartSlice>();
			pieChartSlice2.slice = slices[i];
			slices[i].slice = pieChartSlice2;
			slices[i].mesh = mesh;
			slices[i].material = sharedMaterial;
			sliceObjects.Add(pieChartSlice2);
			list2.Clear();
			list.Clear();
		}
	}

	public void CreateEnds(float angle, float endAngle, float deltaAngle, List<Vector3> verts, List<int> tris)
	{
		int count = verts.Count;
		verts.Add(Vector3.zero);
		verts.Add(CreateDirection(angle, 0f) * radius);
		verts.Add(CreateDirection(endAngle, 0f) * radius);
		int count2 = verts.Count;
		int num = count2 - count;
		for (int i = count; i < count2; i++)
		{
			verts.Add(new Vector3(verts[i].x, verts[i].y, depth));
		}
		tris.Add(count);
		tris.Add(count + num);
		tris.Add(count + 1);
		tris.Add(count + 1);
		tris.Add(count + num);
		tris.Add(count + num + 1);
		tris.Add(count);
		tris.Add(count + 2);
		tris.Add(count + num + 2);
		tris.Add(count);
		tris.Add(count + num + 2);
		tris.Add(count + num);
	}

	public void CreateTopBottom(float angle, float endAngle, float deltaAngle, List<Vector3> verts, List<int> tris)
	{
		int count = verts.Count;
		verts.Add(Vector3.zero);
		verts.Add(CreateDirection(angle, 0f) * radius);
		while ((angle += deltaAngle) <= endAngle)
		{
			verts.Add(CreateDirection(angle, 0f) * radius);
		}
		if (angle > endAngle)
		{
			verts.Add(CreateDirection(endAngle, 0f) * radius);
		}
		int count2 = verts.Count;
		int num = count2 - count;
		for (int i = count; i < count2; i++)
		{
			verts.Add(new Vector3(verts[i].x, verts[i].y, depth));
		}
		for (int j = count + 1; j < count2 - 1; j++)
		{
			tris.Add(count);
			tris.Add(j);
			tris.Add(j + 1);
			tris.Add(num);
			tris.Add(j + 1 + num);
			tris.Add(j + num);
		}
	}

	public void CreateEdges(float angle, float endAngle, float deltaAngle, List<Vector3> verts, List<int> tris)
	{
		int count = verts.Count;
		verts.Add(CreateDirection(angle, 0f) * radius);
		while ((angle += deltaAngle) <= endAngle)
		{
			verts.Add(CreateDirection(angle, 0f) * radius);
		}
		if (angle > endAngle)
		{
			verts.Add(CreateDirection(endAngle, 0f) * radius);
		}
		int count2 = verts.Count;
		int num = count2 - count;
		for (int i = count; i < count2; i++)
		{
			verts.Add(new Vector3(verts[i].x, verts[i].y, depth));
		}
		for (int j = count; j < count2 - 1; j++)
		{
			tris.Add(j);
			tris.Add(j + num);
			tris.Add(j + 1);
			tris.Add(j + 1);
			tris.Add(j + num);
			tris.Add(j + num + 1);
		}
	}

	public static Vector3 CreateDirection(float angle, float height)
	{
		return new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), height);
	}
}
