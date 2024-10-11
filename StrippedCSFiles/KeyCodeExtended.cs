using System.Runtime.CompilerServices;
using UnityEngine;

public class KeyCodeExtended
{
	public KeyCode code;

	public string name;

	public bool isNone
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyCodeExtended()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyCodeExtended(KeyCode key)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyCodeExtended(string key)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyCodeExtended(KeyCode key, string keyName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}
}
