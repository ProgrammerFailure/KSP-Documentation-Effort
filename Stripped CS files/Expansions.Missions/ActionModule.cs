using System.Collections;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

public class ActionModule : MonoBehaviour, IConfigNode, IActionModule, IMENodeDisplay
{
	public string title = "";

	public List<string> parametersDisplayedInSAP;

	public bool restartOnSceneLoad;

	public bool isRunning;

	public MENode node;

	public new string name { get; set; }

	public void VesselPersistentIdChangedWrapper(uint oldId, uint newId)
	{
		if (oldId != 0)
		{
			OnVesselPersistentIdChanged(oldId, newId);
		}
	}

	public virtual void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
	}

	public void PartPersistentIdChangedWrapper(uint vesselID, uint oldId, uint newId)
	{
		if (vesselID != 0 && oldId != 0)
		{
			OnPartPersistentIdChanged(vesselID, oldId, newId);
		}
	}

	public virtual void OnPartPersistentIdChanged(uint vesselID, uint oldId, uint newId)
	{
	}

	public void VesselsUndockingWrapper(Vessel oldVessel, Vessel newVessel)
	{
		OnVesselsUndocking(oldVessel, newVessel);
	}

	public void VesselDockingWrapper(uint oldId, uint newId)
	{
		OnVesselsDocking(oldId, newId);
	}

	public virtual void OnVesselsUndocking(Vessel oldVessel, Vessel newVessel)
	{
	}

	public virtual void OnVesselsDocking(uint oldId, uint newId)
	{
	}

	public virtual void Initialize(MENode node)
	{
		this.node = node;
	}

	public virtual void Awake()
	{
		name = GetType().Name;
		parametersDisplayedInSAP = new List<string>();
		GameEvents.onPartPersistentIdChanged.Add(PartPersistentIdChangedWrapper);
		GameEvents.onVesselPersistentIdChanged.Add(VesselPersistentIdChangedWrapper);
		GameEvents.onVesselsUndocking.Add(VesselsUndockingWrapper);
		GameEvents.onVesselDocking.Add(VesselDockingWrapper);
	}

	public void Start()
	{
		byte[] hashBytes = null;
		string signature = null;
		if (node != null && node.mission != null)
		{
			hashBytes = node.mission.HashBytes;
			signature = node.mission.Signature;
		}
		if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory", hashBytes, signature))
		{
			Object.Destroy(this);
		}
	}

	public virtual void OnDestroy()
	{
		GameEvents.onPartPersistentIdChanged.Remove(PartPersistentIdChangedWrapper);
		GameEvents.onVesselPersistentIdChanged.Remove(VesselPersistentIdChangedWrapper);
		GameEvents.onVesselsUndocking.Remove(VesselsUndockingWrapper);
		GameEvents.onVesselDocking.Remove(VesselDockingWrapper);
	}

	public virtual IEnumerator Fire()
	{
		yield return null;
	}

	public virtual Vector3 ActionLocation()
	{
		return Vector3.zero;
	}

	public virtual void OnCloned(ref ActionModule actionModuleBase)
	{
	}

	public void RunValidationWrapper(MissionEditorValidator validator)
	{
		RunValidation(validator);
	}

	public virtual void RunValidation(MissionEditorValidator validator)
	{
	}

	public string GetName()
	{
		return name;
	}

	public string GetDisplayName()
	{
		return Localizer.Format(title);
	}

	public void AddParameterToSAP(string parameter)
	{
		parametersDisplayedInSAP.AddUnique(parameter);
	}

	public void RemoveParameterFromSAP(string parameter)
	{
		parametersDisplayedInSAP.Remove(parameter);
		if (node.HasNodeBodyParameter(name, parameter))
		{
			node.RemoveParameterFromNodeBody(name, parameter);
			UpdateNodeBodyUI();
		}
	}

	public void AddParameterToNodeBody(string parameter)
	{
		node.AddParameterToNodeBody(name, parameter);
	}

	public void AddParameterToNodeBodyAndUpdateUI(string parameter)
	{
		node.AddParameterToNodeBody(name, parameter);
		UpdateNodeBodyUI();
	}

	public void RemoveParameterFromNodeBody(string parameter)
	{
		node.RemoveParameterFromNodeBody(name, parameter);
	}

	public void RemoveParameterFromNodeBodyAndUpdateUI(string parameter)
	{
		node.RemoveParameterFromNodeBody(name, parameter);
		UpdateNodeBodyUI();
	}

	public bool HasNodeBodyParameter(string parameter)
	{
		return node.HasNodeBodyParameter(name, parameter);
	}

	public bool HasSAPParameter(string parameter)
	{
		return parametersDisplayedInSAP.Contains(parameter);
	}

	public virtual string GetNodeBodyParameterString(BaseAPField field)
	{
		string text = Localizer.Format("#autoLOC_6003083");
		if (field.GetValue() != null)
		{
			text = MissionsUtils.GetFieldValueForDisplay(field);
		}
		return Localizer.Format("#autoLOC_8004190", field.guiName, text);
	}

	public void UpdateNodeBodyUI()
	{
		if (node.guiNode != null)
		{
			node.guiNode.DisplayNodeBodyParameters();
		}
	}

	public virtual List<IMENodeDisplay> GetInternalParametersToDisplay()
	{
		return new List<IMENodeDisplay>();
	}

	public MENode GetNode()
	{
		return node;
	}

	public virtual string GetInfo()
	{
		return GetType().Name;
	}

	public virtual string GetAppObjectiveInfo()
	{
		string text = "";
		BaseAPFieldList baseAPFieldList = new BaseAPFieldList(this);
		int i = 0;
		for (int count = baseAPFieldList.Count; i < count; i++)
		{
			string nodeBodyParameterString = GetNodeBodyParameterString(baseAPFieldList[i]);
			if (!string.IsNullOrEmpty(nodeBodyParameterString))
			{
				text += StringBuilderCache.Format("{0}\n", nodeBodyParameterString);
			}
		}
		return text;
	}

	public virtual void ParameterSetupComplete()
	{
	}

	public virtual void Load(ConfigNode node)
	{
		node.TryGetValue("title", ref title);
		parametersDisplayedInSAP = node.GetValuesList("parametersDisplayedInSAP");
		node.TryGetValue("restartOnSceneLoad", ref restartOnSceneLoad);
		node.TryGetValue("isRunning", ref isRunning);
	}

	public virtual void Save(ConfigNode node)
	{
		node.AddValue("name", name);
		node.AddValue("title", title);
		for (int i = 0; i < parametersDisplayedInSAP.Count; i++)
		{
			node.AddValue("parametersDisplayedInSAP", parametersDisplayedInSAP[i]);
		}
		node.AddValue("restartOnSceneLoad", restartOnSceneLoad);
		node.AddValue("isRunning", isRunning);
	}
}
