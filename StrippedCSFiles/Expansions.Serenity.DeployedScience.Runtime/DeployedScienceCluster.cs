using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Serenity.DeployedScience.Runtime;

public class DeployedScienceCluster : MonoBehaviour, IConfigNode
{
	[CompilerGenerated]
	private sealed class _003CUpdateScience_003Ed__56 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public DeployedScienceCluster _003C_003E4__this;

		private int _003Ci_003E5__2;

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
		public _003CUpdateScience_003Ed__56(int _003C_003E1__state)
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
	private bool isPowered;

	public CelestialBody DeployedBody;

	[SerializeField]
	private bool partialPower;

	[SerializeField]
	private float partialPowerMultiplier;

	public List<DeployedSciencePart> DeployedScienceParts;

	[SerializeField]
	private int powerRequired;

	[SerializeField]
	private int powerAvailable;

	[SerializeField]
	internal List<DeployedSciencePart> solarPanelParts;

	[SerializeField]
	internal List<DeployedSciencePart> antennaParts;

	[SerializeField]
	private uint controlModulePartId;

	[SerializeField]
	private bool controllerPartEnabled;

	[SerializeField]
	internal double lastScienceGeneratedUT;

	[SerializeField]
	internal double lastScienceTransmittedUT;

	[SerializeField]
	private bool updatingScience;

	[SerializeField]
	private bool dataSendFailed;

	public bool IsPowered
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool PartialPower
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float PartialPowerMultiplier
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int PowerRequired
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int PowerAvailable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<DeployedSciencePart> SolarPanelParts
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HasSolarPanels
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<DeployedSciencePart> AntennaParts
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public uint ControlModulePartId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool ControllerPartEnabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double LastScienceGeneratedUT
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double LastScienceTransmittedUT
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool UpdatingScience
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

	public bool DataSendFailed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeployedScienceCluster()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static DeployedScienceCluster Spawn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static DeployedScienceCluster Spawn(ModuleGroundExpControl controlUnit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static DeployedScienceCluster SpawnandLoad(ConfigNode node)
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
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int SolarPanelUnitsProduced()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdatePowerState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateCluster(ModuleGroundExpControl controlUnit, bool replaceList, List<ModuleGroundSciencePart> scienceParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateExperimentDiminishReturns()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GroundSciencePartEnabledStateChanged(ModuleGroundSciencePart sciencePart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GroundScienceModuleRemoved(ModuleGroundSciencePart part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CUpdateScience_003Ed__56))]
	public IEnumerator UpdateScience()
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
