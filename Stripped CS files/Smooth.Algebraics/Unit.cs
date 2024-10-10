using System;
using System.Runtime.InteropServices;

namespace Smooth.Algebraics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Unit : IComparable<Unit>, IEquatable<Unit>
{
	public override bool Equals(object o)
	{
		return o is Unit;
	}

	public bool Equals(Unit other)
	{
		return true;
	}

	public override int GetHashCode()
	{
		return 0;
	}

	public int CompareTo(Unit other)
	{
		return 0;
	}

	public static bool operator ==(Unit lhs, Unit rhs)
	{
		return true;
	}

	public static bool operator >=(Unit lhs, Unit rhs)
	{
		return true;
	}

	public static bool operator <=(Unit lhs, Unit rhs)
	{
		return true;
	}

	public static bool operator !=(Unit lhs, Unit rhs)
	{
		return false;
	}

	public static bool operator >(Unit lhs, Unit rhs)
	{
		return false;
	}

	public static bool operator <(Unit lhs, Unit rhs)
	{
		return false;
	}

	public override string ToString()
	{
		return "Unit";
	}
}
