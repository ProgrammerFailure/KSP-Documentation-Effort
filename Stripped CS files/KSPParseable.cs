using System;
using UnityEngine;

public struct KSPParseable
{
	public enum Type
	{
		STRING,
		BOOL,
		UINT,
		const_3,
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
		get
		{
			if (type != Type.const_3)
			{
				return 0;
			}
			return int.Parse(value);
		}
	}

	public uint value_uint
	{
		get
		{
			if (type != Type.UINT)
			{
				return 0u;
			}
			return uint.Parse(value);
		}
	}

	public float value_float
	{
		get
		{
			if (type != Type.FLOAT)
			{
				return 0f;
			}
			return float.Parse(value);
		}
	}

	public double value_double
	{
		get
		{
			if (type != Type.DOUBLE)
			{
				return 0.0;
			}
			return double.Parse(value);
		}
	}

	public bool value_bool
	{
		get
		{
			if (type != Type.BOOL)
			{
				return false;
			}
			return bool.Parse(value);
		}
	}

	public Vector2 value_v2
	{
		get
		{
			if (type != Type.VECTOR2)
			{
				return Vector2.zero;
			}
			return KSPUtil.ParseVector2(value);
		}
	}

	public Vector3 value_v3
	{
		get
		{
			if (type != Type.VECTOR3)
			{
				return Vector3.zero;
			}
			return KSPUtil.ParseVector3(value);
		}
	}

	public Vector4 value_v4
	{
		get
		{
			if (type != Type.VECTOR4)
			{
				return Vector4.zero;
			}
			return KSPUtil.ParseVector4(value);
		}
	}

	public Quaternion value_quat
	{
		get
		{
			if (type != Type.QUATERNION)
			{
				return Quaternion.identity;
			}
			return KSPUtil.ParseQuaternion(value);
		}
	}

	public KSPParseable(object Value, Type Type)
	{
		switch (Type)
		{
		case Type.STRING:
			value = (string)Value;
			break;
		default:
			value = Value.ToString();
			break;
		case Type.VECTOR2:
			value = KSPUtil.WriteVector((Vector2)Value);
			break;
		case Type.VECTOR3:
			value = KSPUtil.WriteVector((Vector3)Value);
			break;
		case Type.VECTOR4:
			value = KSPUtil.WriteVector((Vector4)Value);
			break;
		case Type.QUATERNION:
			value = KSPUtil.WriteQuaternion((Quaternion)Value);
			break;
		}
		type = Type;
	}

	public KSPParseable(string Value)
	{
		value = "";
		type = Type.UNKNOWN;
		try
		{
			string[] array = Value.Split(',');
			value = array[0].Trim();
			if (array.Length > 1)
			{
				type = (Type)Enum.Parse(typeof(Type), array[array.Length - 1].Trim());
			}
		}
		catch
		{
		}
	}

	public string Save()
	{
		return value + ", " + type;
	}
}
