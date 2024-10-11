using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Tri()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Tri(int a, int b, int c)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Tri(int a, int b, int c, Tri ab, Tri bc, Tri ca)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetVerts(int a, int b, int c)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetEdges(Tri ab, Tri bc, Tri ca)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void ReplaceEdge(Tri oldTri, Tri newTri)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddTriangle(List<int> triList)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool HasVertex(int v)
		{
			throw null;
		}
	}

	public class TriList : LinkedList<Tri>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public TriList()
		{
			throw null;
		}
	}

	public class VertList : IEnumerable<Vector3>, IEnumerable
	{
		[CompilerGenerated]
		private sealed class _003CGetEnumerator_003Ed__16 : IEnumerator<Vector3>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private Vector3 _003C_003E2__current;

			public VertList _003C_003E4__this;

			private int _003Ci_003E5__2;

			Vector3 IEnumerator<Vector3>.Current
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
			public _003CGetEnumerator_003Ed__16(int _003C_003E1__state)
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

		[CompilerGenerated]
		private sealed class _003CSystem_002DCollections_002DIEnumerable_002DGetEnumerator_003Ed__17 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public VertList _003C_003E4__this;

			private int _003Ci_003E5__2;

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
			public _003CSystem_002DCollections_002DIEnumerable_002DGetEnumerator_003Ed__17(int _003C_003E1__state)
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

		private List<Vector3> verts;

		private List<bool> assigned;

		private List<Vector3d> doubleVerts;

		private int index;

		private int total;

		private int count;

		public int Count
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public Vector3 this[int index]
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public VertList(int length)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int GetVertex(Vector3 newVert)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RemoveVertex(int index)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3d GetDoubleVert(int index)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetDoubleVert(int index, Vector3d doubleVert)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[IteratorStateMachine(typeof(_003CGetEnumerator_003Ed__16))]
		public IEnumerator<Vector3> GetEnumerator()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[IteratorStateMachine(typeof(_003CSystem_002DCollections_002DIEnumerable_002DGetEnumerator_003Ed__17))]
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}
	}

	public int maxSubdivision;

	public int minSubdivision;

	public float radius;

	public PQS targetPQS;

	private TriList triList;

	private VertList vertList;

	private Vector3d radial;

	private double pqsHeight;

	private double vertHeight;

	private float heightOffset;

	private double pqsRadius;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMeshPlanet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Subdivide(int level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateBox()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupLists()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool Split(Tri tri)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateMeshObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Mesh CreateMesh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateIcosahedron()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Create")]
	private void CreatePlanetMesh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Wrap()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool SplitWrap(Tri tri)
	{
		throw null;
	}
}
