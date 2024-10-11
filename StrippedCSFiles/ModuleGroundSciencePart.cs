using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Expansions.Serenity.DeployedScience.Runtime;
using UnityEngine;

public class ModuleGroundSciencePart : ModuleGroundPart
{
	[CompilerGenerated]
	private sealed class _003CSunTrackerToOrigin_003Ed__59 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleGroundSciencePart _003C_003E4__this;

		private Quaternion _003CrotationAngle_003E5__2;

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
		public _003CSunTrackerToOrigin_003Ed__59(int _003C_003E1__state)
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

	[KSPField(guiActiveUnfocused = true, isPersistant = false, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002231")]
	public string ConnectionState;

	[KSPField(guiActiveUnfocused = true, isPersistant = false, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002232")]
	public string PowerState;

	[KSPField(guiActiveUnfocused = false, isPersistant = true, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8002353")]
	[SerializeField]
	protected int powerUnitsProduced;

	[KSPField(guiActiveEditor = true, guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002235")]
	[SerializeField]
	protected int actualPowerUnitsProduced;

	[KSPField(guiActiveEditor = true, guiActiveUnfocused = true, isPersistant = true, guiActive = true, unfocusedRange = 20f, guiName = "#autoLOC_8002236")]
	[SerializeField]
	protected int powerUnitsRequired;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	protected bool isSolarPanel;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
	public uint ControlUnitId;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = false)]
	public float DistanceToController;

	[KSPField(unfocusedRange = 20f, guiActiveUnfocused = true, isPersistant = false, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_8002252")]
	[SerializeField]
	protected bool moduleEnabled;

	[SerializeField]
	private DeployedScienceCluster scienceClusterData;

	[KSPField]
	public bool TrackSun;

	[KSPField]
	public string secondaryTransformName;

	[KSPField]
	public float raycastOffset;

	[KSPField]
	public string pivotName;

	[KSPField]
	public float trackingSpeed;

	[KSPField]
	public string localRotationAxis;

	[KSPField]
	public Vector3 targetRotationAngle;

	[KSPField]
	public float packingRotationMultiplier;

	protected UIPartActionWindow partActionWindow;

	protected RaycastHit hit;

	protected bool trackingLOS;

	protected string blockingObject;

	protected CelestialBody trackingBody;

	protected Transform secondaryTransform;

	protected int planetLayerMask;

	protected int defaultLayerMask;

	protected Transform trackingTransformScaled;

	protected Transform panelRotationTransform;

	private static string cacheAutoLOC_7003285;

	private static string cacheAutoLOC_8002238;

	private static string cacheAutoLOC_8002239;

	private static string cacheAutoLOC_8002240;

	private static string cacheAutoLOC_8002241;

	private static string cacheAutoLOC_8002242;

	private static string cacheAutoLOC_8002243;

	private static string cacheAutoLOC_8002244;

	private static string cacheAutoLOC_8002253;

	private static string cacheAutoLOC_6006034;

	public int PowerUnitsProduced
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

	public int ActualPowerUnitsProduced
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

	public int PowerUnitsRequired
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

	public bool IsSolarPanel
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

	public bool Enabled
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

	public bool DeployedOnGround
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public DeployedScienceCluster ScienceClusterData
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleGroundSciencePart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 4f, guiName = "")]
	public void ToggleActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction(guiName = "#autoLOC_8002237")]
	public void ToggleActiveAction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CalculateTrackingLOS(Vector3 trackingDirection, ref string blocker)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetDeployedOnGround()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void RetrievePart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSunTrackerToOrigin_003Ed__59))]
	private IEnumerator SunTrackerToOrigin()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGroundScienceClusterUpdated(ModuleGroundExpControl controlUnit, DeployedScienceCluster cluster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGroundScienceClusterPowerStateChanged(DeployedScienceCluster scienceCluster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGroundScienceDeregisterCluster(uint controlUnitId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartActionUIOpened(UIPartActionWindow window, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartActionUIDismiss(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateModuleUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetTooltip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal new static void CacheLocalStrings()
	{
		throw null;
	}
}
