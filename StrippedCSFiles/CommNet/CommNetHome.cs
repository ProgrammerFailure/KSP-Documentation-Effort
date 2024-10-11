using System.Runtime.CompilerServices;
using UnityEngine;

namespace CommNet;

public class CommNetHome : MonoBehaviour
{
	public string nodeName;

	public string displaynodeName;

	public Transform nodeTransform;

	public bool isKSC;

	public bool isPermanent;

	public double antennaPower;

	protected CommNode comm;

	protected CelestialBody body;

	protected double lat;

	protected double lon;

	protected double alt;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CommNetHome()
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
	protected virtual void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnNetworkPreUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnNetworkInitialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CreateNode()
	{
		throw null;
	}
}
