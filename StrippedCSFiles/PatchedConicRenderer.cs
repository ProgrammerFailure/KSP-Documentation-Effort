using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens.Mapview;
using UnityEngine;

[RequireComponent(typeof(PatchedConicSolver))]
public class PatchedConicRenderer : MonoBehaviour
{
	public int patchSamples;

	public int interpolations;

	public PatchedConicSolver solver;

	public List<PatchRendering> patchRenders;

	public List<PatchRendering> flightPlanRenders;

	public PatchRendering.RelativityMode relativityMode;

	public CelestialBody relativeTo;

	public bool drawTimes;

	public bool renderEnabled;

	private bool mouseOverGizmos;

	private bool mouseOverNodes;

	public Vessel vessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public OrbitDriver obtDriver
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Orbit orbit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool MouseOverNodes
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PatchedConicRenderer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Color GetPatchColor(int index, Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameSettingsApplied()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ManeuverUINode_OnUpdateVisible(MapNode node, MapNode.IconData iData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ManeuverUINode_OnUpdateCaption(MapNode node, MapNode.CaptionData cData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3d ManeuverUINode_OnUpdatePosition(MapNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ManeuverUINode_OnClick(MapNode mn, Mouse.Buttons btns)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CanDrawAnyNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneSwitch(GameScenes scn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PatchRendering FindRenderingForPatch(Orbit patch)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddManeuverNode(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMouseOverGizmo(bool h)
	{
		throw null;
	}
}
