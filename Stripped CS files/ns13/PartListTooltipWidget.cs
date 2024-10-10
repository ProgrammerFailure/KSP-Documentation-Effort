using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns13;

public class PartListTooltipWidget : MonoBehaviour
{
	public TextMeshProUGUI textName;

	public TextMeshProUGUI textInfo;

	public Image widgetColourImage;

	public void Setup(string name, string info)
	{
		textName.text = name;
		textInfo.text = info;
	}
}
