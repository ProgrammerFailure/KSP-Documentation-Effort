using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObjectCollider : MonoBehaviour
{
	public enum TestResult
	{
		Include,
		Exclude,
		Ignore
	}

	public class MarchStep
	{
		public CompositeSolid solid;

		public Chunk c;

		public float slopeScore;

		public MarchStep(CompositeSolid solid, Chunk c, float slope)
		{
			this.solid = solid;
			this.c = c;
			slopeScore = slope;
		}
	}

	[Serializable]
	public class CompositeSolid
	{
		[SerializeField]
		public List<Chunk> chunks;

		[SerializeField]
		public Vector3[] verts;

		[SerializeField]
		public Vector3[] normals;

		[SerializeField]
		public int[] tris;

		public Color color;

		public Mesh mesh;

		public MeshCollider collider;

		public CompositeSolid()
		{
			chunks = new List<Chunk>();
		}

		public void AddChunk(Chunk c)
		{
			chunks.AddUnique(c);
		}

		public void RemoveChunk(Chunk c)
		{
			chunks.Remove(c);
		}

		public bool ContainsChunk(Chunk c)
		{
			return chunks.Contains(c);
		}

		public void BuildCompositeMesh(Vector3[] srcVerts, Vector3[] srcNormals, Color c)
		{
			List<int> list = new List<int>();
			if (chunks.Count == 1)
			{
				tris = new int[12];
			}
			else
			{
				tris = new int[chunks.Count * 3 + 3];
			}
			int num = 0;
			int i = 0;
			for (int count = chunks.Count; i < count; i++)
			{
				list.AddUnique(chunks[i].i0);
				list.AddUnique(chunks[i].i1);
				list.AddUnique(chunks[i].i2);
				tris[num] = list.IndexOf(chunks[i].i0);
				tris[num + 1] = list.IndexOf(chunks[i].i1);
				tris[num + 2] = list.IndexOf(chunks[i].i2);
				num += 3;
			}
			if (chunks.Count == 1)
			{
				verts = new Vector3[4]
				{
					srcVerts[list[0]],
					srcVerts[list[1]],
					srcVerts[list[2]],
					chunks[0].srfRadial
				};
				verts[3] *= Mathf.Sqrt(Mathf.Min(Mathf.Min(verts[0].sqrMagnitude, verts[1].sqrMagnitude), verts[2].sqrMagnitude)) * 0.95f;
				tris[num] = 3;
				tris[num + 1] = 0;
				tris[num + 2] = 1;
				num += 3;
				tris[num] = 3;
				tris[num + 1] = 1;
				tris[num + 2] = 2;
				num += 3;
				tris[num] = 3;
				tris[num + 1] = 2;
				tris[num + 2] = 0;
				num += 3;
				normals = new Vector3[4]
				{
					srcNormals[list[0]],
					srcNormals[list[1]],
					srcNormals[list[2]],
					-chunks[0].srfRadial
				};
			}
			else
			{
				verts = new Vector3[list.Count + 1];
				normals = new Vector3[list.Count + 1];
				int j = 0;
				for (int num2 = verts.Length; j < num2 - 1; j++)
				{
					verts[j] = srcVerts[list[j]];
					normals[j] = srcNormals[list[j]];
				}
				float num3 = float.MaxValue;
				int count2 = chunks.Count;
				while (count2-- > 0)
				{
					float sqrMagnitude = chunks[count2].srfCenter.sqrMagnitude;
					if ((float)num < num3)
					{
						num3 = sqrMagnitude;
					}
				}
				verts[verts.Length - 1] = GetSrfRadial() * Mathf.Sqrt(num3) * 0.95f;
				normals[normals.Length - 1] = -GetSrfRadial();
				num = verts.Length - 1;
				tris[num] = 3;
				tris[num + 1] = 0;
				tris[num + 2] = 1;
			}
			color = c;
			if (verts.Length <= 3)
			{
				Debug.LogError("Invalid Solid: " + verts.Length + " defined, but at least 4 are required.");
				return;
			}
			mesh = new Mesh();
			mesh.vertices = verts;
			mesh.normals = normals;
			mesh.triangles = tris;
			mesh.RecalculateBounds();
		}

		public void DrawSolid(Color c, float duration)
		{
			int i = 0;
			for (int count = chunks.Count; i < count; i++)
			{
				Debug.DrawLine(Vector3.Lerp(chunks[i].p0, chunks[i].srfCenter, 0.1f), Vector3.Lerp(chunks[i].p1, chunks[i].srfCenter, 0.1f), c, duration);
				Debug.DrawLine(Vector3.Lerp(chunks[i].p0, chunks[i].srfCenter, 0.1f), Vector3.Lerp(chunks[i].p2, chunks[i].srfCenter, 0.1f), c, duration);
				Debug.DrawLine(Vector3.Lerp(chunks[i].p1, chunks[i].srfCenter, 0.1f), Vector3.Lerp(chunks[i].p2, chunks[i].srfCenter, 0.1f), c, duration);
				if (ContainsChunk(chunks[i].n01))
				{
					Debug.DrawLine(chunks[i].srfCenter, chunks[i].n01.srfCenter, c, duration);
				}
				if (ContainsChunk(chunks[i].n02))
				{
					Debug.DrawLine(chunks[i].srfCenter, chunks[i].n02.srfCenter, c, duration);
				}
				if (ContainsChunk(chunks[i].n12))
				{
					Debug.DrawLine(chunks[i].srfCenter, chunks[i].n12.srfCenter, c, duration);
				}
			}
		}

		public void CreateCollider(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name);
			gameObject.transform.NestToParent(parent);
			if (mesh != null)
			{
				collider = gameObject.AddComponent<MeshCollider>();
				collider.sharedMesh = mesh;
				collider.convex = true;
			}
		}

		public Vector3 GetSrfRadial()
		{
			return Mathfx.GetWeightedAvgVector3(chunks.Count, (int i) => chunks[i].srfRadial, (int i) => 1f);
		}

		public Vector3 GetSrfCenter()
		{
			return Mathfx.GetWeightedAvgVector3(chunks.Count, (int i) => chunks[i].srfCenter, (int i) => 1f);
		}
	}

	[Serializable]
	public class Chunk
	{
		public Vector3 p0;

		public Vector3 p1;

		public Vector3 p2;

		public int i0;

		public int i1;

		public int i2;

		public Vector3 srfCenter;

		public Vector3 srfRadial;

		public Vector3 srfNormal;

		public float radiusSqr;

		public Chunk n01;

		public Chunk n12;

		public Chunk n02;

		[NonSerialized]
		public CompositeSolid solid;

		[NonSerialized]
		public MarchStep step;

		public Chunk(int i0, int i1, int i2, Vector3[] verts, Vector3 origin)
		{
			this.i0 = i0;
			this.i1 = i1;
			this.i2 = i2;
			p0 = verts[i0] - origin;
			p1 = verts[i1] - origin;
			p2 = verts[i2] - origin;
			radiusSqr = Mathf.Max(p0.sqrMagnitude, Mathf.Max(p1.sqrMagnitude, p2.sqrMagnitude));
			srfCenter = (p0 + p1 + p2) / 3f;
			srfRadial = srfCenter.normalized;
			srfNormal = Vector3.Cross(p1 - p0, p2 - p0).normalized;
		}

		public float GetSlope(Chunk t)
		{
			return Vector3.Dot(Vector3.Cross((t.srfCenter - srfCenter).normalized, srfRadial), Vector3.Cross(t.srfNormal, srfNormal));
		}

		public void ClearNeighbours()
		{
			n01 = null;
			n02 = null;
			n12 = null;
		}

		public bool SetNeighbour(int eA, int eB, Chunk n)
		{
			if (eA == i0)
			{
				if (eB == i0)
				{
					return false;
				}
				if (eB == i1)
				{
					n01 = n;
					return true;
				}
				if (eB == i2)
				{
					n02 = n;
					return true;
				}
				return false;
			}
			if (eA == i1)
			{
				if (eB == i0)
				{
					n01 = n;
					return true;
				}
				if (eB == i1)
				{
					return false;
				}
				if (eB == i2)
				{
					n12 = n;
					return true;
				}
				return false;
			}
			if (eA == i2)
			{
				if (eB == i0)
				{
					n02 = n;
					return true;
				}
				if (eB == i1)
				{
					n12 = n;
					return true;
				}
				return false;
			}
			return false;
		}

		public Chunk GetNeighbour(int i)
		{
			return i switch
			{
				0 => n01, 
				1 => n02, 
				2 => n12, 
				_ => throw new IndexOutOfRangeException(i + " is not a valid index. Chunks always have 3 neighbours, at indices 0, 1 and 2."), 
			};
		}

		public Vector3 GetTri(int i)
		{
			return i switch
			{
				0 => p0, 
				1 => p1, 
				2 => p2, 
				_ => Vector3.zero, 
			};
		}

		public int GetTriIndex(int i)
		{
			return i switch
			{
				0 => i0, 
				1 => i1, 
				2 => i2, 
				_ => 0, 
			};
		}

		public void SetTri(Vector3 value, int i)
		{
			switch (i)
			{
			case 0:
				p0 = value;
				break;
			case 1:
				p1 = value;
				break;
			case 2:
				p2 = value;
				break;
			}
		}
	}

	public Mesh refMesh;

	public Vector3 center;

	public Transform trf;

	public Chunk[] chunks;

	[SerializeField]
	public List<CompositeSolid> solids;

	public List<MarchStep> marchList;

	public bool setup;

	[SerializeField]
	public bool clustering = true;

	[SerializeField]
	public bool drawSolids;

	[SerializeField]
	public int marchStepsPerFrame;

	[SerializeField]
	public int meshGensPerFrame;

	[SerializeField]
	public int colliderGensPerFrame;

	public Func<Transform, float> rangefinder;

	public float maxRange;

	public float minRange;

	public float rangeScale;

	public Callback onGenComplete;

	[SerializeField]
	public float minScore;

	public float vertex => i switch
	{
		0 => tri.x, 
		1 => tri.y, 
		2 => tri.z, 
		_ => 0f, 
	};

	public float vertex
	{
		set
		{
			if (i == 0)
			{
				tri.x = value;
			}
			if (i == 1)
			{
				tri.y = value;
			}
			if (i == 2)
			{
				tri.z = value;
			}
		}
	}

	public void Setup(PAsteroid pa, Mesh refMesh, Vector3 center, Func<Transform, float> rangefinder, float maxRange, float minRange, Callback onGenComplete)
	{
		Setup((PSpaceObject)pa, refMesh, center, rangefinder, maxRange, minRange, onGenComplete);
	}

	public void Setup(PSpaceObject pso, Mesh refMesh, Vector3 center, Func<Transform, float> rangefinder, float maxRange, float minRange, Callback onGenComplete)
	{
		this.refMesh = refMesh;
		this.center = center;
		trf = base.transform;
		this.rangefinder = rangefinder;
		this.minRange = Mathf.Max(minRange, 1f);
		this.maxRange = Mathf.Max(maxRange, minRange + 1f);
		this.onGenComplete = onGenComplete;
		Generate();
	}

	[ContextMenu("Clear Colliders")]
	public void Clear()
	{
		if (!setup)
		{
			Debug.LogError("Already Clear!");
			return;
		}
		int count = solids.Count;
		while (count-- > 0)
		{
			UnityEngine.Object.Destroy(solids[count].collider.gameObject);
		}
		solids.Clear();
		setup = false;
	}

	public void UpdateGenRange(float range)
	{
		rangeScale = Mathf.InverseLerp(minRange, maxRange, range);
		if (rangeScale == 0f)
		{
			marchStepsPerFrame = 0;
			meshGensPerFrame = 0;
			colliderGensPerFrame = 0;
		}
		else
		{
			marchStepsPerFrame = (int)Mathf.Lerp(2000f, 10f, rangeScale);
			meshGensPerFrame = (int)Mathf.Lerp(1000f, 1f, rangeScale);
			colliderGensPerFrame = (int)Mathf.Lerp(500f, 1f, rangeScale);
		}
	}

	[ContextMenu("Generate")]
	public void Generate()
	{
		if (setup)
		{
			Debug.LogError("Already generated!");
			return;
		}
		setup = true;
		StartCoroutine(GenCoroutine());
	}

	public IEnumerator GenCoroutine()
	{
		yield return null;
		UpdateGenRange(rangefinder(trf));
		_ = Time.realtimeSinceStartup;
		_ = Time.frameCount;
		chunks = CreateChunks(refMesh.vertices, refMesh.triangles, center);
		LinkChunks(chunks);
		solids = new List<CompositeSolid>();
		solids.Add(new CompositeSolid());
		marchList = new List<MarchStep>();
		chunks[0].step = new MarchStep(solids[0], chunks[0], 0f);
		marchList.Add(chunks[0].step);
		int num = 0;
		while (marchList.Count > 0)
		{
			MarchNext(marchList[0]);
			if (marchStepsPerFrame > 0)
			{
				num++;
				if (num > marchStepsPerFrame)
				{
					yield return null;
					num = 0;
					UpdateGenRange(rangefinder(trf));
				}
			}
		}
		int num2 = 0;
		int count = solids.Count;
		while (count-- > 0)
		{
			if (solids[count].chunks.Count == 0)
			{
				solids.RemoveAt(count);
				num2++;
			}
		}
		Vector3[] srcVerts = refMesh.vertices;
		Vector3[] srcNormals = refMesh.normals;
		num = 0;
		int j = 0;
		int iC2 = solids.Count;
		while (j < iC2)
		{
			solids[j].BuildCompositeMesh(srcVerts, srcNormals, RandomColor(UnityEngine.Random.onUnitSphere));
			if (meshGensPerFrame > 0)
			{
				num++;
				if (num > meshGensPerFrame)
				{
					yield return null;
					num = 0;
					UpdateGenRange(rangefinder(trf));
				}
			}
			int num3 = j + 1;
			j = num3;
		}
		int num4 = 0;
		int count2 = solids.Count;
		while (count2-- > 0)
		{
			if (solids[count2].mesh.vertexCount == 0)
			{
				solids.RemoveAt(count2);
				num4++;
			}
		}
		fix_multi_shared_edges(solids);
		num = 0;
		iC2 = 0;
		j = solids.Count;
		while (iC2 < j)
		{
			solids[iC2].CreateCollider("solid" + iC2, trf);
			if (colliderGensPerFrame > 0)
			{
				num++;
				if (num > colliderGensPerFrame)
				{
					yield return null;
					num = 0;
					UpdateGenRange(rangefinder(trf));
				}
			}
			int num3 = iC2 + 1;
			iC2 = num3;
		}
		onGenComplete();
	}

	public void MarchNext(MarchStep step)
	{
		if (step.c.solid != null)
		{
			step.c.solid.RemoveChunk(step.c);
			step.c.solid = null;
		}
		step.solid.AddChunk(step.c);
		step.c.solid = step.solid;
		MarchEdge(step.c.n01, step);
		MarchEdge(step.c.n02, step);
		MarchEdge(step.c.n12, step);
		marchList.Remove(step);
	}

	public void MarchEdge(Chunk n, MarchStep step)
	{
		float score = 0f;
		switch (TestEdge(step.c, n, step.solid, ref score))
		{
		case TestResult.Include:
			if (!clustering)
			{
				if (n.step == null)
				{
					n.step = new MarchStep(step.solid, n, score);
					marchList.Add(n.step);
				}
			}
			else if (n.step == null)
			{
				n.step = new MarchStep(step.solid, n, score);
				marchList.Insert(0, n.step);
			}
			else if (score > step.slopeScore)
			{
				n.step.slopeScore = score;
				n.step.solid = step.solid;
				marchList.Remove(n.step);
				marchList.Insert(0, n.step);
			}
			break;
		case TestResult.Exclude:
			if (n.step == null)
			{
				CompositeSolid compositeSolid = new CompositeSolid();
				solids.Add(compositeSolid);
				n.step = new MarchStep(compositeSolid, n, 0f);
				marchList.Add(n.step);
			}
			break;
		case TestResult.Ignore:
			break;
		}
	}

	public TestResult TestEdge(Chunk c, Chunk n, CompositeSolid solid, ref float score)
	{
		if (n != c && !solid.ContainsChunk(n))
		{
			float num = -1f;
			if (TestWedge(n.n01, solid))
			{
				num += 1f;
			}
			if (TestWedge(n.n02, solid))
			{
				num += 1f;
			}
			if (TestWedge(n.n12, solid))
			{
				num += 1f;
			}
			score = Mathf.Max(c.GetSlope(n), num);
			if (score > minScore && solid.chunks.Count < 200)
			{
				return TestResult.Include;
			}
			return TestResult.Exclude;
		}
		return TestResult.Ignore;
	}

	public bool TestWedge(Chunk n, CompositeSolid solid)
	{
		if (n.step != null)
		{
			return n.step.solid == solid;
		}
		return false;
	}

	public void LateUpdate()
	{
		if (setup && drawSolids)
		{
			int i = 0;
			for (int count = solids.Count; i < count; i++)
			{
				solids[i].DrawSolid(solids[i].color, 0f);
			}
		}
	}

	public Color RandomColor(Vector3 unitVector, float a = 1f)
	{
		return new Color(unitVector.x, unitVector.y, unitVector.z, a);
	}

	public Chunk[] CreateChunks(Vector3[] verts, int[] triangles, Vector3 center)
	{
		Chunk[] array = new Chunk[triangles.Length / 3];
		int num = 0;
		int i = 0;
		for (int num2 = triangles.Length; i < num2; i += 3)
		{
			array[num] = new Chunk(triangles[i], triangles[i + 1], triangles[i + 2], verts, center);
			num++;
		}
		return array;
	}

	public void LinkChunks(Chunk[] chunks)
	{
		int i = 0;
		for (int num = chunks.Length; i < num; i++)
		{
			int j = 0;
			for (int num2 = chunks.Length; j < num2; j++)
			{
				if (i != j)
				{
					chunks[i].SetNeighbour(chunks[j].i0, chunks[j].i1, chunks[j]);
					chunks[i].SetNeighbour(chunks[j].i1, chunks[j].i2, chunks[j]);
					chunks[i].SetNeighbour(chunks[j].i0, chunks[j].i2, chunks[j]);
				}
			}
		}
	}

	public void fix_multi_shared_edges(List<CompositeSolid> solids)
	{
		int count = solids.Count;
		while (count-- > 0)
		{
			bool flag = false;
			bool flag2 = false;
			if (solids[count].chunks.Count > 2)
			{
				int num = solids[count].tris.Length / 3;
				for (int i = 0; i < solids[count].tris.Length; i += 3)
				{
					if (solids[count].tris[i] == solids[count].tris[i + 1] && solids[count].tris[i] == solids[count].tris[i + 2] && num < 5)
					{
						flag = true;
						break;
					}
				}
				List<Vector3> list = new List<Vector3>();
				for (int j = 0; j < solids[count].tris.Length; j += 3)
				{
					Vector3 item = new Vector3(solids[count].tris[j], solids[count].tris[j + 1], solids[count].tris[j + 2]);
					if (!list.Contains(item))
					{
						list.Add(item);
						continue;
					}
					flag2 = true;
					break;
				}
			}
			else
			{
				flag2 = true;
				flag = true;
			}
			if (flag && flag2)
			{
				int[] tris = solids[count].tris;
				Vector3[] verts = solids[count].verts;
				Vector3[] normals = solids[count].normals;
				solids[count].tris = new int[tris.Length + 3];
				for (int k = 0; k < tris.Length; k++)
				{
					solids[count].tris[k] = tris[k];
				}
				solids[count].verts = new Vector3[verts.Length + 1];
				for (int l = 0; l < verts.Length; l++)
				{
					solids[count].verts[l] = verts[l];
				}
				solids[count].normals = new Vector3[normals.Length + 1];
				for (int m = 0; m < normals.Length; m++)
				{
					solids[count].normals[m] = normals[m];
				}
				solids[count].verts[solids[count].verts.Length - 1] = solids[count].chunks[0].srfRadial * Mathf.Sqrt(Mathf.Min(Mathf.Min(solids[count].verts[0].sqrMagnitude, solids[count].verts[1].sqrMagnitude), solids[count].verts[2].sqrMagnitude)) * 0.95f;
				solids[count].tris[solids[count].tris.Length - 3] = solids[count].verts.Length - 1;
				solids[count].tris[solids[count].tris.Length - 2] = 0;
				solids[count].tris[solids[count].tris.Length - 1] = 1;
				solids[count].normals[solids[count].normals.Length - 1] = -solids[count].GetSrfRadial();
				solids[count].mesh = new Mesh();
				solids[count].mesh.vertices = solids[count].verts;
				solids[count].mesh.normals = solids[count].normals;
				solids[count].mesh.triangles = solids[count].tris;
				solids[count].mesh.RecalculateBounds();
			}
		}
	}

	public void fix_wind_errors(Chunk[] chunks, Vector3[] verts)
	{
		for (int i = 0; i < chunks.Length; i++)
		{
			Vector3 tri = chunks[i].GetTri(0);
			Vector3 vector = chunks[i].GetTri(1);
			Vector3 vector2 = chunks[i].GetTri(2);
			if (share_edge(tri, vector) && winding_conflict(tri, vector))
			{
				vector = flip_winding_order(vector);
				chunks[i].SetTri(vector, 0);
			}
			if (share_edge(tri, vector2) && winding_conflict(tri, vector2))
			{
				vector2 = flip_winding_order(vector2);
				chunks[i].SetTri(vector2, 1);
			}
			if (share_edge(vector, vector2) && winding_conflict(vector, vector2))
			{
				Debug.Log("Problem here");
			}
			for (int j = 0; j < 3; j++)
			{
				Chunk neighbour = chunks[i].GetNeighbour(j);
				if (neighbour != null)
				{
					find_fix_shared_edge(chunks[i], neighbour);
				}
			}
		}
	}

	public void find_fix_shared_edge(Chunk c1, Chunk c2)
	{
		for (int i = 0; i < 3; i++)
		{
			Vector3 tri = c1.GetTri(i);
			for (int j = 0; j < 3; j++)
			{
				Vector3 tri2 = c2.GetTri(j);
				if (share_edge(tri, tri2) && winding_conflict(tri, tri2))
				{
					tri2 = flip_winding_order(tri2);
					c2.SetTri(tri2, j);
				}
			}
		}
	}

	public bool share_edge(Vector3 tl, Vector3 tr)
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				num += ((this.get_vertex(tl, i) == this.get_vertex(tr, j)) ? 1 : 0);
			}
		}
		return num > 1;
	}

	public int shared_edges(Chunk c1, Chunk c2)
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (share_edge(c1.GetTri(i), c2.GetTri(j)))
				{
					num++;
				}
			}
		}
		return num;
	}

	public bool winding_conflict(Vector3 ti, Vector3 tj)
	{
		for (int i = 0; i < 3; i++)
		{
			int i2 = (i + 1) % 3;
			for (int j = 0; j < 3; j++)
			{
				int i3 = (j + 1) % 3;
				if (this.get_vertex(ti, j) == this.get_vertex(tj, i) && this.get_vertex(ti, i3) == this.get_vertex(tj, i2))
				{
					return true;
				}
			}
		}
		return false;
	}

	public Vector3 flip_winding_order(Vector3 t)
	{
		Vector3 tri = default(Vector3);
		this.set_vertex(ref tri, 0, this.get_vertex(t, 1));
		this.set_vertex(ref tri, 1, this.get_vertex(t, 0));
		this.set_vertex(ref tri, 2, this.get_vertex(t, 2));
		return tri;
	}
}
