using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI;

public class VesselListItem : MonoBehaviour
{
	public TextMeshProUGUI vesselName;

	public TextMeshProUGUI vesselParts;

	public TextMeshProUGUI vesselWarnings;

	public TextMeshProUGUI vesselCost;

	public TextMeshProUGUI vesselMass;

	public TextMeshProUGUI vesselSize;

	public UIRadioButton radioButton;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselListItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(string name, float mass, float cost, float size)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(string name, string parts, string warnings, string mass, string cost, string size)
	{
		throw null;
	}
}
