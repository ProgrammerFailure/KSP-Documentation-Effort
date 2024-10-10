using System.Collections;
using System.Collections.Generic;
using Expansions;
using Highlighting;
using ns9;
using UnityEngine;

public class ModuleCargoPart : PartModule
{
	[KSPField]
	public float packedVolume;

	[KSPField]
	public float kinematicDelay = 3f;

	[KSPField]
	public float placementMaxRivotVelocity = 0.003f;

	[KSPField]
	public string inventoryTooltip = "";

	[KSPField]
	public int stackableQuantity = 1;

	public ModuleGroundSciencePart groundScienceModule;

	public AudioSource weldingSparkingsFX;

	public List<FXGroup> weldingSparkingGroup;

	public bool UIHidden;

	public List<Part> linkedCompoundParts;

	[KSPField(isPersistant = true)]
	public bool beingAttached;

	[KSPField(isPersistant = true)]
	public bool beingSettled;

	[KSPField(isPersistant = true)]
	public bool reinitResourcesOnStoreInVessel;

	public float preSettlingPrefabMass;

	[SerializeField]
	public bool inConstructionMode;

	[SerializeField]
	public bool constructionModeHighlight;

	[SerializeField]
	public float distanceToEngineer;

	public float MassForWeightTesting
	{
		get
		{
			if (preSettlingPrefabMass > 0f)
			{
				return preSettlingPrefabMass;
			}
			return base.part.mass;
		}
	}

	public virtual void OnDestroy()
	{
		StopCoroutine("MakePartSettle");
		GameEvents.OnEVAConstructionMode.Remove(OnEVAConstructionMode);
		GameEvents.onPartWillDie.Remove(OnPartWillDie);
		GameEvents.OnEVAConstructionWeldStart.Remove(OnWeldStart);
		GameEvents.OnEVAConstructionWeldFinish.Remove(OnWeldFinish);
		GameEvents.onHideUI.Remove(HideUI);
		GameEvents.onShowUI.Remove(ShowUI);
		GameEvents.OnFlightCompoundPartLinked.Remove(OnCompoundPartLinked);
		GameEvents.OnFlightCompoundPartDetached.Remove(OnCompoundPartDetached);
	}

	public virtual void OnPartPack()
	{
	}

	public virtual void OnPartUnpack()
	{
		if (beingAttached)
		{
			beingAttached = false;
		}
		else if (beingSettled || (base.vessel.parts.Count == 1 && (base.vessel.situation == Vessel.Situations.LANDED || base.vessel.situation == Vessel.Situations.PRELAUNCH)))
		{
			StartCoroutine(MakePartSettle(waitForMotionStop: false));
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		if (stackableQuantity > 1 && base.part != null && base.part.Resources != null && base.part.Resources.Count > 0)
		{
			Debug.Log("[ModuleCargoPart]: Setting stackableQuantity to 1 as " + base.part.name + " contains resources");
			stackableQuantity = 1;
		}
	}

	public override void OnStart(StartState state)
	{
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		GameEvents.onPartWillDie.Add(OnPartWillDie);
		linkedCompoundParts = new List<Part>();
		GameEvents.OnFlightCompoundPartLinked.Add(OnCompoundPartLinked);
		GameEvents.OnFlightCompoundPartDetached.Add(OnCompoundPartDetached);
		GameEvents.OnEVAConstructionWeldStart.Add(OnWeldStart);
		GameEvents.OnEVAConstructionWeldFinish.Add(OnWeldFinish);
		GameEvents.onHideUI.Add(HideUI);
		GameEvents.onShowUI.Add(ShowUI);
		if (weldingSparkingGroup == null)
		{
			weldingSparkingGroup = new List<FXGroup>();
		}
		List<AudioClip> list = GameDatabase.Instance.AudioClipNameContains("sparking");
		if (weldingSparkingGroup.Count < list.Count)
		{
			for (int i = 0; i < list.Count; i++)
			{
				FXGroup fXGroup = new FXGroup("welding_spark");
				fXGroup.name = list[i].name;
				fXGroup.sfx = list[i];
				SetupFXGroup(fXGroup);
			}
		}
		inConstructionMode = EVAConstructionModeController.Instance != null && (EVAConstructionModeController.Instance.IsOpen || EVAConstructionModeController.Instance.panelMode != EVAConstructionModeController.PanelMode.Construction);
	}

	public void SetupFXGroup(FXGroup fXGroup)
	{
		if (fXGroup != null)
		{
			fXGroup.setActive(value: false);
			if (weldingSparkingsFX == null)
			{
				weldingSparkingsFX = base.gameObject.AddComponent<AudioSource>();
			}
			weldingSparkingsFX.playOnAwake = false;
			weldingSparkingsFX.loop = false;
			weldingSparkingsFX.rolloffMode = AudioRolloffMode.Linear;
			weldingSparkingsFX.dopplerLevel = 0f;
			weldingSparkingsFX.volume = GameSettings.SHIP_VOLUME;
			weldingSparkingsFX.spatialBlend = 1f;
			fXGroup.begin(weldingSparkingsFX);
			weldingSparkingGroup.Add(fXGroup);
		}
	}

	public override void OnInitialize()
	{
		base.OnInitialize();
		GameEvents.OnEVAConstructionMode.Add(OnEVAConstructionMode);
	}

	public override string GetInfo()
	{
		string text = "";
		text = ((!(packedVolume < 0f)) ? Localizer.Format("#autoLOC_8002220") : Localizer.Format("#autoLOC_6002641"));
		text += "\n\n";
		text = text + Localizer.Format("#autoLOC_8002186") + " " + (base.part.partInfo.partPrefab.mass + base.part.GetModuleMass(base.part.partInfo.partPrefab.mass)).ToString("F3") + " t\\n";
		if (packedVolume > 0f)
		{
			text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8003414"), Localizer.Format("<<1>><<2>>", packedVolume.ToString("0.0"), "L")) + "\n";
		}
		if (stackableQuantity > 1)
		{
			text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8003418"), stackableQuantity.ToString("0")) + "\n";
		}
		return text;
	}

	public void OnPartWillDie(Part p)
	{
		if (base.part != null && p.persistentId == base.part.persistentId && base.part.Rigidbody != null)
		{
			base.part.Rigidbody.constraints = RigidbodyConstraints.None;
			base.part.Rigidbody.ResetInertiaTensor();
		}
	}

	public void HideUI()
	{
		UIHidden = true;
		constructionModeHighlight = false;
		SetConstructionHighlight(constructionModeHighlight);
	}

	public void ShowUI()
	{
		UIHidden = false;
	}

	public virtual IEnumerator MakePartSettle(bool waitForMotionStop)
	{
		bool adjustMass = false;
		preSettlingPrefabMass = base.part.prefabMass;
		if (base.vessel != null && base.vessel.mainBody != null)
		{
			if (base.vessel.mainBody.GeeASL < 0.800000011920929)
			{
				beingSettled = true;
				adjustMass = true;
				base.part.prefabMass = 20f;
			}
			else if (base.vessel.mainBody.GeeASL < 1.100000023841858)
			{
				beingSettled = true;
				adjustMass = true;
				base.part.prefabMass = 4f;
			}
		}
		if (kinematicDelay > 0f)
		{
			yield return new WaitForSeconds(kinematicDelay);
		}
		if (waitForMotionStop)
		{
			double timeWaitingForSrfVelocity = Planetarium.GetUniversalTime();
			float settleDelay = kinematicDelay * 10f;
			while (!(Planetarium.GetUniversalTime() - timeWaitingForSrfVelocity > (double)settleDelay) && !(base.vessel.srf_velocity.magnitude < (double)placementMaxRivotVelocity))
			{
				yield return null;
			}
		}
		if (adjustMass)
		{
			base.part.prefabMass = preSettlingPrefabMass;
			preSettlingPrefabMass = 0f;
			beingSettled = false;
		}
	}

	public void OnEVAConstructionMode(bool opened)
	{
		if ((bool)EVAConstructionModeController.Instance && EVAConstructionModeController.Instance.panelMode == EVAConstructionModeController.PanelMode.Construction)
		{
			inConstructionMode = opened;
		}
		else
		{
			inConstructionMode = false;
		}
		if (!inConstructionMode)
		{
			OnUpdateHighlight();
		}
	}

	public virtual void OnWeldStart(KerbalEVA kerbalEVA)
	{
		if (EVAConstructionModeController.Instance.evaEditor.LastAttachedPart.persistentId == base.part.persistentId && weldingSparkingsFX != null && !weldingSparkingsFX.isPlaying)
		{
			int index = Random.Range(0, weldingSparkingGroup.Count);
			weldingSparkingsFX.clip = weldingSparkingGroup[index].sfx;
			weldingSparkingsFX.PlayDelayed(kerbalEVA.WeldFX.StartDelay);
		}
	}

	public virtual void OnWeldFinish(KerbalEVA kerbalEVA)
	{
		if (EVAConstructionModeController.Instance.evaEditor.LastAttachedPart.persistentId == base.part.persistentId && weldingSparkingsFX != null && weldingSparkingsFX.isPlaying)
		{
			weldingSparkingsFX.Stop();
		}
	}

	public void SetConstructionHighlight(bool state)
	{
		if (!(base.part == null))
		{
			if (state)
			{
				base.part.SetHighlightColor(Highlighter.colorPartConstructionValid);
				base.part.SetHighlightType(Part.HighlightType.AlwaysOn);
				base.part.SetHighlight(active: true, recursive: true);
			}
			else
			{
				base.part.SetHighlightColor(Highlighter.colorPartHighlightDefault);
				base.part.SetHighlightType(Part.HighlightType.OnMouseOver);
				base.part.SetHighlight(active: false, recursive: true);
			}
		}
	}

	public override void OnUpdate()
	{
		if (inConstructionMode)
		{
			OnUpdateHighlight();
		}
	}

	public void OnUpdateHighlight()
	{
		bool flag = false;
		bool flag2 = false;
		if (!UIHidden && !(base.part == null))
		{
			distanceToEngineer = (FlightGlobals.ActiveVessel.vesselTransform.position - base.part.transform.position).magnitude;
			if (constructionModeHighlight)
			{
				if (!inConstructionMode || distanceToEngineer > GameSettings.EVA_CONSTRUCTION_RANGE || !base.part.IsUnderConstructionWeightLimit() || HasLinkedCompoundParts() || base.part.children.Count > 0 || !base.part.PartCanBeDetached())
				{
					flag = true;
					flag2 = false;
				}
			}
			else if (inConstructionMode && distanceToEngineer <= GameSettings.EVA_CONSTRUCTION_RANGE && base.part.IsUnderConstructionWeightLimit() && !HasLinkedCompoundParts() && base.part.children.Count <= 0 && base.part.PartCanBeDetached())
			{
				flag = true;
				flag2 = true;
			}
		}
		else
		{
			flag = true;
			flag2 = false;
		}
		if (flag)
		{
			constructionModeHighlight = flag2;
			SetConstructionHighlight(constructionModeHighlight);
		}
	}

	public virtual string GetTooltip()
	{
		if (string.IsNullOrEmpty(inventoryTooltip))
		{
			return GetInfo();
		}
		return Localizer.Format(inventoryTooltip);
	}

	public bool IsDeployedSciencePart()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			return false;
		}
		return base.part.FindModuleImplementing<ModuleGroundSciencePart>() != null;
	}

	public virtual void EnableModule()
	{
		isEnabled = true;
	}

	public virtual void PlayDeployAnimation()
	{
	}

	public void OnCompoundPartLinked(CompoundPart linkedPart)
	{
		if (!(linkedPart == null))
		{
			Part target = linkedPart.target;
			if (target != null && target.persistentId == base.part.persistentId)
			{
				linkedCompoundParts.AddUnique(linkedPart);
			}
		}
	}

	public void OnCompoundPartDetached(CompoundPart linkedPart)
	{
		if (!(linkedPart == null))
		{
			Part target = linkedPart.target;
			if (target != null && target.persistentId == base.part.persistentId)
			{
				linkedCompoundParts.Remove(linkedPart);
			}
		}
	}

	public bool HasLinkedCompoundParts()
	{
		if (linkedCompoundParts != null)
		{
			return linkedCompoundParts.Count > 0;
		}
		return false;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_8002221");
	}
}
