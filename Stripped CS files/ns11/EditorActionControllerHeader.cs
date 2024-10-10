using Expansions.Serenity;
using ns2;
using TMPro;
using UnityEngine.UI;

namespace ns11;

public class EditorActionControllerHeader : UISelectableGridLayoutGroupItem
{
	public TextMeshProUGUI text;

	public Button editButton;

	public TMP_InputField inputField;

	public string controllerName;

	public ModuleRoboticController controller;

	public override void Select()
	{
		base.Select();
		editButton.interactable = true;
		editButton.gameObject.SetActive(value: true);
		inputField.gameObject.SetActive(value: false);
	}

	public override void Deselect()
	{
		base.Deselect();
		editButton.interactable = false;
		editButton.gameObject.SetActive(value: false);
		inputField.gameObject.SetActive(value: false);
	}

	public void Setup(string controllerName, ModuleRoboticController controller)
	{
		this.controllerName = controllerName;
		this.controller = controller;
		text.text = controllerName;
		editButton.onClick.AddListener(EditButtonClicked);
		inputField.onEndEdit.AddListener(InputFieldDone);
	}

	public void EditButtonClicked()
	{
		inputField.gameObject.SetActive(value: true);
		inputField.text = controllerName;
		inputField.ActivateInputField();
	}

	public void InputFieldDone(string newValue)
	{
		text.text = newValue;
		controller.SetDisplayName(newValue);
		if (HighLogic.LoadedSceneIsEditor && EditorActionGroups.Instance != null && EditorActionGroups.Instance.interfaceActive)
		{
			EditorActionGroups.Instance.RebuildLists(fullRebuild: true, keepSelection: true);
		}
		inputField.gameObject.SetActive(value: false);
	}
}
