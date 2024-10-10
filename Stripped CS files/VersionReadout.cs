using Expansions;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VersionReadout : MonoBehaviour
{
	public TextMeshProUGUI displayText;

	public void Start()
	{
		if (GetComponent<TextMeshProUGUI>() != null)
		{
			displayText = GetComponent<TextMeshProUGUI>();
			displayText.text = Versioning.GetVersionStringFull() + " " + Localizer.CurrentLanguage;
			displayText.text += ExpansionsLoader.GetInstalledExpansionsString();
		}
		else if (GetComponent<Text>() != null)
		{
			Text component = GetComponent<Text>();
			component.text = Versioning.GetVersionStringFull() + " " + Localizer.CurrentLanguage;
			component.text += ExpansionsLoader.GetInstalledExpansionsString();
		}
	}
}
