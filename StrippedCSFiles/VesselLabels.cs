using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions;
using UnityEngine;

public class VesselLabels : MonoBehaviour
{
	public enum InfoLevel
	{
		None,
		Icon,
		Range,
		Ident
	}

	public enum HoverBehaviour
	{
		NoChange,
		RevealNext,
		RevealAll
	}

	[Serializable]
	public class VesselLabelType
	{
		public Sprite sprite;

		public Color labelColor;

		public float minDrawDistance;

		public float maxDrawDistance;

		public float maxRangeDistance;

		public float maxIdentDistance;

		public bool fadeByDistance;

		public float alphaMin;

		public float alphaMax;

		public float iconSize;

		public HoverBehaviour hoverBehaviour;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public VesselLabelType()
		{
			throw null;
		}
	}

	public VesselLabel labelPrefab;

	public NodeLabel nodeLabelPrefab;

	public VesselLabelType ShipLabel;

	public VesselLabelType DebrisLabel;

	public VesselLabelType DefaultLabel;

	public VesselLabelType FlagLabel;

	public VesselLabelType EVALabel;

	public VesselLabelType BaseLabel;

	public VesselLabelType StationLabel;

	public VesselLabelType NodeLabel;

	private List<BaseLabel> labels;

	private string metricUnit;

	public static VesselLabels Instance;

	private Vector3 canvasCoordinates;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselLabels()
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
	private void OnVesselCreate(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselDestroy(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoad(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateVesselLabel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static NodeLabel AddNodeLabel(ITestNodeLabel testModule)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private NodeLabel CreateNodeLabel(ITestNodeLabel testModule)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void RemoveNodeLabel(NodeLabel label)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyVesselLabel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyAllLabels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyNodeLabels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselLabel GetLabel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public NodeLabel GetLabel(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselLabelType GetLabelType(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselLabelType GetLabelType(ITestNodeLabel testModule)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDoubleClickLabel(VesselLabel l)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessLabel(BaseLabel label, Vessel activeVessel, ITargetable target, Vector3 activePosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisableAllLabels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnableAllLabels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ITargetable GetCurrentTarget()
	{
		throw null;
	}
}
