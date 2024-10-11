using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens.Mapview;
using UnityEngine;
using Vectrosity;

public class PatchRendering
{
	public enum RelativityMode
	{
		LOCAL_TO_BODIES,
		LOCAL_AT_SOI_ENTRY_UT,
		LOCAL_AT_SOI_EXIT_UT,
		RELATIVE,
		DYNAMIC
	}

	private MapNode mnAp;

	private MapNode mnPe;

	private MapNode mnCB;

	private MapNode mnCBatUT;

	private MapNode mnEnd;

	private MapNode mnNextStart;

	private MapObject moAp;

	private MapObject moPe;

	private MapObject moCB;

	private MapObject moCBatUT;

	private MapObject moEnd;

	private MapObject moNextStart;

	public CelestialBody currentMainBody;

	public CelestialBody relativeTo;

	public Orbit patch;

	public Trajectory trajectory;

	public RelativityMode relativityMode;

	public Vector3d pe;

	public Vector3d ap;

	public Vector3d st;

	public Vector3d end;

	public Vector3d cb;

	public double UTpe;

	public double UTap;

	public double UTcb;

	public Vector3d[] tPoints;

	public Vector3[] scaledTPoints;

	public List<Vector3> vectorPoints;

	public Color[] colors;

	public Color patchColor;

	public Color nodeColor;

	private List<Color32> vectorColours;

	private VectorLine vectorLine;

	public Material lineMaterial;

	public int samples;

	public int interpolations;

	public double dynamicLinearity;

	public string vectorName;

	public float lineWidth;

	private bool smoothLineTexture;

	private bool draw3dLines;

	public bool visible;

	private bool hasMapNodes;

	private PatchedConicRenderer pcrCache;

	private double trailOffset;

	private double eccOffset;

	private float twkOffset;

	public List<Color32> VectorColours
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool enabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PatchRendering(string name, int nSamples, int nInterpolations, Orbit patchRef, Material lineMat, float LineWidth, bool smoothTexture, PatchedConicRenderer pcr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetColor(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroyUINodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateMapObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyMapObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AttachUINodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DetachUINodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void mnNextStart_OnUpdateVisible(MapNode n, MapNode.IconData iData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void mnCBatUT_OnUpdateVisible(MapNode n, MapNode.IconData iData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void mnEnd_OnUpdateVisible(MapNode n, MapNode.IconData iData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void mnAp_OnUpdateVisible(MapNode n, MapNode.IconData vData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void mnPe_OnUpdateVisible(MapNode n, MapNode.IconData vData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void mnCB_OnUpdateVisible(MapNode n, MapNode.IconData iData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void mnEnd_OnUpdateCaption(MapNode n, MapNode.CaptionData cData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void mnAp_OnUpdateCaption(MapNode n, MapNode.CaptionData cData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void mnPe_OnUpdateCaption(MapNode n, MapNode.CaptionData cData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void mnCB_OnUpdateCaption(MapNode n, MapNode.CaptionData cData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3d mnCBatUT_OnUpdatePosition(MapNode n)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void mnNextStart_OnUpdateType(MapNode n, MapNode.TypeData tData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void mnEnd_OnUpdateType(MapNode n, MapNode.TypeData tData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CanDrawAnyNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MakeVector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroyVector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdatePR()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSpline()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void DrawSplines()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<Color32> SetColorSegments(Color[] pointColors, int interpolations)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateUINodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetScaledSpacePointFromTA(double TA, double UT)
	{
		throw null;
	}
}
