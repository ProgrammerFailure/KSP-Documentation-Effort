using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class FieldAttribute : Attribute
{
	[SerializeField]
	private string _guiName;

	public string guiName
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
	public FieldAttribute()
	{
		throw null;
	}
}
