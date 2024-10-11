using System;
using System.Runtime.CompilerServices;

[Serializable]
public class AttachRules
{
	public bool stack;

	public bool srfAttach;

	public bool allowStack;

	public bool allowSrfAttach;

	public bool allowCollision;

	public bool allowDock;

	public bool allowRotate;

	public bool allowRoot;

	public bool StackOrSurfaceAttachable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AttachRules()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AttachRules Parse(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string String()
	{
		throw null;
	}
}
