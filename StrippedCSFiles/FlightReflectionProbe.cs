using System.Runtime.CompilerServices;
using UnityEngine;

public class FlightReflectionProbe : MonoBehaviour
{
	public ReflectionProbe probeComponent;

	public float angleDivisor;

	private AtmosphereFromGround afg;

	private Renderer afgRenderer;

	private int renderID;

	private bool forceRender;

	private bool turnOff;

	private bool rendering;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightReflectionProbe()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static FlightReflectionProbe Spawn()
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
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForceRender()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Enable(bool enable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Color GetSkyColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float expScale(float cos, float scaleDepth)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 VectorMatrixMultiplication(Vector3 vec, Matrix4x4 mat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float Sign(float number)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnSettingsUpdate()
	{
		throw null;
	}
}
