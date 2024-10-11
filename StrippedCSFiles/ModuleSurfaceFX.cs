using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleSurfaceFX : PartModule
{
	public enum SurfaceType
	{
		None,
		Terrain,
		Water,
		Launchpad
	}

	[KSPField]
	public int thrustProviderModuleIndex;

	[KSPField]
	public float fxMax;

	[KSPField]
	public float maxDistance;

	[KSPField]
	public float falloff;

	[KSPField]
	public string thrustTransformName;

	private GameObject terrainPrefab;

	private GameObject waterPrefab;

	private IThrustProvider engineModule;

	private Transform trf;

	private RaycastHit hitInfo;

	private SurfaceType hit;

	private float fxScale;

	private float distance;

	private float scaledDistance;

	private float h0;

	private float dH;

	private bool raycastHit;

	public Vector3 rDir;

	public Vector3 Vsrf;

	public Vector3 point;

	public Vector3 normal;

	public float ScaledFX;

	private SurfaceFX srfFX;

	private SurfaceFX srfFXnext;

	private LaunchPadFX padFX;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleSurfaceFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmos()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSrfFX(RaycastHit hitInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private SurfaceFX GetSurfaceFX(SurfaceFX sFX, GameObject prefabToSpawn, Vector3 wPos, Vector3 wNormal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}
