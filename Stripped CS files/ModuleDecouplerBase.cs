using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using ns9;
using UnityEngine;

public class ModuleDecouplerBase : PartModule, IModuleInfo, IStageSeparator, IStageSeparatorChild, IAirstreamShield
{
	[KSPField]
	public float ejectionForce = 10f;

	[UI_FloatRange(minValue = 0f, stepIncrement = 1f, maxValue = 100f)]
	[KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#autoLOC_6001442")]
	public float ejectionForcePercent = 100f;

	[KSPField(isPersistant = true)]
	public bool isDecoupled;

	[KSPField]
	public bool staged = true;

	[KSPField]
	public bool partDecoupled = true;

	[KSPField]
	public bool isEnginePlate;

	[KSPField]
	public string fxGroupName = "decouple";

	[KSPField]
	public bool isOmniDecoupler;

	[KSPField]
	public string explosiveNodeID = "top";

	public FXGroup fx;

	public AttachNode explosiveNode;

	public ModuleJettison jettisonModule;

	public bool shroudOn;

	public bool refreshStaging;

	public List<AdjusterDecoupleBase> adjusterCache = new List<AdjusterDecoupleBase>();

	public AttachNode ExplosiveNode
	{
		get
		{
			return explosiveNode;
		}
		set
		{
			explosiveNode = value;
		}
	}

	[KSPAction("#autoLOC_6001443", activeEditor = false)]
	public void DecoupleAction(KSPActionParam param)
	{
		Decouple();
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001443")]
	public void Decouple()
	{
		OnDecouple();
	}

	public override void OnAwake()
	{
		fx = base.part.findFxGroup(fxGroupName);
		if (fx == null)
		{
			Debug.LogError("Cannot find fx group of that name for decoupler");
		}
	}

	public override void OnStart(StartState state)
	{
		if (explosiveNodeID == "srf")
		{
			explosiveNode = base.part.srfAttachNode;
		}
		else
		{
			explosiveNode = base.part.FindAttachNode(explosiveNodeID);
		}
		if (explosiveNode == null)
		{
			Debug.LogError("[ModuleDecouple Error]: No attachnode found with id " + explosiveNodeID, base.gameObject);
		}
		if (isEnginePlate)
		{
			jettisonModule = base.part.FindModuleImplementing<ModuleJettison>();
			if (jettisonModule != null && !jettisonModule.isJettisoned)
			{
				SetAirstream(enclosed: true);
				jettisonModule.OnStop.Add(OnJettison);
				GameEvents.onVesselWasModified.Add(OnVesselWasModified);
			}
		}
	}

	public override void OnActive()
	{
	}

	public void LateUpdate()
	{
		if (refreshStaging)
		{
			stagingEnabled = false;
			base.ModuleAttributes.isStageable = false;
			base.part.UpdateStageability(propagate: false, iconUpdate: true);
			refreshStaging = false;
		}
	}

	public void OnDestroy()
	{
		if (jettisonModule != null)
		{
			jettisonModule.OnStop.Remove(OnJettison);
		}
		GameEvents.onVesselWasModified.Remove(OnVesselWasModified);
	}

	public void OnVesselWasModified(Vessel vsl)
	{
		if (base.vessel != null && vsl.persistentId == base.vessel.persistentId)
		{
			CheckShielded();
		}
	}

	public void CheckShielded()
	{
		if (jettisonModule != null && shroudOn)
		{
			AttachNode attachNode = base.part.FindAttachNode(jettisonModule.bottomNodeName);
			if (attachNode != null && attachNode.attachedPart == null)
			{
				SetAirstream(enclosed: false);
			}
		}
	}

	public void OnJettison(float value)
	{
		if (value == 1f)
		{
			SetAirstream(enclosed: false);
		}
	}

	public void SetAirstream(bool enclosed)
	{
		if (jettisonModule != null)
		{
			for (int i = 0; i < base.part.attachNodes.Count; i++)
			{
				if (base.part.attachNodes[i].id != jettisonModule.bottomNodeName && base.part.attachNodes[i].attachedPart != null)
				{
					base.part.attachNodes[i].attachedPart.ShieldedFromAirstream = enclosed;
					if (enclosed)
					{
						base.part.attachNodes[i].attachedPart.AddShield(this);
					}
					else
					{
						base.part.attachNodes[i].attachedPart.RemoveShield(this);
					}
				}
			}
		}
		shroudOn = enclosed;
	}

	public virtual void OnDecouple()
	{
		if (!isDecoupled)
		{
			IsAdjusterBlockingDecouple();
		}
	}

	public string GetModuleTitle()
	{
		if (isOmniDecoupler)
		{
			return Localizer.Format("#autoLOC_6001039");
		}
		return Localizer.Format("#autoLOC_6001040");
	}

	public Callback<Rect> GetDrawModulePanelCallback()
	{
		return null;
	}

	public string GetPrimaryField()
	{
		return Localizer.Format("#autoLOC_240300", ejectionForce.ToString("0.0###"));
	}

	public int GetStageIndex(int fallback)
	{
		if (!stagingEnabled && !(base.part.parent == null))
		{
			return base.part.parent.inverseStage;
		}
		return base.part.inverseStage;
	}

	public bool PartDetaches(out List<Part> decoupledParts)
	{
		decoupledParts = new List<Part>();
		if (explosiveNode == null)
		{
			if (explosiveNodeID == "srf")
			{
				explosiveNode = base.part.srfAttachNode;
			}
			else
			{
				explosiveNode = base.part.FindAttachNode(explosiveNodeID);
			}
		}
		if (explosiveNode != null && explosiveNode.attachedPart != null)
		{
			decoupledParts.Add(explosiveNode.attachedPart);
		}
		return partDecoupled;
	}

	public bool IsEnginePlate()
	{
		return isEnginePlate;
	}

	public bool ClosedAndLocked()
	{
		return shroudOn;
	}

	public Vessel GetVessel()
	{
		return base.vessel;
	}

	public Part GetPart()
	{
		return base.part;
	}

	public override bool IsStageable()
	{
		return staged;
	}

	public override bool StagingEnabled()
	{
		if (base.StagingEnabled())
		{
			return staged;
		}
		return false;
	}

	public override bool StagingToggleEnabledEditor()
	{
		return staged;
	}

	public override bool StagingToggleEnabledFlight()
	{
		return base.StagingToggleEnabledFlight();
	}

	public override string GetStagingEnableText()
	{
		if (!string.IsNullOrEmpty(stagingEnableText))
		{
			return stagingEnableText;
		}
		return Localizer.Format("#autoLOC_240328");
	}

	public override string GetStagingDisableText()
	{
		if (!string.IsNullOrEmpty(stagingDisableText))
		{
			return stagingDisableText;
		}
		return Localizer.Format("#autoLOC_240329");
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterDecoupleBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterDecoupleBase item = adjuster as AdjusterDecoupleBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public bool IsAdjusterBlockingDecouple()
	{
		int num = 0;
		while (true)
		{
			if (num < adjusterCache.Count)
			{
				if (adjusterCache[num].IsBlockingDecouple())
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}
}
