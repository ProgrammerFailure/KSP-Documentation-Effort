using System;
using Smooth.Collections;

namespace Smooth.Algebraics;

public static class Tuple
{
	public static Tuple<T1> Create<T1>(T1 t1)
	{
		return new Tuple<T1>(t1);
	}

	public static Tuple<T1, T2> Create<T1, T2>(T1 t1, T2 t2)
	{
		return new Tuple<T1, T2>(t1, t2);
	}

	public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 t1, T2 t2, T3 t3)
	{
		return new Tuple<T1, T2, T3>(t1, t2, t3);
	}

	public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4)
	{
		return new Tuple<T1, T2, T3, T4>(t1, t2, t3, t4);
	}

	public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
	{
		return new Tuple<T1, T2, T3, T4, T5>(t1, t2, t3, t4, t5);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6>(t1, t2, t3, t4, t5, t6);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7>(t1, t2, t3, t4, t5, t6, t7);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8>(t1, t2, t3, t4, t5, t6, t7, t8);
	}

	public static Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
	{
		return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(t1, t2, t3, t4, t5, t6, t7, t8, t9);
	}
}
[Serializable]
public struct Tuple<T1> : IComparable<Tuple<T1>>, IEquatable<Tuple<T1>>
{
	public readonly T1 Item1;

	public Tuple(T1 Item1)
	{
		this.Item1 = Item1;
	}

	public override bool Equals(object o)
	{
		if (o is Tuple<T1>)
		{
			return Equals((Tuple<T1>)o);
		}
		return false;
	}

	public bool Equals(Tuple<T1> t)
	{
		return EqualityComparer<T1>.Default.Equals(Item1, t.Item1);
	}

	public override int GetHashCode()
	{
		int num = 17;
		return 493 + EqualityComparer<T1>.Default.GetHashCode(Item1);
	}

	public int CompareTo(Tuple<T1> other)
	{
		return Comparer<T1>.Default.Compare(Item1, other.Item1);
	}

	public static bool operator ==(Tuple<T1> lhs, Tuple<T1> rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Tuple<T1> lhs, Tuple<T1> rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static bool operator >(Tuple<T1> lhs, Tuple<T1> rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	public static bool operator <(Tuple<T1> lhs, Tuple<T1> rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	public static bool operator >=(Tuple<T1> lhs, Tuple<T1> rhs)
	{
		return lhs.CompareTo(rhs) >= 0;
	}

	public static bool operator <=(Tuple<T1> lhs, Tuple<T1> rhs)
	{
		return lhs.CompareTo(rhs) <= 0;
	}

	public override string ToString()
	{
		return string.Concat("(", Item1, ")");
	}
}
[Serializable]
public struct Tuple<T1, T2> : IComparable<Tuple<T1, T2>>, IEquatable<Tuple<T1, T2>>
{
	public readonly T1 Item1;

	public readonly T2 Item2;

	public Tuple(T1 Item1, T2 Item2)
	{
		this.Item1 = Item1;
		this.Item2 = Item2;
	}

	public override bool Equals(object o)
	{
		if (o is Tuple<T1, T2>)
		{
			return Equals((Tuple<T1, T2>)o);
		}
		return false;
	}

	public bool Equals(Tuple<T1, T2> t)
	{
		if (EqualityComparer<T1>.Default.Equals(Item1, t.Item1))
		{
			return EqualityComparer<T2>.Default.Equals(Item2, t.Item2);
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = 17;
		num = 493 + EqualityComparer<T1>.Default.GetHashCode(Item1);
		return 29 * num + EqualityComparer<T2>.Default.GetHashCode(Item2);
	}

	public int CompareTo(Tuple<T1, T2> other)
	{
		int num = Comparer<T1>.Default.Compare(Item1, other.Item1);
		if (num != 0)
		{
			return num;
		}
		return Comparer<T2>.Default.Compare(Item2, other.Item2);
	}

	public static bool operator ==(Tuple<T1, T2> lhs, Tuple<T1, T2> rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Tuple<T1, T2> lhs, Tuple<T1, T2> rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static bool operator >(Tuple<T1, T2> lhs, Tuple<T1, T2> rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	public static bool operator <(Tuple<T1, T2> lhs, Tuple<T1, T2> rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	public static bool operator >=(Tuple<T1, T2> lhs, Tuple<T1, T2> rhs)
	{
		return lhs.CompareTo(rhs) >= 0;
	}

	public static bool operator <=(Tuple<T1, T2> lhs, Tuple<T1, T2> rhs)
	{
		return lhs.CompareTo(rhs) <= 0;
	}

	public override string ToString()
	{
		return string.Concat("(", Item1, ", ", Item2, ")");
	}
}
[Serializable]
public struct Tuple<T1, T2, T3> : IComparable<Tuple<T1, T2, T3>>, IEquatable<Tuple<T1, T2, T3>>
{
	public readonly T1 Item1;

	public readonly T2 Item2;

	public readonly T3 Item3;

	public Tuple(T1 item1, T2 item2, T3 item3)
	{
		Item1 = item1;
		Item2 = item2;
		Item3 = item3;
	}

	public override bool Equals(object o)
	{
		if (o is Tuple<T1, T2, T3>)
		{
			return Equals((Tuple<T1, T2, T3>)o);
		}
		return false;
	}

	public bool Equals(Tuple<T1, T2, T3> t)
	{
		if (EqualityComparer<T1>.Default.Equals(Item1, t.Item1) && EqualityComparer<T2>.Default.Equals(Item2, t.Item2))
		{
			return EqualityComparer<T3>.Default.Equals(Item3, t.Item3);
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = 17;
		num = 493 + EqualityComparer<T1>.Default.GetHashCode(Item1);
		num = 29 * num + EqualityComparer<T2>.Default.GetHashCode(Item2);
		return 29 * num + EqualityComparer<T3>.Default.GetHashCode(Item3);
	}

	public int CompareTo(Tuple<T1, T2, T3> other)
	{
		int num = Comparer<T1>.Default.Compare(Item1, other.Item1);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T2>.Default.Compare(Item2, other.Item2);
		if (num != 0)
		{
			return num;
		}
		return Comparer<T3>.Default.Compare(Item3, other.Item3);
	}

	public static bool operator ==(Tuple<T1, T2, T3> lhs, Tuple<T1, T2, T3> rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Tuple<T1, T2, T3> lhs, Tuple<T1, T2, T3> rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static bool operator >(Tuple<T1, T2, T3> lhs, Tuple<T1, T2, T3> rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	public static bool operator <(Tuple<T1, T2, T3> lhs, Tuple<T1, T2, T3> rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	public static bool operator >=(Tuple<T1, T2, T3> lhs, Tuple<T1, T2, T3> rhs)
	{
		return lhs.CompareTo(rhs) >= 0;
	}

	public static bool operator <=(Tuple<T1, T2, T3> lhs, Tuple<T1, T2, T3> rhs)
	{
		return lhs.CompareTo(rhs) <= 0;
	}

	public override string ToString()
	{
		return string.Concat("(", Item1, ", ", Item2, ", ", Item3, ")");
	}
}
[Serializable]
public struct Tuple<T1, T2, T3, T4> : IComparable<Tuple<T1, T2, T3, T4>>, IEquatable<Tuple<T1, T2, T3, T4>>
{
	public readonly T1 Item1;

	public readonly T2 Item2;

	public readonly T3 Item3;

	public readonly T4 Item4;

	public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
	{
		Item1 = item1;
		Item2 = item2;
		Item3 = item3;
		Item4 = item4;
	}

	public override bool Equals(object o)
	{
		if (o is Tuple<T1, T2, T3, T4>)
		{
			return Equals((Tuple<T1, T2, T3, T4>)o);
		}
		return false;
	}

	public bool Equals(Tuple<T1, T2, T3, T4> t)
	{
		if (EqualityComparer<T1>.Default.Equals(Item1, t.Item1) && EqualityComparer<T2>.Default.Equals(Item2, t.Item2) && EqualityComparer<T3>.Default.Equals(Item3, t.Item3))
		{
			return EqualityComparer<T4>.Default.Equals(Item4, t.Item4);
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = 17;
		num = 493 + EqualityComparer<T1>.Default.GetHashCode(Item1);
		num = 29 * num + EqualityComparer<T2>.Default.GetHashCode(Item2);
		num = 29 * num + EqualityComparer<T3>.Default.GetHashCode(Item3);
		return 29 * num + EqualityComparer<T4>.Default.GetHashCode(Item4);
	}

	public int CompareTo(Tuple<T1, T2, T3, T4> other)
	{
		int num = Comparer<T1>.Default.Compare(Item1, other.Item1);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T2>.Default.Compare(Item2, other.Item2);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T3>.Default.Compare(Item3, other.Item3);
		if (num != 0)
		{
			return num;
		}
		return Comparer<T4>.Default.Compare(Item4, other.Item4);
	}

	public static bool operator ==(Tuple<T1, T2, T3, T4> lhs, Tuple<T1, T2, T3, T4> rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Tuple<T1, T2, T3, T4> lhs, Tuple<T1, T2, T3, T4> rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static bool operator >(Tuple<T1, T2, T3, T4> lhs, Tuple<T1, T2, T3, T4> rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	public static bool operator <(Tuple<T1, T2, T3, T4> lhs, Tuple<T1, T2, T3, T4> rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	public static bool operator >=(Tuple<T1, T2, T3, T4> lhs, Tuple<T1, T2, T3, T4> rhs)
	{
		return lhs.CompareTo(rhs) >= 0;
	}

	public static bool operator <=(Tuple<T1, T2, T3, T4> lhs, Tuple<T1, T2, T3, T4> rhs)
	{
		return lhs.CompareTo(rhs) <= 0;
	}

	public override string ToString()
	{
		return string.Concat("(", Item1, ", ", Item2, ", ", Item3, ", ", Item4, ")");
	}
}
[Serializable]
public struct Tuple<T1, T2, T3, T4, T5> : IComparable<Tuple<T1, T2, T3, T4, T5>>, IEquatable<Tuple<T1, T2, T3, T4, T5>>
{
	public readonly T1 Item1;

	public readonly T2 Item2;

	public readonly T3 Item3;

	public readonly T4 Item4;

	public readonly T5 Item5;

	public Tuple(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5)
	{
		this.Item1 = Item1;
		this.Item2 = Item2;
		this.Item3 = Item3;
		this.Item4 = Item4;
		this.Item5 = Item5;
	}

	public override bool Equals(object o)
	{
		if (o is Tuple<T1, T2, T3, T4, T5>)
		{
			return Equals((Tuple<T1, T2, T3, T4, T5>)o);
		}
		return false;
	}

	public bool Equals(Tuple<T1, T2, T3, T4, T5> t)
	{
		if (EqualityComparer<T1>.Default.Equals(Item1, t.Item1) && EqualityComparer<T2>.Default.Equals(Item2, t.Item2) && EqualityComparer<T3>.Default.Equals(Item3, t.Item3) && EqualityComparer<T4>.Default.Equals(Item4, t.Item4))
		{
			return EqualityComparer<T5>.Default.Equals(Item5, t.Item5);
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = 17;
		num = 493 + EqualityComparer<T1>.Default.GetHashCode(Item1);
		num = 29 * num + EqualityComparer<T2>.Default.GetHashCode(Item2);
		num = 29 * num + EqualityComparer<T3>.Default.GetHashCode(Item3);
		num = 29 * num + EqualityComparer<T4>.Default.GetHashCode(Item4);
		return 29 * num + EqualityComparer<T5>.Default.GetHashCode(Item5);
	}

	public int CompareTo(Tuple<T1, T2, T3, T4, T5> other)
	{
		int num = Comparer<T1>.Default.Compare(Item1, other.Item1);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T2>.Default.Compare(Item2, other.Item2);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T3>.Default.Compare(Item3, other.Item3);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T4>.Default.Compare(Item4, other.Item4);
		if (num != 0)
		{
			return num;
		}
		return Comparer<T5>.Default.Compare(Item5, other.Item5);
	}

	public static bool operator ==(Tuple<T1, T2, T3, T4, T5> lhs, Tuple<T1, T2, T3, T4, T5> rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Tuple<T1, T2, T3, T4, T5> lhs, Tuple<T1, T2, T3, T4, T5> rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static bool operator >(Tuple<T1, T2, T3, T4, T5> lhs, Tuple<T1, T2, T3, T4, T5> rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	public static bool operator <(Tuple<T1, T2, T3, T4, T5> lhs, Tuple<T1, T2, T3, T4, T5> rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	public static bool operator >=(Tuple<T1, T2, T3, T4, T5> lhs, Tuple<T1, T2, T3, T4, T5> rhs)
	{
		return lhs.CompareTo(rhs) >= 0;
	}

	public static bool operator <=(Tuple<T1, T2, T3, T4, T5> lhs, Tuple<T1, T2, T3, T4, T5> rhs)
	{
		return lhs.CompareTo(rhs) <= 0;
	}

	public override string ToString()
	{
		return string.Concat("(", Item1, ", ", Item2, ", ", Item3, ", ", Item4, ", ", Item5, ")");
	}
}
[Serializable]
public struct Tuple<T1, T2, T3, T4, T5, T6> : IComparable<Tuple<T1, T2, T3, T4, T5, T6>>, IEquatable<Tuple<T1, T2, T3, T4, T5, T6>>
{
	public readonly T1 Item1;

	public readonly T2 Item2;

	public readonly T3 Item3;

	public readonly T4 Item4;

	public readonly T5 Item5;

	public readonly T6 Item6;

	public Tuple(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6)
	{
		this.Item1 = Item1;
		this.Item2 = Item2;
		this.Item3 = Item3;
		this.Item4 = Item4;
		this.Item5 = Item5;
		this.Item6 = Item6;
	}

	public override bool Equals(object o)
	{
		if (o is Tuple<T1, T2, T3, T4, T5, T6>)
		{
			return Equals((Tuple<T1, T2, T3, T4, T5, T6>)o);
		}
		return false;
	}

	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6> t)
	{
		if (EqualityComparer<T1>.Default.Equals(Item1, t.Item1) && EqualityComparer<T2>.Default.Equals(Item2, t.Item2) && EqualityComparer<T3>.Default.Equals(Item3, t.Item3) && EqualityComparer<T4>.Default.Equals(Item4, t.Item4) && EqualityComparer<T5>.Default.Equals(Item5, t.Item5))
		{
			return EqualityComparer<T6>.Default.Equals(Item6, t.Item6);
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = 17;
		num = 493 + EqualityComparer<T1>.Default.GetHashCode(Item1);
		num = 29 * num + EqualityComparer<T2>.Default.GetHashCode(Item2);
		num = 29 * num + EqualityComparer<T3>.Default.GetHashCode(Item3);
		num = 29 * num + EqualityComparer<T4>.Default.GetHashCode(Item4);
		num = 29 * num + EqualityComparer<T5>.Default.GetHashCode(Item5);
		return 29 * num + EqualityComparer<T6>.Default.GetHashCode(Item6);
	}

	public int CompareTo(Tuple<T1, T2, T3, T4, T5, T6> other)
	{
		int num = Comparer<T1>.Default.Compare(Item1, other.Item1);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T2>.Default.Compare(Item2, other.Item2);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T3>.Default.Compare(Item3, other.Item3);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T4>.Default.Compare(Item4, other.Item4);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T5>.Default.Compare(Item5, other.Item5);
		if (num != 0)
		{
			return num;
		}
		return Comparer<T6>.Default.Compare(Item6, other.Item6);
	}

	public static bool operator ==(Tuple<T1, T2, T3, T4, T5, T6> lhs, Tuple<T1, T2, T3, T4, T5, T6> rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Tuple<T1, T2, T3, T4, T5, T6> lhs, Tuple<T1, T2, T3, T4, T5, T6> rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static bool operator >(Tuple<T1, T2, T3, T4, T5, T6> lhs, Tuple<T1, T2, T3, T4, T5, T6> rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	public static bool operator <(Tuple<T1, T2, T3, T4, T5, T6> lhs, Tuple<T1, T2, T3, T4, T5, T6> rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	public static bool operator >=(Tuple<T1, T2, T3, T4, T5, T6> lhs, Tuple<T1, T2, T3, T4, T5, T6> rhs)
	{
		return lhs.CompareTo(rhs) >= 0;
	}

	public static bool operator <=(Tuple<T1, T2, T3, T4, T5, T6> lhs, Tuple<T1, T2, T3, T4, T5, T6> rhs)
	{
		return lhs.CompareTo(rhs) <= 0;
	}

	public override string ToString()
	{
		return string.Concat("(", Item1, ", ", Item2, ", ", Item3, ", ", Item4, ", ", Item5, ", ", Item6, ")");
	}
}
[Serializable]
public struct Tuple<T1, T2, T3, T4, T5, T6, T7> : IComparable<Tuple<T1, T2, T3, T4, T5, T6, T7>>, IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7>>
{
	public readonly T1 Item1;

	public readonly T2 Item2;

	public readonly T3 Item3;

	public readonly T4 Item4;

	public readonly T5 Item5;

	public readonly T6 Item6;

	public readonly T7 Item7;

	public Tuple(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7)
	{
		this.Item1 = Item1;
		this.Item2 = Item2;
		this.Item3 = Item3;
		this.Item4 = Item4;
		this.Item5 = Item5;
		this.Item6 = Item6;
		this.Item7 = Item7;
	}

	public override bool Equals(object o)
	{
		if (o is Tuple<T1, T2, T3, T4, T5, T6, T7>)
		{
			return Equals((Tuple<T1, T2, T3, T4, T5, T6, T7>)o);
		}
		return false;
	}

	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7> t)
	{
		if (EqualityComparer<T1>.Default.Equals(Item1, t.Item1) && EqualityComparer<T2>.Default.Equals(Item2, t.Item2) && EqualityComparer<T3>.Default.Equals(Item3, t.Item3) && EqualityComparer<T4>.Default.Equals(Item4, t.Item4) && EqualityComparer<T5>.Default.Equals(Item5, t.Item5) && EqualityComparer<T6>.Default.Equals(Item6, t.Item6))
		{
			return EqualityComparer<T7>.Default.Equals(Item7, t.Item7);
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = 17;
		num = 493 + EqualityComparer<T1>.Default.GetHashCode(Item1);
		num = 29 * num + EqualityComparer<T2>.Default.GetHashCode(Item2);
		num = 29 * num + EqualityComparer<T3>.Default.GetHashCode(Item3);
		num = 29 * num + EqualityComparer<T4>.Default.GetHashCode(Item4);
		num = 29 * num + EqualityComparer<T5>.Default.GetHashCode(Item5);
		num = 29 * num + EqualityComparer<T6>.Default.GetHashCode(Item6);
		return 29 * num + EqualityComparer<T7>.Default.GetHashCode(Item7);
	}

	public int CompareTo(Tuple<T1, T2, T3, T4, T5, T6, T7> other)
	{
		int num = Comparer<T1>.Default.Compare(Item1, other.Item1);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T2>.Default.Compare(Item2, other.Item2);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T3>.Default.Compare(Item3, other.Item3);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T4>.Default.Compare(Item4, other.Item4);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T5>.Default.Compare(Item5, other.Item5);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T6>.Default.Compare(Item6, other.Item6);
		if (num != 0)
		{
			return num;
		}
		return Comparer<T7>.Default.Compare(Item7, other.Item7);
	}

	public static bool operator ==(Tuple<T1, T2, T3, T4, T5, T6, T7> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7> rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Tuple<T1, T2, T3, T4, T5, T6, T7> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7> rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static bool operator >(Tuple<T1, T2, T3, T4, T5, T6, T7> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7> rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	public static bool operator <(Tuple<T1, T2, T3, T4, T5, T6, T7> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7> rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	public static bool operator >=(Tuple<T1, T2, T3, T4, T5, T6, T7> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7> rhs)
	{
		return lhs.CompareTo(rhs) >= 0;
	}

	public static bool operator <=(Tuple<T1, T2, T3, T4, T5, T6, T7> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7> rhs)
	{
		return lhs.CompareTo(rhs) <= 0;
	}

	public override string ToString()
	{
		return string.Concat("(", Item1, ", ", Item2, ", ", Item3, ", ", Item4, ", ", Item5, ", ", Item6, ", ", Item7, ")");
	}
}
[Serializable]
public struct Tuple<T1, T2, T3, T4, T5, T6, T7, T8> : IComparable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8>>, IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8>>
{
	public readonly T1 Item1;

	public readonly T2 Item2;

	public readonly T3 Item3;

	public readonly T4 Item4;

	public readonly T5 Item5;

	public readonly T6 Item6;

	public readonly T7 Item7;

	public readonly T8 Item8;

	public Tuple(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8)
	{
		this.Item1 = Item1;
		this.Item2 = Item2;
		this.Item3 = Item3;
		this.Item4 = Item4;
		this.Item5 = Item5;
		this.Item6 = Item6;
		this.Item7 = Item7;
		this.Item8 = Item8;
	}

	public override bool Equals(object o)
	{
		if (o is Tuple<T1, T2, T3, T4, T5, T6, T7, T8>)
		{
			return Equals((Tuple<T1, T2, T3, T4, T5, T6, T7, T8>)o);
		}
		return false;
	}

	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> t)
	{
		if (EqualityComparer<T1>.Default.Equals(Item1, t.Item1) && EqualityComparer<T2>.Default.Equals(Item2, t.Item2) && EqualityComparer<T3>.Default.Equals(Item3, t.Item3) && EqualityComparer<T4>.Default.Equals(Item4, t.Item4) && EqualityComparer<T5>.Default.Equals(Item5, t.Item5) && EqualityComparer<T6>.Default.Equals(Item6, t.Item6) && EqualityComparer<T7>.Default.Equals(Item7, t.Item7))
		{
			return EqualityComparer<T8>.Default.Equals(Item8, t.Item8);
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = 17;
		num = 493 + EqualityComparer<T1>.Default.GetHashCode(Item1);
		num = 29 * num + EqualityComparer<T2>.Default.GetHashCode(Item2);
		num = 29 * num + EqualityComparer<T3>.Default.GetHashCode(Item3);
		num = 29 * num + EqualityComparer<T4>.Default.GetHashCode(Item4);
		num = 29 * num + EqualityComparer<T5>.Default.GetHashCode(Item5);
		num = 29 * num + EqualityComparer<T6>.Default.GetHashCode(Item6);
		num = 29 * num + EqualityComparer<T7>.Default.GetHashCode(Item7);
		return 29 * num + EqualityComparer<T8>.Default.GetHashCode(Item8);
	}

	public int CompareTo(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> other)
	{
		int num = Comparer<T1>.Default.Compare(Item1, other.Item1);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T2>.Default.Compare(Item2, other.Item2);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T3>.Default.Compare(Item3, other.Item3);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T4>.Default.Compare(Item4, other.Item4);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T5>.Default.Compare(Item5, other.Item5);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T6>.Default.Compare(Item6, other.Item6);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T7>.Default.Compare(Item7, other.Item7);
		if (num != 0)
		{
			return num;
		}
		return Comparer<T8>.Default.Compare(Item8, other.Item8);
	}

	public static bool operator ==(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static bool operator >(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	public static bool operator <(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	public static bool operator >=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> rhs)
	{
		return lhs.CompareTo(rhs) >= 0;
	}

	public static bool operator <=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> rhs)
	{
		return lhs.CompareTo(rhs) <= 0;
	}

	public override string ToString()
	{
		return string.Concat("(", Item1, ", ", Item2, ", ", Item3, ", ", Item4, ", ", Item5, ", ", Item6, ", ", Item7, ", ", Item8, ")");
	}
}
[Serializable]
public struct Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IComparable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>, IEquatable<Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
{
	public readonly T1 Item1;

	public readonly T2 Item2;

	public readonly T3 Item3;

	public readonly T4 Item4;

	public readonly T5 Item5;

	public readonly T6 Item6;

	public readonly T7 Item7;

	public readonly T8 Item8;

	public readonly T9 Item9;

	public Tuple(T1 Item1, T2 Item2, T3 Item3, T4 Item4, T5 Item5, T6 Item6, T7 Item7, T8 Item8, T9 Item9)
	{
		this.Item1 = Item1;
		this.Item2 = Item2;
		this.Item3 = Item3;
		this.Item4 = Item4;
		this.Item5 = Item5;
		this.Item6 = Item6;
		this.Item7 = Item7;
		this.Item8 = Item8;
		this.Item9 = Item9;
	}

	public override bool Equals(object o)
	{
		if (o is Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>)
		{
			return Equals((Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>)o);
		}
		return false;
	}

	public bool Equals(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> t)
	{
		if (EqualityComparer<T1>.Default.Equals(Item1, t.Item1) && EqualityComparer<T2>.Default.Equals(Item2, t.Item2) && EqualityComparer<T3>.Default.Equals(Item3, t.Item3) && EqualityComparer<T4>.Default.Equals(Item4, t.Item4) && EqualityComparer<T5>.Default.Equals(Item5, t.Item5) && EqualityComparer<T6>.Default.Equals(Item6, t.Item6) && EqualityComparer<T7>.Default.Equals(Item7, t.Item7) && EqualityComparer<T8>.Default.Equals(Item8, t.Item8))
		{
			return EqualityComparer<T9>.Default.Equals(Item9, t.Item9);
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = 17;
		num = 493 + EqualityComparer<T1>.Default.GetHashCode(Item1);
		num = 29 * num + EqualityComparer<T2>.Default.GetHashCode(Item2);
		num = 29 * num + EqualityComparer<T3>.Default.GetHashCode(Item3);
		num = 29 * num + EqualityComparer<T4>.Default.GetHashCode(Item4);
		num = 29 * num + EqualityComparer<T5>.Default.GetHashCode(Item5);
		num = 29 * num + EqualityComparer<T6>.Default.GetHashCode(Item6);
		num = 29 * num + EqualityComparer<T7>.Default.GetHashCode(Item7);
		num = 29 * num + EqualityComparer<T8>.Default.GetHashCode(Item8);
		return 29 * num + EqualityComparer<T9>.Default.GetHashCode(Item9);
	}

	public int CompareTo(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> other)
	{
		int num = Comparer<T1>.Default.Compare(Item1, other.Item1);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T2>.Default.Compare(Item2, other.Item2);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T3>.Default.Compare(Item3, other.Item3);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T4>.Default.Compare(Item4, other.Item4);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T5>.Default.Compare(Item5, other.Item5);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T6>.Default.Compare(Item6, other.Item6);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T7>.Default.Compare(Item7, other.Item7);
		if (num != 0)
		{
			return num;
		}
		num = Comparer<T8>.Default.Compare(Item8, other.Item8);
		if (num != 0)
		{
			return num;
		}
		return Comparer<T9>.Default.Compare(Item9, other.Item9);
	}

	public static bool operator ==(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> rhs)
	{
		return !lhs.Equals(rhs);
	}

	public static bool operator >(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	public static bool operator <(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	public static bool operator >=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> rhs)
	{
		return lhs.CompareTo(rhs) >= 0;
	}

	public static bool operator <=(Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> lhs, Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9> rhs)
	{
		return lhs.CompareTo(rhs) <= 0;
	}

	public override string ToString()
	{
		return string.Concat("(", Item1, ", ", Item2, ", ", Item3, ", ", Item4, ", ", Item5, ", ", Item6, ", ", Item7, ", ", Item8, ", ", Item9, ")");
	}
}
