using System.Runtime.CompilerServices;
using UnityEngine;

namespace CompoundParts;

public abstract class CompoundPartModule : PartModule
{
	private CompoundPart _compoundPart;

	public CompoundPart compoundPart
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

	public Part target
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected CompoundPartModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public sealed override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnModuleAwake()
	{
		throw null;
	}

	public abstract void OnTargetSet(Part target);

	public abstract void OnTargetLost();

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnPreviewAttachment(Vector3 rDir, Vector3 rPos, Quaternion rRot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnPreviewEnd()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnTargetUpdate()
	{
		throw null;
	}
}
