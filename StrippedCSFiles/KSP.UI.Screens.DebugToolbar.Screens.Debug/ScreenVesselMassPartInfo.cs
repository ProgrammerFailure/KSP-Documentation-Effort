using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens.DebugToolbar.Screens.Debug;

public class ScreenVesselMassPartInfo : MonoBehaviour
{
	[SerializeField]
	private GameObject header;

	[SerializeField]
	private TextMeshProUGUI partName;

	[SerializeField]
	private TextMeshProUGUI stage;

	[SerializeField]
	private TextMeshProUGUI prefabMass;

	[SerializeField]
	private TextMeshProUGUI resourceMass;

	[SerializeField]
	private TextMeshProUGUI inventoryMass;

	[SerializeField]
	private TextMeshProUGUI kerbalMass;

	[SerializeField]
	private TextMeshProUGUI kerbalResourceMass;

	[SerializeField]
	private TextMeshProUGUI kerbalInventoryMass;

	[SerializeField]
	private TextMeshProUGUI totalMass;

	private Part part;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenVesselMassPartInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateData(Part part, bool firstRow)
	{
		throw null;
	}
}
