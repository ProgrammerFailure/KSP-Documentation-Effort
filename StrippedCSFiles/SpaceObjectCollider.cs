using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MarchStep(CompositeSolid solid, Chunk c, float slope)
		{
			throw null;
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CompositeSolid()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddChunk(Chunk c)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RemoveChunk(Chunk c)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool ContainsChunk(Chunk c)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void BuildCompositeMesh(Vector3[] srcVerts, Vector3[] srcNormals, Color c)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void DrawSolid(Color c, float duration)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void CreateCollider(string name, Transform parent)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3 GetSrfRadial()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3 GetSrfCenter()
		{
			throw null;
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Chunk(int i0, int i1, int i2, Vector3[] verts, Vector3 origin)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public float GetSlope(Chunk t)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void ClearNeighbours()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool SetNeighbour(int eA, int eB, Chunk n)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Chunk GetNeighbour(int i)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3 GetTri(int i)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int GetTriIndex(int i)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetTri(Vector3 value, int i)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGenCoroutine_003Ed__22 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SpaceObjectCollider _003C_003E4__this;

		private Vector3[] _003CsrcVerts_003E5__2;

		private Vector3[] _003CsrcNormals_003E5__3;

		private int _003Ci_003E5__4;

		private int _003CiC_003E5__5;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CGenCoroutine_003Ed__22(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	private Mesh refMesh;

	private Vector3 center;

	private Transform trf;

	private Chunk[] chunks;

	[SerializeField]
	private List<CompositeSolid> solids;

	private List<MarchStep> marchList;

	private bool setup;

	[SerializeField]
	private bool clustering;

	[SerializeField]
	private bool drawSolids;

	[SerializeField]
	private int marchStepsPerFrame;

	[SerializeField]
	private int meshGensPerFrame;

	[SerializeField]
	private int colliderGensPerFrame;

	private Func<Transform, float> rangefinder;

	private float maxRange;

	private float minRange;

	private float rangeScale;

	private Callback onGenComplete;

	[SerializeField]
	private float minScore;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SpaceObjectCollider()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(PAsteroid pa, Mesh refMesh, Vector3 center, Func<Transform, float> rangefinder, float maxRange, float minRange, Callback onGenComplete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(PSpaceObject pso, Mesh refMesh, Vector3 center, Func<Transform, float> rangefinder, float maxRange, float minRange, Callback onGenComplete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Clear Colliders")]
	public void Clear()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateGenRange(float range)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Generate")]
	private void Generate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CGenCoroutine_003Ed__22))]
	private IEnumerator GenCoroutine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MarchNext(MarchStep step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MarchEdge(Chunk n, MarchStep step)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private TestResult TestEdge(Chunk c, Chunk n, CompositeSolid solid, ref float score)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool TestWedge(Chunk n, CompositeSolid solid)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Color RandomColor(Vector3 unitVector, float a = 1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Chunk[] CreateChunks(Vector3[] verts, int[] triangles, Vector3 center)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LinkChunks(Chunk[] chunks)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void fix_multi_shared_edges(List<CompositeSolid> solids)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void fix_wind_errors(Chunk[] chunks, Vector3[] verts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void find_fix_shared_edge(Chunk c1, Chunk c2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool share_edge(Vector3 tl, Vector3 tr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int shared_edges(Chunk c1, Chunk c2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float get_vertex(Vector3 tri, int i)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void set_vertex(ref Vector3 tri, int i, float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool winding_conflict(Vector3 ti, Vector3 tj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 flip_winding_order(Vector3 t)
	{
		throw null;
	}
}
