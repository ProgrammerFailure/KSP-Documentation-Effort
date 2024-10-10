using TMPro;
using UnityEngine;

namespace ns30;

public class ScreenMissionNewItem : MonoBehaviour
{
	public TextMeshProUGUI titleText;

	public TextMeshProUGUI assemblytext;

	public void Start()
	{
	}

	public void SetupError(string errorText)
	{
		titleText.text = "<color=red>" + errorText + "</color>";
		titleText.enableWordWrapping = false;
		titleText.overflowMode = TextOverflowModes.Overflow;
		assemblytext.text = string.Empty;
	}
}
