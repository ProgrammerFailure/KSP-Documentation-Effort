using System;
using System.Collections.Generic;
using ns9;
using ProceduralFairings;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPVesselDisplay : ActionPaneDisplay
{
	public ShipConstruct currentVessel;

	public GAPVesselCamera vesselCamera;

	public Part currentHoveredPart;

	public Part currentSelectedPart;

	public GameObject vesselCameraSetup;

	public GAPUtil_VesselFrame vesselFrame;

	public MEGUIParameterVessel parameter;

	public MissionCraft currentCraft;

	public bool isReady;

	public override void Setup(Camera displayCamera, int layerMask)
	{
		base.Setup(displayCamera, layerMask);
		vesselCamera = displayCamera.GetComponent<GAPVesselCamera>();
		vesselCamera.cameraMode = GAPVesselCamera.CameraMode.VesselMode;
		displayImage.gameObject.SetActive(value: true);
		if (vesselFrame == null)
		{
			vesselFrame = UnityEngine.Object.Instantiate(parameter.vesselFramePrefab, base.transform);
			vesselFrame.rightButton.onClick.AddListener(OnNextVessel);
			vesselFrame.leftButton.onClick.AddListener(OnPrevVessel);
			GAPUtil_VesselFrame gAPUtil_VesselFrame = vesselFrame;
			gAPUtil_VesselFrame.onCategoryValueChange = (GAPUtil_VesselFrame.categoryValueChange)Delegate.Combine(gAPUtil_VesselFrame.onCategoryValueChange, new GAPUtil_VesselFrame.categoryValueChange(OnPartCategoryChanged));
		}
	}

	public void SetupVessel(MissionCraft craft, VesselSituation situation, MEGUIParameterVessel parameter)
	{
		currentCraft = craft;
		this.parameter = parameter;
		if (!isReady)
		{
			vesselCameraSetup = UnityEngine.Object.Instantiate(parameter.vesselCameraSetup);
			Setup(vesselCameraSetup.GetComponentInChildren<Camera>(), LayerUtil.DefaultEquivalent | (1 << LayerMask.NameToLayer("WheelCollidersIgnore")) | (1 << LayerMask.NameToLayer("WheelColliders")));
			vesselFrame.SetFooterText("");
			isReady = true;
		}
		displayImage.gameObject.SetActive(value: true);
		if (currentVessel != null)
		{
			HideVessel(currentVessel);
		}
		if (craft != null)
		{
			LoadVessel(craft, situation);
		}
		else if (situation != null)
		{
			string text = situation.vesselName;
			MissionCraft craftBySituationsVesselID = MissionEditorLogic.Instance.EditorMission.GetCraftBySituationsVesselID(situation.persistentId);
			if (craftBySituationsVesselID != null && MissionEditorLogic.Instance.incompatibleCraft.Contains(craftBySituationsVesselID.craftFile))
			{
				text = Localizer.Format("#autoLOC_8004245", text);
			}
			vesselFrame.SetTitleText(text, playerCreated: true);
		}
		else
		{
			vesselFrame.SetTitleText(Localizer.Format("#autoLOC_8001004"));
		}
	}

	public void LoadVessel(MissionCraft vessel, VesselSituation situation)
	{
		FlightGlobals.ClearpersistentIdDictionaries();
		string text = situation.vesselName;
		if (MissionEditorLogic.Instance.incompatibleCraft.Contains(vessel.craftFile))
		{
			text = Localizer.Format("#autoLOC_8004245", text);
		}
		vesselFrame.SetTitleText(text, vessel.CraftNode == null);
		DestroyCurrentVessel();
		if (vessel.CraftNode != null)
		{
			currentVessel = MissionEditorLogic.Instance.LoadVessel(vessel);
			if (currentVessel != null)
			{
				ShowVessel(currentVessel);
				vesselCamera.FocusVessel(currentVessel);
			}
		}
		if (parameter as MEGUIParameterVesselPartSelector != null)
		{
			vesselFrame.ClearSidebar();
			vesselFrame.SetPartTypes(currentVessel);
			OnPartCategoryChanged(PartCategories.none, state: true);
			UpdateFrameFooter();
		}
		else
		{
			ShowFairings(currentVessel);
		}
	}

	public void DestroyCurrentVessel()
	{
		if (currentVessel != null)
		{
			int count = currentVessel.Parts.Count;
			while (count-- > 0)
			{
				if (currentVessel.Parts[count] != null)
				{
					UnityEngine.Object.DestroyImmediate(currentVessel.Parts[count].gameObject);
				}
			}
		}
		currentVessel = null;
	}

	public void ChangeSelectedPart(uint partID)
	{
		if (currentVessel == null)
		{
			return;
		}
		if (partID == 0)
		{
			ChangeSelectedPart(null);
			return;
		}
		int num = 0;
		int count = currentVessel.Parts.Count;
		while (true)
		{
			if (num < count)
			{
				if (currentVessel.Parts[num].persistentId == partID)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		ChangeSelectedPart(currentVessel.Parts[num]);
	}

	public override void Clean()
	{
		base.Clean();
		if (currentVessel != null)
		{
			HideVessel(currentVessel);
		}
		if (currentSelectedPart != null)
		{
			currentSelectedPart.SetHighlight(active: false, recursive: false);
			currentSelectedPart = null;
		}
		if (currentHoveredPart != null)
		{
			currentHoveredPart.SetHighlight(active: false, recursive: false);
			currentHoveredPart = null;
		}
		vesselFrame.SetFooterText("");
		vesselFrame.SetTitleText("");
		displayImage.gameObject.SetActive(value: false);
	}

	public override void Destroy()
	{
		DestroyCurrentVessel();
		base.Destroy();
		UnityEngine.Object.Destroy(displayImage.gameObject);
		UnityEngine.Object.Destroy(vesselFrame.gameObject);
		UnityEngine.Object.Destroy(vesselCameraSetup);
		displayImage = null;
	}

	public void ShowFairings(ShipConstruct vessel)
	{
		if (vessel == null)
		{
			return;
		}
		int count = vessel.Parts.Count;
		while (count-- > 0)
		{
			ModuleJettison module = vessel.Parts[count].Modules.GetModule<ModuleJettison>();
			if (module != null)
			{
				module.isJettisoned = false;
			}
			ModuleProceduralFairing component = vessel.Parts[count].GetComponent<ModuleProceduralFairing>();
			if (component != null)
			{
				List<FairingPanel> panels = component.Panels;
				int i = 0;
				for (int count2 = panels.Count; i < count2; i++)
				{
					panels[i].go.SetActive(value: true);
				}
			}
		}
	}

	public void HideFairings(ShipConstruct vessel)
	{
		if (vessel == null)
		{
			return;
		}
		int count = vessel.Parts.Count;
		while (count-- > 0)
		{
			ModuleJettison module = vessel.Parts[count].Modules.GetModule<ModuleJettison>();
			if (module != null)
			{
				module.isJettisoned = true;
			}
			ModuleColorChanger component = vessel.Parts[count].GetComponent<ModuleColorChanger>();
			if (component != null)
			{
				component.enabled = false;
			}
			ModuleProceduralFairing component2 = vessel.Parts[count].GetComponent<ModuleProceduralFairing>();
			if (component2 != null)
			{
				List<FairingPanel> panels = component2.Panels;
				int i = 0;
				for (int count2 = panels.Count; i < count2; i++)
				{
					panels[i].go.SetActive(value: false);
				}
			}
		}
	}

	public void ChangeSelectedPart(Part part)
	{
		if (part != currentSelectedPart)
		{
			if (currentSelectedPart != null)
			{
				currentSelectedPart.SetHighlight(active: false, recursive: false);
			}
			if (part != null)
			{
				part.SetHighlightColor();
				part.SetHighlight(active: true, recursive: false);
			}
			currentSelectedPart = part;
			currentHoveredPart = null;
			parameter.OnChangePartSelection(currentSelectedPart);
			UpdateFrameFooter();
		}
	}

	public void ChangeHoverPart(Part part)
	{
		if (!(part != currentHoveredPart))
		{
			return;
		}
		if (currentSelectedPart != part)
		{
			if (currentHoveredPart != null)
			{
				currentHoveredPart.SetHighlight(active: false, recursive: false);
			}
			if (part != null)
			{
				part.SetHighlightColor(Color.blue);
				part.SetHighlight(active: true, recursive: false);
			}
			currentHoveredPart = part;
		}
		else
		{
			if (currentHoveredPart != null)
			{
				currentHoveredPart.SetHighlight(active: false, recursive: false);
			}
			currentHoveredPart = null;
		}
		UpdateFrameFooter();
	}

	public void UpdateFrameFooter()
	{
		if (currentSelectedPart != null && currentHoveredPart != null)
		{
			vesselFrame.SetFooterText(Localizer.Format("#autoLOC_8005070", currentSelectedPart.partInfo.title, currentHoveredPart.partInfo.title));
		}
		else if (currentSelectedPart != null)
		{
			vesselFrame.SetFooterText(Localizer.Format("#autoLOC_8005070", currentSelectedPart.partInfo.title, string.Empty));
		}
		else if (currentHoveredPart != null)
		{
			vesselFrame.SetFooterText(Localizer.Format("#autoLOC_8005070", string.Empty, currentHoveredPart.partInfo.title));
		}
		else
		{
			vesselFrame.SetFooterText(Localizer.Format("#autoLOC_8005070", string.Empty, string.Empty));
		}
	}

	public override void OnDisplayClick(RaycastHit? hit)
	{
		if (parameter as MEGUIParameterVesselPartSelector != null)
		{
			if (hit.HasValue)
			{
				Part componentInParent = hit.Value.collider.GetComponentInParent<Part>();
				ChangeSelectedPart(componentInParent);
			}
			else
			{
				ChangeSelectedPart(null);
			}
		}
	}

	public override void OnMouseOver(Vector2 position)
	{
		if (parameter as MEGUIParameterVesselPartSelector != null)
		{
			if (Physics.Raycast(displayCamera.ScreenPointToRay(position), out var hitInfo, hitDistance, layerMask))
			{
				Part componentInParent = hitInfo.collider.GetComponentInParent<Part>();
				ChangeHoverPart(componentInParent);
			}
			else
			{
				ChangeHoverPart(null);
			}
		}
		vesselCamera.UpdateCameraControls();
	}

	public void HideVessel(ShipConstruct ship)
	{
		if (ship != null && ship.Parts.Count > 0)
		{
			ship.Parts[0].gameObject.SetActive(value: false);
		}
	}

	public void ShowVessel(ShipConstruct ship)
	{
		if (ship == null || ship.Parts.Count <= 0)
		{
			return;
		}
		ship.Parts[0].gameObject.SetActive(value: true);
		ParticleSystem[] componentsInChildren = ship.Parts[0].GetComponentsInChildren<ParticleSystem>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			FXPrefab component = componentsInChildren[i].GetComponent<FXPrefab>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
			UnityEngine.Object.Destroy(componentsInChildren[i]);
		}
	}

	public void OnPartCategoryChanged(PartCategories category, bool state)
	{
		if (currentVessel == null)
		{
			return;
		}
		int count = currentVessel.Parts.Count;
		while (count-- > 0)
		{
			Part part = currentVessel.Parts[count];
			if (category != PartCategories.none && !part.partInfo.category.Equals(category))
			{
				continue;
			}
			if (state)
			{
				MeshRenderer[] componentsInChildren = part.partTransform.Find("model").GetComponentsInChildren<MeshRenderer>();
				int num = componentsInChildren.Length;
				while (num-- > 0)
				{
					componentsInChildren[num].enabled = true;
				}
				Collider[] componentsInChildren2 = part.partTransform.Find("model").GetComponentsInChildren<Collider>();
				int num2 = componentsInChildren2.Length;
				while (num2-- > 0)
				{
					componentsInChildren2[num2].enabled = true;
				}
				if (part == currentSelectedPart)
				{
					part.SetHighlight(active: true, recursive: false);
				}
				else
				{
					part.SetHighlight(active: false, recursive: false);
				}
			}
			else
			{
				MeshRenderer[] componentsInChildren3 = part.partTransform.Find("model").GetComponentsInChildren<MeshRenderer>();
				int num3 = componentsInChildren3.Length;
				while (num3-- > 0)
				{
					componentsInChildren3[num3].enabled = false;
				}
				Collider[] componentsInChildren4 = part.partTransform.Find("model").GetComponentsInChildren<Collider>();
				int num4 = componentsInChildren4.Length;
				while (num4-- > 0)
				{
					componentsInChildren4[num4].enabled = false;
				}
			}
		}
	}

	public void OnNextVessel()
	{
		parameter.OnNextVessel();
	}

	public void OnPrevVessel()
	{
		parameter.OnPrevVessel();
	}
}
