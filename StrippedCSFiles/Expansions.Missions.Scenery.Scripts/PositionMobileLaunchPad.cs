using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Scenery.Scripts;

public class PositionMobileLaunchPad : MonoBehaviour
{
	[Serializable]
	public class ExtensionPoint
	{
		public Transform Pivot;

		public Transform Anchor;

		public Vector3 AnchorOriginalPosition;

		public Transform Stretch;

		public Vector3 StretchOriginalPosition;

		public float initialHeight;

		public float scaleFactor;

		public float height;

		public float baseHeight;

		public float anchorHeight;

		public float hitDistance;

		public float distanceMoved;

		public Vector3[] Points;

		public Material material;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ExtensionPoint()
		{
			throw null;
		}
	}

	public ExtensionPoint[] ExtensionPoints;

	public Transform[] BottomPoints;

	public float minimumModelClearance;

	public float waterModelClearance;

	public PQSCity2 City;

	public LaunchSite launchSite;

	public Transform RampStart;

	public Transform RampEnd;

	public Transform[] RampObjects;

	[SerializeField]
	private bool positioningComplete;

	public bool hideRampOverMax;

	public bool scaleMaterial;

	public Transform[] hideOnWaterSurface;

	public Transform[] showOnWaterSurface;

	[SerializeField]
	private bool waterSurfaceMode;

	public bool PositioningComplete
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PositionMobileLaunchPad()
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
	[ContextMenu("Reset")]
	public void ResetPositioning()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("ScenerySettingChange/Reset Legs")]
	public void OnScenerySettingChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void CompleteOrientation(CelestialBody body, string cityName, bool overrideVisible = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckForVessels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleObjects(bool setforGroundMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MoveModel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckGrounded(Transform objectStart, Transform objectEnd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleRampObjects(bool setActive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Collider FindCollider(Transform xform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 MakePoint(Transform xform, Vector3 c, float x, float y, float z)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreatePoints()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ExtendLegs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetLegs()
	{
		throw null;
	}
}
