using System;
using System.Collections.Generic;
using UnityEngine;

public class ModuleServiceModule : PartModule, IStageSeparator, IStageSeparatorChild, IScalarModule
{
	[KSPField(isPersistant = true)]
	public bool IsDeployed;

	[KSPField]
	public string ExteriorColliderName;

	[KSPField]
	public bool UseJettisonZones;

	[KSPField]
	public string JettisonZoneNames;

	[KSPField]
	public string ShellMeshName;

	[KSPField]
	public bool partDecoupled = true;

	[UI_Toggle(disabledText = "#autoLOC_6006087", enabledText = "#autoLOC_6006086")]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001455")]
	public bool showMesh = true;

	[UI_Toggle(disabledText = "#autoLOC_6006089", enabledText = "#autoLOC_6006088")]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6005001")]
	public bool openCutaway;

	public Transform exteriorColliderTransform;

	public Transform shellTransform;

	public List<ModuleJettison> _jettisonModules;

	public List<Part> _jettisonParts;

	public ModuleCargoBay moduleCargoBay;

	public EventData<float, float> OnDeployStart = new EventData<float, float>("ServiceModuleDeployStart");

	public EventData<float> OnDeployEnd = new EventData<float>("ServiceModuleDeployEnd");

	[KSPField]
	public string moduleID = "serviceModule";

	public bool IsShrouded => showMesh;

	public float GetScalar
	{
		get
		{
			if (IsDeployed)
			{
				return 1f;
			}
			return 0f;
		}
	}

	public bool CanMove => !IsDeployed;

	public EventData<float, float> OnMoving => OnDeployStart;

	public EventData<float> OnStop => OnDeployEnd;

	public string ScalarModuleID => moduleID;

	public override void OnStart(StartState state)
	{
		Setup();
	}

	public void OnFieldUpdated(BaseField field, object obj)
	{
		string text = field.name;
		if (!(text == "showMesh"))
		{
			if (text == "openCutaway" && !IsDeployed)
			{
				ToggleCutaway(openCutaway);
			}
		}
		else
		{
			IsDeployed = !showMesh;
			ToggleShrouds();
			UpdateStaging();
		}
	}

	public void ToggleCutaway(bool state)
	{
		ProcessJettisonModules(!state, UpdateCutaway);
	}

	public void UpdateCutaway(ModuleJettison jettison, bool state)
	{
		if (!jettison.isJettisoned && !jettison.shroudHideOverride)
		{
			jettison.jettisonTransform.gameObject.SetActive(state);
			ToggleShellMesh(state);
		}
	}

	public void ProcessJettisonModules(bool state, Action<ModuleJettison, bool> modAction)
	{
		if (_jettisonModules == null)
		{
			SetupJettisonModules();
		}
		int count = _jettisonModules.Count;
		for (int i = 0; i < count; i++)
		{
			ModuleJettison moduleJettison = _jettisonModules[i];
			if (moduleJettison.jettisonTransform != null)
			{
				modAction(moduleJettison, state);
			}
		}
		SetExteriorCollider(state);
	}

	public void ToggleShrouds()
	{
		bool flag = showMesh;
		if (IsDeployed)
		{
			flag = false;
		}
		moduleCargoBay = base.part.Modules.GetModule<ModuleCargoBay>();
		if (moduleCargoBay != null)
		{
			moduleCargoBay.ModifyShieldInEnclosedParts(flag);
		}
		ProcessJettisonModules(flag, UpdateShrouds);
		if (HighLogic.LoadedSceneIsEditor)
		{
			ToggleShellMesh(flag);
		}
	}

	public void ToggleShellMesh(bool state)
	{
		if (shellTransform != null)
		{
			shellTransform.gameObject.SetActive(state);
		}
	}

	public void UpdateShrouds(ModuleJettison jettison, bool state)
	{
		jettison.jettisonTransform.gameObject.SetActive(state);
		jettison.shroudHideOverride = !state;
	}

	public void SetupJettisonModules()
	{
		_jettisonModules = new List<ModuleJettison>();
		_jettisonModules.AddRange(base.part.FindModulesImplementing<ModuleJettison>());
	}

	public void OnDestroy()
	{
		GameEvents.onVesselSwitching.Remove(CheckCutaway);
	}

	public void Setup()
	{
		GameEvents.onVesselSwitching.Add(CheckCutaway);
		if (base.part.stagingIcon == string.Empty && overrideStagingIconIfBlank)
		{
			base.part.stagingIcon = DefaultIcons.FUEL_TANK.ToString();
		}
		exteriorColliderTransform = base.part.FindModelTransform(ExteriorColliderName);
		shellTransform = base.part.FindModelTransform(ShellMeshName);
		base.Fields["showMesh"].uiControlEditor.onFieldChanged = OnFieldUpdated;
		base.Fields["openCutaway"].uiControlFlight.onFieldChanged = OnFieldUpdated;
		base.Fields["openCutaway"].guiActive = !IsDeployed;
		if (HighLogic.LoadedSceneIsEditor || !showMesh || IsDeployed)
		{
			base.Fields["openCutaway"].guiActiveEditor = false;
			base.Fields["openCutaway"].guiActive = false;
		}
		SetupJettisonModules();
		if (!IsDeployed)
		{
			ToggleCutaway(openCutaway);
		}
		ToggleShrouds();
		UpdateStaging();
	}

	public void CheckCutaway(Vessel vFrom, Vessel vTo)
	{
		if (!(base.vessel == vFrom) && !IsDeployed && showMesh)
		{
			ToggleCutaway(openCutaway);
		}
		else
		{
			ToggleCutaway(!showMesh || IsDeployed);
		}
	}

	public override void OnActive()
	{
		if (showMesh && !IsDeployed)
		{
			openCutaway = false;
			ToggleCutaway(openCutaway);
			ShedExternalParts();
			SetExteriorCollider(state: false);
			UpdateStaging(state: false);
			IsDeployed = true;
			int count = _jettisonModules.Count;
			for (int i = 0; i < count; i++)
			{
				_jettisonModules[i].Jettison();
			}
			base.Fields["openCutaway"].guiActive = !IsDeployed;
			OnStop.Fire(1f);
		}
		else
		{
			ToggleShrouds();
		}
	}

	public void ShedExternalParts()
	{
		BuildJettisonPartsList();
		for (int i = 0; i < _jettisonParts.Count; i++)
		{
			_jettisonParts[i].decouple();
		}
	}

	public void BuildJettisonPartsList()
	{
		_jettisonParts = new List<Part>();
		List<BoxCollider> list = new List<BoxCollider>();
		if (UseJettisonZones)
		{
			string[] array = JettisonZoneNames.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				if (!(base.part != null))
				{
					continue;
				}
				Transform transform = base.part.FindModelTransform(array[i]);
				if (transform != null)
				{
					BoxCollider component = transform.gameObject.GetComponent<BoxCollider>();
					if (component != null)
					{
						list.Add(component);
					}
				}
			}
		}
		int count = base.part.children.Count;
		while (count-- > 0)
		{
			Part part = base.part.children[count];
			if (!(part.srfAttachNode.attachedPart == base.part) || part.ShieldedFromAirstream)
			{
				continue;
			}
			bool flag = true;
			if (UseJettisonZones)
			{
				bool flag2 = false;
				for (int j = 0; j < list.Count; j++)
				{
					BoxCollider box = list[j];
					if (PointInBox(part.transform.position, box))
					{
						flag2 = true;
						break;
					}
				}
				flag = flag2;
			}
			if (flag)
			{
				_jettisonParts.Add(part);
			}
		}
	}

	public bool PointInBox(Vector3 point, BoxCollider box)
	{
		point = box.transform.InverseTransformPoint(point) - box.center;
		float num = box.size.x * 0.5f;
		float num2 = box.size.y * 0.5f;
		float num3 = box.size.z * 0.5f;
		if (point.x < num && point.x > 0f - num && point.y < num2 && point.y > 0f - num2 && point.z < num3 && point.z > 0f - num3)
		{
			return true;
		}
		return false;
	}

	public void SetExteriorCollider(bool state)
	{
		if (exteriorColliderTransform != null)
		{
			exteriorColliderTransform.gameObject.SetActive(state);
		}
	}

	public void UpdateStaging()
	{
		bool state = showMesh;
		if (IsDeployed)
		{
			state = false;
		}
		UpdateStaging(state);
	}

	public void UpdateStaging(bool state)
	{
		stagingEnabled = state;
		base.ModuleAttributes.isStageable = state;
		if (_jettisonModules == null)
		{
			SetupJettisonModules();
		}
		int count = _jettisonModules.Count;
		for (int i = 0; i < count; i++)
		{
			ModuleJettison moduleJettison = _jettisonModules[i];
			moduleJettison.stagingEnabled = state;
			moduleJettison.ModuleAttributes.isStageable = state;
		}
		base.part.UpdateStageability(propagate: true, iconUpdate: true);
	}

	public int GetStageIndex(int fallback)
	{
		return base.part.inverseStage;
	}

	public virtual bool PartDetaches(out List<Part> decoupledParts)
	{
		BuildJettisonPartsList();
		decoupledParts = _jettisonParts;
		return partDecoupled;
	}

	public virtual bool IsEnginePlate()
	{
		return false;
	}

	public void SetScalar(float t)
	{
	}

	public void SetUIRead(bool state)
	{
	}

	public void SetUIWrite(bool state)
	{
	}

	public bool IsMoving()
	{
		return false;
	}

	public override string GetInfo()
	{
		return base.GetInfo();
	}

	public override string GetModuleDisplayName()
	{
		return "ServiceModule";
	}
}
