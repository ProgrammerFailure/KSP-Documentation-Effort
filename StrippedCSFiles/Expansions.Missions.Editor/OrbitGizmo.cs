using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class OrbitGizmo : ManeuverGizmoBase
{
	public delegate void HandlesUpdatedCallback(Vector3d dV, double UT);

	public GameObject pointHandles;

	public GameObject globalHandles;

	public ManeuverGizmoHandle handleIncClockwise;

	public ManeuverGizmoHandle handleIncCounter;

	public ManeuverGizmoHandle handleEccIn;

	public ManeuverGizmoHandle handleEccOut;

	public ManeuverGizmoHandle handleSMA;

	public ManeuverGizmoHandle handleAntiSMA;

	public SpriteButton resetBtn;

	public SpriteButton pointModeBtn;

	public SpriteButton globalModeBtn;

	public float incMultiplier;

	public float smaMultiplier;

	public float eccMultiplier;

	public Callback OnOrbitReset;

	public HandlesUpdatedCallback OnPointGizmoUpdated;

	public HandlesUpdatedCallback OnGlobalGizmoUpdated;

	protected OrbitGizmoMode mode;

	public GAPCelestialBodyState_Orbit gapOrbit;

	private OrbitRendererBase.OrbitCastHit hit;

	public bool isDragging;

	public OrbitGizmoMode GizmoMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OrbitGizmo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OrbitGizmo Create(Vector3 position, Camera cam, Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ChangeToPointMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ChangeToGlobalMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnProgradeUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRetrogradeUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnNormalUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAntinormalUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRadialInUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRadialOutUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnIncClockwiseUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnIncCounterUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSMAUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAntiSMAUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEccInUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEccOutUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnResetButtonPress()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPointModeButtonPress()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGlobalModeButtonPress()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnMouseDrag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnMouseUp()
	{
		throw null;
	}
}
