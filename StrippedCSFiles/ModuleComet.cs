using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleComet : PartModule, IVesselAutoRename, IPartMassModifier
{
	[CompilerGenerated]
	private sealed class _003CPostInit_003Ed__52 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleComet _003C_003E4__this;

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
		public _003CPostInit_003Ed__52(int _003C_003E1__state)
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

	internal CometVessel cometVessel;

	private FlightCoMTracker CoMnode;

	[KSPField(isPersistant = true)]
	public int seed;

	[KSPField(isPersistant = true)]
	public string CometName;

	[KSPField(isPersistant = true)]
	public string CometType;

	[KSPField(isPersistant = true)]
	public string prefabBaseURL;

	private const int nullState = 0;

	private const int primaryState = 1;

	private const int secondaryState = 2;

	[KSPField(isPersistant = true)]
	public int currentState;

	[KSPField]
	public float secondaryRate;

	[KSPField]
	public float minRadiusMultiplier;

	[KSPField]
	public float maxRadiusMultiplier;

	[KSPField]
	public float density;

	[KSPField]
	public string sampleExperimentId;

	[KSPField]
	public float sampleExperimentXmitScalar;

	[KSPField]
	public int experimentUsageMask;

	[KSPField]
	public bool forceProceduralDrag;

	private ProceduralComet pcPrefab;

	internal PComet pcGenerated;

	private float radius;

	private float cometMass;

	private Transform modelTransform;

	[KSPField]
	internal float fragmentDynamicPressureModifier;

	internal bool optimizedCollider;

	public int numGeysers;

	public int numNearDustEmitters;

	private List<GameObject> geysers;

	private List<ParticleSystem> geyserParticles;

	private List<float> geyserParticleOriginalRateOverTime;

	private bool geyserParticlesActive;

	private List<GameObject> nearDustParticleObjects;

	private List<ParticleSystem> nearDustParticles;

	private List<float> nearDustParticleOriginalRateOverTime;

	private bool nearDustParticlesActive;

	private Vector3d cometOffset;

	private Vector3d lastPosition;

	private Renderer cometRenderer;

	private bool offsetMaterialUpdates;

	[SerializeField]
	private float offsetMaterialCamDistance;

	private bool changedNearFXSimRate;

	public bool canSpawnFragments;

	internal float debugGeyserEmitterMultiplier;

	internal float debugDustEmitterMultiplier;

	private bool wasStarted;

	[KSPField(isPersistant = true)]
	private double utFragmentSpawned;

	private double fragmentSafeTime;

	private ScienceExperiment experiment;

	private ScienceSubject subject;

	private ScienceData experimentData;

	private PopupDialog renameDialog;

	private string newName;

	private static string cacheAutoLOC_230121;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleComet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
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
	public override void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnFixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CPostInit_003Ed__52))]
	public IEnumerator PostInit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float Rangefinder(Transform t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGenComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateDragCube()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCometMass(float nMass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = false, guiActive = true, unfocusedRange = 500f, guiName = "#autoLOC_6001849")]
	public void MakeTarget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckExplosion()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, unfocusedRange = 1f, guiName = "#autoLOC_6001850")]
	public void TakeSampleEVAEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void performSampleExperiment(ModuleScienceContainer collector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<GameObject> CreateEmittersOnSurface(GameObject emitterPrefab, int numberOfEmitters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ActivateGeysers(bool activate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ActivateNearDust(bool activate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnTimeWarpRateChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetGeyser(bool enabled, float emissionRate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetDust(bool enabled, float emissionRate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 50f, guiName = "#autoLOC_6006052")]
	public void RenameCometEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIBase[] drawRenameWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onRenameConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onRenameDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void renameComet(string newName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselType GetVesselType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetVesselName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
