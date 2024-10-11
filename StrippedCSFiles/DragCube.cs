using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class DragCube
{
	public enum DragFace
	{
		XP,
		XN,
		YP,
		YN,
		ZP,
		ZN
	}

	[SerializeField]
	private float[] area;

	[SerializeField]
	private float[] drag;

	[SerializeField]
	private float[] depth;

	[SerializeField]
	private float[] dragModifiers;

	[SerializeField]
	private Vector3 center;

	[SerializeField]
	private Vector3 size;

	[SerializeField]
	private string name;

	[SerializeField]
	private float weight;

	public float[] Area
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float[] Drag
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float[] Depth
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float[] DragModifiers
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 Center
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

	public Vector3 Size
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

	public string Name
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

	public float Weight
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
	public DragCube()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DragCube(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Load(string[] data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string SaveToString()
	{
		throw null;
	}
}
