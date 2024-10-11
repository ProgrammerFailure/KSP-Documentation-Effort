using System.Runtime.CompilerServices;
using UnityEngine;

namespace CommNet;

public class CommNetBody : MonoBehaviour
{
	protected CelestialBody body;

	protected Occluder occluder;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CommNetBody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnNetworkInitialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnNetworkPreUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Occluder CreateOccluder()
	{
		throw null;
	}
}
