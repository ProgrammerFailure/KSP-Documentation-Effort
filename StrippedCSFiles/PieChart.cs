using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Slice(string name, float fraction, Color color)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Slice(string name, int id, float fraction, Color color)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Slice(int id, float fraction, Color color)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddOnOver(OnInputDelegate onOver)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RemoveOnOver(OnInputDelegate onOver)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddOnExit(OnInputDelegate onExit)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RemoveOnExit(OnInputDelegate onExit)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddOnTap(OnInputDelegate onTap)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RemoveOnTap(OnInputDelegate onTap)
		{
			throw null;
		}
	}

	public Material material;

	public int resolution;

	public float radius;

	public float depth;

	public List<Slice> slices;

	private List<PieChartSlice> sliceObjects;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PieChart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSlices(List<Slice> slices)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateChart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateEnds(float angle, float endAngle, float deltaAngle, List<Vector3> verts, List<int> tris)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateTopBottom(float angle, float endAngle, float deltaAngle, List<Vector3> verts, List<int> tris)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateEdges(float angle, float endAngle, float deltaAngle, List<Vector3> verts, List<int> tris)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector3 CreateDirection(float angle, float height)
	{
		throw null;
	}
}
