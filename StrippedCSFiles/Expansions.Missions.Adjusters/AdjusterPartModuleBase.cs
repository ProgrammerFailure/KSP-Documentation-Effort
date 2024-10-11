using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public abstract class AdjusterPartModuleBase : DynamicModule, IConfigNode, IPartModuleAdjuster
{
	public enum FailureDeploymentState
	{
		[Description("#autoLOC_8005002")]
		CurrentState,
		[Description("#autoLOC_6002269")]
		Deployed,
		[Description("#autoLOC_6001081")]
		Retracted
	}

	public enum FailureActivationState
	{
		[Description("#autoLOC_8005002")]
		CurrentState,
		[Description("#autoLOC_8005003")]
		On,
		[Description("#autoLOC_8005004")]
		Off
	}

	public enum FailureOpenState
	{
		[Description("#autoLOC_8005002")]
		CurrentState,
		[Description("#autoLOC_8005009")]
		Open,
		[Description("#autoLOC_8005416")]
		Closed
	}

	[MEGUI_Label(hideWhenSiblingsExist = true)]
	protected string guiName;

	public bool disableKSPFields;

	public bool disableKSPActions;

	public bool disableKSPEvents;

	[KSPField(guiFormat = "S", guiActive = true, guiName = "#autoLOC_8100275")]
	public string AdjusterState;

	protected PartModule adjustedModule;

	public Guid adjusterID;

	[MEGUI_Checkbox(guiName = "#autoLOC_8100277")]
	public bool canBeRepaired;

	[SerializeField]
	[HideInInspector]
	private string className;

	[SerializeField]
	[HideInInspector]
	private int classID;

	[SerializeField]
	[HideInInspector]
	private BaseEventList events;

	[HideInInspector]
	[SerializeField]
	private BaseFieldList fields;

	public string ClassName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int ClassID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public BaseEventList Events
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public BaseFieldList Fields
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterPartModuleBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterPartModuleBase(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ModularSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(PartModule newPartModule)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CleanUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = false, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 4f, guiName = "Repair Part Failure")]
	public void RemoveAdjuster()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateStatusMessage(string newStatusMessage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<Type> GetPartModulesThatCanBeAffected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Type GetTargetPartModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual List<Type> GetPartModuleTargetList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Deactivate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnPartModuleAdjusterListModified()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<AdjusterPartModuleBase> CreateModuleAdjusterList(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AdjusterPartModuleBase CreateInstanceOfModuleAdjuster(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AdjusterPartModuleBase CreateInstanceOfModuleAdjuster(string className)
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
