using System;
using System.Collections.Generic;
using ns13;
using ns2;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ns11;

public class EditorPanels : MonoBehaviour
{
	public enum Panel
	{
		None,
		Parts,
		Actions,
		Crew,
		PartsAndCargo
	}

	public UIPanelTransitionManager panelManager;

	public UIPanelTransition partsEditor;

	public UIPanelTransition actions;

	public UIPanelTransition crew;

	public UIPanelTransition cargo;

	public UIPanelTransition partsEditorModes;

	public UIPanelTransition partcategorizerModes;

	public UIPanelTransition searchField;

	public Panel panel;

	public List<UIPanelTransition> partsAndCargo;

	public static EditorPanels Instance { get; set; }

	public void Awake()
	{
		Instance = this;
		partsAndCargo = new List<UIPanelTransition> { partsEditor, cargo };
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void updatePartsListMode(Action onFinished = null)
	{
		if (EditorLogic.Mode == EditorLogic.EditorModes.SIMPLE)
		{
			partcategorizerModes.Transition("Simple", onFinished);
		}
		else
		{
			partcategorizerModes.Transition("Advanced", onFinished);
		}
	}

	public bool ShowPartsList(Action onFinished = null)
	{
		if (PartListTooltipMasterController.Instance != null)
		{
			PartListTooltipMasterController.Instance.HideTooltip();
		}
		if (panel == Panel.Parts)
		{
			return false;
		}
		panel = Panel.Parts;
		panelManager.BringIn(partsEditor, onFinished);
		panelManager.Dismiss(cargo);
		searchField.Transition("In");
		updatePartsListMode(onFinished);
		GameEvents.onEditorShowPartList.Fire();
		return true;
	}

	public bool ShowActionGroups(Action onFinished = null)
	{
		if (PartListTooltipMasterController.Instance != null)
		{
			PartListTooltipMasterController.Instance.HideTooltip();
		}
		if (panel == Panel.Actions)
		{
			return false;
		}
		panel = Panel.Actions;
		panelManager.BringIn(actions, onFinished);
		searchField.Transition("Out");
		return true;
	}

	public bool ShowCrewAssignment(Action onFinished = null)
	{
		if (PartListTooltipMasterController.Instance != null)
		{
			PartListTooltipMasterController.Instance.HideTooltip();
		}
		if (panel == Panel.Crew)
		{
			return false;
		}
		panel = Panel.Crew;
		panelManager.BringIn(crew, onFinished);
		searchField.Transition("Out");
		return true;
	}

	public bool ShowCargo(Action onFinished = null)
	{
		if (PartListTooltipMasterController.Instance != null)
		{
			PartListTooltipMasterController.Instance.HideTooltip();
		}
		if (panel == Panel.PartsAndCargo)
		{
			return false;
		}
		panel = Panel.PartsAndCargo;
		panelManager.BringIn(partsAndCargo, onFinished);
		searchField.Transition("In");
		return true;
	}

	public bool IsMouseOver()
	{
		return EventSystem.current.IsPointerOverGameObject();
	}
}
