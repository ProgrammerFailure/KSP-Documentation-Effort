using ns2;
using ns9;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class EditorActionOverrideGroup : UISelectableGridLayoutGroupItem, IPointerClickHandler, IEventSystemHandler, ISubmitHandler
{
	public TextMeshProUGUI text;

	public Button editButton;

	public UIButtonToggle closeButton;

	public TMP_InputField inputField;

	public MenuNavigation menuNavigation;

	public string[] groupNames;

	public string groupName
	{
		get
		{
			if (groupOverride < 1)
			{
				return null;
			}
			return groupNames[groupOverride - 1];
		}
		set
		{
			if (groupOverride > 0)
			{
				groupNames[groupOverride - 1] = value;
			}
		}
	}

	public bool canEdit => groupNames != null;

	public int groupOverride { get; set; }

	public void SetName(string groupName)
	{
		if (string.IsNullOrEmpty(groupName))
		{
			text.text = ((groupOverride > 0) ? Localizer.Format("#autoLOC_6013001", groupOverride.ToString()) : Localizer.Format("#autoLOC_6013000"));
		}
		else
		{
			text.text = groupName;
		}
	}

	public override void Select()
	{
		base.Select();
		closeButton.gameObject.SetActive(value: true);
		if (canEdit)
		{
			editButton.gameObject.SetActive(value: true);
			inputField.gameObject.SetActive(value: false);
		}
	}

	public override void Deselect()
	{
		base.Deselect();
		if (canEdit)
		{
			editButton.gameObject.SetActive(value: false);
			inputField.gameObject.SetActive(value: false);
		}
	}

	public void Setup(int groupOverride, string[] groupNames, bool isOpen)
	{
		this.groupOverride = groupOverride;
		this.groupNames = groupNames;
		SetName(groupName);
		editButton.onClick.AddListener(EditButtonClicked);
		editButton.gameObject.SetActive(isOpen && canEdit);
		closeButton.SetState(isOpen);
		closeButton.gameObject.SetActive(value: true);
		closeButton.onToggleOff.AddListener(CloseGroup);
		closeButton.onToggleOn.AddListener(OpenGroup);
		inputField.onEndEdit.AddListener(InputFieldDone);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		EditorActionGroups.Instance.currentSelectedIndex = base.Index;
		OpenGroup();
	}

	public void OpenGroup()
	{
		EditorActionGroups.Instance.SetGroupOverride(groupOverride);
	}

	public void CloseGroup()
	{
		if (closeButton.state)
		{
			EditorActionGroups.Instance.SetGroupOverride(groupOverride);
		}
		else
		{
			Collapse();
		}
	}

	public void Collapse()
	{
		EditorActionGroups.Instance.CloseGroup(groupOverride);
		if (canEdit)
		{
			editButton.gameObject.SetActive(value: false);
			inputField.gameObject.SetActive(value: false);
		}
	}

	public void EditButtonClicked()
	{
		inputField.gameObject.SetActive(value: true);
		if (groupName != null)
		{
			inputField.text = groupName;
		}
		else
		{
			inputField.text = "";
		}
		inputField.ActivateInputField();
		MenuNavigation.blockPointerEnterExit = true;
		InputLockManager.SetControlLock(ControlTypes.KEYBOARDINPUT, "Action Groups Name Input");
	}

	public void InputFieldDone(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			groupName = null;
		}
		else
		{
			groupName = text;
		}
		SetName(groupName);
		closeButton.gameObject.SetActive(value: true);
		inputField.gameObject.SetActive(value: false);
		MenuNavigation.blockPointerEnterExit = false;
		if (menuNavigation != null)
		{
			menuNavigation.SetItemAsFirstSelected(base.gameObject);
		}
		InputLockManager.RemoveControlLock("Action Groups Name Input");
	}

	public void OnSubmit(BaseEventData eventData)
	{
		if (closeButton.state)
		{
			Collapse();
			return;
		}
		EditorActionGroups.Instance.currentSelectedIndex = base.Index;
		OpenGroup();
	}

	public void OnDestroy()
	{
		InputLockManager.RemoveControlLock("Action Groups Name Input");
		MenuNavigation.blockPointerEnterExit = false;
	}
}
