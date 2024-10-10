using System.Collections.Generic;
using Expansions.Missions.Editor;
using FinePrint;
using ns9;
using UnityEngine;

namespace Expansions.Missions;

public class TestModule : MonoBehaviour, IConfigNode, ITestModule, IMENodeDisplay
{
	public string title = "";

	public List<string> parametersDisplayedInSAP;

	public bool hasWaypoint;

	public Waypoint waypoint;

	public bool hasNodeLabel;

	public NodeLabel nodeLabel;

	public bool hasOrbit;

	public new string name { get; set; }

	public TestGroup testGroup { get; set; }

	public MENode node { get; set; }

	public virtual void Initialize(TestGroup testGroup, string title = "")
	{
		if (!string.IsNullOrEmpty(title))
		{
			this.title = title;
		}
		this.testGroup = testGroup;
		node = testGroup.node;
		hasWaypoint = this is INodeWaypoint;
		hasNodeLabel = this is ITestNodeLabel;
		hasOrbit = this is INodeOrbit;
	}

	public virtual void Awake()
	{
		name = GetType().Name;
		parametersDisplayedInSAP = new List<string>();
		SetupPersistentIdEvents();
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
		GameEvents.onVesselDocking.Remove(VesselDockingWrapper);
		GameEvents.onVesselsUndocking.Remove(VesselsUndockingWrapper);
	}

	public void SetupPersistentIdEvents()
	{
		GameEvents.onPartPersistentIdChanged.Add(PartPersistentIdChangedWrapper);
		GameEvents.onVesselPersistentIdChanged.Add(VesselPersistentIdChangedWrapper);
		GameEvents.onVesselDocking.Add(VesselDockingWrapper);
		GameEvents.onVesselsUndocking.Add(VesselsUndockingWrapper);
	}

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

	public void VesselDockingWrapper(uint oldId, uint newId)
	{
		OnVesselDocking(oldId, newId);
	}

	public virtual void OnVesselDocking(uint oldId, uint newId)
	{
	}

	public void VesselsUndockingWrapper(Vessel oldVessel, Vessel newVessel)
	{
		OnVesselsUndocking(oldVessel, newVessel);
	}

	public virtual void OnVesselsUndocking(Vessel oldVessel, Vessel newVessel)
	{
	}

	public TestModule InitializeTest()
	{
		GameEvents.Mission.onTestInitialized.Fire(this);
		if (hasWaypoint && this is INodeWaypoint nodeWaypoint && nodeWaypoint.HasNodeWaypoint())
		{
			waypoint = nodeWaypoint.GetNodeWaypoint();
			if (waypoint != null)
			{
				ScenarioCustomWaypoints.AddWaypoint(waypoint, isMission: true);
			}
		}
		if (hasNodeLabel)
		{
			nodeLabel = VesselLabels.AddNodeLabel(this as ITestNodeLabel);
		}
		Initialized();
		return this;
	}

	public TestModule ClearTest()
	{
		GameEvents.Mission.onTestCleared.Fire(this);
		if (hasWaypoint && waypoint != null)
		{
			ScenarioCustomWaypoints.RemoveWaypoint(waypoint);
		}
		if (hasNodeLabel && nodeLabel != null)
		{
			VesselLabels.RemoveNodeLabel(nodeLabel);
		}
		Cleared();
		return this;
	}

	public virtual bool ShouldCreateCheckpoint()
	{
		return true;
	}

	public virtual void Initialized()
	{
	}

	public virtual void Cleared()
	{
	}

	public virtual bool Test()
	{
		return true;
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
		if (field.FieldType == typeof(ParamChoices_CelestialBodySurface) && field.GetValue() is ParamChoices_CelestialBodySurface paramChoices_CelestialBodySurface)
		{
			return paramChoices_CelestialBodySurface.NodeBodyParameterString();
		}
		if (field.FieldType == typeof(MissionCelestialBody) && field.GetValue() is MissionCelestialBody missionCelestialBody)
		{
			return Localizer.Format("#autoLOC_8004190", field.guiName, missionCelestialBody.DisplayName);
		}
		if (field.FieldType == typeof(MissionKerbal) && field.GetValue() is MissionKerbal missionKerbal)
		{
			return Localizer.Format("#autoLOC_8004190", field.guiName, missionKerbal.Name);
		}
		string text = Localizer.Format("#autoLOC_6003000");
		if (field.GetValue() != null)
		{
			text = MissionsUtils.GetFieldValueForDisplay(field);
		}
		return Localizer.Format("#autoLOC_8004190", field.guiName, text);
	}

	public void UpdateNodeBodyUI()
	{
		node.guiNode.DisplayNodeBodyParameters();
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

	public virtual void ParameterSetupComplete()
	{
	}

	public virtual void Load(ConfigNode node)
	{
		node.TryGetValue("title", ref title);
		parametersDisplayedInSAP = node.GetValuesList("parametersDisplayedInSAP");
	}

	public virtual void Save(ConfigNode node)
	{
		node.AddValue("name", name);
		node.AddValue("title", title);
		for (int i = 0; i < parametersDisplayedInSAP.Count; i++)
		{
			node.AddValue("parametersDisplayedInSAP", parametersDisplayedInSAP[i]);
		}
	}
}
