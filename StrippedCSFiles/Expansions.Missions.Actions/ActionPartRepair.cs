using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions.Actions;

public class ActionPartRepair : ActionModule
{
	public enum RepairChoices
	{
		[Description("#autoLOC_8100115")]
		entireVessel,
		[Description("#autoLOC_8100116")]
		onePart,
		[Description("#autoLOC_8100117")]
		partModule,
		[Description("#autoLOC_8100118")]
		failureID
	}

	[CompilerGenerated]
	private sealed class _003CFire_003Ed__23 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ActionPartRepair _003C_003E4__this;

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
		public _003CFire_003Ed__23(int _003C_003E1__state)
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

	[MEGUI_VesselPartSelect(onValueChange = "OnPartIDChanged", onControlCreated = "VesselPartSelectorControlCreated", resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8100120")]
	public VesselPartIDPair vesselPartIDs;

	[MEGUI_Dropdown(onDropDownValueChange = "OnRepairTypeValueChange", SetDropDownItems = "SetRepairTypeDropdown", onControlCreated = "OnRepairTypeControlCreated", gapDisplay = false, guiName = "#autoLOC_8100119")]
	public RepairChoices repairType;

	private MEGUIParameterDropdownList RepairTypeDropdown;

	[MEGUI_Dropdown(SetDropDownItems = "SetPartModuleNamesForDropdown", canBePinned = false, onControlCreated = "OnPartModuleDropdownControlCreated", hideOnSetup = true, guiName = "#autoLOC_8100121")]
	public string partModule;

	[MEGUI_Dropdown(SetDropDownItems = "SetFailureNamesForDropdown", canBePinned = false, onControlCreated = "OnFailureDropdownControlCreated", hideOnSetup = true, guiName = "#autoLOC_8100122")]
	public Guid failureID;

	private MEGUIParameterDropdownList FailureDropdown;

	private VesselSituation vesselSituation;

	private bool playerCreated;

	private MEGUIParameterDropdownList PartModuleDropdown;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActionPartRepair()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void VesselPartSelectorControlCreated(MEGUIParameterVesselPartSelector sender)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRepairTypeControlCreated(MEGUIParameterDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartModuleDropdownControlCreated(MEGUIParameterDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFailureDropdownControlCreated(MEGUIParameterDropdownList parameter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<MEGUIDropDownItem> SetRepairTypeDropdown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<MEGUIDropDownItem> SetPartModuleNamesForDropdown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<MEGUIDropDownItem> SetFailureNamesForDropdown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRepairTypeValueChange(MEGUIParameterDropdownList sender, int newIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onFailureListChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartIDChanged(VesselPartIDPair newVesselPartIDs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselSituationChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CFire_003Ed__23))]
	public override IEnumerator Fire()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RepairFailures(Vessel vessel, string repairpartModule = "", bool useGuid = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RepairFailures(Part part, string repairpartModule = "", bool useGuid = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RepairFailures(ProtoVessel vessel, string repairpartModule = "", bool useGuid = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RepairFailures(ProtoPartSnapshot part, string repairpartModule = "", bool useGuid = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void RunValidation(MissionEditorValidator validator)
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
