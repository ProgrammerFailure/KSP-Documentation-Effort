using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalSpace : MonoBehaviour
{
	public static InternalSpace Instance;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalSpace()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 InternalToWorld(Vector3 internalSpacePosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Quaternion InternalToWorld(Quaternion internalSpaceRotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 WorldToInternal(Vector3 worldSpacePosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Quaternion WorldToInternal(Quaternion worldSpaceRotation)
	{
		throw null;
	}
}
