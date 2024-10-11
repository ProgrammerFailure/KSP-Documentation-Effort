using System.Runtime.CompilerServices;
using UnityEngine;

public struct KSPParseable
{
	public enum Type
	{
		STRING,
		BOOL,
		UINT,
		INT,
		FLOAT,
		DOUBLE,
		VECTOR2,
		VECTOR3,
		VECTOR4,
		QUATERNION,
		UNKNOWN
	}

	public string value;

	public Type type;

	public int value_int
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public uint value_uint
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float value_float
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double value_double
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool value_bool
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector2 value_v2
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 value_v3
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector4 value_v4
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Quaternion value_quat
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPParseable(object Value, Type Type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPParseable(string Value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string Save()
	{
		throw null;
	}
}
