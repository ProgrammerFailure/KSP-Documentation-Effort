using System;
using System.Collections.Generic;
using System.ComponentModel;
using Expansions.Missions.Editor;
using ns9;
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
	public string guiName = "#autoLOC_8000002";

	public bool disableKSPFields;

	public bool disableKSPActions;

	public bool disableKSPEvents;

	[KSPField(guiFormat = "S", guiActive = true, guiName = "#autoLOC_8100275")]
	public string AdjusterState = "#autoLOC_8100276";

	public PartModule adjustedModule;

	public Guid adjusterID;

	[MEGUI_Checkbox(guiName = "#autoLOC_8100277")]
	public bool canBeRepaired;

	[SerializeField]
	[HideInInspector]
	public string className;

	[SerializeField]
	[HideInInspector]
	public int classID;

	[SerializeField]
	[HideInInspector]
	public BaseEventList events;

	[HideInInspector]
	[SerializeField]
	public BaseFieldList fields;

	public string ClassName => className;

	public int ClassID => classID;

	public BaseEventList Events => events;

	public BaseFieldList Fields => fields;

	public AdjusterPartModuleBase()
	{
		adjusterID = Guid.NewGuid();
	}

	public AdjusterPartModuleBase(MENode node)
		: base(node)
	{
		adjusterID = Guid.NewGuid();
	}

	public void ModularSetup()
	{
		className = GetType().Name;
		classID = className.GetHashCode();
		events = new BaseEventList(this);
		fields = new BaseFieldList(this);
	}

	public void Setup(PartModule newPartModule)
	{
		adjustedModule = newPartModule;
		if (Fields == null)
		{
			ModularSetup();
		}
		adjustedModule.Fields.AddBaseFieldList(Fields);
		if (canBeRepaired)
		{
			Events["RemoveAdjuster"].guiName = Localizer.Format("#autoLOC_8007224", guiName);
			Events["RemoveAdjuster"].active = true;
		}
		adjustedModule.Events.AddBaseEventList(Events);
		Debug.Log("[ModuleAdjusterBase] Module Adjuster " + ClassName + " set up on " + adjustedModule.ClassName + " part module.");
	}

	public void CleanUp()
	{
		for (int i = 0; i < Fields.Count; i++)
		{
			Fields[i].guiActive = false;
		}
		adjustedModule.Fields.RemoveBaseFieldList(Fields);
		for (int j = 0; j < Events.Count; j++)
		{
			Events.GetByIndex(j).active = false;
		}
		adjustedModule.Events.RemoveBaseEventList(Events);
		Debug.Log("[ModuleAdjusterBase] Module Adjuster " + ClassName + " removed from " + adjustedModule.ClassName + " part module.");
	}

	[KSPEvent(active = false, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 4f, guiName = "Repair Part Failure")]
	public void RemoveAdjuster()
	{
		if (FlightGlobals.ActiveVessel.VesselValues.FailureRepairSkill.value < 1)
		{
			ScreenMessages.PostScreenMessage("#autoLOC_8100278");
		}
		else
		{
			adjustedModule.RemovePartModuleAdjuster(this);
		}
	}

	public void UpdateStatusMessage(string newStatusMessage)
	{
		AdjusterState = newStatusMessage;
	}

	public List<Type> GetPartModulesThatCanBeAffected()
	{
		List<Type> list = new List<Type>();
		List<Type> partModuleTargetList = GetPartModuleTargetList();
		Type targetPartModule = GetTargetPartModule();
		if (targetPartModule != null)
		{
			partModuleTargetList.AddUnique(targetPartModule);
		}
		for (int i = 0; i < partModuleTargetList.Count; i++)
		{
			list.AddUnique(partModuleTargetList[i]);
			List<Type> subclassesOfParentClass = AssemblyLoader.GetSubclassesOfParentClass(partModuleTargetList[i]);
			for (int j = 0; j < subclassesOfParentClass.Count; j++)
			{
				list.AddUnique(subclassesOfParentClass[j]);
			}
		}
		return list;
	}

	public virtual Type GetTargetPartModule()
	{
		return null;
	}

	public virtual List<Type> GetPartModuleTargetList()
	{
		return new List<Type>();
	}

	public virtual void Activate()
	{
	}

	public virtual void Deactivate()
	{
	}

	public virtual void OnPartModuleAdjusterListModified()
	{
	}

	public override string GetDisplayName()
	{
		return Localizer.Format(guiName);
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "guiName" && field.GetValue() != null)
		{
			return field.GetValue().ToString();
		}
		return base.GetNodeBodyParameterString(field);
	}

	public static List<AdjusterPartModuleBase> CreateModuleAdjusterList(ConfigNode node)
	{
		List<AdjusterPartModuleBase> list = new List<AdjusterPartModuleBase>();
		for (int i = 0; i < node.nodes.Count; i++)
		{
			if (node.nodes[i].name == "ADJUSTERMODULE")
			{
				AdjusterPartModuleBase adjusterPartModuleBase = CreateInstanceOfModuleAdjuster(node.nodes[i]);
				if (adjusterPartModuleBase != null)
				{
					list.Add(adjusterPartModuleBase);
				}
			}
		}
		return list;
	}

	public static AdjusterPartModuleBase CreateInstanceOfModuleAdjuster(ConfigNode node)
	{
		AdjusterPartModuleBase adjusterPartModuleBase = null;
		string value = "";
		if (node.TryGetValue("name", ref value))
		{
			adjusterPartModuleBase = CreateInstanceOfModuleAdjuster(value);
			adjusterPartModuleBase?.Load(node);
		}
		return adjusterPartModuleBase;
	}

	public static AdjusterPartModuleBase CreateInstanceOfModuleAdjuster(string className)
	{
		Type classByName = AssemblyLoader.GetClassByName(typeof(DynamicModule), className);
		object obj = null;
		if (classByName != null)
		{
			obj = Activator.CreateInstance(classByName, new object[1]);
		}
		return obj as AdjusterPartModuleBase;
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("canBeRepaired", canBeRepaired);
		node.AddValue("adjusterID", adjusterID.ToString());
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		if (node.HasValue("canBeRepaired"))
		{
			canBeRepaired = bool.Parse(node.GetValue("canBeRepaired"));
		}
		else
		{
			canBeRepaired = false;
		}
		if (node.HasValue("adjusterID"))
		{
			adjusterID = new Guid(node.GetValue("adjusterID"));
		}
		else
		{
			adjusterID = Guid.NewGuid();
		}
	}
}
