using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FloatingOrigin : MonoBehaviour
{
	public float threshold;

	public float thresholdSqr;

	public double velForContinuous;

	public double velForContinuousSqr;

	public double altToStopMovingExplosions;

	public double CoMRecalcOffsetMaxSqr;

	public bool continuous;

	public bool bypassAuto;

	public Vector3d offset;

	public Vector3d offsetNonKrakensbane;

	public Vector3d reverseoffset;

	public Vector3d outOfFrameAdditional;

	public Transform forcedCenterTransform;

	public static FloatingOrigin fetch;

	public bool canEngageThisFrame;

	public List<ParticleSystem> particleSystems;

	public int pCount;

	public static int particlesLength;

	private bool SetOffsetThisFrame;

	private Vector3d terrainShaderOffset;

	public static Vector3d Offset
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d ReverseOffset
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d OffsetNonKrakensbane
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector3d TerrainShaderOffset
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FloatingOrigin()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FloatingOrigin()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void setOffset(Vector3d refPos, Vector3d nonFrame)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool RegisterParticleSystem(ParticleSystem sys)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool UnregisterParticleSystem(ParticleSystem sys)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetOffset(Vector3d refPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetOutOfFrameOffset(Vector3d add)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetSafeToEngage(bool val)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Reset Origin")]
	public void ResetOffset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ResetTerrainShaderOffset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMapExited()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLevelLoaded(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCameraChange(CameraManager.CameraMode newMode)
	{
		throw null;
	}
}
