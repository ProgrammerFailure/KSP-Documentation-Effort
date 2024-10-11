using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Misc/City LOD Controller")]
public class PQSCity : PQSSurfaceObject
{
	[Serializable]
	public class LODRange
	{
		public float visibleRange;

		public GameObject[] renderers;

		private List<Renderer> rendererList;

		public GameObject[] objects;

		private bool isActive;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LODRange()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Setup()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetActive(bool active)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CPostSetup_003Ed__44 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PQSCity _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CPostSetup_003Ed__44(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	protected Vector3d planetRelativePosition;

	[HideInInspector]
	public bool debugOrientated;

	public LODRange[] lod;

	public float frameDelta;

	public bool randomizeOnSphere;

	public bool repositionToSphere;

	public bool repositionToSphereSurface;

	public bool repositionToSphereSurfaceAddHeight;

	public Vector3 repositionRadial;

	public double repositionRadiusOffset;

	public bool reorientToSphere;

	public Vector3 reorientInitialUp;

	public float reorientFinalAngle;

	public double lat;

	public double lon;

	public double alt;

	public PSystemSetup.SpaceCenterFacility spaceCenterFacility;

	public LaunchSite launchSite;

	private Collider[] colliders;

	private bool transformsActive;

	private bool isLoaded;

	private double loadRange;

	private bool inPOIRange;

	private double poiRange;

	[SerializeField]
	private bool useIndividualPOIRange;

	[SerializeField]
	public float individualPOIRange;

	private CelestialBody body;

	private Vector3 planetPosition;

	private Quaternion planetRotation;

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

	public CelestialBody celestialBody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSCity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ResetCelestialBody()
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
	[IteratorStateMachine(typeof(_003CPostSetup_003Ed__44))]
	private IEnumerator PostSetup()
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
	public void Randomize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Orientate(bool allowReposition = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OrientateToOrigin()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OrientateToWorld()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheckLocals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetPQSCitySeed(PQSCity city)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetPQSCitySeed(string name, string body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int GetStableHashCode(string str, bool Bit32 = false)
	{
		throw null;
	}
}
