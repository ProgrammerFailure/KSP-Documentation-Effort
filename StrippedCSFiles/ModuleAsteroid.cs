using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleAsteroid : PartModule, IVesselAutoRename, IPartMassModifier
{
	[CompilerGenerated]
	private sealed class _003CPostInit_003Ed__26 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleAsteroid _003C_003E4__this;

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
		public _003CPostInit_003Ed__26(int _003C_003E1__state)
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

	private FlightCoMTracker CoMnode;

	[KSPField(isPersistant = true)]
	public int seed;

	[KSPField(isPersistant = true)]
	public string AsteroidName;

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

	private ProceduralAsteroid paPrefab;

	private PAsteroid paGenerated;

	private float radius;

	private float asteroidMass;

	private Transform modelTransform;

	private bool wasStarted;

	private ScienceExperiment experiment;

	private ScienceSubject subject;

	private ScienceData experimentData;

	private PopupDialog renameDialog;

	private string newName;

	private static string cacheAutoLOC_230121;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleAsteroid()
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
	[IteratorStateMachine(typeof(_003CPostInit_003Ed__26))]
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
	public void SetAsteroidMass(float nMass)
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
	[KSPEvent(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 50f, guiName = "#autoLOC_6001851")]
	public void RenameAsteroidEvent()
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
	private void renameAsteroid(string newName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void VesselsUndockingAsteroid(Vessel oldVessel, Vessel newVessel)
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
