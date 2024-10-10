using System;
using System.Collections.Generic;
using ns12;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEGUIParameter : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IMEHistoryTarget
{
	public TextMeshProUGUI title;

	public Toggle pinToggle;

	public UIStateImage pinToggleImage;

	public Image selectedIndicator;

	public GameObject gapDisplayIndicator;

	public RectTransform gapDisplayPartner;

	public Button resetButton;

	public bool isSelectable = true;

	[SerializeField]
	public TooltipController_Text tooltipComponent;

	public BaseAPField field;

	public CanvasGroup pinCanvasGroup;

	public bool isPinned;

	public IMENodeDisplay module;

	public List<UIBehaviour> maskedControls;

	public MEGUIParameterGroup parentGroup;

	public int _order;

	public bool _tabStop;

	public bool isMouseOver { get; set; }

	public bool IsSelected { get; set; }

	public virtual bool IsInteractable { get; set; }

	public bool HasGAP
	{
		get
		{
			if (field != null && field.Attribute != null)
			{
				return field.Attribute.gapDisplay;
			}
			return false;
		}
	}

	public bool IsPinnable
	{
		get
		{
			if (field != null && field.Attribute != null)
			{
				return field.Attribute.canBePinned;
			}
			return true;
		}
	}

	public string Tooltip
	{
		get
		{
			if (field != null && field.Attribute != null)
			{
				return field.Attribute.Tooltip;
			}
			return "";
		}
	}

	public int Order
	{
		get
		{
			if (field != null && field.Attribute != null)
			{
				return field.Attribute.order;
			}
			return _order;
		}
		set
		{
			_order = value;
		}
	}

	public bool GroupStartCollapsed
	{
		get
		{
			if (field != null && field.Attribute != null)
			{
				return field.Attribute.groupStartCollapsed;
			}
			return false;
		}
	}

	public object FieldHost
	{
		get
		{
			if (field != null && field.host != null)
			{
				return field.host;
			}
			return null;
		}
	}

	public bool TabStop
	{
		get
		{
			if (_tabStop)
			{
				return true;
			}
			if (field != null && field.Attribute != null)
			{
				return field.Attribute.tabStop;
			}
			return _tabStop;
		}
		set
		{
			_tabStop = value;
		}
	}

	public virtual void OnEnable()
	{
		if (selectedIndicator != null)
		{
			selectedIndicator.canvasRenderer.SetAlpha(IsSelected ? 1f : 0f);
		}
	}

	public virtual void Awake()
	{
		if (pinToggle != null)
		{
			pinCanvasGroup = pinToggle.GetComponent<CanvasGroup>();
		}
		isMouseOver = false;
	}

	public virtual void Start()
	{
		if (pinToggle != null)
		{
			pinCanvasGroup.alpha = (isPinned ? 1 : 0);
			pinToggle.onValueChanged.AddListener(OnPinValueChanged);
			pinToggleImage = pinToggle.GetComponent<UIStateImage>();
			SetPinToggleImage();
		}
		if (selectedIndicator != null)
		{
			selectedIndicator.canvasRenderer.SetAlpha(0f);
		}
	}

	public virtual void Update()
	{
		if (isSelectable && isMouseOver && Input.GetMouseButtonDown(0))
		{
			MissionEditorLogic.Instance.actionPane.OnParameterClick(this);
		}
	}

	public MEGUIParameter Create(BaseAPField field, Transform parent, string name = null)
	{
		MEGUIParameter mEGUIParameter = UnityEngine.Object.Instantiate(this);
		mEGUIParameter.transform.SetParent(parent);
		mEGUIParameter.transform.localScale = Vector3.one;
		Vector3 localPosition = mEGUIParameter.transform.localPosition;
		localPosition.z = 0f;
		mEGUIParameter.transform.localPosition = localPosition;
		mEGUIParameter.field = field;
		if (field != null && field.OnControlCreated != null)
		{
			field.OnControlCreated.Invoke(field.host, new object[1] { mEGUIParameter });
		}
		if (field != null)
		{
			mEGUIParameter.Setup((name == null) ? field.guiName : name);
			mEGUIParameter.module = field.host as IMENodeDisplay;
			mEGUIParameter.ToggleGAPButton();
			if (mEGUIParameter.tooltipComponent != null)
			{
				if (string.IsNullOrEmpty(mEGUIParameter.Tooltip))
				{
					mEGUIParameter.tooltipComponent.enabled = false;
				}
				else
				{
					mEGUIParameter.tooltipComponent.enabled = true;
					mEGUIParameter.tooltipComponent.SetText(mEGUIParameter.Tooltip);
				}
			}
			mEGUIParameter.DisplayParameterObject();
		}
		else
		{
			mEGUIParameter.Setup(name);
			mEGUIParameter.HideGAPButton();
		}
		MaskScrollControls(mEGUIParameter);
		try
		{
			mEGUIParameter.LockLocalizedText();
		}
		catch (NotImplementedException ex)
		{
			Debug.Log("The LockLocalizedText is not implemented for this parameter: " + ex.Message);
		}
		if (field != null && field.OnControlSetupComplete != null)
		{
			field.OnControlSetupComplete.Invoke(field.host, new object[1] { mEGUIParameter });
		}
		return mEGUIParameter;
	}

	public void ToggleGAPButton()
	{
		if (!(gapDisplayIndicator != null))
		{
			return;
		}
		gapDisplayIndicator.SetActive(HasGAP);
		if (gapDisplayPartner != null)
		{
			if (HasGAP)
			{
				gapDisplayPartner.offsetMax = new Vector2(0f - ((RectTransform)gapDisplayIndicator.transform).sizeDelta.x, 0f);
			}
			else
			{
				gapDisplayPartner.offsetMax = new Vector2(0f, 0f);
			}
		}
	}

	public void HideGAPButton()
	{
		if (gapDisplayIndicator != null)
		{
			gapDisplayIndicator.SetActive(value: false);
			if (gapDisplayPartner != null)
			{
				gapDisplayPartner.offsetMax = new Vector2(0f, 0f);
			}
		}
	}

	public void SetTooltipActive(bool state)
	{
		tooltipComponent.enabled = state;
	}

	public void SetTooltipText(string newText)
	{
		tooltipComponent.SetText(newText);
	}

	public virtual void Setup(string name)
	{
	}

	public virtual void LockLocalizedText()
	{
	}

	public void MaskScrollControls(MEGUIParameter param)
	{
		param.maskedControls = new List<UIBehaviour>();
		param.maskedControls.AddRange(param.GetComponentsInChildren<TMP_InputField>(includeInactive: true));
		ScrollRect[] componentsInChildren = param.GetComponentsInChildren<ScrollRect>(includeInactive: true);
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			if (componentsInChildren[i].GetComponentsInParent<TMP_Dropdown>(includeInactive: true) == null)
			{
				param.maskedControls.Add(componentsInChildren[i]);
			}
		}
		int j = 0;
		for (int count = param.maskedControls.Count; j < count; j++)
		{
			UIBehaviour maskedControl = param.maskedControls[j];
			if (!maskedControl.transform.Find("scrollMask"))
			{
				GameObject gameObject = new GameObject("scrollMask", typeof(Button), typeof(Image));
				gameObject.GetComponent<Image>().color = Color.clear;
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.SetParent(maskedControl.transform, worldPositionStays: false);
				component.localPosition = Vector3.zero;
				component.anchorMin = Vector2.zero;
				component.anchorMax = Vector2.one;
				component.offsetMin = Vector2.zero;
				component.offsetMax = Vector2.zero;
				maskedControl.enabled = false;
				Button scrollMaskButton = gameObject.GetComponent<Button>();
				scrollMaskButton.onClick.AddListener(delegate
				{
					param.UnblockMaskedControls(scrollMaskButton, maskedControl);
				});
			}
		}
	}

	public void UnblockMaskedControls(Button scrollMaskButton, UIBehaviour maskedControl)
	{
		maskedControl.enabled = true;
		if (maskedControl is IPointerClickHandler)
		{
			((IPointerClickHandler)maskedControl).OnPointerClick(new PointerEventData(EventSystem.current));
		}
		else if (!isSelectable)
		{
			MissionEditorLogic.Instance.actionPane.OnParameterClick(this);
		}
		scrollMaskButton.enabled = false;
	}

	public void BlockMaskedControls()
	{
		if (maskedControls != null)
		{
			int i = 0;
			for (int count = maskedControls.Count; i < count; i++)
			{
				maskedControls[i].enabled = false;
				maskedControls[i].GetComponentInChildren<Button>().enabled = true;
			}
		}
	}

	public virtual string GetGroupName()
	{
		if (field == null)
		{
			return "";
		}
		return field.Group;
	}

	public void DisplayParameterObject()
	{
		if (!field.HideWhenStartNode && !field.HideWhenDocked && !field.HideWhenInputConnected && !field.HideWhenOutputConnected && !field.HideWhenNoTestModules && !field.HideWhenNoActionModules)
		{
			base.gameObject.SetActive(!field.HideOnSetup);
		}
		else
		{
			MENode mENode = null;
			if (module == null && field.host != null)
			{
				mENode = field.host as MENode;
			}
			else if (module != null && module.GetNode() != null)
			{
				mENode = module.GetNode();
			}
			if ((mENode != null && ((field.HideWhenStartNode && (mENode.isStartNode || mENode.IsDockedToStartNode)) || (field.HideWhenDocked && mENode.IsDocked) || (field.HideWhenInputConnected && mENode.fromNodes.Count > 0) || (field.HideWhenOutputConnected && mENode.toNodes.Count > 0))) || (field.HideWhenNoTestModules && (mENode.testGroups.Count == 0 || mENode.testGroups[0].testModules.Count == 0)) || (field.HideWhenNoActionModules && mENode.actionModules.Count == 0))
			{
				base.gameObject.SetActive(value: false);
			}
			else
			{
				base.gameObject.SetActive(value: true);
			}
		}
		if (pinToggle != null && module != null && IsPinnable)
		{
			bool flag2 = (pinToggle.isOn = module.HasNodeBodyParameter(field.name));
			isPinned = flag2;
			SetPinToggleImage();
			if (!isPinned)
			{
				pinCanvasGroup.alpha = 0f;
			}
		}
		else if (pinToggle != null)
		{
			pinToggle.gameObject.SetActive(value: false);
		}
	}

	public virtual void Display()
	{
		DisplayParameterObject();
		if (selectedIndicator != null)
		{
			selectedIndicator.canvasRenderer.SetAlpha(IsSelected ? 1f : 0f);
		}
	}

	public void UpdateNodeBodyUI()
	{
		if (module != null)
		{
			module.UpdateNodeBodyUI();
			return;
		}
		MEGUICompoundParameter componentInParent = base.transform.parent.GetComponentInParent<MEGUICompoundParameter>();
		if (componentInParent != null)
		{
			componentInParent.UpdateNodeBodyUI();
		}
	}

	public virtual void Select()
	{
		IsSelected = true;
		ShowColor();
	}

	public void ShowColor()
	{
		if (selectedIndicator != null)
		{
			selectedIndicator.CrossFadeAlpha(1f, 0.3f, ignoreTimeScale: true);
		}
	}

	public virtual void UnSelect()
	{
		IsSelected = false;
		HideColor();
		BlockMaskedControls();
	}

	public void HideColor()
	{
		if (selectedIndicator != null)
		{
			selectedIndicator.CrossFadeAlpha(0f, 0.3f, ignoreTimeScale: true);
		}
	}

	public void ChangeColor(Color color)
	{
		if (selectedIndicator != null)
		{
			selectedIndicator.color = color;
		}
	}

	public void SetPinToggleImage()
	{
		if (pinToggleImage != null)
		{
			pinToggleImage.SetState(isPinned ? "Pinned" : "Unpinned");
		}
	}

	public virtual void DisplayGAP()
	{
	}

	public virtual void ResetDefaultValue(string value)
	{
	}

	public virtual void RefreshUI()
	{
	}

	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		if (pinToggle != null && !isPinned)
		{
			pinCanvasGroup.alpha = 1f;
		}
		isMouseOver = true;
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		if (pinToggle != null && !isPinned)
		{
			pinCanvasGroup.alpha = 0f;
		}
		isMouseOver = false;
	}

	public void OnPinValueChanged(bool state)
	{
		MissionEditorHistory.PushUndoAction(this, OnHistoryPinned);
		isPinned = state;
		SetPinToggleImage();
		if (module != null)
		{
			if (isPinned)
			{
				module.AddParameterToNodeBodyAndUpdateUI(field.name);
			}
			else
			{
				module.RemoveParameterFromNodeBodyAndUpdateUI(field.name);
			}
		}
		MissionEditorLogic.Instance.actionPane.OnParameterClick(this);
	}

	public void RemoveParameterByGroup(string group)
	{
		if (module != null && field != null && field.Group == group)
		{
			module.RemoveParameterFromNodeBodyAndUpdateUI(field.name);
		}
	}

	public void OnParameterReset()
	{
		if (field != null)
		{
			if (field.Attribute.resetValue != null)
			{
				ResetDefaultValue(field.Attribute.resetValue);
			}
		}
		else
		{
			ResetDefaultValue(null);
		}
		RefreshUI();
	}

	public virtual void OnHistoryPinned(ConfigNode data, HistoryType type)
	{
		if (data.TryGetValue("pinned", ref isPinned))
		{
			pinToggle.isOn = isPinned;
			pinCanvasGroup.alpha = (isPinned ? 1 : 0);
			SetPinToggleImage();
		}
	}

	public virtual ConfigNode GetState()
	{
		ConfigNode configNode = new ConfigNode();
		configNode.AddValue("pinned", isPinned);
		configNode.AddValue("value", field.GetValue());
		return configNode;
	}
}
