using System.Runtime.CompilerServices;
using KSP.UI.Screens.Mapview;

public class OrbitTargetRenderer : OrbitRenderer
{
	public OrbitSnapshot snapshot;

	public float animationTimerMax;

	public float animationTimerCurrent;

	public bool activeDraw;

	protected const string endColor = "</color>";

	protected string startColor;

	private CelestialBody focusBody;

	private bool activeFlight;

	protected bool ANDNVisible;

	protected double relativeInclination;

	protected Vessel targetVessel;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OrbitTargetRenderer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static T Setup<T>(string name, int seed, Orbit orbit, bool activedraw = true) where T : OrbitTargetRenderer
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Cleanup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateLocals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckVisibility()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LockOrbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AnimateOrbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool CanDrawAnyIcons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void ANDNNodes_OnUpdateIcon(MapNode n, MapNode.IconData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override Vector3d ascNode_OnUpdatePosition(MapNode n)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override Vector3d descNode_OnUpdatePosition(MapNode n)
	{
		throw null;
	}
}
