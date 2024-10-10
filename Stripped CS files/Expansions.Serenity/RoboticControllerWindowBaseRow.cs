using System.Collections.Generic;
using Highlighting;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Serenity;

public abstract class RoboticControllerWindowBaseRow : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public enum rowTypes
	{
		Action,
		Axis,
		None
	}

	public struct HighlightStateStore
	{
		public bool state;

		public Color color;
	}

	[SerializeField]
	public LayoutElement layout;

	public TextMeshProUGUI titleTextPart;

	public Button editNickButton;

	public UIHoverText editNickTextHover;

	public TMP_InputField partNickNameInput;

	public TextMeshProUGUI titleTextField;

	[SerializeField]
	public Button removeRowButton;

	public UIHoverText removeRowTextHover;

	[SerializeField]
	public RectTransform headerTransform;

	[SerializeField]
	public float heightCollapsed;

	[SerializeField]
	public float heightExpanded;

	public bool expanded;

	public PointerClickHandler headerClickHandler;

	[SerializeField]
	public List<GameObject> hideWhenCollapsed;

	public ControlledBase controlledItem;

	public ControlledAxis controlledAxis;

	public ControlledAction controlledAction;

	public int rowIndex = -1;

	public bool isMouseOver;

	public HighlightStateStore[] lastHighlightState;

	public int lastHighlightPartCount;

	public bool Expanded => expanded;

	public ModuleRoboticController Controller { get; set; }

	public RoboticControllerWindow Window { get; set; }

	public ControlledBase ControlledItem => controlledItem;

	public rowTypes rowType { get; set; }

	public uint PartPersistentId => controlledItem.PartPersistentId;

	public uint PartModulePersistentId => controlledItem.moduleId;

	public string RowName => controlledItem.BaseName;

	public bool IsAxis => rowType == rowTypes.Axis;

	public bool IsAction => rowType == rowTypes.Action;

	public RoboticControllerWindowBaseRow()
	{
	}

	public void AssignBaseReferenceVars(RoboticControllerWindow window, ModuleRoboticController controller, ControlledBase controlledItem)
	{
		Controller = controller;
		Window = window;
		this.controlledItem = controlledItem;
		controlledAxis = controlledItem as ControlledAxis;
		controlledAction = controlledItem as ControlledAction;
		if (controlledAxis != null)
		{
			rowType = rowTypes.Axis;
		}
		else
		{
			rowType = rowTypes.Action;
		}
		rowIndex = controlledItem.rowIndex;
	}

	public void Awake()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			Object.Destroy(base.gameObject);
		}
	}

	public abstract void OnRowStart();

	public abstract void OnRowDestroy();

	public abstract void OnRowExpanded();

	public abstract void OnRowCollapsed();

	public abstract void UpdateUILayout(bool recreateLine = false);

	public abstract void InsertPoint(float timeValue);

	public abstract void SelectAllPoints();

	public abstract void SelectPointAtTime(float timeValue);

	public abstract void OnPointSelectionChanged(CurvePanel panel, List<CurvePanelPoint> points);

	public abstract void OnPointDragging(List<CurvePanelPoint> points);

	public abstract void ReverseCurve();

	public abstract void ReloadCurve();

	public abstract void RedrawCurve();

	public void Start()
	{
		isMouseOver = false;
		removeRowTextHover = removeRowButton.GetComponent<UIHoverText>();
		if (removeRowTextHover != null && Window != null)
		{
			removeRowTextHover.textTargetForHover = Window.StatusHelpLabel;
		}
		headerClickHandler = headerTransform.GetComponent<PointerClickHandler>();
		if (headerClickHandler != null)
		{
			headerClickHandler.onPointerClick.AddListener(OnClickRow);
		}
		editNickTextHover = editNickButton.GetComponent<UIHoverText>();
		if (editNickTextHover != null && Window != null)
		{
			editNickTextHover.textTargetForHover = Window.StatusHelpLabel;
		}
		removeRowButton.gameObject.SetActive(HighLogic.LoadedSceneIsEditor);
		removeRowButton.onClick.AddListener(OnRemoveRowClick);
		editNickButton.onClick.AddListener(EditButtonClicked);
		partNickNameInput.onEndEdit.AddListener(InputFieldDone);
		OnRowStart();
	}

	public void OnDestroy()
	{
		if (headerClickHandler != null)
		{
			headerClickHandler.onPointerClick.RemoveListener(OnClickRow);
		}
		removeRowButton.onClick.RemoveListener(OnRemoveRowClick);
		editNickButton.onClick.RemoveListener(EditButtonClicked);
		partNickNameInput.onEndEdit.RemoveListener(InputFieldDone);
		UnHighlightParts();
		OnRowDestroy();
		Controller = null;
		Window = null;
	}

	public void EditButtonClicked()
	{
		partNickNameInput.gameObject.SetActive(value: true);
		partNickNameInput.text = controlledItem.PartNickName;
		partNickNameInput.ActivateInputField();
	}

	public void InputFieldDone(string newValue)
	{
		titleTextPart.text = newValue;
		controlledItem.SetPartNickName(newValue);
		partNickNameInput.gameObject.SetActive(value: false);
	}

	public void OnClickRow(PointerEventData data)
	{
		ToggleExpansion();
	}

	public void OnRemoveRowClick()
	{
		if (controlledAxis != null)
		{
			Controller.RemovePartAxis(controlledAxis, transferToSymPartner: false);
			Window.RemoveRow(controlledAxis, updateUIElements: true);
		}
		else if (controlledAction != null)
		{
			Controller.RemovePartAction(controlledAction, transferToSymPartner: false);
			Window.RemoveRow(controlledAction, updateUIElements: true);
		}
		Window.SaveRowIndexes();
	}

	public void ToggleExpansion()
	{
		if (expanded)
		{
			Collapse();
		}
		else
		{
			Expand();
		}
	}

	public void Collapse()
	{
		if (expanded)
		{
			expanded = false;
			UpdateUILayout();
			OnRowCollapsed();
		}
	}

	public void Expand()
	{
		if (!expanded)
		{
			expanded = true;
			if (Window != null)
			{
				Window.CollapseOtherRows(PartPersistentId, RowName, PartModulePersistentId);
			}
			UpdateUILayout();
			OnRowExpanded();
		}
	}

	public bool AnyTextFieldHasFocus()
	{
		return partNickNameInput.isFocused;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (isMouseOver)
		{
			UnHighlightParts();
		}
		isMouseOver = false;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!isMouseOver)
		{
			HighlightParts();
		}
		isMouseOver = true;
	}

	public void HighlightParts()
	{
		if (lastHighlightState == null)
		{
			lastHighlightState = new HighlightStateStore[100];
		}
		if (ControlledItem.SymmetryParts != null && ControlledItem.SymmetryParts.Count > lastHighlightState.Length - 1)
		{
			lastHighlightState = new HighlightStateStore[lastHighlightState.Length + 100];
		}
		lastHighlightState[0].state = ControlledItem.Part.HighlightActive;
		lastHighlightState[0].color = ControlledItem.Part.highlightColor;
		SetPartHighlight(ControlledItem.Part);
		for (int i = 0; i < controlledItem.SymmetryParts.Count; i++)
		{
			lastHighlightState[i + 1].state = ControlledItem.SymmetryParts[i].HighlightActive;
			lastHighlightState[i + 1].color = ControlledItem.SymmetryParts[i].highlightColor;
			SetPartHighlight(ControlledItem.SymmetryParts[i]);
		}
		lastHighlightPartCount = controlledItem.SymmetryParts.Count + 1;
	}

	public void SetPartHighlight(Part part)
	{
		part.SetHighlightColor(Highlighter.colorPartEditorActionHighlight);
		part.SetHighlight(active: true, recursive: false);
	}

	public void UnHighlightParts()
	{
		if (lastHighlightPartCount > 0)
		{
			ControlledItem.Part.SetHighlightColor(lastHighlightState[0].color);
			ControlledItem.Part.SetHighlight(lastHighlightState[0].state, recursive: false);
		}
		if (controlledItem.SymmetryParts != null && controlledItem.SymmetryParts.Count > 0)
		{
			for (int i = 0; i < controlledItem.SymmetryParts.Count && i + 1 < lastHighlightPartCount; i++)
			{
				ControlledItem.SymmetryParts[i].SetHighlightColor(lastHighlightState[i + 1].color);
				ControlledItem.SymmetryParts[i].SetHighlight(lastHighlightState[i + 1].state, recursive: false);
			}
		}
		lastHighlightPartCount = 0;
	}
}
