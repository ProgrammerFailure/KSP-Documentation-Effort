using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.Settings.Controls;

public abstract class AccessorBase
{
	protected object obj;

	protected MemberInfo member;

	public abstract object Value { get; set; }

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected AccessorBase(object obj, MemberInfo member)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool IsIdentifier(char ch, bool numok)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool IsDigit(char ch)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AccessorBase Create(Type objType, object obj, string memberExpr)
	{
		throw null;
	}
}
