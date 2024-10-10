using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

[Serializable]
public class TypeButton
{
	public Toggle toggle;

	public VesselType type;

	public Image icon;

	public TextMeshProUGUI label;

	public GameObject gameObject => toggle.gameObject;

	public void Select()
	{
		icon.color = XKCDColors.ElectricLime;
		label.color = XKCDColors.ElectricLime;
		toggle.isOn = true;
	}

	public void Deselect()
	{
		icon.color = Color.white;
		label.color = XKCDColors.ColorTranslator.FromHtml("#C0C4B0FF");
		toggle.isOn = false;
	}
}
