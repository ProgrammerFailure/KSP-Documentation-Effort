using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class PartRawInfo
{
	[SerializeField]
	private string name;

	[SerializeField]
	private string value;

	public string Name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string Value
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartRawInfo(string name, string value)
	{
		throw null;
	}
}
