using TMPro;
using UnityEngine;

namespace ns32;

public class ScreenVesselMassPartInfo : MonoBehaviour
{
	[SerializeField]
	public GameObject header;

	[SerializeField]
	public TextMeshProUGUI partName;

	[SerializeField]
	public TextMeshProUGUI stage;

	[SerializeField]
	public TextMeshProUGUI prefabMass;

	[SerializeField]
	public TextMeshProUGUI resourceMass;

	[SerializeField]
	public TextMeshProUGUI inventoryMass;

	[SerializeField]
	public TextMeshProUGUI kerbalMass;

	[SerializeField]
	public TextMeshProUGUI kerbalResourceMass;

	[SerializeField]
	public TextMeshProUGUI kerbalInventoryMass;

	[SerializeField]
	public TextMeshProUGUI totalMass;

	public Part part;

	public void UpdateData(Part part, bool firstRow)
	{
		if (header != null)
		{
			header.SetActive(firstRow);
		}
		this.part = part;
		partName.text = part.partInfo.title;
		stage.text = part.stageOffset.ToString("0");
		prefabMass.text = part.prefabMass.ToString("0.000");
		resourceMass.text = part.resourceMass.ToString("0.000");
		inventoryMass.text = part.inventoryMass.ToString("0.000");
		kerbalMass.text = part.kerbalMass.ToString("0.000");
		kerbalResourceMass.text = part.kerbalResourceMass.ToString("0.000");
		kerbalInventoryMass.text = part.kerbalInventoryMass.ToString("0.000");
		totalMass.text = (part.mass + part.resourceMass).ToString("0.000");
	}
}
