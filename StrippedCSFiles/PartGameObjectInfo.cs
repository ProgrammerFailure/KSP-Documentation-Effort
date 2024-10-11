using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class PartGameObjectInfo
{
	[SerializeField]
	private string name;

	[SerializeField]
	private bool status;

	public string Name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Status
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartGameObjectInfo(string name, bool status)
	{
		throw null;
	}
}
