using System.Reflection;
using TMPro;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_Button]
public class MEGUIParameterButton : MEGUIParameter
{
	public Button button;

	public TMP_Text buttonText;

	public MethodInfo OnClick { get; set; }

	public override bool IsInteractable
	{
		get
		{
			return button.interactable;
		}
		set
		{
			button.interactable = value;
		}
	}

	public string FieldValue
	{
		get
		{
			return (string)field.GetValue();
		}
		set
		{
			field.SetValue(value);
			buttonText.text = value;
		}
	}

	public override void Setup(string name)
	{
		title.text = name;
		buttonText.text = FieldValue;
		if (!string.IsNullOrEmpty(((MEGUI_Button)field.Attribute).onClick))
		{
			OnClick = field.host.GetType().GetMethod(((MEGUI_Button)field.Attribute).onClick, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}
		button.onClick.AddListener(OnButtonClick);
	}

	public void OnButtonClick()
	{
		if (OnClick != null)
		{
			OnClick.Invoke(field.host, null);
		}
	}
}
