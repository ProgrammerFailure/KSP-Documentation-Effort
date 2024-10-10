using ns9;
using TMPro;
using UnityEngine;

namespace ns2;

public class VesselListItem : MonoBehaviour
{
	public TextMeshProUGUI vesselName;

	public TextMeshProUGUI vesselParts;

	public TextMeshProUGUI vesselWarnings;

	public TextMeshProUGUI vesselCost;

	public TextMeshProUGUI vesselMass;

	public TextMeshProUGUI vesselSize;

	public UIRadioButton radioButton;

	public void Setup(string name, float mass, float cost, float size)
	{
		vesselName.text = name;
		vesselMass.text = Localizer.Format("#autoLOC_482576", mass.ToString());
		vesselCost.text = Localizer.Format("#autoLOC_6003093", cost.ToString());
		vesselSize.text = Localizer.Format("#autoLOC_482578", size.ToString());
	}

	public void Setup(string name, string parts, string warnings, string mass, string cost, string size)
	{
		vesselName.text = name;
		vesselParts.text = parts;
		vesselWarnings.text = warnings;
		vesselCost.text = cost;
		vesselMass.text = mass;
		vesselSize.text = size;
	}
}
