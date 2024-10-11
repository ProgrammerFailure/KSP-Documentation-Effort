using System.Runtime.CompilerServices;
using UnityEngine;

public class LoadingSystem : MonoBehaviour
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public LoadingSystem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsReady()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string ProgressTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float ProgressFraction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void StartLoad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float LoadWeight()
	{
		throw null;
	}
}
