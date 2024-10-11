using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using UnityEngine;

namespace Expansions.Missions.Actions;

public class ActionCreateKerbal : ActionModule, INodeOrbit, IMissionKerbal
{
	[CompilerGenerated]
	private sealed class _003CFire_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ActionCreateKerbal _003C_003E4__this;

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
		public _003CFire_003Ed__13(int _003C_003E1__state)
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

	[MEGUI_MissionKerbal(tabStop = true, statusToShow = ProtoCrewMember.RosterStatus.Available, showStranded = false, showAllRosterStatus = false, gapDisplay = true, guiName = "#autoLOC_8007219")]
	public MissionKerbal missionKerbal;

	[MEGUI_Checkbox(onValueChange = "OnHelmetValueChange", order = 8, guiName = "#autoLOC_6010016", Tooltip = "#autoLOC_6010017")]
	public bool isHelmetEnabled;

	[MEGUI_Checkbox(order = 9, onControlCreated = "OnNeckRingControlCreated", guiName = "#autoLOC_6010018", Tooltip = "#autoLOC_6010019")]
	public bool isNeckRingEnabled;

	[MEGUI_Checkbox(order = 10, resetValue = "false", guiName = "#autoLOC_8000024", Tooltip = "#autoLOC_8000026")]
	public bool isStranded;

	[MEGUI_ParameterSwitchCompound(order = 20, guiName = "#autoLOC_8000027")]
	public ParamChoices_VesselSimpleLocation location;

	public uint persistentId;

	private string kerbalName;

	private double TimeDeadline;

	private float kerbalRotationOffset;

	private MEGUIParameterCheckbox neckRingCheckbox;

	private float defaultKerbalFeetToPivotDist;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActionCreateKerbal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Initialize(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CFire_003Ed__13))]
	public override IEnumerator Fire()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal ConfigNode SpawnKerbal(bool addtoCurrentGame = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode GetProtoVesselNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode GetProtoVesselNode(float feetToPivotDistance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ConfigNode CreateKerbalPartNode(uint id, params ProtoCrewMember[] crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Vector3 ActionLocation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnCloned(ref ActionModule actionModuleBase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasNodeOrbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Orbit GetNodeOrbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNeckRingControlCreated(MEGUIParameterCheckbox control)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHelmetValueChange(bool newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void NodeDeleted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void KerbalRosterStatusChange(ProtoCrewMember kerbal, ProtoCrewMember.RosterStatus oldStatus, ProtoCrewMember.RosterStatus newStatus)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void KerbalAdded(ProtoCrewMember kerbal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void KerbalTypeChange(ProtoCrewMember kerbal, ProtoCrewMember.KerbalType oldType, ProtoCrewMember.KerbalType newType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void KerbalNameChange(ProtoCrewMember kerbal, string oldName, string newName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void KerbalRemoved(ProtoCrewMember kerbal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node)
	{
		throw null;
	}
}
