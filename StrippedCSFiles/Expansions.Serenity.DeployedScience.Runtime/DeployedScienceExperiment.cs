using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Serenity.DeployedScience.Runtime;

public class DeployedScienceExperiment : MonoBehaviour, IConfigNode
{
	[Serializable]
	public class SeismicPartData
	{
		public float velocity;

		public uint partId;

		public float mass;

		public float resourceMass;

		public double impactDistance;

		public float impactDistanceMultiplier;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SeismicPartData()
		{
			throw null;
		}
	}

	public delegate float SeismicDataCalc(DeployedScienceExperiment scienceExp, float impactJoules, float impactJoulesMaxValue, List<SeismicPartData> seismicPartData);

	[CompilerGenerated]
	private sealed class _003CCalcScience_003Ed__72 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public DeployedScienceExperiment _003C_003E4__this;

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
		public _003CCalcScience_003Ed__72(int _003C_003E1__state)
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

	[SerializeField]
	private float totalScienceGenerated;

	[SerializeField]
	private float scienceLimit;

	[SerializeField]
	private float scienceValue;

	[SerializeField]
	private float storedScienceData;

	[SerializeField]
	private float transmittedScienceData;

	[SerializeField]
	private uint partId;

	[SerializeField]
	private string experimentId;

	[SerializeField]
	private string experimentName;

	[SerializeField]
	private float scienceModifierRate;

	[SerializeField]
	private float scienceDiminishingModifierRate;

	public ScienceExperiment Experiment;

	public Vessel ExperimentVessel;

	public DeployedScienceCluster Cluster;

	public DeployedSciencePart sciencePart;

	public double LastScienceGeneratedUT;

	[SerializeField]
	private bool experimentSituationValid;

	[SerializeField]
	private ModuleGroundExperiment moduleGroundExperiment;

	public float xmitDataScalar;

	[SerializeField]
	private ScienceSubject subject;

	[SerializeField]
	private Guid vesselID;

	[SerializeField]
	private Vessel ControllerVessel;

	[SerializeField]
	private float solarMultiplier;

	[SerializeField]
	private bool seismicEventDataSendPending;

	[SerializeField]
	private bool noCommNetMsgPosted;

	[SerializeField]
	private bool noPowerMsgPosted;

	private float seismicEventVelocity;

	public List<SeismicPartData> seismicPartList;

	public FloatCurve distanceCurve;

	private double distanceMultiplier;

	private double totalJoules;

	private float impactRate;

	private SeismicDataCalc seismicDataCalcOverride;

	public float TotalScienceGenerated
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float ScienceLimit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float ScienceValue
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float StoredScienceData
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float TransmittedScienceData
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public uint PartId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string ExperimentId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string ExperimentName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float ScienceModifierRate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float ScienceDiminishingModifierRate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal set
		{
			throw null;
		}
	}

	public bool ExperimentSituationValid
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ModuleGroundExperiment GroundExperimentModule
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool SeismicEventDataSendPending
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeployedScienceExperiment()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static DeployedScienceExperiment Spawn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static DeployedScienceExperiment Spawn(ModuleGroundExperiment moduleGroundExperiment, DeployedScienceCluster cluster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static DeployedScienceExperiment SpawnandLoad(ConfigNode node)
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
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SeismicDataCalcOverrideAdd(SeismicDataCalc method)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SeismicDataCalcOverrideRemove(SeismicDataCalc method)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GroundScienceClusterPowerStateChanged(DeployedScienceCluster cluster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GroundSciencePartEnabledStateChanged(ModuleGroundSciencePart sciencePart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CCalcScience_003Ed__72))]
	private IEnumerator CalcScience()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateScienceLimitExperiment(ModuleGroundExperiment moduleGroundExperiment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateScienceValueExperiment(ModuleGroundExperiment moduleGroundExperiment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateScienceExperiment(ModuleGroundExperiment moduleGroundExperiment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CalculateScience(float solarMultiplier)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CalcSendSeismicScience(Vessel vessel, Part part, float solarMultiplier)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void processImpactVesselPart(Vessel vessel, Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool SeismicListContains(uint partId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessSeismicEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TimeToSendStoredData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GatherScienceData(out ScienceData experimentData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SendDataToComms()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetControllerVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}
