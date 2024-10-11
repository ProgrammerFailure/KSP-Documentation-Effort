using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GalaxyCameraControl : MonoBehaviour
{
	public static GalaxyCameraControl Instance;

	public float defaultFoV;

	private Camera _camera;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GalaxyCameraControl()
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
	public void SetFoV(float FoV)
	{
		throw null;
	}
}
