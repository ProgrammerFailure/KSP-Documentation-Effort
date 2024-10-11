using System;
using System.Runtime.CompilerServices;
using Expansions.Missions.Scenery.Scripts;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Misc/City2")]
public class PQSCity2 : PQSSurfaceObject
{
	[Serializable]
	public class LodObject
	{
		public float visibleRange;

		public GameObject[] objects;

		protected bool isActive;

		public bool KeepActive;

		public float visibleRangeSqr
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			protected set
			{
				throw null;
			}
		}

		public bool IsActive
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LodObject()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual void Setup()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual void SetRange()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual void SetActive(bool active)
		{
			throw null;
		}
	}

	protected Vector3d planetRelativePosition;

	public LodObject[] objects;

	public string objectName;

	public string displayobjectName;

	public double lat;

	public double lon;

	public double alt;

	public Vector3 up;

	public double rotation;

	public Transform PositioningPoint;

	public bool snapToSurface;

	public bool raycastSurface;

	public bool setOnWaterSurface;

	public double snapHeightOffset;

	public bool baseRotFacesNorthPole;

	public PSystemSetup.SpaceCenterFacility spaceCenterFacility;

	public LaunchSite launchSite;

	[SerializeField]
	private PositionMobileLaunchPad positionMobileLaunchPad;

	public CrashObjectName crashObjectName;

	private bool inPOIRange;

	private double poiRange;

	[SerializeField]
	private bool useIndividualPOIRange;

	[SerializeField]
	public float individualPOIRange;

	[SerializeField]
	private bool inVisibleRange;

	[SerializeField]
	private bool raycastCompleted;

	[SerializeField]
	private bool positioningCompleted;

	[SerializeField]
	private bool positionedWhenVisible;

	public bool OrientateOnStart;

	public bool OrientateOnPostSetup;

	private CelestialBody body;

	public override Vector3d PlanetRelativePosition
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public override string SurfaceObjectName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public override string DisplaySurfaceObjectName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool InPOIRange
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double POIRange
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool InVisibleRange
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool PositioningCompleted
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public CelestialBody celestialBody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSCity2()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetBody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Orientate")]
	public virtual void Orientate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool OrientateOnWater(Vector3d surfPosNorm, Transform sphereTarget)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double OrientateRayCast(Vector3d surfPosNorm, double surfaceHeight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetInactive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Reset")]
	internal virtual void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnScenerySettingChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool OnSphereStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPostSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSphereReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSphereActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSphereInactive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdateFinished()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setVisibility(bool showMsg = false)
	{
		throw null;
	}
}
