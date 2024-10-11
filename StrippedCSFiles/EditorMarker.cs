using System.Runtime.CompilerServices;
using UnityEngine;

public class EditorMarker : MonoBehaviour
{
	public GameObject posMarkerObject;

	public GameObject dirMarkerObject;

	public Part rootPart;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EditorMarker()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Vector3 UpdatePosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Vector3 UpdateDirection()
	{
		throw null;
	}
}
