using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PQSMeshPlanet : MonoBehaviour
{
	public class Tri
	{
		public int subdivision;

		public int a;

		public int b;

		public int c;

		public Tri ab;

		public Tri bc;

		public Tri ca;

		public Tri parentTri;

		public Tri dab;

		public Tri dca;

		public Vector3 midPoint;

		public double offset;

		public Tri()
		{
		}

		public Tri(int a, int b, int c)
		{
			this.a = a;
			this.b = b;
			this.c = c;
		}

		public Tri(int a, int b, int c, Tri ab, Tri bc, Tri ca)
		{
			this.a = a;
			this.b = b;
			this.c = c;
			this.ab = ab;
			this.bc = bc;
			this.ca = ca;
		}

		public void SetVerts(int a, int b, int c)
		{
			this.a = a;
			this.b = b;
			this.c = c;
		}

		public void SetEdges(Tri ab, Tri bc, Tri ca)
		{
			this.ab = ab;
			this.bc = bc;
			this.ca = ca;
		}

		public void ReplaceEdge(Tri oldTri, Tri newTri)
		{
			if (ab == oldTri)
			{
				ab = newTri;
			}
			else if (bc == oldTri)
			{
				bc = newTri;
			}
			else if (ca == oldTri)
			{
				ca = newTri;
			}
		}

		public void AddTriangle(List<int> triList)
		{
			if (dab != null)
			{
				dab.AddTriangle(triList);
				dca.AddTriangle(triList);
			}
			else
			{
				triList.Add(a);
				triList.Add(b);
				triList.Add(c);
			}
		}

		public bool HasVertex(int v)
		{
			if (a != v && b != v && c != v)
			{
				return false;
			}
			return true;
		}
	}

	public class TriList : LinkedList<Tri>
	{
	}

	public class VertList : IEnumerable<Vector3>, IEnumerable
	{
		public List<Vector3> verts;

		public List<bool> assigned;

		public List<Vector3d> doubleVerts;

		public int index;

		public int total;

		public int count;

		public int Count => count;

		public Vector3 this[int index]
		{
			get
			{
				return verts[index];
			}
			set
			{
				verts[index] = value;
			}
		}

		public VertList(int length)
		{
			verts = new List<Vector3>(length);
			assigned = new List<bool>(length);
			doubleVerts = new List<Vector3d>();
			for (int i = 0; i < length; i++)
			{
				verts.Add(Vector3.zero);
				assigned.Add(item: false);
				doubleVerts.Add(Vector3d.zero);
			}
			index = 0;
			total = length;
			count = 0;
		}

		public int GetVertex(Vector3 newVert)
		{
			if (count == total)
			{
				Debug.Log("End of vert array! " + count);
				return -1;
			}
			if (!assigned[index])
			{
				assigned[index] = true;
				verts[index] = newVert;
				count++;
				return index++;
			}
			int num = index;
			do
			{
				if (++index != num)
				{
					if (index >= total)
					{
						index = 0;
					}
					continue;
				}
				return -1;
			}
			while (assigned[index]);
			assigned[index] = true;
			verts[index] = newVert;
			count++;
			return index++;
		}

		public void RemoveVertex(int index)
		{
			if (assigned[index])
			{
				assigned[index] = false;
				count--;
			}
		}

		public Vector3d GetDoubleVert(int index)
		{
			return doubleVerts[index];
		}

		public void SetDoubleVert(int index, Vector3d doubleVert)
		{
			doubleVerts[index] = doubleVert;
		}

		public IEnumerator<Vector3> GetEnumerator()
		{
			for (int i = 0; i < total; i++)
			{
				if (assigned[i])
				{
					yield return verts[i];
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			for (int i = 0; i < total; i++)
			{
				if (assigned[i])
				{
					yield return verts[i];
				}
			}
		}
	}

	public int maxSubdivision;

	public int minSubdivision;

	public float radius;

	public GClass4 targetPQS;

	public TriList triList;

	public VertList vertList;

	public Vector3d radial;

	public double pqsHeight;

	public double vertHeight;

	public float heightOffset;

	public double pqsRadius;

	public void Reset()
	{
		maxSubdivision = 0;
		radius = 1f;
	}

	public void Awake()
	{
		vertList = new VertList(65535);
		triList = new TriList();
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		CreateBox();
		Subdivide(maxSubdivision);
		CreateMeshObject();
		Debug.Log("Creation time: " + (Time.realtimeSinceStartup - realtimeSinceStartup).ToString("F3") + "s");
	}

	public void Subdivide(int level)
	{
		LinkedListNode<Tri> linkedListNode = triList.First;
		while (true)
		{
			if (linkedListNode != null)
			{
				Tri value = linkedListNode.Value;
				if (value.dca == null && value.subdivision < level && !Split(value))
				{
					break;
				}
				linkedListNode = linkedListNode.Next;
				continue;
			}
			return;
		}
		Debug.Log("Subdivision cancelled.");
	}

	public void CreateBox()
	{
		int vertex = vertList.GetVertex(new Vector3(-1f, -1f, 1f));
		int vertex2 = vertList.GetVertex(new Vector3(1f, -1f, 1f));
		int vertex3 = vertList.GetVertex(new Vector3(1f, 1f, 1f));
		int vertex4 = vertList.GetVertex(new Vector3(-1f, 1f, 1f));
		int vertex5 = vertList.GetVertex(new Vector3(-1f, -1f, -1f));
		int vertex6 = vertList.GetVertex(new Vector3(1f, -1f, -1f));
		int vertex7 = vertList.GetVertex(new Vector3(1f, 1f, -1f));
		int vertex8 = vertList.GetVertex(new Vector3(-1f, 1f, -1f));
		Tri[] array = new Tri[12]
		{
			new Tri(vertex2, vertex3, vertex),
			new Tri(vertex4, vertex, vertex3),
			new Tri(vertex6, vertex5, vertex7),
			new Tri(vertex8, vertex7, vertex5),
			new Tri(vertex2, vertex6, vertex3),
			new Tri(vertex7, vertex3, vertex6),
			new Tri(vertex, vertex5, vertex2),
			new Tri(vertex6, vertex2, vertex5),
			new Tri(vertex8, vertex5, vertex4),
			new Tri(vertex, vertex4, vertex5),
			new Tri(vertex7, vertex8, vertex3),
			new Tri(vertex4, vertex3, vertex8)
		};
		foreach (Tri value in array)
		{
			triList.AddLast(value);
		}
		SetupLists();
	}

	public void SetupLists()
	{
		foreach (Tri tri in triList)
		{
			foreach (Tri tri2 in triList)
			{
				if (tri != tri2)
				{
					if (tri2.HasVertex(tri.a) && tri2.HasVertex(tri.b))
					{
						tri.ab = tri2;
					}
					else if (tri2.HasVertex(tri.b) && tri2.HasVertex(tri.c))
					{
						tri.bc = tri2;
					}
					else if (tri2.HasVertex(tri.c) && tri2.HasVertex(tri.a))
					{
						tri.ca = tri2;
					}
				}
			}
		}
		int i = 0;
		for (int count = vertList.Count; i < count; i++)
		{
			vertList[i] = vertList[i].normalized * radius;
		}
		foreach (Tri tri3 in triList)
		{
			tri3.midPoint = (vertList[tri3.b] + vertList[tri3.c]) / 2f;
		}
	}

	public bool Split(Tri tri)
	{
		if (tri.bc.bc != tri && !Split(tri.bc))
		{
			return false;
		}
		Tri bc = tri.bc;
		int vertex = vertList.GetVertex(tri.midPoint.normalized * radius);
		if (vertex == -1)
		{
			return false;
		}
		tri.dab = new Tri(vertex, tri.a, tri.b);
		tri.dca = new Tri(vertex, tri.c, tri.a);
		bc.dab = new Tri(vertex, bc.a, bc.b);
		bc.dca = new Tri(vertex, bc.c, bc.a);
		tri.dab.SetEdges(tri.dca, tri.ab, bc.dca);
		tri.dca.SetEdges(bc.dab, tri.ca, tri.dab);
		bc.dab.SetEdges(bc.dca, bc.ab, tri.dca);
		bc.dca.SetEdges(tri.dab, bc.ca, bc.dab);
		tri.ab.ReplaceEdge(tri, tri.dab);
		tri.ca.ReplaceEdge(tri, tri.dca);
		bc.ab.ReplaceEdge(bc, bc.dab);
		bc.ca.ReplaceEdge(bc, bc.dca);
		tri.dab.midPoint = (vertList[tri.a] + vertList[tri.b]) / 2f;
		tri.dca.midPoint = (vertList[tri.c] + vertList[tri.a]) / 2f;
		bc.dab.midPoint = (vertList[bc.a] + vertList[bc.b]) / 2f;
		bc.dca.midPoint = (vertList[bc.c] + vertList[bc.a]) / 2f;
		tri.dab.parentTri = tri;
		tri.dca.parentTri = tri;
		bc.dab.parentTri = bc;
		bc.dca.parentTri = bc;
		int subdivision = tri.subdivision + 1;
		tri.dab.subdivision = subdivision;
		tri.dca.subdivision = subdivision;
		bc.dab.subdivision = subdivision;
		bc.dca.subdivision = subdivision;
		triList.AddLast(tri.dab);
		triList.AddLast(tri.dca);
		triList.AddLast(bc.dab);
		triList.AddLast(bc.dca);
		return true;
	}

	public void CreateMeshObject()
	{
		GameObject obj = new GameObject();
		obj.AddComponent<MeshFilter>().mesh = CreateMesh();
		obj.AddComponent<MeshRenderer>();
	}

	public Mesh CreateMesh()
	{
		Vector3[] array = new Vector3[vertList.Count];
		List<int> list = new List<int>();
		Vector2[] array2 = new Vector2[vertList.Count];
		int num = 0;
		foreach (Vector3 vert in vertList)
		{
			array[num] = vert;
			array2[num] = Vector2.zero;
			num++;
		}
		foreach (Tri tri in triList)
		{
			tri.AddTriangle(list);
		}
		Mesh mesh = new Mesh();
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.triangles = list.ToArray();
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}

	public void CreateIcosahedron()
	{
		vertList.GetVertex(new Vector3(0f, 0f, -0.9510565f));
		vertList.GetVertex(new Vector3(0f, 0.8506508f, -0.42532536f));
		vertList.GetVertex(new Vector3(0.809017f, 0.26286554f, -0.42532536f));
		vertList.GetVertex(new Vector3(0.5f, -0.68819094f, -0.42532536f));
		vertList.GetVertex(new Vector3(-0.5f, -0.68819094f, -0.42532536f));
		vertList.GetVertex(new Vector3(-0.809017f, 0.26286557f, -0.42532536f));
		vertList.GetVertex(new Vector3(0.5f, 0.68819094f, 0.42532536f));
		vertList.GetVertex(new Vector3(0.809017f, -0.26286557f, 0.42532536f));
		vertList.GetVertex(new Vector3(0f, -0.8506508f, 0.42532536f));
		vertList.GetVertex(new Vector3(-0.809017f, -0.26286554f, 0.42532536f));
		vertList.GetVertex(new Vector3(-0.5f, 0.68819094f, 0.42532536f));
		vertList.GetVertex(new Vector3(0f, 0f, 0.9510565f));
		Tri[] array = new Tri[20]
		{
			new Tri(1, 3, 2),
			new Tri(1, 4, 3),
			new Tri(1, 5, 4),
			new Tri(1, 6, 5),
			new Tri(1, 2, 6),
			new Tri(2, 7, 11),
			new Tri(2, 3, 7),
			new Tri(3, 8, 7),
			new Tri(3, 4, 8),
			new Tri(4, 9, 8),
			new Tri(4, 5, 9),
			new Tri(5, 10, 9),
			new Tri(5, 6, 10),
			new Tri(6, 11, 10),
			new Tri(6, 2, 11),
			new Tri(11, 7, 12),
			new Tri(7, 8, 12),
			new Tri(8, 9, 12),
			new Tri(9, 10, 12),
			new Tri(10, 11, 12)
		};
		foreach (Tri tri in array)
		{
			tri.a--;
			tri.b--;
			tri.c--;
			int c = tri.c;
			tri.c = tri.b;
			tri.b = c;
			triList.AddLast(tri);
		}
		SetupLists();
	}

	[ContextMenu("Create")]
	public void CreatePlanetMesh()
	{
		vertList = new VertList(65535);
		triList = new TriList();
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		CreateBox();
		Wrap();
		CreateMeshObject();
		Debug.Log("Creation time: " + (Time.realtimeSinceStartup - realtimeSinceStartup).ToString("F3") + "s");
	}

	public void Wrap()
	{
		pqsRadius = targetPQS.radius;
		int i = 0;
		for (int count = vertList.Count; i < count; i++)
		{
			vertList[i] = vertList[i].normalized * (float)pqsRadius;
		}
		foreach (Tri tri in triList)
		{
			tri.midPoint = (vertList[tri.b] + vertList[tri.c]) / 2f;
		}
		targetPQS.SetupExternalRender();
		LinkedListNode<Tri> linkedListNode = triList.First;
		while (true)
		{
			if (linkedListNode != null)
			{
				Tri value = linkedListNode.Value;
				if (value.dca == null && value.subdivision < maxSubdivision && (value.subdivision < minSubdivision || value.offset > 0.0010000000474974513) && !SplitWrap(value))
				{
					break;
				}
				linkedListNode = linkedListNode.Next;
				continue;
			}
			targetPQS.CloseExternalRender();
			double num = (double)radius / pqsRadius;
			int j = 0;
			for (int count2 = vertList.Count; j < count2; j++)
			{
				vertList[j] = vertList.GetDoubleVert(j) * num;
			}
			return;
		}
		Debug.Log("Subdivision cancelled.");
	}

	public bool SplitWrap(Tri tri)
	{
		Tri bc = tri.bc;
		if (bc.bc != tri && !SplitWrap(bc))
		{
			return false;
		}
		int vertex = vertList.GetVertex(tri.midPoint);
		if (vertex == -1)
		{
			return false;
		}
		radial = tri.midPoint;
		radial.Normalize();
		pqsHeight = targetPQS.GetSurfaceHeight(radial);
		vertHeight = Vector3.SqrMagnitude(tri.midPoint);
		heightOffset = (float)(vertHeight / pqsHeight);
		vertList.SetDoubleVert(vertex, radial * pqsHeight);
		tri.dab = new Tri(vertex, tri.a, tri.b);
		tri.dca = new Tri(vertex, tri.c, tri.a);
		bc.dab = new Tri(vertex, bc.a, bc.b);
		bc.dca = new Tri(vertex, bc.c, bc.a);
		tri.dab.SetEdges(tri.dca, tri.ab, bc.dca);
		tri.dca.SetEdges(bc.dab, tri.ca, tri.dab);
		bc.dab.SetEdges(bc.dca, bc.ab, tri.dca);
		bc.dca.SetEdges(tri.dab, bc.ca, bc.dab);
		tri.ab.ReplaceEdge(tri, tri.dab);
		tri.ca.ReplaceEdge(tri, tri.dca);
		bc.ab.ReplaceEdge(bc, bc.dab);
		bc.ca.ReplaceEdge(bc, bc.dca);
		tri.dab.midPoint = (vertList[tri.a] + vertList[tri.b]) / 2f;
		tri.dca.midPoint = (vertList[tri.c] + vertList[tri.a]) / 2f;
		bc.dab.midPoint = (vertList[bc.a] + vertList[bc.b]) / 2f;
		bc.dca.midPoint = (vertList[bc.c] + vertList[bc.a]) / 2f;
		tri.dab.offset = heightOffset;
		tri.dca.offset = heightOffset;
		bc.dab.offset = heightOffset;
		bc.dca.offset = heightOffset;
		tri.dab.parentTri = tri;
		tri.dca.parentTri = tri;
		bc.dab.parentTri = bc;
		bc.dca.parentTri = bc;
		int subdivision = tri.subdivision + 1;
		tri.dab.subdivision = subdivision;
		tri.dca.subdivision = subdivision;
		bc.dab.subdivision = subdivision;
		bc.dca.subdivision = subdivision;
		triList.AddLast(tri.dab);
		triList.AddLast(tri.dca);
		triList.AddLast(bc.dab);
		triList.AddLast(bc.dca);
		return true;
	}
}
