using TMPro;
using UnityEngine;

public class ManeuverToolVisualTextBox : MonoBehaviour
{
	[SerializeField]
	public TextMeshProUGUI nameText;

	[SerializeField]
	public TextMeshProUGUI valueText;

	public void Setup(string name, string value)
	{
		nameText.text = name;
		valueText.text = value;
	}

	public void SetNameText(string name)
	{
		nameText.text = name;
	}

	public void SetValueText(string value)
	{
		valueText.text = value;
	}
}
